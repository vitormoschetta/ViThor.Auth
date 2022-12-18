using System;

namespace ViThor.Auth.Responses
{
    public class LoginResponse
    {
        public Guid Id { get; set; }        
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}