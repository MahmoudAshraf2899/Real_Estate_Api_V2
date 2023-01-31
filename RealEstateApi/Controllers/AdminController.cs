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
using RealEstateApi.Queries;
using RealEstateApi.Queries.CustomerServices;
using RealEstateApi.Queries.Location;
using RealEstateApi.Queries.LocationTypes;
using RealEstateApi.Queries.PaymentTypes;
using RealEstateApi.Queries.Projects;
using RealEstateApi.Services;
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
        public IActionResult SendMailToVisitors(EmailFigureDto dto)
        {
            //Only Super Admin Who Can Use This Feature
            if (_isSuberAdmin != true)
            {
                return Unauthorized();
            }
            else
            {
                //Get All Visitors Emails
                var visitorsList = _context.Visitors.AsNoTracking().Where(c => c.IsActive != false).ToList();
                foreach (var item in visitorsList)
                {
                    try
                    {
                        using (var client = new SmtpClient())
                        {
                            var message = new MailMessage();
                            message.To.Add(new MailAddress(item.Email));//Make It to all Visitors which they are Active
                            message.From = new MailAddress("mahmodashrf79@gmail.com", "Real Estate");//Move To AppSettings

                            message.Subject = dto.subject;
                            message.Body = dto.body;

                            client.Host = "smtp.gmail.com";
                            client.Port = 587;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("mahmodashrf79@gmail.com", "vohoaanrrbqkoaww");//Move To AppSettings
                            client.EnableSsl = true;
                            client.Send(message);
                        }
                    }

                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                return Ok();
            }
        }

        [MyAuthorize]
        [Route("GetAllVisitors")]
        [HttpGet]
        public async Task<IActionResult> GetAllVisitors(int pageNumber, int pageSize)
        {
            var result = await _visitorRepository.getAllVisitors(pageNumber, pageSize, _language);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("GetVisitorById")]
        [HttpGet]
        public async Task<IActionResult> GetVisitorById(int id)
        {
            var result = await _visitorRepository.getvisitorById(id, _language);
            return Ok(result);
        }

        [MyAuthorize]
        [Route("AddVisitorByAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddVisitorByAdmin(VisitorAddDto dto)
        {
            Visitor newVisitor = new Visitor();
            newVisitor.IsActive = true;
            newVisitor.CreatedBy = _accountId;
            newVisitor.Email = dto.email;
            newVisitor.ContactNameEn = dto.enContactName;
            newVisitor.ContactNameAr = dto.arContactName;
            newVisitor.UserName = dto.userName;
            newVisitor.GroupPermission = 3;
            newVisitor.CreatedAt = DateTime.Now.Date;
            newVisitor.Mobile = dto.mobile;
            newVisitor.SecMobile = dto.secMobile;
            newVisitor.Password = "12345";

            await _visitorRepository.AddAsync(newVisitor);
            await _visitorRepository.SaveAsync();
            return Ok(dto);
        }

        [MyAuthorize]
        [Route("BlockVisitorByAdmin")]
        [HttpPost]
        public async Task<IActionResult> BlockVisitorByAdmin(int id)
        {
            //Get visitor by Id 
            var _obj = _visitorRepository.FindBy(c => c.Id == id).FirstOrDefault();
            if (_obj is not null)
            {
                _obj.DeletedBy = _accountId;
                _obj.IsDeleted = true;
                _obj.IsActive = false;

                await _visitorRepository.UpdateAsync(_obj);
                await _visitorRepository.SaveAsync();
                return Ok();

            }
            else
            {
                return BadRequest();
            }

        }

        #endregion


    }
}
