using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Visitors
{
    public class GetVisitorByIdQuery : IRequest<VisitorsGetByIdDto>
    {
        public int id { get; }
        public string lang { get; }
        public GetVisitorByIdQuery(int Id, string Lang)
        {
            id = Id;
            lang = Lang;
        }
    }
}
