using MediatR;
using Nabta.Application;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Authentication.Register
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, AuthModel>
    {
        private readonly IAuthService _authService;

        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(request.dto);
        }
    }
}
