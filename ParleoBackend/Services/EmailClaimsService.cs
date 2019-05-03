using Parleo.BLL.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ParleoBackend.Services
{
    public class EmailClaimsService: ClaimsService
    {
        public EmailClaimsService(
            IJwtSettings jwtSettings
        ) : base(jwtSettings)
        { }

        public override Claim[] GetClaims(UserModel user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };

            return claims;
        }
    }
}
