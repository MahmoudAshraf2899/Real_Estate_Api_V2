using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Roles
{
    public class GetRolesForDropDownForAdminQuery : IRequest<List<DtoRolesDropDown>>
    {
       
        public GetRolesForDropDownForAdminQuery()
        {
            
        }
    }
}
