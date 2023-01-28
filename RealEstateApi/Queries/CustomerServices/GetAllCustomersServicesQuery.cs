using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.CustomerServices
{
    public class GetAllCustomersServicesQuery : IRequest<List<CustomerServicesTableDto>>
    {
        public int pageNumber { get; }
        public int pageSize { get; }
        public string lang { get; }
        public GetAllCustomersServicesQuery(int PageNumber , int PageSize , string Lang)
        {
            pageNumber = PageNumber;
            pageSize = PageSize;    
            lang = Lang;    
        }
    }
}
