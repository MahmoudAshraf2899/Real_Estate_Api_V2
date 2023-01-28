using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Projects;

namespace RealEstateApi.Handler.Projects
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectGyIdDto>
    {
        private readonly IProjectsRepository _projectsRepository;

        public GetProjectByIdHandler(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }
        public async Task<ProjectGyIdDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _projectsRepository.getById(request.id, request.lang);
            return result;
        }
    }
}
