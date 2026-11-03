using MediatR;
using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ForgetPassword
{
    public sealed class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, Task>
    {
        private readonly IAuthService _authService;
        public ForgetPasswordHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<Task> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.ForgotPasswordAsync(request.email);

        }
    }
}
