using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.CustomerServices;

namespace RealEstateApi.Handler.CustomerServices
{
    public class GetCustomerServicesByIdHandler : IRequestHandler<GetCustomerServicesByIdQuery, CustomerServicesByIdDto>
    {
        private readonly ICustomerServicesRepository _customerServicesRepository;

        public GetCustomerServicesByIdHandler(ICustomerServicesRepository customerServicesRepository)
        {
            _customerServicesRepository = customerServicesRepository;
        }
        public async Task<CustomerServicesByIdDto> Handle(GetCustomerServicesByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _customerServicesRepository.getCustomerServicesById(request.id, request.lang);
            return result;
        }
    }
}
