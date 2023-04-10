using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Reservation;

namespace RealEstateApi.Handler.Reservation
{
    public class GetAllReservationsHandler : IRequestHandler<GetAllReservationsQuery, List<DtoGetAllReservationsList>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetAllReservationsHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public async Task<List<DtoGetAllReservationsList>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _reservationRepository.GetAllReservations(request.pageNumber, request.pageSize, request.lang);
            return result;
        }
    }
}
