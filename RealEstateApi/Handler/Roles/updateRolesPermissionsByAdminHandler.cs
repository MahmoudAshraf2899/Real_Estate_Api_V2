using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.Roles;

namespace RealEstateApi.Handler.Roles
{
    public class UpdateRolesPermissionsByAdminHandler : IRequestHandler<updateRolesPermissionsByAdminCommand, RolesPermission>
    {
        private readonly IRolesPermissionsRepository _rolesPermissionsRepository;

        public UpdateRolesPermissionsByAdminHandler(IRolesPermissionsRepository rolesPermissionsRepository)
        {
            _rolesPermissionsRepository = rolesPermissionsRepository;
        }

        public async Task<RolesPermission> Handle(updateRolesPermissionsByAdminCommand request, CancellationToken cancellationToken)
        {
            var getRolePermissionObj = _rolesPermissionsRepository.FindBy(c => c.RoleId == request.roleId && c.PermissionId == request.permissionId).FirstOrDefault();
            if (getRolePermissionObj != null)
            {
                getRolePermissionObj.IsActive = request.isActive;
                await _rolesPermissionsRepository.UpdateAsync(getRolePermissionObj);
                await _rolesPermissionsRepository.SaveAsync();

            }
            return getRolePermissionObj;
        }
    }
}
