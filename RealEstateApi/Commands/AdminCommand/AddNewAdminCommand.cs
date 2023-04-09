using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.AdminCommand
{
    public class AddNewAdminCommand : IRequest<string>
    {
        public string userName { get; set; }
        public string contactNameAr { get; set; }
        public string contactNameEn { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public int accountId { get; set; }
    }
}
