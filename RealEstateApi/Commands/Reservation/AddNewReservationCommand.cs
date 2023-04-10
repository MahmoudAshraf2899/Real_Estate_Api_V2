using MediatR;

namespace RealEstateApi.Commands.Reservation
{
    public class AddNewReservationCommand : IRequest<Real_Estate_Context.Models.Reservation>
    {
        public int accountId { get; set; }
        public string subject { get; set; }
        public int? locationId { get; set; }
        public int? customerId { get; set; }

    }
}
