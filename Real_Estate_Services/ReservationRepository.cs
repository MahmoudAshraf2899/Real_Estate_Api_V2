 using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using System;
//using System.Data.Entity;


namespace Real_Estate_Services
{
    public class ReservationRepository : Repository<ecommerce_real_estateContext, Reservation>, IReservationRepository
    {
        private readonly ecommerce_real_estateContext _dbContext;

        public ReservationRepository(ecommerce_real_estateContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DtoGetAllReservationsList>> GetAllReservations(int pageNumber, int pageSize, string lang)
        {
            var result = await (from q in Context.Reservations.AsNoTracking()
                                select new DtoGetAllReservationsList
                                {
                                    Id = q.Id,
                                    creatorName = lang == "en" ? q.CreatedByNavigation.ContactNameEn : q.CreatedByNavigation.ContactNameAr,
                                    customerName = q.Customer.Name,
                                    locationName = lang == "en" ? q.Location.LocationNameEn : q.Location.LocationNameAr,
                                    subject = q.Subject,

                                }).OrderByDescending(c => c.Id).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
            return result;
        }
        public IEnumerable<DtoGetAllReservationsList> GetAllReservationsCompiledQuery(int pageNumber, int pageSize, string lang)
        {


            IEnumerable<DtoGetAllReservationsList> reservationsList = getAllReservationQuery(_dbContext, 0, lang, pageNumber, pageSize);

            return reservationsList;
        }

        //private static Func<ecommerce_real_estateContext, int, string, int, int, IEnumerable<DtoGetAllReservationsList>> getAllReservationQuery =
        //   EF.CompileQuery((ecommerce_real_estateContext context, int id, string lang, int pageNumber, int pageSize) =>
        //    from q in context.Reservations.AsNoTracking()
        //    select new DtoGetAllReservationsList
        //    {
        //        Id = q.Id,
        //        creatorName = lang == "en" ? q.CreatedByNavigation.ContactNameEn : q.CreatedByNavigation.ContactNameAr,
        //        customerName = q.Customer.Name,
        //        locationName = lang == "en" ? q.Location.LocationNameEn : q.Location.LocationNameAr,
        //        subject = q.Subject,

        //    }
        //   );

        //private static Func<ecommerce_real_estateContext, string, int, int, List<DtoGetAllReservationsList?>> getAllReservationQuery =
        //   EF.CompileQuery((ecommerce_real_estateContext context, string lang, int pageNumber, int pageSize) =>
        //    (from q in context.Reservations.AsNoTracking()
        //     select new DtoGetAllReservationsList
        //     {
        //         Id = q.Id,
        //         creatorName = lang == "en" ? q.CreatedByNavigation.ContactNameEn : q.CreatedByNavigation.ContactNameAr,
        //         customerName = q.Customer.Name,
        //         locationName = lang == "en" ? q.Location.LocationNameEn : q.Location.LocationNameAr,
        //         subject = q.Subject,
        //     }).AsEnumerable().Skip(pageNumber * pageSize).Take(pageSize).ToList()
        //   );

        private static readonly Func<ecommerce_real_estateContext, int, string, int, int, IEnumerable<DtoGetAllReservationsList>> getAllReservationQuery =
    EF.CompileQuery((ecommerce_real_estateContext context, int id, string lang, int pageNumber, int pageSize) =>
        (from q in context.Reservations.AsNoTracking()
         select new DtoGetAllReservationsList
         {
             Id = q.Id,
             creatorName = lang == "en" ? q.CreatedByNavigation.ContactNameEn : q.CreatedByNavigation.ContactNameAr,
             customerName = q.Customer.Name,
             locationName = lang == "en" ? q.Location.LocationNameEn : q.Location.LocationNameAr,
             subject = q.Subject
         })
        .Skip(pageNumber * pageSize)
        .Take(pageSize)
    );

    }
}
