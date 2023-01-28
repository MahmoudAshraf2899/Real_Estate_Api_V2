using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.CustomerServices
{
    public class CustomerServiceDeleteCommand : IRequest<CustomerService>
    {
        public int id { get;  }
        public int accountId { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="AccountId"></param>
        public CustomerServiceDeleteCommand(int Id,int AccountId)
        {
            id = Id;
            accountId = AccountId;
        }
    }
}
