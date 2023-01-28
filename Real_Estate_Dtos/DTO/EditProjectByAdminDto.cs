using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class EditProjectByAdminDto
    {
        public int id { get; set; }
        public string nameEn { get; set; }
        public string nameAr { get; set; }
        public string description { get; set; }
        public bool? isActive { get; set; }
        public string address { get; set; }
        public List<string>? features { get; set; }

    }
}
