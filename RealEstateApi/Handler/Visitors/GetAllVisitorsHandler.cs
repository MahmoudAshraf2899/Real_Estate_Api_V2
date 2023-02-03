using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Visitors;

namespace RealEstateApi.Handler.Visitors
{
    public class GetAllVisitorsHandler : IRequestHandler<GetAllVisitorsQuery, List<VisitorsGetAllDto>>
    {
        private readonly IVisitorRepository _visitorRepository;

        public GetAllVisitorsHandler(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }
        public async Task<List<VisitorsGetAllDto>> Handle(GetAllVisitorsQuery request, CancellationToken cancellationToken)
        {
            var result = await _visitorRepository.getAllVisitors(request.pageNumber, request.pageSize, request.lang);
            return result;
        }
    }
}

