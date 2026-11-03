using MediatR;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Authentication.RevokeToken
{
    public sealed class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly IAuthService _authService;

        public RevokeTokenHandler(IAuthService authService)
        {
            _authService = authService;
        }

        async Task<bool> IRequestHandler<RevokeTokenCommand, bool>.Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RevokeToken(request.token);
        }
    }
}