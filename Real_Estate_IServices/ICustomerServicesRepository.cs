using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface ICustomerServicesRepository : IRepository<CustomerService>
    {
        Task<CustomerServicesByIdDto> getCustomerServicesById(int id, string lang);
        Task<List<CustomerServicesTableDto>> getAll(int pageNumber, int pageSize, string lang);

    }
}
