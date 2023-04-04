using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Dtos.DTO
{
    public class UserInfo
    {        
        public int id { get; set; }
        public short? groupId { get; set; }
        public string userName { get; set; }
        public bool? isSuperAdmin { get; set; }        
        public bool? isSuperVisor { get; set; }        
        public bool? isSalesMan { get; set; }        
    }
}
