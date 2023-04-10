using MediatR;
using Real_Estate_IServices;
using RealEstateApi.Commands.Reservation;

namespace RealEstateApi.Handler.Reservation
{
    public class AddNewReservationHandler : IRequestHandler<AddNewReservationCommand, Real_Estate_Context.Models.Reservation>
    {
        private readonly IReservationRepository _reservationRepository;

        public AddNewReservationHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public async Task<Real_Estate_Context.Models.Reservation> Handle(AddNewReservationCommand request, CancellationToken cancellationToken)
        {
            Real_Estate_Context.Models.Reservation reservation = new Real_Estate_Context.Models.Reservation();
            reservation.CreatedBy = request.accountId;
            reservation.CustomerId = request.customerId;
            reservation.LocationId = request.locationId;
            reservation.Subject = request.subject;
            reservation.Date = DateTime.Now.Date;
            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveAsync();
            
            return reservation;

        }
    }
}
