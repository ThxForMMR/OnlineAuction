using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuction.Models
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string key, List<string> emails, string subject, string message);
    }
}
