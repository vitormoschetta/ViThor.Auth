using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<ViThorAuthSettings> _appSettings;

        public EmailService(IOptions<ViThorAuthSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task SendEmail(string to, string subject, string body)
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
    }
}