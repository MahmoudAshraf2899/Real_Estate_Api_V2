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
    public class locationsRepository : Repository<ecommerce_real_estateContext, Location>, IlocationsRepository
    {

        public async Task<List<LocationsGetAllDtoEncapsulationTest>> getAllLocationsTest()
        {
            var list = new List<LocationsGetAllDtoEncapsulationTest>();
             
                list = await(from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.DeletedyBy == null)
                             let addedBy = q.AddedByNavigation.ContactNameEn
                             let projectName = q.Project.NameEn
                             select new LocationsGetAllDtoEncapsulationTest(q.Price)
                             {
                                  Price = q.Price
                             }).ToListAsync();
            return list;
            
        }
        public async Task<List<LocationsGetAllDto>> getAllLocations(int pageNumber, int pageSize, string lang)
        {
            var list = new List<LocationsGetAllDto>();
            if (lang == "en")
            {
                list = await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.DeletedyBy == null)
                              let addedBy = q.AddedByNavigation.ContactNameEn
                              let projectName = q.Project.NameEn
                              select new LocationsGetAllDto
                              {
                                  addedByName = addedBy,
                                  area = q.Area,
                                  id = q.Id,
                                  isAvailable = q.IsAvailable,
                                  locationNameEn = q.LocationNameEn,
                                  LocationNameAr = q.LocationNameAr,
                                  Price = q.Price,
                                  projectName = projectName
                              }).OrderByDescending(c => c.id).Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();
            }
            else
            {
                list = await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.DeletedyBy == null)
                              let addedBy = q.AddedByNavigation.ContactNameAr
                              let projectName = q.Project.NameAr
                              select new LocationsGetAllDto
                              {
                                  addedByName = addedBy,
                                  area = q.Area,
                                  id = q.Id,
                                  isAvailable = q.IsAvailable,
                                  LocationNameAr = q.LocationNameAr,
                                  Price = q.Price,
                                  projectName = projectName
                              }).OrderByDescending(c => c.id).Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();
            }
            return list;
        }

        

        public async Task<LocationByIdDto> getById(int id, string lang)
        {
            var result = new LocationByIdDto();
            if (lang == "en")
            {
                result = await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.Id == id && c.DeletedyBy == null)
                          let addedBy = q.AddedByNavigation.ContactNameEn
                          let typeOfLocation = q.LocationType.EnType
                          let typeOfPayment = q.PaymentType.EnType
                          let projectName = q.Project.NameEn
                          select new LocationByIdDto
                          {
                              addedByName = addedBy,
                              area = q.Area,
                              description = q.Description,
                              garageValue = q.WithGarage == true ? q.GarageValue : 0,
                              id = q.Id,
                              isAvailable = q.IsAvailable,
                              locationNameEn = q.LocationNameEn,
                              locationType = typeOfLocation,
                              meterPrice = q.MeterPrice,
                              noBathRooms = q.NoBathRooms,
                              noRooms = q.NoRooms,
                              paymentType = typeOfPayment,
                              price = q.Price,
                              projectName = projectName,
                              withGarage = q.WithGarage,
                              yearBuilt = q.YearBuilt

                          }).FirstOrDefaultAsync();
            }
            else
            {
                result =await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.Id == id && c.DeletedyBy == null)
                          let addedBy = q.AddedByNavigation.ContactNameAr
                          let typeOfLocation = q.LocationType.ArType
                          let typeOfPayment = q.PaymentType.ArType
                          let projectName = q.Project.NameAr
                          select new LocationByIdDto
                          {
                              addedByName = addedBy,
                              area = q.Area,
                              description = q.Description,
                              garageValue = q.WithGarage == true ? q.GarageValue : 0,
                              id = q.Id,
                              isAvailable = q.IsAvailable,
                              locationNameAr = q.LocationNameAr,
                              locationType = typeOfLocation,
                              meterPrice = q.MeterPrice,
                              noBathRooms = q.NoBathRooms,
                              noRooms = q.NoRooms,
                              paymentType = typeOfPayment,
                              price = q.Price,
                              projectName = projectName,
                              withGarage = q.WithGarage,
                              yearBuilt = q.YearBuilt

                          }).FirstOrDefaultAsync();
            }
            return result;
        }
    }
}
