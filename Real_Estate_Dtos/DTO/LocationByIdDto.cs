using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class LocationByIdDto
    {
        public int id { get; set; }
        public string locationNameAr { get; set; }
        public string locationNameEn { get; set; }
        public string addedByName { get; set; }
        public bool? isAvailable { get; set; }
        public string projectName { get; set; }
        public short? noRooms { get; set; }
        public short? noBathRooms { get; set; }
        public bool? withGarage { get; set; }
        public double? price { get; set; }
        public double? area { get; set; }
        public double? garageValue { get; set; }
        public double? meterPrice { get; set; }
        public string description { get; set; }
        public string locationType { get; set; }
        public string paymentType { get; set; }
        public int? yearBuilt { get; set; }
        public List<string> images { get; set; }





    }
}
