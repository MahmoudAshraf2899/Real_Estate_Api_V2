using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Projects;

namespace RealEstateApi.Handler.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand,Project>
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProjectFeatureRepository _projectFeatureRepository;
        private readonly IMemoryCache _cache;

        public DeleteProjectHandler(IProjectsRepository projectsRepository, 
            IProjectFeatureRepository projectFeatureRepository,
            IMemoryCache cache)
        {
            _projectsRepository = projectsRepository;
            _projectFeatureRepository = projectFeatureRepository;
            _cache = cache;
        }
        public async Task<Project> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            //First:Get project from db
            var obj = _projectsRepository.FindBy(c => c.Id == request.id).SingleOrDefault();
            if (obj is not null)
            {
                obj.DeletedBy = request.accountId;
                obj.IsActive = false;
                obj.DeletedAt = DateTime.Now;
                await _projectsRepository.UpdateAsync(obj);
                await _projectsRepository.SaveAsync();
                _cache.Remove("Projects Data Key");

                var projectIds = _projectFeatureRepository.FindBy(c => c.ProjectId == obj.Id).ToList();
                foreach (var item in projectIds)
                {
                    //Second:Remove Project Features
                    await _projectFeatureRepository.DeleteAsync(item);
                    await _projectFeatureRepository.SaveAsync();
                    _cache.Remove("Projects Features Key");
                }
                return obj;
            }
            else
            {
                throw new Exception();
            }

        }
    }
}
