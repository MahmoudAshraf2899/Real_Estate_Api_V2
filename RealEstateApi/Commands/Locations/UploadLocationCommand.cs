using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Locations
{
    public class UploadLocationCommand : IRequest<Location>
    {
        public int locationId { get; }
        public IFormFile[] photos { get; }

        public UploadLocationCommand(IFormFile[] Photos ,int LocationId)
        {
            locationId = LocationId;
            photos = Photos;
        }
         

    }
}
