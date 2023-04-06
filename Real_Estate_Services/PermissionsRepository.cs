using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_IServices;

namespace Real_Estate_Services
{
    public class PermissionsRepository : Repository<ecommerce_real_estateContext, Permission>, IPermissionsRepository
    {
    }
}
