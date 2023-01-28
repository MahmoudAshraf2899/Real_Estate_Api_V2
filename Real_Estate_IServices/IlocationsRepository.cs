using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface IlocationsRepository : IRepository<Location>
    {
        Task<List<LocationsGetAllDto>> getAllLocations(int pageNumber, int pageSize,string lang);
        Task<LocationByIdDto> getById(int id,string lang);
    }
}
