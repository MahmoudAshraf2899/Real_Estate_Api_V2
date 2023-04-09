using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Roles;

namespace RealEstateApi.Handler.Roles
{
    public class GetRolePermissionsForAdminHandler : IRequestHandler<GetRolePermissionsForAdminQuery, List<DtoGetRolePermissionsList>>
    {
        private readonly IRolesPermissionsRepository _rolesPermissionsRepository;

        public GetRolePermissionsForAdminHandler(IRolesPermissionsRepository rolesPermissionsRepository)
        {
            _rolesPermissionsRepository = rolesPermissionsRepository;
        }
        public async Task<List<DtoGetRolePermissionsList>> Handle(GetRolePermissionsForAdminQuery request, CancellationToken cancellationToken)
        {
            var result = await _rolesPermissionsRepository.getRolePermissionsForAdmin(request.roleId);
            return result;
        }
    }
}
