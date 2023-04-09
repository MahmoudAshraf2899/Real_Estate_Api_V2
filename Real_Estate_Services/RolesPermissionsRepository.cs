using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;

namespace Real_Estate_Services
{
    public class RolesPermissionsRepository : Repository<ecommerce_real_estateContext, RolesPermission>, IRolesPermissionsRepository
    {
        public async Task<List<DtoGetRolePermissionsList>> getRolePermissionsForAdmin(int roleId)
        {
            var result = await (from q in Context.RolesPermissions.AsNoTracking().Where(c => c.RoleId == roleId)
                                
                                select new DtoGetRolePermissionsList
                                {
                                    id = q.Id,
                                    permissionId = q.PermissionId,
                                    roleId = q.RoleId,
                                    code = q.Permission.Code,
                                    permissionTitle = q.Permission.Title,
                                    roleTitle = q.Role.Title,
                                    isActive = q.IsActive !=true ? false : true
                                }).ToListAsync();
            return result;
        }
    }
}
