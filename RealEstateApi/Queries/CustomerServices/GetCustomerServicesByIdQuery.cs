using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.CustomerServices
{
    public class GetCustomerServicesByIdQuery : IRequest<CustomerServicesByIdDto>
    {
        public int id { get; set; }
        public string lang { get; set; }
        public GetCustomerServicesByIdQuery(int Id , string Lang)
        {
            id = Id;
            lang = Lang;
        }
    }
}
