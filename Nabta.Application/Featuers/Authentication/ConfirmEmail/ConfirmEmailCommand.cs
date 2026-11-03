using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Featuers.Authentication.ConfirmEmail
{
    public record ConfirmEmailCommand(string userId, string token): IRequest<bool>;
}