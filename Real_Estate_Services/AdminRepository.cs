using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Services
{

    public class AdminRepository : Repository<ecommerce_real_estateContext, Admin>, IAdminRepository
    {
        public async Task<List<AdminsTableDto>> getAllAdmins(int pageNumber, int pageSize, string lang)
        {
            var result = new List<AdminsTableDto>();
            if (lang == "en")
            {

                result = await (from q in Context.Admins.AsNoTracking()
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

                result = await (from q in Context.Admins.AsNoTracking()
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
