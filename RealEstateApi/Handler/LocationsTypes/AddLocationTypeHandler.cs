using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.LocationsTypes;

namespace RealEstateApi.Handler.LocationsTypes
{
    public class AddLocationTypeHandler : IRequestHandler<LocationTypeAddCommand, LocationsType>
    {
        private readonly IlocationsTypesRepository _locationsTypesRepository;

        public AddLocationTypeHandler(IlocationsTypesRepository locationsTypesRepository)
        {
           _locationsTypesRepository = locationsTypesRepository;
        }
        public async Task<LocationsType> Handle(LocationTypeAddCommand request, CancellationToken cancellationToken)
        {
            //Check if type is exist before
            var enIsExist = _locationsTypesRepository.FindBy(c => c.EnType == request.enType).Any();
            var arIsExist = _locationsTypesRepository.FindBy(c => c.ArType == request.arType).Any();
            if (enIsExist || arIsExist)
            {
                throw new Exception();
            }
            else
            {
                LocationsType newType = new LocationsType();
                newType.EnType = request.enType;
                newType.ArType = request.arType;

                await _locationsTypesRepository.AddAsync(newType);
                await _locationsTypesRepository.SaveAsync();
                return newType;
            }
        }
    }
}
