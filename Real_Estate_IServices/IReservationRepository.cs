using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;

namespace Real_Estate_IServices
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<List<DtoGetAllReservationsList>> GetAllReservations(int pageNumber, int pageSize , string lang);
    }
}
