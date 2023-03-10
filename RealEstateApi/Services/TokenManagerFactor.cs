using Real_Estate_Context.Context;
using Real_Estate_Dtos.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace RealEstateApi.Services
{
    public class TokenManagerFactor
    {
        public static UserInfo GetAdminUserInfo(IIdentity token)
        {
            var identity = (ClaimsIdentity)token;
            List<Claim> claims = identity.Claims.ToList();
            var account = new UserInfo();

            var sub = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var dns = claims.FirstOrDefault(x => x.Type == ClaimTypes.Dns);//To Get PermissionId

            if (sub != null)
            {
                using var context = new ecommerce_real_estateContext();
                account = context.Admins.Where(x => x.IsDeleted != true && dns.Value == x.GroupPermission.ToString() && x.UserName == sub.Value && x.IsActive == true)
                .Select(x => new UserInfo
                {
                    id = x.Id,
                    userName = x.UserName,
                    groupId = x.GroupPermission,
                    isSuperAdmin = x.IsSuperAdmin

                }).FirstOrDefault();

                if (account == null)
                {

                    return new UserInfo();
                }
                return account;
            }

            return account;
        }
        //Todo : Create Get CustomerServicesInfoCore
        //Todo : Create Get VistiorInfoCore
         
    }
}
