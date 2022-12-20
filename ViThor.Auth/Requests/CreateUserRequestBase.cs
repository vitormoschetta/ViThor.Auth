using ViThor.Auth.Models;

namespace ViThor.Auth.Requests
{
    public class CreateUserRequestBase
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "user";

        // public virtual TUserBase ToUser(string refreshToken, byte[] salt, string password)
        // {
        //     return new TUserBase
        //     {
        //         Username = Username,
        //         Email = Email,
        //         Password = password,
        //         Salt = salt,
        //         RefreshToken = refreshToken,
        //         Role = Role
        //     };
        // }
    }
}