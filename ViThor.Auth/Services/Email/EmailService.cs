using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<ViThorAuthSettings> _appSettings;
        private ILogger<EmailService> _logger;

        public EmailService(IOptions<ViThorAuthSettings> appSettings, ILogger<EmailService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }       

        public async Task SendEmail(string to, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(_appSettings.Value.SmtpConfig.From);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(_appSettings.Value.SmtpConfig.Host, _appSettings.Value.SmtpConfig.Port))
                {
                    smtp.Credentials = new NetworkCredential(_appSettings.Value.SmtpConfig.Username, _appSettings.Value.SmtpConfig.Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}. Please check your SMTP configuration in appsettings.json");
            }
        }
    }
}