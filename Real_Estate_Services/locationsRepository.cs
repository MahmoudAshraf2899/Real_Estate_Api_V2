using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
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



        //#region Old Code
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="worksheet"></param>
        ///// <param name="rowNumber"></param>
        ///// <param name="columnName"></param>
        ///// <returns></returns>
        //private object FindValueInColumn(IXLWorksheet worksheet, int rowNumber, string columnName)
        //{

        //    var column = worksheet.ColumnsUsed(c => c.Cell(1).GetString() == columnName).FirstOrDefault();

        //    if (column != null)
        //    {
        //        var row = column.Cells().FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.GetString()));
        //        if (row != null)
        //        {
        //            return row.Value;
        //        }
        //    }

        //    return null;
        //}
        ///// <summary>
        /////(J) Refer To Column Number
        /////(I) Refer To Row Number
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //private List<LocationAddDto> OldReadExcelData(IFormFile file)
        //{
        //    using (var stream = file.OpenReadStream())
        //    {
        //        var workbook = new XLWorkbook(stream);
        //        var worksheet = workbook.Worksheet(1);

        //        var rows = worksheet.RowsUsed();
        //        var columns = worksheet.ColumnsUsed().Count();

        //        var data = new List<LocationAddDto>();


        //        for (int i = 2; i <= rows.Count(); i++) // Start from row 2 to skip header row
        //        {
        //            var rowData = new LocationAddDto();
        //            for (int j = 1; j <= columns; j++)
        //            {
        //                string? locationNameEn = "";
        //                string locationNameAr = "";
        //                string? projectName = "";
        //                int? paymentType = 0;
        //                string? locationType = "";
        //                string? description = "";
        //                bool? isAvailable = false;
        //                var price = 0;
        //                var area = 0;
        //                var garageValue = 0;
        //                short? noRooms = 1;
        //                var meterPrice = 0;
        //                var yearBuilt = 0;
        //                short? NoBathRooms = 1;
        //                var column = worksheet.Cell(1, j).Value.ToString();//NO 1 Refer To Columns Header


        //                if (column == "LocationNameEn")
        //                {
        //                    locationNameEn = worksheet.Cell(i, j).Value.GetText();
        //                }
        //                else if (column == "LocationNameAr")
        //                {
        //                    locationNameAr = worksheet.Cell(i, j).Value.GetText();
        //                }
        //                else if (column == "Price")
        //                {
        //                    var x = worksheet.Cell(i, j).Value.GetNumber();
        //                    price = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "ProjectName")
        //                {
        //                    projectName = worksheet.Cell(i, j).Value.GetText();
        //                }
        //                else if (column == "Area")
        //                {
        //                    area = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "PaymentType")
        //                {
        //                    paymentType = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "LocationType")
        //                {
        //                    locationType = worksheet.Cell(i, j).Value.GetText();
        //                }
        //                else if (column == "GarageValue")
        //                {
        //                    garageValue = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "NoRooms")
        //                {

        //                    noRooms = short.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "NoBathRooms")
        //                {
        //                    NoBathRooms = short.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "Description")
        //                {
        //                    description = worksheet.Cell(i, j).Value.GetText();
        //                }
        //                else if (column == "IsAvailable")
        //                {
        //                    //var x = worksheet.Cell(i, j).Value.GetBoolean();
        //                    isAvailable = worksheet.Cell(i, j).Value.GetBoolean();
        //                }
        //                else if (column == "MeterPrice")
        //                {
        //                    meterPrice = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }
        //                else if (column == "YearBuilt")
        //                {
        //                    yearBuilt = int.Parse(worksheet.Cell(i, j).Value.GetNumber().ToString());
        //                }

        //                rowData.locationNameEn = locationNameEn;
        //                if (locationNameEn == "")
        //                {
        //                    var rowNumber = i;
        //                }
        //                rowData.locationNameAr = locationNameAr;
        //                rowData.price = price;
        //                rowData.garageValue = garageValue;
        //                rowData.withGarage = garageValue == 0 || garageValue == null ? false : true;
        //                rowData.isAvailable = isAvailable;
        //                rowData.area = area;
        //                rowData.description = description;
        //                rowData.meterPrice = meterPrice;
        //                rowData.noBathRooms = NoBathRooms;
        //                rowData.noRooms = noRooms;
        //                rowData.yearBuilt = yearBuilt;
        //                data.Add(rowData);

        //            }

        //        }
        //        insertDataIntoLocationsTable(data);
        //        return data;
        //    }
        //}
        //#endregion

        private void insertDataIntoLocationsTable(List<LocationAddDto> data, int accountId)
        {
            try
            {
                foreach (var item in data)
                {
                    Location newLocation = new Location();
                    var paymentTypeId = Context.PaymentTypes.Where(c => c.Code == item.paymentTypeId)
                                                                .Select(c => c.Id)
                                                                .FirstOrDefault();

                    var locationTypeId = Context.LocationsTypes.Where(c => c.Code == item.locationTypeId)
                                                                .Select(c => c.Id)
                                                                .FirstOrDefault();

                    var projectId = Context.Projects.Where(c => c.NameEn == item.projectName)
                                                     .Select(c => c.Id)
                                                     .FirstOrDefault();

                    newLocation.LocationNameAr = item.locationNameAr;
                    newLocation.LocationNameEn = item.locationNameEn;
                    newLocation.Price = item.price;
                    newLocation.GarageValue = item.garageValue;
                    newLocation.WithGarage = (item.garageValue == 0 || item.garageValue == null) ? false : true;
                    newLocation.IsAvailable = item.isAvailable;
                    newLocation.IsActive = true;
                    newLocation.CreatedAt = DateTime.Now.Date;
                    newLocation.AddedBy = accountId;
                    newLocation.Area = item.area;
                    newLocation.Description = item.description;
                    newLocation.MeterPrice = item.meterPrice;
                    newLocation.NoBathRooms = item.noBathRooms;
                    newLocation.NoRooms = item.noRooms;
                    newLocation.YearBuilt = item.yearBuilt;
                    newLocation.PaymentTypeId = (paymentTypeId == 0 || paymentTypeId == null) ? null : paymentTypeId;
                    newLocation.LocationTypeId = (locationTypeId == 0 || locationTypeId == null) ? null : locationTypeId;
                    newLocation.ProjectId = (projectId == 0 || projectId == null) ? null : projectId;


                    Context.Set<Location>().AddRange(newLocation);
                }
                Context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void ReadExcelData(IFormFile file, int accountId)
        {
            try
            {
                var list = new List<LocationAddDto>();
                if (file == null || file.Length == 0)
                {
                    throw new Exception();
                }

                var fileName = file.FileName;
                using (var stream = file.OpenReadStream())
                {
                    var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RowsUsed().Skip(1); // Skip header row
                    var columns = worksheet.ColumnsUsed().Count();
                    var headers = worksheet.RowsUsed().FirstOrDefault();
                    if (headers == null)
                    {
                        throw new Exception();
                    }
                    var columnNames = headers.Cells().Select(c => c.Value.ToString()).ToList();

                    string? locationNameEn = "";
                    string locationNameAr = "";
                    string? projectName = "";
                    int? paymentType = 0;
                    string? locationType = "";
                    string? description = "";
                    bool? isAvailable = false;
                    var price = 0;
                    var area = 0;
                    var garageValue = 0;
                    short? noRooms = 1;
                    var meterPrice = 0;
                    var yearBuilt = 0;
                    short? NoBathRooms = 1;
                    foreach (var row in rows)
                    {
                        var rowData = new LocationAddDto();
                        locationNameEn = row.Cell(1).Value.ToString();
                        locationNameAr = row.Cell(2).Value.ToString();
                        projectName = row.Cell(3).Value.ToString();
                        price = (int)row.Cell(4).Value.GetNumber();
                        area = (int)row.Cell(5).Value.GetNumber();
                        paymentType = (int)row.Cell(6).Value.GetNumber();
                        locationType = row.Cell(7).Value.ToString();
                        garageValue = (int)row.Cell(8).Value.GetNumber();
                        noRooms = (short)row.Cell(9).Value.GetNumber();
                        NoBathRooms = (short)row.Cell(10).Value.GetNumber();
                        description = row.Cell(11).Value.ToString();
                        isAvailable = row.Cell(12).Value.GetBoolean();
                        meterPrice = (int)row.Cell(13).Value.GetNumber();
                        yearBuilt = (int)row.Cell(14).Value.GetNumber();

                        #region Store List Of Objects To Use in Next Method
                        rowData.locationNameEn = locationNameEn;
                        rowData.locationNameAr = locationNameAr;
                        rowData.projectName = projectName;
                        rowData.price = price;
                        rowData.garageValue = garageValue;
                        rowData.withGarage = garageValue == 0 || garageValue == null ? false : true;
                        rowData.isAvailable = isAvailable;
                        rowData.area = area;
                        rowData.description = description;
                        rowData.meterPrice = meterPrice;
                        rowData.noBathRooms = NoBathRooms;
                        rowData.noRooms = noRooms;
                        rowData.yearBuilt = yearBuilt;
                        rowData.paymentTypeId = paymentType;

                        list.Add(rowData);
                        #endregion



                    }
                    insertDataIntoLocationsTable(list, accountId);
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<List<LocationsGetAllDtoEncapsulationTest>> getAllLocationsTest()
        {
            var list = new List<LocationsGetAllDtoEncapsulationTest>();

            list = await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.DeletedyBy == null)
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
                result = await (from q in Context.Locations.AsNoTracking().Where(c => c.IsActive != false && c.Id == id && c.DeletedyBy == null)
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
