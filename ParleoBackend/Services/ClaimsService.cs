using Microsoft.IdentityModel.Tokens;
using Parleo.BLL.Models.Entities;
using ParleoBackend.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParleoBackend.Services
{
    public class ClaimsService: IClaimsService
    {
        private readonly IJwtSettings _jwtSettings;
        public ClaimsService(
            IJwtSettings jwtSettings
        )
        {
            _jwtSettings = jwtSettings;
        }

        public virtual Claim[] GetClaims(UserModel user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim("IsAuthorization", "true")
            };

            return claims;
        }

        public Claim GetClaimsFromToken(string token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.JWTKey)
                ),
                ValidateAudience = false,
                ValidateIssuer = false
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            return principal.FindFirst(JwtRegisteredClaimNames.Jti);
        }
    }
}
