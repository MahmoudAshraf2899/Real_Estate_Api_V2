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
    public class CustomerServicesRepository : Repository<ecommerce_real_estateContext, CustomerService>, ICustomerServicesRepository
    {
        public async Task<List<CustomerServicesTableDto>> getAll(int pageNumber, int pageSize, string lang)
        {
            var result = new List<CustomerServicesTableDto>();
            if (lang == "en")
            {
                result = await (from q in Context.CustomerServices.AsNoTracking()
                                    .Where(c => c.IsDeleted != true)
                                select new CustomerServicesTableDto
                                {
                                    id = q.Id,
                                    address = q.Address,
                                    contactNameEn = q.ContactNameEn,
                                    email = q.Email,
                                    isInsured = q.Insured,
                                    phone = q.Phone,
                                    salary = q.Salary,
                                    userName = q.UserName
                                }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                result = await (from q in Context.CustomerServices.AsNoTracking()
                                        .Where(c => c.IsDeleted != true)
                                select new CustomerServicesTableDto
                                {
                                    id = q.Id,
                                    address = q.Address,
                                    contactNameAr = q.ContactNameAr,
                                    email = q.Email,
                                    isInsured = q.Insured,
                                    phone = q.Phone,
                                    salary = q.Salary,
                                    userName = q.UserName
                                }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            return result;
        }

        public async Task<CustomerServicesByIdDto> getCustomerServicesById(int id, string lang)
        {
            var result = new CustomerServicesByIdDto();
            if (lang == "en")
            {
                result = await (from q in Context.CustomerServices.AsNoTracking()
                               .Where(c => c.Id == id && c.IsDeleted != true)
                                let createdBy = q.CreatedByNavigation.UserName
                                select new CustomerServicesByIdDto
                                {
                                    id = q.Id,
                                    address = q.Address,                                    
                                    contactNameEn = q.ContactNameEn,
                                    email = q.Email,
                                    isInsured = q.Insured,
                                    phone = q.Phone,
                                    salary = q.Salary,
                                    userName = q.Password,
                                    createdById = q.CreatedBy,
                                    createdByName = createdBy,

                                }).FirstOrDefaultAsync();
            }
            else
            {
                result = await (from q in Context.CustomerServices.AsNoTracking()
                                   .Where(c => c.Id == id && c.IsDeleted != true)
                                let createdBy = q.CreatedByNavigation.UserName
                                select new CustomerServicesByIdDto
                                {
                                    id = q.Id,
                                    address = q.Address,
                                    contactNameAr = q.ContactNameAr,                                   
                                    email = q.Email,
                                    isInsured = q.Insured,
                                    phone = q.Phone,
                                    salary = q.Salary,
                                    userName = q.Password,
                                    createdById = q.CreatedBy,
                                    createdByName = createdBy,
                                }).FirstOrDefaultAsync();
            }
            return result;
        }


    }
}
