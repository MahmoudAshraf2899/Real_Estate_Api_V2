using MediatR;
using Real_Estate_IServices;
using RealEstateApi.Commands.Visitors;

namespace RealEstateApi.Handler.Visitors
{
    public class BlockVisitorHandler : IRequestHandler<VisitorBlockCommand, VisitorBlockCommand>
    {
        private readonly IVisitorRepository _visitorRepository;

        public BlockVisitorHandler(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }
        public async Task<VisitorBlockCommand> Handle(VisitorBlockCommand request, CancellationToken cancellationToken)
        {
            var _obj = _visitorRepository.FindBy(c => c.Id == request.id).FirstOrDefault();
            if (_obj is not null)
            {
                _obj.DeletedBy = request.deletedBy;
                _obj.IsDeleted = true;
                _obj.IsActive = false;

                await _visitorRepository.UpdateAsync(_obj);
                await _visitorRepository.SaveAsync();
                return request;

            }
            else
            {
                throw new Exception();
            }
        }
    }
}
