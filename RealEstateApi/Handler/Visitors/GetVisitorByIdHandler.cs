using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Visitors;

namespace RealEstateApi.Handler.Visitors
{
    public class GetVisitorByIdHandler : IRequestHandler<GetVisitorByIdQuery, VisitorsGetByIdDto>
    {
        private readonly IVisitorRepository _visitorRepository;

        public GetVisitorByIdHandler(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }
        public async Task<VisitorsGetByIdDto> Handle(GetVisitorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _visitorRepository.getvisitorById(request.id, request.lang);
        }
    }
}
