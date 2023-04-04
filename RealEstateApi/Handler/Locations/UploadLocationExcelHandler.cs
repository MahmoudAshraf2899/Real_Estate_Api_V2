using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Locations;

namespace RealEstateApi.Handler.Locations
{
    public class UploadLocationExcelHandler : IRequestHandler<LocationUploadExcelCommand, Location>
    {
        private readonly IlocationsRepository _locationRepository;

        public UploadLocationExcelHandler(IlocationsRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<Location> Handle(LocationUploadExcelCommand request, CancellationToken cancellationToken)
        {
            var obj = new Location();
            _locationRepository.ReadExcelData(request.file,request.accountId);
            return obj; 
        }
    }
}
