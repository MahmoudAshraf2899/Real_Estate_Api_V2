using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class VisitorsGetByIdDto
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string contactNameEn { get; set; }
        public string contactNameAr { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string creatorName { get; set; }
        public DateTime? createdAt { get; set; }
        public string SecMobile { get; set; }
    }
}
