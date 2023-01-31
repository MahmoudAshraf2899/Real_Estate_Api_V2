using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.PaymentTypes
{
    public class PaymentTypeAddCommand : IRequest<PaymentType>
    {
        public string arType { get; set; }
        public string enType { get; set; }
    }
}
