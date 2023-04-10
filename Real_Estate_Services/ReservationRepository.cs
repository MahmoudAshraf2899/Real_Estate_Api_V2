using Microsoft.EntityFrameworkCore;
using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;


namespace Real_Estate_Services
{
    public class ReservationRepository : Repository<ecommerce_real_estateContext, Reservation>, IReservationRepository
    {
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
    }
}
