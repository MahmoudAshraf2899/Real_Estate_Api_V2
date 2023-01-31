using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using RealEstateApi.Commands.PaymentTypes;

namespace RealEstateApi.Handler.PaymentTypes
{
    public class AddPaymentTypeHandler : IRequestHandler<PaymentTypeAddCommand, PaymentType>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        public AddPaymentTypeHandler(IPaymentTypeRepository paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }
        public async Task<PaymentType> Handle(PaymentTypeAddCommand request, CancellationToken cancellationToken)
        {
            PaymentType newPaymentType = new PaymentType();
            newPaymentType.ArType = request.arType;
            newPaymentType.EnType = request.enType;
            await _paymentTypeRepository.AddAsync(newPaymentType);
            await _paymentTypeRepository.SaveAsync();
            return newPaymentType;
        }
    }
}
