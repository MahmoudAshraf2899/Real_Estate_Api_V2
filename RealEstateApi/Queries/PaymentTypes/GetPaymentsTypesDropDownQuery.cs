using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries.PaymentTypes
{
    public class GetPaymentsTypesDropDownQuery : IRequest<List<paymentTypeDropDownDto>>
    {
        public string lang { get; set; }
        public GetPaymentsTypesDropDownQuery(string Lang)
        {
            lang = Lang;
        }
    }
}
