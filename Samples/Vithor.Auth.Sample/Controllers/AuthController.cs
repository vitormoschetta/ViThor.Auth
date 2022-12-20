using Microsoft.Extensions.Options;
using Vithor.Auth.Sample.Requests;
using ViThor.Auth.Controllers;
using ViThor.Auth.Models;
using ViThor.Auth.Requests;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Services.Email;
using ViThor.Auth.Services.Jwt;
using ViThor.Auth.Services.Mapper;
using ViThor.Auth.Services.User;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Sample.Controllers
{
    public class AuthController : AuthControllerBase<User, CreateUserRequest>
    {
        public AuthController(IJwtService jwtServices, IEmailService emailService, IUserService<User> userService, IMapperService<User, CreateUserRequest> mapper, IOptions<ViThorAuthSettings> viThorAuthSettings) : base(jwtServices, emailService, userService, mapper, viThorAuthSettings)
        {
        }
    }
}