using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Projects
{
    public class GetAllProjectsQuery : IRequest<List<ProjectsTableDto>>
    {
        public int pageNumber { get;}
        public int pageSize { get;}
        public string lang { get;}
        public GetAllProjectsQuery(int PageNumber, int PageSize , string Lang)
        {
            pageNumber = PageNumber;
            pageSize = PageSize;    
            lang = Lang;
        }
    }
}
