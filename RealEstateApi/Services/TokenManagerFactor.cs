using Microsoft.EntityFrameworkCore;
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
            try
            {
                var identity = (ClaimsIdentity)token;
                List<Claim> claims = identity.Claims.ToList();
                var account = new UserInfo();

                var sub = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                var dns = claims.FirstOrDefault(x => x.Type == ClaimTypes.Dns);//To Get PermissionId

                if (sub != null)
                {
                    using var context = new ecommerce_real_estateContext();
                    account = context.Admins.Where(x => x.IsDeleted != true &&
                                                        dns.Value == x.GroupPermission.ToString() &&
                                                        x.UserName == sub.Value &&
                                                        x.IsActive == true)
                    .Select(x => new UserInfo
                    {
                        id = x.Id,
                        userName = x.UserName,
                        groupId = x.GroupPermission,
                        isSuperAdmin = x.IsSuperAdmin,


                    }).FirstOrDefault();

                    if (account == null)
                    {

                        return new UserInfo();
                    }
                    //Check If User Is Supevisor on another account
                    var isSuperVisor = context.Admins.AsNoTracking().Where(c => c.SupervisorId == account.id).Any();
                    var isSalesMan = context.Admins.AsNoTracking().Where(c => c.Id == account.id && c.IsSales == true)
                                                                  .FirstOrDefault();
                    if (isSuperVisor)
                    {
                        account.isSuperVisor = true;
                    }
                    if (isSalesMan != null)
                    {
                        account.isSalesMan = true;
                    }
                    return account;
                }

                return account;
            }
            catch (Exception ex)
            {
                UserInfo info = new UserInfo();
                info.userName = ex.Message;
                return info;

            }
        }
        //Todo : Create Get CustomerServicesInfoCore
        //Todo : Create Get VistiorInfoCore

    }
}
