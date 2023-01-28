using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Projects;

namespace RealEstateApi.Handler.Projects
{
    public class EditProjectHandler : IRequestHandler<ProjectEditCommand, Project>
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProjectFeatureRepository _projectFeatureRepository;
        private readonly IMemoryCache _cache;

        public EditProjectHandler(IProjectsRepository projectsRepository, 
            IProjectFeatureRepository projectFeatureRepository,
            IMemoryCache cache)
        {
            _projectsRepository = projectsRepository;
            _projectFeatureRepository = projectFeatureRepository;
            _cache = cache;
        }
        public async Task<Project> Handle(ProjectEditCommand request, CancellationToken cancellationToken)
        {
            //First:Get Project From Db
            var obj = _projectsRepository.FindBy(c => c.Id == request.id).FirstOrDefault();
            if (obj is not null)
            {
                if (obj.NameEn != request.nameEn)//Second:If User Change The Current En Name
                {
                    var enNameExist = _projectsRepository.FindBy(c => c.NameEn == request.nameEn && c.IsActive == true).Any();
                    if (enNameExist)
                        throw new Exception();
                }
                else if (obj.NameAr != request.nameAr) //Third:If User Change The Current Ar Name
                {
                    var arNameExist = _projectsRepository.FindBy(c => c.NameAr == request.nameAr && c.IsActive == true).Any();
                    if (arNameExist)
                        throw new Exception();
                }
                else
                {
                    #region Project Table
                    obj.Address = request.address;
                    obj.EditedBy = request.accountId;
                    obj.LastDateModified = DateTime.Now.Date;
                    obj.NameEn = request.nameEn;
                    obj.NameAr = request.nameAr;
                    obj.Description = request.description;
                    obj.IsActive = request.isActive;

                    await _projectsRepository.UpdateAsync(obj);
                    await _projectsRepository.SaveAsync();
                    _cache.Remove("Projects Data Key");
                    #endregion
                    #region Projects Feature Table
                    var projectIds = _projectFeatureRepository.FindBy(c => c.ProjectId == request.id).ToList();
                    foreach (var item in projectIds)
                    {
                        //Todo : Try To Change This Scenario
                        //Delete Old Project Features
                        await _projectFeatureRepository.DeleteAsync(item);
                        await _projectFeatureRepository.SaveAsync();
                        _cache.Remove("Projects Features Key");
                    }

                    foreach (var item in request.features)
                    {
                        //Add New Project Features
                        ProjectsFeature projectFeature = new ProjectsFeature();
                        projectFeature.ProjectId = obj.Id;
                        projectFeature.Feature = item;
                        await _projectFeatureRepository.AddAsync(projectFeature);
                        await _projectFeatureRepository.SaveAsync();
                        _cache.Remove("Projects Features Key");
                    }
                    #endregion
                    return obj;
                }
            }
            else
            {
               throw new Exception();
            }             
            return obj;
        }
    }
}
