using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Locations;

namespace RealEstateApi.Handler.Locations
{
    public class EditLocationHandler : IRequestHandler<LocationEditCommand, Location>
    {
        private readonly IlocationsRepository _locationsRepository;
        private readonly IMemoryCache _cache;
        private readonly IProjectsRepository _projectsRepository;

        public EditLocationHandler(IlocationsRepository locationsRepository, IMemoryCache cache,
            IProjectsRepository projectsRepository)
        {
            _locationsRepository = locationsRepository;
            _cache = cache;
            _projectsRepository = projectsRepository;
        }
        public async Task<Location> Handle(LocationEditCommand request, CancellationToken cancellationToken)
        {
            var _obj = _locationsRepository.FindBy(c => c.Id == request.id).SingleOrDefault();
            if (_obj != null)
            {
                _obj.NoRooms = request.noRooms;
                _obj.NoBathRooms = request.NoBathRooms;
                _obj.LocationNameEn = request.LocationNameEn;
                _obj.LocationNameAr = request.LocationNameAr;
                _obj.WithGarage = request.WithGarage;
                _obj.Area = request.area;
                _obj.Description = request.description;
                _obj.GarageValue = request.garageValue;
                _obj.IsAvailable = request.IsAvailable;
                _obj.LocationTypeId = request.LocationTypeId;
                _obj.PaymentTypeId = request.PaymentTypeId;
                _obj.Price = request.price;
                _obj.ProjectId = request.projectId;
                _obj.YearBuilt = request.yearBuilt;
                _obj.EditedBy = request.EditedBy;
                await _locationsRepository.UpdateAsync(_obj);
                await _locationsRepository.SaveAsync();
                //Remove Old Data in Cache Storage To Get Accurate data
                _cache.Remove("locations Data");
                //Work With Project No.Units
                if (request.projectId is not null)
                {
                    //Find project
                    var project = _projectsRepository.FindBy(c => c.Id == request.projectId).FirstOrDefault();
                    if (project is not null)
                    {
                        project.NoUnits = project.NoUnits + 1;
                        await _projectsRepository.UpdateAsync(project);
                        await _projectsRepository.SaveAsync();
                    }
                }
            }
            return _obj;
        }
    }
}
