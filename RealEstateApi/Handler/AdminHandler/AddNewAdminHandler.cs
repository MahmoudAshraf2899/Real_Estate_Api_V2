using MediatR;
using Microsoft.Identity.Client;
using NuGet.Protocol.Plugins;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.AdminCommand;
using RealEstateApi.Services;

namespace RealEstateApi.Handler.AdminHandler
{
    public class AddNewAdminHandler : IRequestHandler<AddNewAdminCommand, string>
    {
        private readonly IAdminRepository _adminRepository;

        public AddNewAdminHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<string> Handle(AddNewAdminCommand request, CancellationToken cancellationToken)
        {
            //First : Check If User Name is Exist in Visitor Table
            bool isExist = _adminRepository.FindBy(c => c.UserName == request.userName).Any();
            if (isExist)
            {
                string userNameAlert = "User Name is Already Exist";
                //User Already Exist
                return userNameAlert;
            }
            //Second : Check If Passwords Matches
            string passwordMatching = "Password Doesn't Match";
            if (request.password != request.confirmPassword)
            {
                return passwordMatching;
            }
            //Third : Add Operation
            #region Admin
            Real_Estate_Context.Models.Admin newAdmin = new Real_Estate_Context.Models.Admin();
            newAdmin.Email = request.email;
            newAdmin.CreatedAt = DateTime.Now.Date;
            newAdmin.CreatedBy = request.accountId;
            newAdmin.UserName = request.userName;
            newAdmin.ContactNameEn = request.contactNameEn;
            newAdmin.ContactNameAr = request.contactNameAr;
            newAdmin.IsActive = true;
            newAdmin.GroupPermission = 1;
            newAdmin.Password = PasswordHash.CreateHash(request.password);

            await _adminRepository.AddAsync(newAdmin);
            await _adminRepository.SaveAsync();
            return "Done";
            #endregion
        }
    }
}
