using System.Security.Claims;
using Microsoft.Extensions.Options;
using ViThor.Auth.Requests;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Services.User;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Sample.Services
{
    public class UserService : IUserService<User>
    {
        private List<User> _users = new List<User>();
        private readonly IOptions<ViThorAuthSettings> _appSettings;

        public UserService(IOptions<ViThorAuthSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public Task Create(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> Get()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<User?> GetByEmail(string email)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.Email == email));
        }

        public Task<User?> GetById(Guid id)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.Id == id));
        }

        public Task<User?> GetByUsername(string username)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.Username == username));
        }

        public Task<Claim[]> GetClaim(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("iss", _appSettings.Value.JwtConfig.Issuer),
                new Claim("aud", _appSettings.Value.JwtConfig.Audience),
                new Claim("document", "123456789") // custom claim,
            };

            return Task.FromResult(claims);
        }

        // public Task<User?> MapRequestToUser(CreateUserRequestBase<User> request, string refrashToken, byte[] salt, string password)
        // {
        //     var user = new User
        //     {
        //         Id = Guid.NewGuid(),
        //         Username = request.Username,
        //         Email = request.Email,
        //         Password = password,
        //         Salt = salt,
        //         Role = request.Role,
        //         RefreshToken = refrashToken
        //     };

        //     return Task.FromResult(user) as Task<User?>;
        // }

        public Task Update(User user)
        {
            var index = _users.FindIndex(x => x.Id == user.Id);
            _users[index] = user;
            return Task.CompletedTask;
        }
    }
}