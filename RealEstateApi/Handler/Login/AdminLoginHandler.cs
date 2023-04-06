using MediatR;
using Microsoft.IdentityModel.Tokens;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Login;
using RealEstateApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateApi.Handler.Login
{
    public sealed class AdminLoginHandler : IRequestHandler<AdminLoginCommand, object>
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _config;

        public AdminLoginHandler(IAdminRepository adminRepository, IConfiguration config)
        {
            _adminRepository = adminRepository;
            _config = config;
        }
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

        public async Task<object> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            //Get User
            var admin = _adminRepository.FindBy(c => c.IsDeleted != true && c.IsActive == true && c.UserName == request.userName)
                                        .FirstOrDefault();
            if (admin is not null)
            {
                var checkPassword = PasswordHash.CreateHash(request.password);
                if (!PasswordHash.ValidatePassword(request.password, admin.Password))
                {
                    return "InvalidPassword";
                }
            }
            else
            {
                return "UserNotFound";
            }

            var token = GenerateAdminToken(admin);

            return token;

        }
    }
}
