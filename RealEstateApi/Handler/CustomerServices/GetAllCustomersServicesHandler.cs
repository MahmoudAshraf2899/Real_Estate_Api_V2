using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.CustomerServices;

namespace RealEstateApi.Handler.CustomerServices
{
    public class GetAllCustomersServicesHandler : IRequestHandler<GetAllCustomersServicesQuery, List<CustomerServicesTableDto>>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;
        private readonly IMemoryCache _cache;

        public GetAllCustomersServicesHandler(ICustomerServicesRepository customerServicesRepository,
            IMemoryCache cache)
        {
            _customerServicesRepository = customerServicesRepository;
            _cache = cache;
        }
        public async Task<List<CustomerServicesTableDto>> Handle(GetAllCustomersServicesQuery request, CancellationToken cancellationToken)
        {
            string key = "Customer Services Data";
            var result = new List<CustomerServicesTableDto>();
            if (!_cache.TryGetValue(key, out result))
            {
                // Data not found in cache, fetch it from the Db
                result = await _customerServicesRepository.getAll(request.pageNumber, request.pageSize, request.lang);
                // Store the data in the cache for 10 minutes
                _cache.Set(key, result, TimeSpan.FromMinutes(10));
            }
            return result;
        }
    }
}
