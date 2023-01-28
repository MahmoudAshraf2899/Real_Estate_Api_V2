using Real_Estate_Context.Context;
using Real_Estate_Context.Models;
using Real_Estate_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Services
{
    public class ProjectFeatureRepository : Repository<ecommerce_real_estateContext, ProjectsFeature>, IProjectFeatureRepository
    {
    }
}
