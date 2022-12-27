using System.Collections.Generic;
using ViThor.Auth.Models;

namespace ViThor.Auth.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public IList<Role> Roles { get; set; } = new List<Role>();
    }
}