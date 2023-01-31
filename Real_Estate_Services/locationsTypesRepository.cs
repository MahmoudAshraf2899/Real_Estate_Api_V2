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
    public class locationsTypesRepository : Repository<ecommerce_real_estateContext, LocationsType>, IlocationsTypesRepository
    {
        public async Task<List<LocationsTypesGetAllDto>> GetAllTypes(int pageNumber, int pageSize, string lang)
        {
            var list = new List<LocationsTypesGetAllDto>();
            if (lang == "en")
            {
                list = await (from q in Context.LocationsTypes.AsNoTracking()
                              select new LocationsTypesGetAllDto
                              {
                                  id = q.Id,
                                  enName = q.EnType,

                              }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                list = await (from q in Context.LocationsTypes.AsNoTracking()
                              select new LocationsTypesGetAllDto
                              {
                                  id = q.Id,
                                  arName = q.ArType,

                              }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }

            return list;
        }

        public async Task<List<locationTypesDropDownDto>> getDropDown(string lang)
        {
            var list = new List<locationTypesDropDownDto>();
            if (lang == "en")
            {
                list = await (from q in Context.LocationsTypes.AsNoTracking()
                              select new locationTypesDropDownDto
                              {
                                  id = q.Id,
                                  enType = q.EnType
                              }).ToListAsync();
            }
            else
            {
                list = await (from q in Context.LocationsTypes.AsNoTracking()
                              select new locationTypesDropDownDto
                              {
                                  id = q.Id,
                                  arType = q.ArType
                              }).ToListAsync();
            }
            return list;
        }
    }
}
