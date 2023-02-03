using MediatR;

namespace RealEstateApi.Commands.Visitors
{
    public class VisitorBlockCommand : IRequest<VisitorBlockCommand>
    {
        public int id { get;}
        public int? deletedBy { get;}
        public VisitorBlockCommand(int Id,int? DeletedBy)
        {
            id = Id;
            deletedBy = DeletedBy;
        }
    }
}
