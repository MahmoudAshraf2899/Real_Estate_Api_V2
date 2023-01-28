using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.CustomerServices;

namespace RealEstateApi.Handler.CustomerServices
{
    public class AddCustomerServicesHandler : IRequestHandler<CustomerServicesAddCommand, CustomerService>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;

        public AddCustomerServicesHandler(ICustomerServicesRepository customerServicesRepository)
        {
            _customerServicesRepository = customerServicesRepository;
        }
        public async Task<CustomerService> Handle(CustomerServicesAddCommand request, CancellationToken cancellationToken)
        {
            CustomerService newCustomerServices = new CustomerService();
            //Check If The Db has this user before
            var isExist = _customerServicesRepository.FindBy(c => c.UserName == request.userName).Any();
            if (isExist)
            {
                throw new Exception();
            }
            else
            {                 
                newCustomerServices.UserName = request.userName;
                newCustomerServices.ContactNameEn = request.contactNameEn;
                newCustomerServices.ContactNameAr = request.contactNameAr;
                newCustomerServices.Address = request.address;
                newCustomerServices.Phone = request.phone;
                newCustomerServices.Email = request.email;
                newCustomerServices.CreatedAt = DateTime.Now.Date;
                newCustomerServices.GroupPermission = 2;
                newCustomerServices.Password = request.password;
                newCustomerServices.Insured = request.isInsured;
                newCustomerServices.CreatedBy = request.accountId;
                await _customerServicesRepository.AddAsync(newCustomerServices);
                await _customerServicesRepository.SaveAsync();

            }
            return newCustomerServices;
        }
    }
}
