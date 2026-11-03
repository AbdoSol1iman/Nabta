
using MediatR;
using Nabta.Application.Interfaces.Services;

namespace Nabta.Application.Featuers.Authentication.RefreshToken
{
    public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthModel>
    {
        private readonly IAuthService _authService;

        public RefreshTokenHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(request.token);
        }
    }
}
