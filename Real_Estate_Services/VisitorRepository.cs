using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Real_Estate_Services.VisitorRepository;

namespace Real_Estate_Services
{
    public class VisitorRepository : Repository<ecommerce_real_estateContext, Visitor>, IVisitorRepository
    {
        //private Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(string propertyName, string propertyValue)
        //{
        //    //Todo :Add Check of the incoming value
        //    //if(propertyName.GetType() == typeof(string))
        //    //{
        //    //    if (!string.IsNullOrEmpty(propertyName.ToString()) && !string.IsNullOrEmpty(propertyValue))
        //    //    {
        //    //        ParameterExpression parameter = Expression.Parameter(typeof(Visitor), "p");
        //    //        Expression property = Expression.Property(parameter, propertyName.ToString());
        //    //        Expression value = Expression.Constant(propertyValue);
        //    //        Expression condition = Expression.Equal(property, value);
        //    //        Expression<Func<Visitor, bool>> lambda = Expression.Lambda<Func<Visitor, bool>>(condition, parameter);

        //    //    }
        //    // }


        //    // Get the property info for the specified property name
        //    var propertyInfo = typeof(TEntity).GetProperty(propertyName);

        //    // Create a parameter expression for the entity type
        //    var parameterExpression = Expression.Parameter(typeof(TEntity), "x");

        //    // Create a member expression for the property
        //    var propertyExpression = Expression.Property(parameterExpression, propertyInfo);

        //    // Create a constant expression for the property value
        //    var constantExpression = Expression.Constant(propertyValue);

        //    // Create a method call expression for the Contains method of the string class
        //    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //    var methodCallExpression = Expression.Call(propertyExpression, containsMethod, constantExpression);

        //    // Create a lambda expression that takes an object of type TEntity and returns a boolean
        //    var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(methodCallExpression, parameterExpression);

        //    return lambdaExpression;




        //    //if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(propertyValue))
        //    //{
        //    //    ParameterExpression parameter = Expression.Parameter(typeof(Visitor), "p");
        //    //    Expression property = Expression.Property(parameter, propertyName);
        //    //    Expression value = Expression.Constant(propertyValue);
        //    //    Expression condition = Expression.Equal(property, value);
        //    //    Expression<Func<Visitor, bool>> lambda = Expression.Lambda<Func<Visitor, bool>>(condition, parameter);                
        //    //    return lambda;
        //    //}

        //    // Apply the second filter condition if provided
        //    //if (!string.IsNullOrEmpty(filterBy2) && !string.IsNullOrEmpty(filterValue2))
        //    //{
        //    //    ParameterExpression parameter = Expression.Parameter(typeof(Visitor), "p");
        //    //    Expression property = Expression.Property(parameter, filterBy2);
        //    //    Expression value = Expression.Constant(filterValue2);
        //    //    Expression condition = Expression.Equal(property, value);
        //    //    Expression<Func<Visitor, bool>> lambda = Expression.Lambda<Func<Visitor, bool>>(condition, parameter);
        //    //    return lambda;
        //    //}
        //    return null;
        //}

        public static Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(Dictionary<string, string> filters)
        {
            // Create a parameter expression for the entity type
            var parameterExpression = Expression.Parameter(typeof(TEntity), "x");

            // Create an initial expression that is null
            Expression trueExpression = null;

            // Loop through each filter and create a filter expression for each one
            foreach (var filter in filters)
            {
                // Get the property info for the specified property name
                var propertyInfo = typeof(TEntity).GetProperty(filter.Key);

                // Create a member expression for the property
                var propertyExpression = Expression.Property(parameterExpression, propertyInfo);

                // Create a constant expression for the property value
                var constantExpression = Expression.Constant(filter.Value);

                // Create a method call expression for the Contains method of the string class
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var methodCallExpression = Expression.Call(propertyExpression, containsMethod, constantExpression);

                // Combine the filter expression with the previous filters using the AndAlso operator
                if (trueExpression == null)
                {
                    trueExpression = methodCallExpression;
                }
                else
                {
                    trueExpression = Expression.AndAlso(trueExpression, methodCallExpression);
                }
            }

            // Create a lambda expression that takes an object of type TEntity and returns a boolean
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(trueExpression, parameterExpression);

            return lambdaExpression;
        }
        public async Task<List<VisitorsEmailsDto>> getAllActiveVisitorsEmails()
        {
            var list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.IsActive != false)
                              select new VisitorsEmailsDto
                              {
                                  email = q.Email
                              }).ToListAsync();
            return list;
        }

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
                list = await (from q in Context.Visitors.AsNoTracking().Where(c => c.Id == id && c.IsActive != false)
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


        //public Expression<Func<Visitor, bool>> GetFilteredAndSortedPeople(
        //                            string filterBy1,
        //                            string filterValue1,
        //                            string filterBy2,
        //                            string filterValue2)
        //{
        //    // Create a base query that includes all visitor
        //    //IQueryable<Visitor> query = visitor.AsQueryable();

        //    // Apply the first filter condition if provided
        //    if (!string.IsNullOrEmpty(filterBy1) && !string.IsNullOrEmpty(filterValue1))
        //    {
        //        ParameterExpression parameter = Expression.Parameter(typeof(Visitor), "p");
        //        Expression property = Expression.Property(parameter, filterBy1);
        //        Expression value = Expression.Constant(filterValue1);
        //        Expression condition = Expression.Equal(property, value);
        //        Expression<Func<Visitor, bool>> lambda = Expression.Lambda<Func<Visitor, bool>>(condition, parameter);
        //        Expression<Func<Visitor, bool>> x = lambda;
        //        return lambda;
        //    }

        //    // Apply the second filter condition if provided
        //    if (!string.IsNullOrEmpty(filterBy2) && !string.IsNullOrEmpty(filterValue2))
        //    {
        //        ParameterExpression parameter = Expression.Parameter(typeof(Visitor), "p");
        //        Expression property = Expression.Property(parameter, filterBy2);
        //        Expression value = Expression.Constant(filterValue2);
        //        Expression condition = Expression.Equal(property, value);
        //        Expression<Func<Visitor, bool>> lambda = Expression.Lambda<Func<Visitor, bool>>(condition, parameter);
        //        return lambda;
        //    }
        //    return null;

        //}
        //)
        public async Task<List<VisitorsGetAllDto>> getFilteredVisitors()
        {
            var filters = new Dictionary<string, string>
                {
                    { "UserName", "a" },
                    { "Password", "1" }
                };
            var newExpression = CreateFilterExpression<Visitor>(filters);
            

            var result = await (from q in Context.Visitors.AsNoTracking()
                          .Where(newExpression)
                                select new VisitorsGetAllDto
                                {
                                    contactNameEn = q.ContactNameEn,
                                    contactNameAr = q.ContactNameAr,
                                    email = q.Email,
                                    id = q.Id,
                                    mobile = q.Mobile,
                                    userName = q.UserName

                                }).ToListAsync();
            return result;
        }

    }
}
