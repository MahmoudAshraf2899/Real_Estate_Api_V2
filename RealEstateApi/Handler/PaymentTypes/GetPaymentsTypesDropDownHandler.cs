using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.PaymentTypes;

namespace RealEstateApi.Handler.PaymentTypes
{
    public class GetPaymentsTypesDropDownHandler : IRequestHandler<GetPaymentsTypesDropDownQuery, List<paymentTypeDropDownDto>>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        public GetPaymentsTypesDropDownHandler(IPaymentTypeRepository paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }
        public async Task<List<paymentTypeDropDownDto>> Handle(GetPaymentsTypesDropDownQuery request, CancellationToken cancellationToken)
        {
            return await _paymentTypeRepository.getForDrop(request.lang);             
        }
    }
}
