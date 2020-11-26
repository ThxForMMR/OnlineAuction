using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace OnlineAuction.Models
{
    public class EmailSender : IEmailSender
    {
        private AuctionContext _context;
        public EmailSender(IOptions<EmailAuthOptions> optionsAccessor, AuctionContext context)
        {
            Options = optionsAccessor.Value;
            _context = context;
        }

        public EmailAuthOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string key, List<string> emails, string subject, string message)
        {           
            
            return Execute(key, subject, message, emails);
        }

        public Task Execute(string apiKey, string subject, string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("myrealak@gmail.com", "Auction"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }
    }
}
