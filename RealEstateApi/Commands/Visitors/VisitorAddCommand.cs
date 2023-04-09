using MediatR;

namespace RealEstateApi.Commands.Visitors
{
    public class VisitorAddCommand :IRequest<VisitorAddCommand>
    {
        public string? userName { get; set; }
        public string? enContactName { get; set; }
        public string? arContactName { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
        public string? secMobile { get; set; }
        public int? createdBy { get; set; }
    }
}
