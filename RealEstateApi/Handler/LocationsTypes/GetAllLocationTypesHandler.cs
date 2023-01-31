using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.LocationTypes;

namespace RealEstateApi.Handler.LocationsTypes
{
    public class GetAllLocationTypesHandler : IRequestHandler<GetAllTypesQuery, List<LocationsTypesGetAllDto>>
    {
        private readonly IlocationsTypesRepository _locationsTypesRepository;

        public GetAllLocationTypesHandler(IlocationsTypesRepository locationsTypesRepository)
        {
            _locationsTypesRepository = locationsTypesRepository;
        }
        public async Task<List<LocationsTypesGetAllDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            return await _locationsTypesRepository.GetAllTypes(request.pageNumber, request.pageSize, request.lang);
            
        }
    }
}
