using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Commands.Visitors
{
    public class SendMailToVisitorsCommand : IRequest<List<VisitorsEmailsDto>>
    {
        public string? body { get; set; }
        public string? subject { get; set; }
    }
     
     
}
