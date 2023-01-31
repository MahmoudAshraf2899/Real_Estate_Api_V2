using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.PaymentTypes
{
    public class GetAllPaymentsTypesQuery : IRequest<List<paymentTypeGetAllDto>>
    {
        public int pageNumber { get; }
        public int pageSize { get; }
        public string lang { get; }
        public GetAllPaymentsTypesQuery(int PageNumber, int PageSize, string Lang)
        {
            pageNumber = PageNumber;
            pageSize = PageSize;
            lang = Lang;
        }
    }
}
