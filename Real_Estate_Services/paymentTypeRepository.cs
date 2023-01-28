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
    public class paymentTypeRepository : Repository<ecommerce_real_estateContext, PaymentType>, IPaymentTypeRepository
    {
        public async Task<List<paymentTypeGetAllDto>> getAllPaymentTypes(int pageNumber, int pageSize, string lang)
        {
            var list = new List<paymentTypeGetAllDto>();
            if(lang == "en")
            {
                list = await (from q in Context.PaymentTypes.AsNoTracking()
                        select new paymentTypeGetAllDto
                        {
                            id = q.Id,
                            enType = q.EnType,

                        }).OrderByDescending(c=>c.id).Skip(pageNumber*pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                list = await (from q in Context.PaymentTypes.AsNoTracking()
                              select new paymentTypeGetAllDto
                              {
                                  id = q.Id,
                                  enType = q.ArType,

                              }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            return list;
        }

        public async Task<List<paymentTypeDropDownDto>> getForDrop(string lang)
        {
            var list = new List<paymentTypeDropDownDto>();
            if (lang == "en")
            {
                list = await (from q in Context.PaymentTypes.AsNoTracking()
                              select new paymentTypeDropDownDto
                              {
                                  id=q.Id,
                                  enType=q.EnType,
                              }).ToListAsync(); 
            }
            else
            {
                list = await (from q in Context.PaymentTypes.AsNoTracking()
                              select new paymentTypeDropDownDto
                              {
                                  id = q.Id,
                                  arType = q.ArType,
                              }).ToListAsync();
            }
            return list;
             
        }
    }
}
