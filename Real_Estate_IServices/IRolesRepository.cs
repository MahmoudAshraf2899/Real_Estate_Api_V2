using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;

namespace Real_Estate_IServices
{
    public interface IRolesRepository : IRepository<Role>
    {
        Task<List<DtoRolesDropDown>> getDropDownListForAdmin();
    }
}
