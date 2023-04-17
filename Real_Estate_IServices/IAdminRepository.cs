using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;

namespace Real_Estate_IServices
{
    public interface IAdminRepository : IRepository<User>
    {
        Task<List<AdminsTableDto>> getAllAdmins(int pageNumber, int pageSize,string lang);
    }
}
