using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ResetPassword
{
    public record ResetPasswordCommand(string email, string token, string newPassword) : IRequest<bool>;
}
