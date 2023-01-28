using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries;

namespace RealEstateApi.Handler
{
    public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, List<LocationsGetAllDto>>
    {
        private readonly IMemoryCache _cache;
        private readonly IlocationsRepository _locationsRepository;

        public GetAllLocationsHandler(IMemoryCache cache,IlocationsRepository locationsRepository)
        {
            _cache = cache;
            _locationsRepository = locationsRepository;
        }
        public async Task<List<LocationsGetAllDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {           
            string key = "locations Data";
            List<LocationsGetAllDto>? locations = new List<LocationsGetAllDto>();
            if (!_cache.TryGetValue(key, out locations))
            {
                // Data not found in cache, fetch it from the Db
                locations = await _locationsRepository.getAllLocations(request.pageNumber, request.pageSize, request.lang);
                // Store the data in the cache for 10 minutes
                _cache.Set(key, locations, TimeSpan.FromMinutes(10));
            }
            return locations;          
        }
    }
}
