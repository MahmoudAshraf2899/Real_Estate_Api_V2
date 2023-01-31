using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.LocationsTypes
{
    public class LocationTypeAddCommand : IRequest<LocationsType>
    {
        public string arType { get; set; }
        public string enType { get; set; }
    }
}
