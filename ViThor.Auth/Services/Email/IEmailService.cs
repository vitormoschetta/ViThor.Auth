using System.Threading.Tasks;

namespace ViThor.Auth.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}