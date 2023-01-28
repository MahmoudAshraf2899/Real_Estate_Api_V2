using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class AdminsTableDto
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string contactNameEn { get; set; }
        public string contactNameAr { get; set; }
        public string email { get; set; }
        public bool? isActive { get; set; }

    }
}
