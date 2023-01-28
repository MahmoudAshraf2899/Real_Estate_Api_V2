using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class ProjectGyIdDto
    {
        public int id { get; set; }
        public string nameEn { get; set; }
        public string nameAr { get; set; }
        public string description { get; set; }
        public string adress { get; set; }
        public int? noUnits { get; set; }
        public int? createdBy { get; set; }
        public string creatorName { get; set; }
        public DateTime? createdAt { get; set; }
        public int? editedBy { get; set; }
        public string editorName { get; set; }
        public List<string> features { get; set; }
    }
}
