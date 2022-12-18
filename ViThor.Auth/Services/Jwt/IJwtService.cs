using System.Security.Claims;
using System.Threading.Tasks;

namespace ViThor.Auth.Services.Jwt
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(Claim[] claims);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    }
}