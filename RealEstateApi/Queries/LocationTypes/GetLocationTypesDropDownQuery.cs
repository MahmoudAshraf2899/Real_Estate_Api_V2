using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.LocationTypes
{
    public class GetLocationTypesDropDownQuery : IRequest<List<locationTypesDropDownDto>>
    {
        public string lang { get;  }
        public GetLocationTypesDropDownQuery(string Lang)
        {
            lang = Lang;
        }
    }
}
