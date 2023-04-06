using MediatR;

namespace RealEstateApi.Commands.Login
{
    public record AdminLoginCommand(string userName,string password) : IRequest<object>;

}
