using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.CustomerServices
{
    public class CustomerServicesAddCommand : IRequest<CustomerService>
    {
        
        public string userName { get; set; }
        public string contactNameEn { get; set; }
        public string contactNameAr { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public bool? isInsured { get; set; }
        public int? salary { get; set; }
        public int accountId { get; set; }       
    }
}
