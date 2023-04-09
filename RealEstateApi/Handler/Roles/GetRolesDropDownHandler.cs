using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Roles;

namespace RealEstateApi.Handler.Roles
{
    public class GetRolesDropDownHandler : IRequestHandler<GetRolesForDropDownForAdminQuery, List<DtoRolesDropDown>>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetRolesDropDownHandler(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public async Task<List<DtoRolesDropDown>> Handle(GetRolesForDropDownForAdminQuery request, CancellationToken cancellationToken)
        {
            var result = await _rolesRepository.getDropDownListForAdmin();
            return result;
        }
    }
}
