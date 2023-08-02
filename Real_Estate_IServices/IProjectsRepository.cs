using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface IProjectsRepository : IRepository<Project>
    {
        Task<List<ProjectsTableDto>> getAll(int pageNumber ,int pageSize,string lang);
        Task<ProjectGyIdDto> getById(int id,string lang);
        IEnumerable<ProjectsTableDto> GetAllProjectsCompiledQuery(int pageNumber, int pageSize, string lang);

    }
}
