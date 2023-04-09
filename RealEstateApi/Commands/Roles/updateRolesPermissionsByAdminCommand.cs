using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Roles
{
    public class updateRolesPermissionsByAdminCommand : IRequest<RolesPermission>
    {
        public bool? isActive { get; set; }
        public int? roleId { get; set; }
        public int? permissionId { get; set; }
    }
}
