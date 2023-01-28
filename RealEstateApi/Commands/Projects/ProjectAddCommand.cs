using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Projects
{
    public class ProjectAddCommand : IRequest<Project>
    {
        public string nameAr { get; set;}
        public string nameEn { get; set;}
        public string description { get; set; }
        public string address { get; set; }
        public int accountId { get; set; }
        public List<string>? features { get; set; }
    }
}
