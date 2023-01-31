using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.LocationTypes;

namespace RealEstateApi.Handler.LocationsTypes
{

    public class GetLocationTypesDropDownHandler : IRequestHandler<GetLocationTypesDropDownQuery, List<locationTypesDropDownDto>>
    {
        private readonly IlocationsTypesRepository _locationsTypesRepository;

        public GetLocationTypesDropDownHandler(IlocationsTypesRepository locationsTypesRepository)
        {
            _locationsTypesRepository = locationsTypesRepository;
        }
        public async Task<List<locationTypesDropDownDto>> Handle(GetLocationTypesDropDownQuery request, CancellationToken cancellationToken)
        {
            return await _locationsTypesRepository.getDropDown(request.lang);

        }
    }
}
