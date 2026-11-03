using Nabta.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Nabta.Infrastructure.implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string from, string to, string msg)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("abdofeky123@gmail.com", "jmfvhnrokfjujbcx"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage()
            {
                From = new MailAddress("abdofeky123@gmail.com"),
                Subject = "New Message",
                Body = msg
            };
            mailMessage.To.Add(to);
            mailMessage.ReplyToList.Add(new MailAddress(from));

            await client.SendMailAsync(mailMessage);
        }
    }
}
