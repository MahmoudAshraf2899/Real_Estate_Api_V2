using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Projects
{
    public class GetProjectByIdQuery : IRequest<ProjectGyIdDto>
    {
        public int id { get;}
        public string lang { get;}
        public GetProjectByIdQuery(int Id,string Lang)
        {
            id = Id;
            lang = Lang;
        }
    }
}
