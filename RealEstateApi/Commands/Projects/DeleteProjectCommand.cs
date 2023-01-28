using MediatR;
using Real_Estate_Context.Models;

namespace RealEstateApi.Commands.Projects
{
    public class DeleteProjectCommand : IRequest<Project>
    {
        public int id { get;}
        public int accountId { get;}
        public DeleteProjectCommand(int Id ,int AccountId)
        {
            id = Id;
            accountId = AccountId;
        }
    }
}
