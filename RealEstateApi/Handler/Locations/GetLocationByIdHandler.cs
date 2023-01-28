using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries;
using RealEstateApi.Queries.Location;

namespace RealEstateApi.Handler.Locations
{
    public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, LocationByIdDto>
    {
        private readonly IlocationsRepository _locationsRepository;

        public GetLocationByIdHandler(IlocationsRepository locationsRepository)
        {
            _locationsRepository = locationsRepository;
        }
        public async Task<LocationByIdDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationsRepository.getById(request.id, request.lang);
            return result;
        }
    }
}
