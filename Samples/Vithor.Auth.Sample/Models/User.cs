using ViThor.Auth.Models;

namespace ViThor.Auth.Sample.Models
{
    public class User : UserBase
    {
        public string Phone { get; set; } = string.Empty;
    }
}