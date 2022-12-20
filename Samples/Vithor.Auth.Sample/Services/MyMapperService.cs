using Vithor.Auth.Sample.Requests;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Services.Mapper;

namespace Vithor.Auth.Sample.Services
{
    public class MyMapperService : IMapperService<User, CreateUserRequest>
    {
        public User MapRequestToUser(CreateUserRequest request, string refrashToken, byte[] salt, string password)
        {
            return new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = request.Role,
                Phone = request.Phone,
                RefreshToken = refrashToken,
                Salt = salt,
                Password = password
            };
        }
    }
}