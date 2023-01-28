using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface IVisitorRepository : IRepository<Visitor>
    {
        Task<List<VisitorsGetAllDto>> getAllVisitors(int pageNumber, int pageSize, string lang);
        Task<VisitorsGetByIdDto> getvisitorById(int id, string lang);
    }
}
