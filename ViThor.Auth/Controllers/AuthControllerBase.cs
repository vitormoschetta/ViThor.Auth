using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ViThor.Auth.Models;
using ViThor.Auth.Requests;
using ViThor.Auth.Responses;
using ViThor.Auth.Services.Email;
using ViThor.Auth.Services.Jwt;
using ViThor.Auth.Services.Mapper;
using ViThor.Auth.Services.User;
using ViThor.Auth.Settings;
using ViThor.Auth.Utils;

namespace ViThor.Auth.Controllers
{
    [ApiController]
    [Route("api/authenticate")]
    public class AuthControllerBase<TUserBase, TCreateUserRequest> : ControllerBase
                                                    where TUserBase : UserBase, new()
                                                    where TCreateUserRequest : CreateUserRequestBase, new()
    {
        private readonly IJwtService _jwtServices;
        private readonly IEmailService _emailService;
        private readonly IUserService<TUserBase> _userService;
        private readonly IMapperService<TUserBase, TCreateUserRequest> _mapper;
        private readonly IOptions<ViThorAuthSettings> _viThorAuthSettings;

        public AuthControllerBase(
            IJwtService jwtServices, IEmailService emailService, IUserService<TUserBase> userService, IMapperService<TUserBase, TCreateUserRequest> mapper,
            IOptions<ViThorAuthSettings> viThorAuthSettings)
        {
            _jwtServices = jwtServices;
            _emailService = emailService;
            _userService = userService;
            _mapper = mapper;
            _viThorAuthSettings = viThorAuthSettings;
        }


        [HttpPost("register")]
        public async Task<ActionResult<TUserBase>> Register([FromBody] TCreateUserRequest request)
        {
            var user = await _userService.GetByUsername(request.Username);
            if (user != null)
                return BadRequest("Username already exists");

            var refreshToken = await _jwtServices.GenerateRefreshToken();
            byte[] salt = HashManager.GenerateSalt();
            var password = HashManager.GenerateHash(request.Password, salt);

            user = _mapper.MapRequestToUser(request, refreshToken, salt, password);
            await _userService.Create(user);

            SendEmailVerification(user);

            return Ok(user);
        }


        [HttpPost("login")]
        public virtual async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetByUsername(request.Username);
            if (user == null)
            {
                user = await _userService.GetByEmail(request.Username);
                if (user == null)
                    return BadRequest("Invalid credentials");
            }

            var passwordHash = HashManager.GenerateHash(request.Password, user.Salt);

            if (user.Password != passwordHash)
                return BadRequest(new { message = "Username or password is incorrect" });

            if (_viThorAuthSettings.Value?.SmtpConfig != null)
            {
                if (_viThorAuthSettings.Value.SmtpConfig.Enabled && !user.IsEmailVerified)
                    return BadRequest(new { message = "Email not verified" });
            }

            var claims = await _userService.GetClaim(user);

            var token = await _jwtServices.GenerateJwtToken(claims);

            return Ok(new LoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                RefreshToken = user.RefreshToken,
                Token = token
            });
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var principal = await _jwtServices.GetPrincipalFromExpiredToken(request.Token);
            var username = principal.Identity?.Name;

            if (username == null)
                return Unauthorized(new { message = "Invalid token: username not found" });

            var user = await _userService.GetByUsername(username);
            if (user == null)
            {
                user = await _userService.GetByEmail(username);
                if (user == null)
                    return Unauthorized(new { message = "Invalid token: user not found" });
            }

            if (user.RefreshToken != request.RefreshToken)
                return Unauthorized(new { message = "Invalid token: refresh token not found" });

            var claims = await _userService.GetClaim(user);
            var token = await _jwtServices.GenerateJwtToken(claims);

            var refreshToken = await _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userService.Update(user);

            return Ok(new LoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                RefreshToken = user.RefreshToken,
                Token = token
            });
        }


        [HttpGet("current-user")]
        public async Task<ActionResult<TUserBase>> CurrentUser()
        {
            var username = User.Identity?.Name;

            if (username == null)
                return Unauthorized(new { message = "Invalid token: username not found" });

            var user = await _userService.GetByUsername(username);
            if (user == null)
            {
                user = await _userService.GetByEmail(username);
                if (user == null)
                    return Unauthorized(new { message = "Invalid token: user not found" });
            }

            return Ok(user);
        }


        [HttpGet("email-verification/{id}")]
        public async Task<ContentResult> Confirm(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"<div style='text-align:center; margin-top:3em; color:#808080'><h1>Usuario nao ncontrado</h1></div>"
                };

            user.IsEmailVerified = true;
            await _userService.Update(user);

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $"<div style='text-align:center; margin-top:3em; color:#808080'><h1>Email verificado com sucesso</h1></div>"
            };
        }


        private async void SendEmailVerification(TUserBase user)
        {
            if (_viThorAuthSettings.Value?.SmtpConfig != null)
            {
                if (_viThorAuthSettings.Value.SmtpConfig.Enabled)
                {
                    var body = $@"
                    <h1>Welcome to our platform</h1>
                    <p>Email: {user}</p>

                    <p>Click <a href='{_viThorAuthSettings.Value.BaseAddress}/api/authenticate/email-verification/{user.Id}'>here</a> to validate your email</p>";

                    await _emailService.SendEmail(user.Email, "Welcome", body);
                }
            }
        }
    }
}