using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;

namespace Real_Estate_Services
{
    public class RolesRepository : Repository<ecommerce_real_estateContext, Role>, IRolesRepository
    {        
        public async Task<List<DtoRolesDropDown>> getDropDownListForAdmin()
        {
            var list = await (from q in Context.Roles.AsNoTracking().Where(c => c.IsActive == true)
                        select new DtoRolesDropDown
                        {
                            id = q.Id,
                            title = q.Title
                        }).ToListAsync();
            return list;
        }
    }
}
