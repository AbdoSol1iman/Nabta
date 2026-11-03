using MediatR;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ResetPassword
{
    public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IAuthService _authenticationModule;
        public ResetPasswordHandler(IAuthService authenticationModule)
        {
            _authenticationModule = authenticationModule;
        }
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationModule.ResetPasswordAsync(request.email, request.token, request.newPassword);
        }
    }
}
