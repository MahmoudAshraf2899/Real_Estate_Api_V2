using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands
{
    public class LocationAddCommand : IRequest<Location>
    {
        public string locationNameEn { get; set; }
        public string locationNameAr { get; set; }
        public bool? isAvailable { get; set; }
        public short? noRooms { get; set; }
        public short? noBathRooms { get; set; }
        public bool? withGarage { get; set; }
        public double? price { get; set; }
        public double? area { get; set; }
        public double? garageValue { get; set; }
        public double? meterPrice { get; set; }
        public string description { get; set; }
        public int? projectId { get; set; }
        public int? locationTypeId { get; set; }
        public int? paymentTypeId { get; set; }
        public int? yearBuilt { get; set; }
        public int? accountId { get; set; }
    }
}
