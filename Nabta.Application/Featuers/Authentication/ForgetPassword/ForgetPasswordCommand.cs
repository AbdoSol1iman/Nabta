using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ForgetPassword
{
    public record ForgetPasswordCommand(string email) : IRequest<Task>;
}