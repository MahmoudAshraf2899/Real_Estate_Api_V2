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
    public class VisitorRepository : Repository<ecommerce_real_estateContext, Visitor>, IVisitorRepository
    {
        public async Task<List<VisitorsGetAllDto>> getAllVisitors(int pageNumber, int pageSize, string lang)
        {
            var list = new List<VisitorsGetAllDto>();
            if (lang == "en")
            {
                list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.IsActive != false && c.IsDeleted != true)
                              select new VisitorsGetAllDto
                              {
                                  id = q.Id,
                                  contactNameEn = q.ContactNameEn,
                                  email = q.Email,
                                  mobile = q.Mobile,
                                  userName = q.UserName
                              }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.IsActive != false && c.IsDeleted != true)
                              select new VisitorsGetAllDto
                              {
                                  id = q.Id,
                                  contactNameAr = q.ContactNameAr,
                                  email = q.Email,
                                  mobile = q.Mobile,
                                  userName = q.UserName
                              }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            return list;
        }

        public async Task<VisitorsGetByIdDto> getvisitorById(int id, string lang)
        {
            var list = new VisitorsGetByIdDto();
            if (lang == "en")
            {
                list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.Id == id && c.IsActive == true)
                              let creatorName = q.CreatedBy != null ? q.CreatedByNavigation.ContactNameEn : "Normal Visitor"
                              select new VisitorsGetByIdDto
                              {
                                  id = q.Id,
                                  contactNameEn = q.ContactNameEn,
                                  createdAt = q.CreatedAt,
                                  email = q.Email,
                                  mobile = q.Mobile,
                                  SecMobile = q.SecMobile,
                                  userName = q.UserName,
                                  creatorName = creatorName,
                              }).FirstOrDefaultAsync();
            }
            else
            {
                list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.Id == id && c.IsActive == true)
                              let creatorName = q.CreatedBy != null ? q.CreatedByNavigation.ContactNameAr : "Normal Visitor"
                              select new VisitorsGetByIdDto
                              {
                                  id = q.Id,
                                  contactNameAr = q.ContactNameAr,
                                  createdAt = q.CreatedAt,
                                  email = q.Email,
                                  mobile = q.Mobile,
                                  SecMobile = q.SecMobile,
                                  userName = q.UserName,
                                  creatorName = creatorName,

                              }).FirstOrDefaultAsync();
            }
            return list;
        }
    }
}
