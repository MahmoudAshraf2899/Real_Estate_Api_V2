using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.CustomerServices;

namespace RealEstateApi.Handler.CustomerServices
{
    public class EditCustomerServicesHandler : IRequestHandler<CustomerServicesEditCommand, CustomerService>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;
        private readonly IMemoryCache _cache;

        public EditCustomerServicesHandler(ICustomerServicesRepository customerServicesRepository,
            IMemoryCache cache)
        {
            _customerServicesRepository = customerServicesRepository;
            _cache = cache;
        }
        public async Task<CustomerService> Handle(CustomerServicesEditCommand request, CancellationToken cancellationToken)
        {
            //First: Find This Acc by ID
            var obj = _customerServicesRepository.FindBy(c => c.Id == request.id).SingleOrDefault();
            if (obj is not null)
            {
                obj.Address = request.address;
                obj.Phone = request.phone;
                obj.Email = request.email;
                obj.EditedBy = request.accountId;
                obj.GroupPermission = 2;
                obj.ContactNameAr = request.contactNameAr;
                obj.ContactNameEn = request.contactNameEn;
                obj.LastEditDate = DateTime.Now.Date;
                obj.Insured = request.isInsured;
                obj.Salary = request.salary;
                obj.UserName = request.userName;
                obj.EditedBy = request.accountId;

                await _customerServicesRepository.UpdateAsync(obj);
                await _customerServicesRepository.SaveAsync();
                //Second : Remove caching Becuase We Modify The Data in Data Source
                _cache.Remove("Customer Services Data");
                return obj;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
