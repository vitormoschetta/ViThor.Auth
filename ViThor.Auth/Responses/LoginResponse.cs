using System;
using System.Collections.Generic;
using ViThor.Auth.Models;

namespace ViThor.Auth.Responses
{
    public class LoginResponse
    {
        public Guid Id { get; set; }        
        public string Email { get; set; } = null!;
        public IList<Role> Roles { get; set; } = new List<Role>();
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}