using MediatR;
using Real_Estate_Context.Models;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Commands.Visitors;

namespace RealEstateApi.Handler.Visitors
{
    public class AddVisitorHandler : IRequestHandler<VisitorAddCommand, VisitorAddCommand>
    {
        private readonly IVisitorRepository _visitorRepository;

        public AddVisitorHandler(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }

        public async Task<VisitorAddCommand> Handle(VisitorAddCommand request, CancellationToken cancellationToken)
        {
            Visitor newVisitor = new Visitor();
            newVisitor.IsActive = true;
            newVisitor.CreatedBy = request.createdBy;
            newVisitor.Email = request.email;
            newVisitor.ContactNameEn =request.enContactName;
            newVisitor.ContactNameAr = request.arContactName;
            newVisitor.UserName = request.userName;
            newVisitor.GroupPermission = 3;
            newVisitor.CreatedAt = DateTime.Now.Date;
            newVisitor.Mobile = request.mobile;
            newVisitor.SecMobile = request.secMobile;
            newVisitor.Password = "12345";

            await _visitorRepository.AddAsync(newVisitor);
            await _visitorRepository.SaveAsync();
            return request;
        }
    }
}
