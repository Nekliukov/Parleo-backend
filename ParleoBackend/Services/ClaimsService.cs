using Microsoft.IdentityModel.Tokens;
using Parleo.BLL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ParleoBackend.Services
{
    public class ClaimsService
    {
        public Claim[] GetClaims(UserModel user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            return claims;
        }

        public void GetClaimsFromToken(string token)
        {
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.IssuerSigningKey = 

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
