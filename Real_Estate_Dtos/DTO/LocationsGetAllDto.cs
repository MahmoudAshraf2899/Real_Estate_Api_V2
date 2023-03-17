using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class LocationsGetAllDto
    {
        public int id { get; set; }
        public string locationNameEn { get; set; }
        public string LocationNameAr { get; set; }
        public double? Price { get; set; }
        public double? area { get; set; }
        public string projectName { get; set; }
        public string addedByName { get; set; }
        public bool? isAvailable { get; set; }

    }
    public class LocationsGetAllDtoEncapsulationTest
    {
        private double? _price;
        public double? Price
        {
            get
            {
                return _price;
            }
            set
            {
                this._price = processValue(value);
            }
        }
        public LocationsGetAllDtoEncapsulationTest(double? price)
        {
            _price = price;
        }
        private double? processValue(double? value) => value <= 0 ? 0 : value;




    }
}
