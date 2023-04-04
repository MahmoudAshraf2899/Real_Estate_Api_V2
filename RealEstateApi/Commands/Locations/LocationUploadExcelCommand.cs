using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Locations
{
    public class LocationUploadExcelCommand : IRequest<Location>
    {
        public IFormFile file { get; set; }
        public int accountId { get; set; }
        public LocationUploadExcelCommand(IFormFile File,int AccountId)
        {
            accountId = AccountId;
            file = File;
        }
    }
}
