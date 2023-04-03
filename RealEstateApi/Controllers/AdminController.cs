using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Commands;
using RealEstateApi.Commands.CustomerServices;
using RealEstateApi.Commands.Locations;
using RealEstateApi.Commands.LocationsTypes;
using RealEstateApi.Commands.PaymentTypes;
using RealEstateApi.Commands.Projects;
using RealEstateApi.Commands.Visitors;
using RealEstateApi.Queries;
using RealEstateApi.Queries.CustomerServices;
using RealEstateApi.Queries.Location;
using RealEstateApi.Queries.LocationTypes;
using RealEstateApi.Queries.PaymentTypes;
using RealEstateApi.Queries.Projects;
using RealEstateApi.Queries.Visitors;
using RealEstateApi.Services;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace RealEstateApi.Controllers
{

    [Route("api/realestate/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly IAdminRepository _adminRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProjectFeatureRepository _projectFeatureRepository;
        private readonly IlocationsTypesRepository _locationsTypesRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly ILocationImageRepository _locationImageRepository;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IlocationsRepository _locationsRepository;
        private readonly IMemoryCache _cache;
        private readonly IMediator _meditor;
        private readonly ecommerce_real_estateContext _context;
        private int _accountId;
        private string _language;
        private bool? _isSuberAdmin;
        public AdminController(IHttpContextAccessor accessor, IConfiguration config,
            IAdminRepository adminRepository,
            IProjectsRepository projectsRepository,
            IProjectFeatureRepository projectFeatureRepository,
            IlocationsTypesRepository locationsTypesRepository,
            IPaymentTypeRepository paymentTypeRepository,
            ILocationImageRepository locationImageRepository,
            IVisitorRepository visitorRepository,
            IlocationsRepository locationsRepository,
            IMemoryCache cache,
            IMediator meditor,
            ecommerce_real_estateContext context)
        {
            _accessor = accessor;
            _config = config;
            _adminRepository = adminRepository;
            _projectsRepository = projectsRepository;
            _projectFeatureRepository = projectFeatureRepository;
            _locationsTypesRepository = locationsTypesRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _locationImageRepository = locationImageRepository;
            _visitorRepository = visitorRepository;
            _locationsRepository = locationsRepository;
            _cache = cache;
            _meditor = meditor;
            _context = context;
            StringValues languageHeader = "";
            StringValues tokenHeader = "";
            _accessor.HttpContext.Request.Headers.TryGetValue("Lang", out languageHeader);

            if (languageHeader.Any() == true)
            {
                _language = languageHeader[0].ToString();
            }
            else
            {
                _language = "en";
            }

            #region Extract Data From Token

            var token = _accessor.HttpContext.User.Identity;
            var identity = (ClaimsIdentity)token;
            _accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out tokenHeader);

            if (tokenHeader.Any() == true)
            {
                if (tokenHeader[0] != null)
                {
                    var userInfo = TokenManagerFactor.GetAdminUserInfo(identity);
                    if (userInfo.id != 0)
                    {
                        _accountId = userInfo.id;

                        _isSuberAdmin = userInfo.isSuperAdmin;
                    }
                }
            }
            #endregion

        }

        #region Authorization && Authentication

        private object GenerateAdminToken(Admin info)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var iss = "Seven Seas Server";

            var claims = new[] {
                new Claim(ClaimTypes.Email, info.UserName),
                new Claim(ClaimTypes.Actor, iss),
                new Claim(ClaimTypes.Version, info.Id.ToString()),
                new Claim(ClaimTypes.Hash, Guid.NewGuid().ToString("D")),
                new Claim(ClaimTypes.Dns, info.GroupPermission.ToString()),

             };
            //Let Token Expires After Three Days
            var token = new JwtSecurityToken(expires: DateTime.Now.AddDays(3), claims: claims, signingCredentials: credentials);

            var tokenValid = new JwtSecurityTokenHandler().WriteToken(token);

            var result = new
            {
                access_token = tokenValid,
                iss = "api/Nightmare",
                sub = info.UserName,
                spi = info.GroupPermission.ToString(),
                acn = info.ContactNameEn,
                aci = info.Id.ToString(),
            };
            return result;
        }

        [MyAuthorize]
        [HttpPost]
        [Route("AddNewAdmin")]
        public async Task<IActionResult> AddNewAdmin(DtoRegister model)
        {

            if (_isSuberAdmin != true)
            {
                return BadRequest();
            }
            else
            {
                //First : Check If User Name is Exist in Visitor Table
                bool isExist = _context.Admins.Where(C => C.UserName == model.userName).Any();
                if (isExist)
                {
                    string userNameAlert = "User Name is Already Exist";
                    //User Already Exist
                    return BadRequest(userNameAlert);
                }
                //Second : Check If Passwords Matches
                string passwordMatching = "Password Doesn't Match";
                if (model.password != model.confirmPassword)
                {
                    return BadRequest(passwordMatching);
                }
                //Third : Add Operation
                #region Admin
                Admin newAdmin = new Admin();
                newAdmin.Email = model.email;
                newAdmin.CreatedAt = DateTime.Now.Date;
                newAdmin.CreatedBy = _accountId;
                newAdmin.UserName = model.userName;
                newAdmin.ContactNameEn = model.contactNameEn;
                newAdmin.ContactNameAr = model.contactNameAr;
                newAdmin.IsActive = true;
                newAdmin.GroupPermission = 1;
                newAdmin.Password = PasswordHash.CreateHash(model.password);

                await _context.Admins.AddAsync(newAdmin);
                await _context.SaveChangesAsync();
                #endregion
                return Ok();
            }
        }

        [HttpPost]
        [Route("AdminLogin")]
        public IActionResult AdminLogin([FromBody] LoginDTO user)
        {
            var client = _context.Admins.Where(c => c.IsDeleted != true && c.IsActive == true && c.UserName == user.userName).FirstOrDefault();
            if (client == null)
            {
                return BadRequest("Invalid User Name : " + user.userName);
            }

            if (client != null)
            {
                var checkPassword = PasswordHash.CreateHash(user.password);
                if (!PasswordHash.ValidatePassword(user.password, client.Password))
                {
                    return BadRequest("Wrong User's Password.");
                }
            }

            return Ok(GenerateAdminToken(client));

        }

        [MyAuthorizeAttribute]
        [HttpGet]
        [Route("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins(int pageNumber, int pageSize)
        {
            var result = await _adminRepository.getAllAdmins(pageNumber, pageSize, _language);
            return Ok(result);
        }
        #endregion

        #region Admin With Customer Services
        [MyAuthorize]
        [HttpGet]
        [Route("GetAllCustomerServicesTable")]
        public async Task<IActionResult> GetAllCustomerServices(int pageNumber, int pageSize)
        {
            var query = new GetAllCustomersServicesQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpGet]
        [Route("GetCustomerServiceById")]
        public async Task<IActionResult> getCustomerServiceById(int id)
        {
            var query = new GetCustomerServicesByIdQuery(id, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }


        [MyAuthorize]
        [HttpPost]
        [Route("AddNewCustomerServicesByAdmin")]
        public async Task<IActionResult> AddNewCustomerServicesByAdmin(CustomerServicesAddCommand dto)
        {
            dto.accountId = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("UploadCustomerServicePic")]
        public async Task<IActionResult> UploadCustomerServicePic([FromForm] IFormFile photo, int id)
        {
            var query = new CustomerServicesUploadPhotoCommand(id, photo);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("EditCustomerServiceByAdmin")]
        public async Task<IActionResult> EditCustomerServiceByAdmin(CustomerServicesEditCommand dto)
        {
            dto.accountId = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("SoftDeleteCustomerServiceByAdmin")]
        public async Task<IActionResult> SoftDeleteCustomerServiceByAdmin(int id)
        {
            var query = new CustomerServiceDeleteCommand(id, _accountId);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        #endregion

        #region Admin With Projects        
        [MyAuthorize]
        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects(int pageNumber, int pageSize)
        {
            var query = new GetAllProjectsQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }


        [MyAuthorize]
        [HttpGet]
        [Route("GetProjectById")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var query = new GetProjectByIdQuery(id, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("AddNewProject")]
        public async Task<IActionResult> AddNewProject(ProjectAddCommand dto)
        {
            dto.accountId = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("EditProject")]
        public async Task<IActionResult> EditProject(ProjectEditCommand dto)
        {
            dto.accountId = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpPost]
        [Route("SoftDeleteProject")]
        public async Task<IActionResult> SoftDeleteProject(int id)
        {
            var query = new DeleteProjectCommand(id, _accountId);
            var result = await _meditor.Send(query);
            return Ok(result);

        }
        #endregion

        #region Admin With Locations

        [MyAuthorize]
        [Route("GetAllLocationsByAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllLocationsByAdmin(int pageNumber, int pageSize)
        {
            #region Encapsulation Test
            //var result = await _locationsRepository.getAllLocationsTest();
            //return Ok(result); 
            #endregion
            var query = new GetAllLocationsQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("GetLocationById")]
        [HttpGet]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var query = new GetLocationByIdQuery(id, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("AddNewLocation")]
        [HttpPost]
        public async Task<IActionResult> AddNewLocation(LocationAddCommand dto)
        {
            dto.accountId = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("EditLocation")]
        [HttpPost]
        public async Task<IActionResult> EditLocation(LocationEditCommand dto)
        {
            dto.EditedBy = _accountId;
            var result = await _meditor.Send(dto);
            if (result is not null)
                return Ok(result);

            return NoContent();
        }


        [MyAuthorize]
        [Route("UploadLocationPhotos")]
        [HttpPost]
        public async Task<IActionResult> UploadLocationPhotos([FromForm] IFormFile[] photos, int locationId)
        {
            var query = new UploadLocationCommand(photos, locationId);
            var result = await _meditor.Send(query);
            if (result is not null)
                return Ok(result);
            else
                return NoContent();
        }

        #endregion

        #region Admin With Location Type

        [MyAuthorize]
        [Route("GetAllLocationsTypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllLocationsTypes(int pageNumber, int pageSize)
        {
            var query = new GetAllTypesQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }
        [MyAuthorize]
        [Route("AddNewLocationType")]
        [HttpPost]
        public async Task<IActionResult> AddNewLocationType(LocationTypeAddCommand dto)
        {
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("GetLocationTypesForDropDown")]
        [HttpGet]
        public async Task<IActionResult> GetLocationTypesForDropDown()
        {
            var query = new GetLocationTypesDropDownQuery(_language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }
        #endregion

        #region Admin With Payment Type
        [MyAuthorize]
        [Route("GetAllPaymentTypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllPaymentTypes(int pageNumber, int pageSize)
        {
            var query = new GetAllPaymentsTypesQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("GetPaymentTypeDropDown")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentTypeDropDown()
        {
            var query = new GetPaymentsTypesDropDownQuery(_language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }
        [MyAuthorize]
        [Route("AddNewPaymentType")]
        [HttpPost]
        public async Task<IActionResult> AddNewPaymentType(PaymentTypeAddCommand dto)
        {
            var result = await _meditor.Send(dto);
            return Ok(result);
        }
        #endregion

        #region Admin With Visitors        
        [MyAuthorize]
        [Route("SendMailToVisitors")]
        [HttpPost]
        public async Task<IActionResult> SendMailToVisitors(SendMailToVisitorsCommand dto)
        {
            //Only Super Admin Who Can Use This Feature
            if (_isSuberAdmin != true)
            {
                return Unauthorized();
            }
            else
            {
                var result = await _meditor.Send(dto);
                return Ok(result);
            }
        }

        [MyAuthorize]
        [Route("GetAllVisitors")]
        [HttpGet]
        public async Task<IActionResult> GetAllVisitors(int pageNumber, int pageSize)
        {
            //Todo : Implement Caching
            var query = new GetAllVisitorsQuery(pageNumber, pageSize, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("GetVisitorById")]
        [HttpGet]
        public async Task<IActionResult> GetVisitorById(int id)
        {
            var query = new GetVisitorByIdQuery(id, _language);
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("AddVisitorByAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddVisitorByAdmin(VisitorAddCommand dto)
        {
            dto.createdBy = _accountId;
            var result = await _meditor.Send(dto);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("BlockVisitorByAdmin")]
        [HttpPost]
        public async Task<IActionResult> BlockVisitorByAdmin(int id)
        {
            var command = new VisitorBlockCommand(id, _accountId);
            var result = await _meditor.Send(command);
            return Ok(result);
        }


        #endregion
        [Authorize]
        [HttpPost]
        [Route("UploadExcelData")]
        public IActionResult UploadExcelData(IFormFile file)
        {
            var data = ReadExcelData(file);

            return Ok(data);
        }

        private void insertDataIntoLocationsTable(List<LocationAddDto> data)
        {
            Location newLocation = new Location();
            foreach (var item in data)
            {
                newLocation.LocationNameAr = item.locationNameAr;
                newLocation.LocationNameEn = item.locationNameEn;
                newLocation.Price = item.price;
                newLocation.GarageValue = item.garageValue;
                newLocation.WithGarage = item.garageValue == 0 || item.garageValue == null ? false : true;
                newLocation.IsAvailable = item.isAvailable;
                newLocation.Area = item.area;
                newLocation.Description = item.description;
                newLocation.MeterPrice = item.meterPrice;
                newLocation.NoBathRooms = item.noBathRooms;
                newLocation.NoRooms = item.noRooms;
                newLocation.YearBuilt = item.yearBuilt;
                _locationsRepository.AddAsync(newLocation);
                _locationsRepository.SaveAsync();
            }
        }


        #region Old Code
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="rowNumber"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private object FindValueInColumn(IXLWorksheet worksheet, int rowNumber, string columnName)
        {

            var column = worksheet.ColumnsUsed(c => c.Cell(1).GetString() == columnName).FirstOrDefault();

            if (column != null)
            {
                var row = column.Cells().FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.GetString()));
                if (row != null)
                {
                    return row.Value;
                }
            }

            return null;
        }
        /// <summary>
        ///(J) Refer To Column Number
        ///(I) Refer To Row Number
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private List<LocationAddDto> OldReadExcelData(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);

                var rows = worksheet.RowsUsed();
                var columns = worksheet.ColumnsUsed().Count();

                var data = new List<LocationAddDto>();


                for (int i = 2; i <= rows.Count(); i++) // Start from row 2 to skip header row
                {
                    var rowData = new LocationAddDto();
                    for (int j = 1; j <= columns; j++)
                    {
                        string? locationNameEn = "";
                        string locationNameAr = "";
                        string? projectName = "";
                        int? paymentType = 0;
                        string? locationType = "";
                        string? description = "";
                        bool? isAvailable = false;
                        var price = 0;
                        var area = 0;
                        var garageValue = 0;
                        short? noRooms = 1;
                        var meterPrice = 0;
                        var yearBuilt = 0;
                        short? NoBathRooms = 1;
                        var column = worksheet.Cell(1, j).Value.ToString();//NO 1 Refer To Columns Header


                        if (column == "LocationNameEn")
                        {
                            locationNameEn = worksheet.Cell(i, j).Value.GetText();
                        }
                        else if (column == "LocationNameAr")
                        {
                            locationNameAr = worksheet.Cell(i, j).Value.GetText();
                        }
                        else if (column == "Price")
                        {
                            var x = worksheet.Cell(i, j).Value.GetNumber();
                            price = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "ProjectName")
                        {
                            projectName = worksheet.Cell(i, j).Value.GetText();
                        }
                        else if (column == "Area")
                        {
                            area = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "PaymentType")
                        {
                            paymentType = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "LocationType")
                        {
                            locationType = worksheet.Cell(i, j).Value.GetText();
                        }
                        else if (column == "GarageValue")
                        {
                            garageValue = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "NoRooms")
                        {

                            noRooms = short.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "NoBathRooms")
                        {
                            NoBathRooms = short.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "Description")
                        {
                            description = worksheet.Cell(i, j).Value.GetText();
                        }
                        else if (column == "IsAvailable")
                        {
                            //var x = worksheet.Cell(i, j).Value.GetBoolean();
                            isAvailable = worksheet.Cell(i, j).Value.GetBoolean();
                        }
                        else if (column == "MeterPrice")
                        {
                            meterPrice = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }
                        else if (column == "YearBuilt")
                        {
                            yearBuilt = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
                        }

                        rowData.locationNameEn = locationNameEn;
                        if (locationNameEn == "")
                        {
                            var rowNumber = i;
                        }
                        rowData.locationNameAr = locationNameAr;
                        rowData.price = price;
                        rowData.garageValue = garageValue;
                        rowData.withGarage = garageValue == 0 || garageValue == null ? false : true;
                        rowData.isAvailable = isAvailable;
                        rowData.area = area;
                        rowData.description = description;
                        rowData.meterPrice = meterPrice;
                        rowData.noBathRooms = NoBathRooms;
                        rowData.noRooms = noRooms;
                        rowData.yearBuilt = yearBuilt;
                        data.Add(rowData);

                    }

                }
                insertDataIntoLocationsTable(data);
                return data;
            }
        } 
        #endregion

        private List<LocationAddDto> ReadExcelData(IFormFile file)
        {
            var list = new List<LocationAddDto>();
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var fileName = file.FileName;             
            using (var stream = file.OpenReadStream())
            {
                var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // Skip header row
                var columns = worksheet.ColumnsUsed().Count();
                var headers = worksheet.RowsUsed().FirstOrDefault();
                if (headers == null)
                {
                    return null;
                }
                var columnNames = headers.Cells().Select(c => c.Value.ToString()).ToList();

                string? locationNameEn = "";
                string locationNameAr = "";
                string? projectName = "";
                int? paymentType = 0;
                string? locationType = "";
                string? description = "";
                bool? isAvailable = false;
                var price = 0;
                var area = 0;
                var garageValue = 0;
                short? noRooms = 1;
                var meterPrice = 0;
                var yearBuilt = 0;
                short? NoBathRooms = 1;
                foreach (var row in rows)
                {
                    var rowData = new LocationAddDto();                   
                    locationNameEn = row.Cell(1).Value.ToString();
                    locationNameAr = row.Cell(2).Value.ToString();
                    projectName = row.Cell(3).Value.ToString();
                    price = (int)row.Cell(4).Value.GetNumber();
                    area = (int)row.Cell(5).Value.GetNumber();
                    paymentType = (int)row.Cell(6).Value.GetNumber();
                    locationType = row.Cell(7).Value.ToString();
                    garageValue = (int)row.Cell(8).Value.GetNumber();
                    noRooms = (short)row.Cell(9).Value.GetNumber();
                    NoBathRooms = (short)row.Cell(10).Value.GetNumber();
                    description = row.Cell(11).Value.ToString();
                    isAvailable = row.Cell(12).Value.GetBoolean();
                    meterPrice = (int)row.Cell(13).Value.GetNumber();
                    yearBuilt = (int)row.Cell(14).Value.GetNumber();

                    #region Store List Of Objects To Use in Next Method
                    rowData.locationNameEn = locationNameEn;
                    rowData.locationNameAr = locationNameAr;
                    rowData.price = price;
                    rowData.garageValue = garageValue;
                    rowData.withGarage = garageValue == 0 || garageValue == null ? false : true;
                    rowData.isAvailable = isAvailable;
                    rowData.area = area;
                    rowData.description = description;
                    rowData.meterPrice = meterPrice;
                    rowData.noBathRooms = NoBathRooms;
                    rowData.noRooms = noRooms;
                    rowData.yearBuilt = yearBuilt;

                    list.Add(rowData); 
                    #endregion



                   insertDataIntoLocationsTable(list);
                }
            }

            return list;
        }
    }

}
