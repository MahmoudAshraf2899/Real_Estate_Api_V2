using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Location
{
    public class GetLocationByIdQuery : IRequest<LocationByIdDto>
    {
        public int id { get; }
        public string lang { get; }
        public GetLocationByIdQuery(int Id,string Lang)
        {
            id = Id;
            lang = Lang;
        }
    }
}
