using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Locations
{
    public class LocationEditCommand : IRequest<Location>
    {
        public int id { get; set; }
        public short? noRooms { get; set; }
        public short? NoBathRooms { get; set; }
        public string? LocationNameEn { get; set; }
        public string? LocationNameAr { get; set; }
        public bool? WithGarage { get; set; }
        public double? price { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? LocationTypeId { get; set; }
        public bool? IsAvailable { get; set; }
        public double? garageValue { get; set; }
        public string description { get; set; }
        public double? meterPrice { get; set; }
        public double? area { get; set; }
        public int? projectId { get; set; }
        public int? yearBuilt { get; set; }
        public int? EditedBy { get; set; }        
    }
}
