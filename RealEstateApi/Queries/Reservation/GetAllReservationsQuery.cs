using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.Reservation
{
    public class GetAllReservationsQuery : IRequest<List<DtoGetAllReservationsList>>
    {
        public GetAllReservationsQuery(int PageNumber, int PageSize, string Lang)
        {
            this.pageNumber = PageNumber;
            this.pageSize = PageSize;
            this.lang = Lang;
        }

        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string lang { get; set; }
    }
}
