using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Locations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RealEstateApi.Handler.Locations
{
    public class UploadLocationHandler : IRequestHandler<UploadLocationCommand, Location>
    {
        private readonly IlocationsRepository _locationsRepository;
        private readonly ILocationImageRepository _locationImageRepository;

        public UploadLocationHandler(IlocationsRepository locationsRepository,
            ILocationImageRepository locationImageRepository)
        {
            _locationsRepository = locationsRepository;
            _locationImageRepository = locationImageRepository;
        }
        public async Task<Location> Handle(UploadLocationCommand request, CancellationToken cancellationToken)
        {
            var obj = new Location();
            int counter = 0;
            foreach (var file in request.photos)
            {
                if (file.FileName.Contains("jpg") && file.FileName.Contains("png"))
                    throw new Exception();

                string folderName = "LocationImages";
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (file == null || file.Length == 0)
                    throw new Exception();

                //Get location ById
                obj = _locationsRepository.FindBy(c => c.Id == request.locationId).FirstOrDefault();
                if (obj != null)
                {
                    string? path = "";
                    string? fileName = "";
                    #region Resize Image 
                    using (var image = Image.FromStream(file.OpenReadStream()))
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
                        counter = counter + 1;
                        fileName = $"{obj.LocationNameEn}_Picture{counter}.jpg";

                        path = Path.Combine(Directory.GetCurrentDirectory(), pathToSave, fileName);
                        thumbnailImg.Save(path, ImageFormat.Jpeg);
                    }
                    //Assign Photos to Locations
                    LocationImage newLocationImage = new LocationImage();
                    newLocationImage.LocationId = request.locationId;
                    newLocationImage.ImagePath = fileName;
                    await _locationImageRepository.AddAsync(newLocationImage);
                    await _locationImageRepository.SaveAsync();

                    #endregion                    
                }
                else
                {
                    throw new Exception();
                }
            }
            return obj;
        }
    }
}
