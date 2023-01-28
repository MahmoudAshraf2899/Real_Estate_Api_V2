using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class CustomerServicesByIdDto
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string contactNameEn { get; set; }
        public string contactNameAr { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public bool? isInsured { get; set; }
        public int? salary { get; set; }
        public int? createdById { get; set; }
        public string createdByName { get; set; }
    }
}
