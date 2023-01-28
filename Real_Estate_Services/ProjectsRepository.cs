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
    public class ProjectsRepository : Repository<ecommerce_real_estateContext, Project>, IProjectsRepository
    {
        public async Task<List<ProjectsTableDto>> getAll(int pageNumber, int pageSize, string lang)
        {
            var result = new List<ProjectsTableDto>();
            if (lang == "en")
            {
                result = await (from q in Context.Projects.AsNoTracking()
                                                           .Where(c => c.IsActive != false && c.DeletedBy == null)
                                let editorName = q.EditedBy != null ? q.EditedByNavigation.UserName : null
                                select new ProjectsTableDto
                                {
                                    id = q.Id,
                                    adress = q.Address,
                                    description = q.Description,
                                    nameEn = q.NameEn,
                                    noUnits = q.NoUnits ?? 0,
                                    editedBy = q.EditedBy,
                                    editorName = editorName
                                }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            else
            {
                result = await (from q in Context.Projects.AsNoTracking()
                                    .Where(c => c.IsActive != false && c.DeletedBy == null)
                                let editorName = q.EditedBy != null ? q.EditedByNavigation.UserName : null
                                select new ProjectsTableDto
                                {
                                    id = q.Id,
                                    adress = q.Address,
                                    description = q.Description,
                                    nameAr = q.NameAr,
                                    noUnits = q.NoUnits ?? 0,
                                    editedBy = q.EditedBy,
                                    editorName = editorName
                                }).OrderByDescending(c => c.id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            }
            return result;
        }

        public async Task<ProjectGyIdDto> getById(int id, string lang)
        {
            var result = new ProjectGyIdDto();
            if (lang == "en")
            {
                result = await (from q in Context.Projects.AsNoTracking().Where(c => c.Id == id)
                                let creatorName = q.AddedByNavigation.UserName
                                let editorName = q.EditedBy != null ? q.EditedByNavigation.UserName : null
                                let projectFeatures = q.ProjectsFeatures.Select(c => c.Feature).ToList()

                                select new ProjectGyIdDto
                                {
                                    id = q.Id,
                                    adress = q.Address,
                                    createdAt = q.CreatedAt,
                                    createdBy = q.AddedBy,
                                    creatorName = creatorName,
                                    description = q.Description,
                                    editedBy = q.EditedBy,
                                    editorName = editorName,
                                    nameEn = q.NameEn,
                                    noUnits = q.NoUnits ?? 0,
                                    features = projectFeatures

                                }).FirstOrDefaultAsync();
            }
            else
            {
                result = await (from q in Context.Projects.AsNoTracking().Where(c => c.Id == id)
                                let creatorName = q.AddedByNavigation.UserName
                                let editorName = q.EditedBy != null ? q.EditedByNavigation.UserName : null
                                let projectFeatures = q.ProjectsFeatures.Select(c => c.Feature).ToList()

                                select new ProjectGyIdDto
                                {
                                    id = q.Id,
                                    adress = q.Address,
                                    createdAt = q.CreatedAt,
                                    createdBy = q.AddedBy,
                                    creatorName = creatorName,
                                    description = q.Description,
                                    editedBy = q.EditedBy,
                                    editorName = editorName,
                                    nameAr = q.NameAr,
                                    noUnits = q.NoUnits ?? 0,
                                    features = projectFeatures

                                }).FirstOrDefaultAsync();
            }
            return result;

        }
    }
}
