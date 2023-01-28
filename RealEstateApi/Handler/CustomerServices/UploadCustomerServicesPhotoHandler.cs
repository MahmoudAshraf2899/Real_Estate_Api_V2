using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.CustomerServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RealEstateApi.Handler.CustomerServices
{
    public class UploadCustomerServicesPhotoHandler : IRequestHandler<CustomerServicesUploadPhotoCommand, CustomerService>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;

        public UploadCustomerServicesPhotoHandler(ICustomerServicesRepository customerServicesRepository)
        {
            _customerServicesRepository = customerServicesRepository;
        }
        public async Task<CustomerService> Handle(CustomerServicesUploadPhotoCommand request, CancellationToken cancellationToken)
        {
            if (request.photo.FileName.Contains("jpg") && request.photo.FileName.Contains("png"))
                throw new Exception();
            string folderName = "CustomerServicesPhotos";
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            if (request.photo == null || request.photo.Length == 0)
               throw new Exception();
            //Get CustomerServices ById
            var obj = _customerServicesRepository.FindBy(c => c.Id == request.id).FirstOrDefault();
            if (obj != null)
            {
                obj.ProfilePicture = request.photo.FileName;
                string? path = "";
                #region Resize Image 
                using (var image = Image.FromStream(request.photo.OpenReadStream()))
                {
                    var newWidth = 300;
                    var newHeight = 300;
                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    var fileName = $"{obj.UserName}_Picture.jpg";

                    path = Path.Combine(Directory.GetCurrentDirectory(), pathToSave, fileName);
                    thumbnailImg.Save(path, ImageFormat.Jpeg);
                }
                #endregion                 
                await _customerServicesRepository.UpdateAsync(obj);
                await _customerServicesRepository.SaveAsync();
                return obj;
            }
            else { throw new Exception(); }
        }
    }
}
