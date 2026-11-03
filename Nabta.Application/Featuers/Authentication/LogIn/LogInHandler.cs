using MediatR;
using Nabta.Application.Featuers.Authentication.LogIn;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers
{
    public sealed class LogInHandler : IRequestHandler<LogInCommand, AuthModel>
    {
        private readonly IAuthService _authService;

        public LogInHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthModel> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.dto);
        }
    }
}
