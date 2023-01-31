using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.PaymentTypes;

namespace RealEstateApi.Handler.PaymentTypes
{
    public class GetAllPaymentTypesHandler : IRequestHandler<GetAllPaymentsTypesQuery, List<paymentTypeGetAllDto>>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;

        public GetAllPaymentTypesHandler(IPaymentTypeRepository paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }
        public async Task<List<paymentTypeGetAllDto>> Handle(GetAllPaymentsTypesQuery request, CancellationToken cancellationToken)
        {
            return await _paymentTypeRepository.getAllPaymentTypes(request.pageNumber, request.pageSize, request.lang);
            
        }
    }
}
