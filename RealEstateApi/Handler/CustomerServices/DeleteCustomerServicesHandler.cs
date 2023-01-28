using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.CustomerServices;

namespace RealEstateApi.Handler.CustomerServices
{
    public class DeleteCustomerServicesHandler : IRequestHandler<CustomerServiceDeleteCommand,CustomerService>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;
        private readonly IMemoryCache _cache;

        public DeleteCustomerServicesHandler(ICustomerServicesRepository customerServicesRepository,
            IMemoryCache cache)
        {
            _customerServicesRepository = customerServicesRepository;
            _cache = cache;
        }

        public async Task<CustomerService> Handle(CustomerServiceDeleteCommand request, CancellationToken cancellationToken)
        {
            //First: Get Account By Id
            var obj = _customerServicesRepository.FindBy(c => c.Id == request.id).SingleOrDefault();
            if (obj is not null)
            {
                obj.IsDeleted = true;
                obj.DeletedBy = request.accountId;
                obj.DeletedAt = DateTime.Now.Date;

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
