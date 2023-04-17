using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;

namespace Real_Estate_IServices
{
    public interface ISalesRepository : IRepository<User>
    {
        Task<List<DtoGetAllSalesUsers>> GetAllSales(int pageNumber, int pageSize, bool? isSuperVisor, bool? isSalesMan, string lang);
    }
}
