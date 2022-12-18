using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Services.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<ViThorAuthSettings> _appSettings;

        public JwtService(IOptions<ViThorAuthSettings> appSettings)
        {
            _appSettings = appSettings;

            if (_appSettings.Value.JwtConfig is null)
                throw new System.Exception("JWT configuration is missing.");
        }

        public Task<string> GenerateJwtToken(Claim[] claims)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.JwtConfig.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();            
            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            tokenDescriptor.Subject = new ClaimsIdentity(claims);
            tokenDescriptor.Expires = GetTokenExpirationTime();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.JwtConfig.Secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false, // esse token está expirado, então não vamos validar a data de expiração
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }

        private DateTime GetTokenExpirationTime()
        {
            DateTime expirationTime;

            switch (_appSettings.Value.JwtConfig.ExpirationType.ToLower())
            {
                case "seconds":
                    expirationTime = DateTime.UtcNow.AddSeconds(_appSettings.Value.JwtConfig.Expiration);
                    break;
                case "minutes":
                    expirationTime = DateTime.UtcNow.AddMinutes(_appSettings.Value.JwtConfig.Expiration);
                    break;
                case "hours":
                    expirationTime = DateTime.UtcNow.AddHours(_appSettings.Value.JwtConfig.Expiration);
                    break;
                case "days":
                    expirationTime = DateTime.UtcNow.AddDays(_appSettings.Value.JwtConfig.Expiration);
                    break;
                default:
                    expirationTime = DateTime.UtcNow.AddSeconds(_appSettings.Value.JwtConfig.Expiration);
                    break;
            }

            return expirationTime;
        }
    }
}