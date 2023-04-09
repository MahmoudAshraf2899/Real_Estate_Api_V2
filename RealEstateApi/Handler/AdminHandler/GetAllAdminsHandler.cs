using MediatR;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using RealEstateApi.Queries.Admin;

namespace RealEstateApi.Handler.Admin
{
    public class GetAllAdminsHandler : IRequestHandler<GetAllAdminsQuery, List<AdminsTableDto>>
    {
        private readonly IAdminRepository _adminRepository;

        public GetAllAdminsHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<List<AdminsTableDto>> Handle(GetAllAdminsQuery request, CancellationToken cancellationToken)
        {
            var result = await _adminRepository.getAllAdmins(request.pageNumber, request.pageSize, request.lang);
            return result;
        }
    }
}
