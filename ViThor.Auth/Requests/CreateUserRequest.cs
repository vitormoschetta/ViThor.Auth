namespace ViThor.Auth.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "user";
    }
}