using Microsoft.Extensions.Options;
using ViThor.Auth.Controllers;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Services.Email;
using ViThor.Auth.Services.Jwt;
using ViThor.Auth.Services.User;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Sample.Controllers
{
    public class AuthController : AuthControllerBase<User>
    {
        public AuthController(IJwtService jwtServices, IEmailService emailService, IUserService<User> userService, IOptions<ViThorAuthSettings> appSettings) : base(jwtServices, emailService, userService, appSettings)
        {
        }
    }
}