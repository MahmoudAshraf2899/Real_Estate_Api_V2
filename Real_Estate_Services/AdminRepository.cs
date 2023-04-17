using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;

namespace Real_Estate_Services
{

    public class AdminRepository : Repository<ecommerce_real_estateContext, User>, IAdminRepository
    {
        public async Task<List<AdminsTableDto>> getAllAdmins(int pageNumber, int pageSize, string lang)
        {
            var result = new List<AdminsTableDto>();
            if (lang == "en")
            {

                result = await (from q in Context.Users.AsNoTracking()
                             .Where(c => c.IsActive == true && c.IsDeleted != true)
                          select new AdminsTableDto
                          {
                              id = q.Id,
                              userName = q.UserName,
                              email = q.Email,
                              isActive = q.IsActive,
                              contactNameEn =q.ContactNameEn,
                              
                             
                          }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {

                result = await (from q in Context.Users.AsNoTracking()
                             .Where(c => c.IsActive == true && c.IsDeleted != true)
                          select new AdminsTableDto
                          {
                              id = q.Id,
                              userName = q.UserName,
                              email = q.Email,
                              isActive = q.IsActive,
                              contactNameAr=q.ContactNameAr,
                          }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            return result;
        }
    }
}
