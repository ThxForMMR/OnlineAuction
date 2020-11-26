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

        public Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            
            if (_context.Keys.FirstOrDefault() == null)
            {
                var key = new ConnectionKey();
                key.Value = "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";
                _context.Keys.Add(key); 
            }
            return Execute(_context.Keys.FirstOrDefault().Value, subject, message, emails);
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
