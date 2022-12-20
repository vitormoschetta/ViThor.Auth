using ViThor.Auth.Models;
using ViThor.Auth.Requests;

namespace ViThor.Auth.Services.Mapper
{
    public class MapperService : IMapperService<UserBase, CreateUserRequestBase>
    {
        public UserBase MapRequestToUser(CreateUserRequestBase request, string refreshToken, byte[] salt, string password)
        {
            return new UserBase
            {
                Username = request.Username,
                Email = request.Email,
                Role = request.Role,
                RefreshToken = refreshToken,                
                Salt = salt,
                Password = password
            };
        }
    }
}