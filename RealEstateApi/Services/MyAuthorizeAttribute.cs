using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;

namespace RealEstateApi.Services
{
    public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private int _accountId;
        private short? _groupPermissionId;
        private bool? _isSuberAdmin;
        StringValues tokenHeader = "";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.User.Identity;
            var identity = (ClaimsIdentity)token;
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out tokenHeader);
            if (tokenHeader.Any() == true)
            {
                if (tokenHeader[0] != null)
                {
                    var userInfo = TokenManagerFactor.GetAdminUserInfo(identity);
                    if (userInfo.id != 0)
                    {

                        _accountId = userInfo.id;
                        _groupPermissionId = userInfo.groupId;
                        _isSuberAdmin = userInfo.isSuperAdmin;

                    }

                }
                if (_accountId == 0)
                {
                    context.Result = new UnauthorizedResult();
                    return;

                }
            }

        }
         

    }
}
