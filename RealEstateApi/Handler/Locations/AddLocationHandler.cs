using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands;

namespace RealEstateApi.Handler
{
    public class AddLocationHandler : IRequestHandler<LocationAddCommand, Location>
    {
        private readonly IMemoryCache _cache;
        private readonly IlocationsRepository _locationsRepository;
        private readonly IProjectsRepository _projectsRepository;

        public AddLocationHandler(IMemoryCache cache, IlocationsRepository locationsRepository,IProjectsRepository projectsRepository)
        {
            _cache = cache;
            _locationsRepository = locationsRepository;
            _projectsRepository = projectsRepository;
        }
        public async Task<Location> Handle(LocationAddCommand request, CancellationToken cancellationToken)
        {

            Location newLocation = new Location();
            newLocation.IsActive = true;
            newLocation.IsAvailable = true;
            newLocation.AddedBy = request.accountId;
            newLocation.CreatedAt = DateTime.Now.Date;
            newLocation.Area = request.area;
            newLocation.NoRooms = request.noRooms;
            newLocation.NoBathRooms = request.noBathRooms;
            newLocation.Description = request.description;
            newLocation.LocationNameEn = request.locationNameEn;
            newLocation.LocationNameAr = request.locationNameAr;
            newLocation.GarageValue = request.garageValue;
            newLocation.WithGarage = request.withGarage;
            newLocation.LocationTypeId = request.locationTypeId;
            newLocation.PaymentTypeId = request.paymentTypeId;
            newLocation.YearBuilt = request.yearBuilt;

            await _locationsRepository.AddAsync(newLocation);
            await _locationsRepository.SaveAsync();

            //Remove Old Data in Cache Storage To Get Accurate data
            _cache.Remove("locations Data");
            //Increase NoUnits In this Project
            if (request.projectId != null)
            {
                var project = _projectsRepository.FindBy(c => c.Id == request.projectId).FirstOrDefault();
                if (project != null)
                {
                    project.NoUnits = project.NoUnits + 1;
                    await _projectsRepository.UpdateAsync(project);
                    await _projectsRepository.SaveAsync();
                }
            }
            return newLocation;

        }
    }
}
