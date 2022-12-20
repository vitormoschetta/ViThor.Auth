using ViThor.Auth.Requests;
using ViThor.Auth.Sample.Models;

namespace Vithor.Auth.Sample.Requests
{
    public class CreateUserRequest : CreateUserRequestBase
    {
        public string Phone { get; set; } = string.Empty;
    }
}