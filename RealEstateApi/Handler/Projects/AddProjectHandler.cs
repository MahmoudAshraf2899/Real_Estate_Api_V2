using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Projects;

namespace RealEstateApi.Handler.Projects
{
    public class AddProjectHandler : IRequestHandler<ProjectAddCommand, Project>
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProjectFeatureRepository _projectFeatureRepository;
        private readonly IMemoryCache _cache;

        public AddProjectHandler(IProjectsRepository projectsRepository,
            IProjectFeatureRepository projectFeatureRepository,
            IMemoryCache cache)
        {
            _projectsRepository = projectsRepository;
            _projectFeatureRepository = projectFeatureRepository;
            _cache = cache;
        }
        public async Task<Project> Handle(ProjectAddCommand request, CancellationToken cancellationToken)
        {
            //Check if En Name is Exist 
            var enNameExist = _projectsRepository.FindBy(c => c.NameEn == request.nameEn).Any();
            if (enNameExist)
                throw new Exception();

            //Check if Ar Name is Exist 
            var arNameExist = _projectsRepository.FindBy(c => c.NameAr == request.nameAr).Any();
            if (arNameExist)
                throw new Exception();

            #region Project Table
            Project newProject = new Project();
            newProject.AddedBy = request.accountId;
            newProject.Description = request.description;
            newProject.CreatedAt = DateTime.Now.Date;
            newProject.IsActive = true;
            newProject.NameEn = request.nameEn;
            newProject.NameAr = request.nameAr;
            newProject.Address = request.address;

            await _projectsRepository.AddAsync(newProject);
            await _projectsRepository.SaveAsync();
            _cache.Remove("Projects Data Key");
            #endregion
            #region Project_Features
            foreach (var item in request.features)
            {
                ProjectsFeature projectFeature = new ProjectsFeature();
                projectFeature.ProjectId = newProject.Id;
                projectFeature.Feature = item;
                await _projectFeatureRepository.AddAsync(projectFeature);
                await _projectFeatureRepository.SaveAsync();
                _cache.Remove("Projects Features Key");

            }
            #endregion
            return newProject;
        }
    }
}
