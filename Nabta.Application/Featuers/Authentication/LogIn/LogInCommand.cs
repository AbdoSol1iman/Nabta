using MediatR;
using Nabta.Application.Dtos.Auth;

namespace Nabta.Application.Featuers.Authentication.LogIn
{
    public record LogInCommand(LoginDto dto) : IRequest<AuthModel>;
}
