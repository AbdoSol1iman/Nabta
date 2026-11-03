using MediatR;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ConfirmEmail
{
    public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IAuthService _authenticationModule;   

        public ConfirmEmailHandler(IAuthService authenticationModule)
        {
            _authenticationModule = authenticationModule;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationModule.ConfirmEmailAsync(request.userId, request.token);    
        }
    }
}