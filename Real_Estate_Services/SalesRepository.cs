using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;

namespace Real_Estate_Services
{
    public class SalesRepository : Repository<ecommerce_real_estateContext, User>, ISalesRepository
    {
        public async Task<List<DtoGetAllSalesUsers>> GetAllSales(int pageNumber, int pageSize, bool? isSupervisor, bool? isSalesMan, string lang)
        {
            var list = new List<DtoGetAllSalesUsers>();
            if (lang == "en")
            {
                if (isSupervisor == true && isSalesMan == true)//User Is Supervisor 
                {
                      
                }
                //list = (from q in Context.Admins.AsNoTracking().Where(c=>c.i))
            }
            else
            {

            }
            return list;
        }
    }
}
