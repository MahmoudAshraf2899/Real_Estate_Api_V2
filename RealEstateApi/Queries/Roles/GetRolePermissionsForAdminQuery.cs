using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Roles
{
    public class GetRolePermissionsForAdminQuery : IRequest<List<DtoGetRolePermissionsList>>
    {
        public int roleId { get; set; }
        public GetRolePermissionsForAdminQuery(int RoleId)
        {
            roleId = RoleId;
        }
    }
}
