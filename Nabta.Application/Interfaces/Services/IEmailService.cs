using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string from, string to, string msg);
    }
}
