using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface IVisitorRepository : IRepository<Visitor>
    {
        Task<List<VisitorsGetAllDto>> getAllVisitors(int pageNumber, int pageSize, string lang);
        Task<VisitorsGetByIdDto> getvisitorById(int id, string lang);
        Task<List<VisitorsEmailsDto>> getAllActiveVisitorsEmails();
        //Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(string propertyName, string propertyValue);
        Task<List<VisitorsGetAllDto>> getFilteredVisitors();

    }
}
