using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands;
using RealEstateApi.Commands.AdminCommand;
using RealEstateApi.Commands.CustomerServices;
using RealEstateApi.Commands.Locations;
using RealEstateApi.Commands.LocationsTypes;
using RealEstateApi.Commands.Login;
using RealEstateApi.Commands.PaymentTypes;
using RealEstateApi.Commands.Projects;
using RealEstateApi.Commands.Reservation;
using RealEstateApi.Commands.Roles;
using RealEstateApi.Commands.Visitors;
using RealEstateApi.Queries;
using RealEstateApi.Queries.Admin;
using RealEstateApi.Queries.CustomerServices;
using RealEstateApi.Queries.Location;
using RealEstateApi.Queries.LocationTypes;
using RealEstateApi.Queries.PaymentTypes;
using RealEstateApi.Queries.Projects;
using RealEstateApi.Queries.Reservation;
using RealEstateApi.Queries.Roles;
using RealEstateApi.Queries.Visitors;
using RealEstateApi.Services;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

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
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IRolesPermissionsRepository _rolesPermissionsRepository;
        private readonly IMemoryCache _cache;
        private readonly IMediator _meditor;
        private readonly ecommerce_real_estateContext _context;
        private int _accountId;
        private string _language;
        private bool? _isSuberAdmin;
        private bool? _isSuperVisor;
        private bool? _isSalesMan;
        public AdminController(IHttpContextAccessor accessor, IConfiguration config,
            IAdminRepository adminRepository,
            IProjectsRepository projectsRepository,
            IProjectFeatureRepository projectFeatureRepository,
            IlocationsTypesRepository locationsTypesRepository,
            IPaymentTypeRepository paymentTypeRepository,
            ILocationImageRepository locationImageRepository,
            IVisitorRepository visitorRepository,
            IlocationsRepository locationsRepository,
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository,
            IRolesPermissionsRepository rolesPermissionsRepository,
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
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _rolesPermissionsRepository = rolesPermissionsRepository;
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
                        _isSuperVisor = userInfo.isSuperVisor;
                        _isSuberAdmin = userInfo.isSuperAdmin;
                        _isSalesMan = userInfo.isSalesMan;
                    }
                }
            }
            #endregion

        }
        #region Private Methods
        /// <summary>
        /// This boolean method to check that user has permission to access specific data by passing 
        /// Code and check if this code exist in permission table or not
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool isAllow(int code)
        {
            //Get User Role
            var userObj = _adminRepository.FindBy(c => c.Id == _accountId).Select(c => new { c.RoleId, c.IsSuperAdmin })
                                                                           .FirstOrDefault();
            if (userObj.IsSuperAdmin == true)
            {
                return true;
            }
            else
            {

                var permissionId = _permissionsRepository.FindBy(c => c.Code == code).Select(c => c.Id).FirstOrDefault();

                var isActive = _rolesPermissionsRepository.FindBy(c => c.PermissionId == permissionId && c.RoleId == userObj.RoleId && c.IsActive == true).Any();
                if (isActive)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #region Old Code
            ////Get Role Permissions
            //var userRolePermissions = _rolesPermissionsRepository.FindBy(c => c.RoleId == userRole)
            //                                                     .Select(c => c.PermissionId)
            //                                                     .ToList();
            ////Todo : Check if this permission is Active for this Role or Not
            //if (userRolePermissions != null)
            //{
            //    foreach (var item in userRolePermissions)
            //    {
            //        var getPermissionByCode = _permissionsRepository.FindBy(c => c.Id == item).Select(c => new { c.Id, c.Code }).FirstOrDefault();

            //        if (getPermissionByCode.Code == code)
            //        {
            //            return true;
            //            break;

            //        }
            //        else
            //        {
            //            continue;
            //        }

            //    }
            //}
            //return true; 
            #endregion

        }
        #endregion


        [HttpGet]
        [Route("test")]
        public IActionResult test()
        {
            return Ok();
        }
        #region Authorization && Authentication


        [MyAuthorize]
        [HttpPost]
        [Route("AddNewAdmin")]
        public async Task<IActionResult> AddNewAdmin(AddNewAdminCommand command)
        {
            if (_isSuberAdmin != true)
            {
                return BadRequest();
            }
            else
            {
                command.accountId = _accountId;
                var result = await _meditor.Send(command);
                if (result == "User Name is Already Exist" || result == "Password Doesn't Match")
                    return BadRequest();
                else
                    return Ok(result);

            }
        }


        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin(AdminLoginCommand request)
        {
            try
            {
                var result = await _meditor.Send(request);

                if (result == "InvalidPassword")
                    return BadRequest();

                if (result == "UserNotFound")
                    return BadRequest();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }


        [MyAuthorize]
        [HttpGet]
        [Route("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins(int pageNumber, int pageSize)
        {
            if (isAllow(1))
            {
                var query = new GetAllAdminsQuery(pageNumber, pageSize, _language);
                var result = await _meditor.Send(query);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
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

        [MyAuthorize]
        [HttpPost]
        [Route("UploadExcelData")]
        public async Task<IActionResult> UploadExcelData(IFormFile file)
        {
            var query = new LocationUploadExcelCommand(file, _accountId);
            var result = await _meditor.Send(query);
            return Ok(result);
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

        //Todo :Move To Work With CQRS 
        [MyAuthorize]
        [Route("FilterVisitors")]
        [HttpGet]
        public async Task<IActionResult> FilterVisitors()
        {
            var result = await _visitorRepository.getFilteredVisitors();
            return Ok(result);
        }

        #endregion

        #region Work With Roles and Permissions

        [MyAuthorize]
        [HttpGet]
        [Route("GetAllRolesDropDownlistForAdmin")]

        public async Task<IActionResult> GetAllRolesDropDownlistForAdmin()
        {
            var query = new GetRolesForDropDownForAdminQuery();
            var result = await _meditor.Send(query);
            return Ok(result);
        }

        [MyAuthorize]
        [HttpGet]
        [Route("GetAllRolesPermissionsForAdmin")]
        public async Task<IActionResult> GetAllRolesPermissionsForAdmin(int roleId)
        {
            var query = new GetRolePermissionsForAdminQuery(roleId);
            var result = await _meditor.Send(query);
            return Ok(result);
        }
        [MyAuthorize]
        [HttpPost]
        [Route("UpdateRolePermissonByAdmin")]
        public async Task<IActionResult> UpdateRolePermissonByAdmin(List<updateRolesPermissionsByAdminCommand> dto)
        {
            object? result = new object();
            foreach (var item in dto)
            {
                result = await _meditor.Send(item);
            }
            return Ok();
        }

        #endregion

        #region Reservation
        [MyAuthorize]
        [HttpGet]
        [Route("GetAllReservations")]
        public async Task<IActionResult> GetAllReservations(int pageNumber, int pageSize)
        {
            if (isAllow(3))
            {
                var query = new GetAllReservationsQuery(pageNumber, pageSize, _language);
                var result = await _meditor.Send(query);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [MyAuthorize]
        [HttpPost]
        [Route("AddNewLocaionReservation")]
        public async Task<IActionResult> AddNewLocaionReservation(AddNewReservationCommand command)
        {
            if (isAllow(4))
            {

                command.accountId = _accountId;
                var result = await _meditor.Send(command);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        #endregion

        #region Old Code
        //[HttpGet]
        //[Route("TestFitlerSearch")]
        //public async Task<IActionResult> TestFitlerSearch([FromQuery] string q, [FromQuery] string field, [FromQuery] string op)
        //{
        //    var query = _context.Locations.AsEnumerable();

        //    if (!string.IsNullOrEmpty(q) && !string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(op))
        //    {
        //        query = query.Where(BuildFilterExpression<Location>(field, op, q));
        //    }
        //    return Ok(query);
        //}
        //private static Func<TEntity, bool> BuildFilterExpression<TEntity>(string propertyName, string op, string value)
        //{
        //    var param = Expression.Parameter(typeof(TEntity), "entity");
        //    var property = Expression.Property(param, propertyName);
        //    var constant = Expression.Constant(value);

        //    Expression body = op switch
        //    {
        //        "eq" => Expression.Equal(property, constant),
        //        "ne" => Expression.NotEqual(property, constant),
        //        "lt" => Expression.LessThan(property, constant),
        //        "le" => Expression.LessThanOrEqual(property, constant),
        //        "gt" => Expression.GreaterThan(property, constant),
        //        "ge" => Expression.GreaterThanOrEqual(property, constant),
        //        "cn" => Expression.Call(property, "Contains", null, constant),
        //        "sw" => Expression.Call(property, "StartsWith", null, constant),
        //        "ew" => Expression.Call(property, "EndsWith", null, constant),
        //        _ => throw new ArgumentException($"Invalid operator '{op}'.")
        //    };

        //    return Expression.Lambda<Func<TEntity, bool>>(body, param).Compile();
        //} 
        #endregion

    }

}
