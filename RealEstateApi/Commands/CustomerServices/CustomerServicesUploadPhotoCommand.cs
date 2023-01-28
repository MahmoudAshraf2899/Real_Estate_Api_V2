using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.CustomerServices
{
    public class CustomerServicesUploadPhotoCommand : IRequest<CustomerService>
    {
        public int id { get; }
        public IFormFile photo { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Photo"></param>
        public CustomerServicesUploadPhotoCommand(int Id,IFormFile Photo)
        {
            id = Id;
            photo = Photo;

        }
    }
}
