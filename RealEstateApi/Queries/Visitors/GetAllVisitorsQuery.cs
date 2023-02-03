using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Visitors
{
    public class GetAllVisitorsQuery : IRequest<List<VisitorsGetAllDto>>
    {
        public int pageNumber { get; }
        public int pageSize { get; }
        public string lang { get; }
        public GetAllVisitorsQuery(int PageNumber, int PageSize, string Lang)
        {
            pageNumber = PageNumber;    
            pageSize = PageSize;
            lang = Lang;
        }
    }
}
