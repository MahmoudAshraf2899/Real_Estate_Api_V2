using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Projects;

namespace RealEstateApi.Handler.Projects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectsTableDto>>
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IMemoryCache _cache;

        public GetAllProjectsHandler(IProjectsRepository projectsRepository , IMemoryCache cache)
        {
            _projectsRepository = projectsRepository;
            _cache = cache;
        }
        public async Task<List<ProjectsTableDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            string key = "Projects Data Key";
            List<ProjectsTableDto>? projects = new List<ProjectsTableDto>();
            if (!_cache.TryGetValue(key, out projects))
            {
                // Data not found in cache, fetch it from the Db
                projects = await _projectsRepository.getAll(request.pageNumber, request.pageSize, request.lang);
                // Store the data in the cache for 10 minutes
                _cache.Set(key, projects, TimeSpan.FromMinutes(10));
            }
            return projects;             
        }
    }
}
