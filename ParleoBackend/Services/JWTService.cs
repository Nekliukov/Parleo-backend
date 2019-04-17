using Microsoft.IdentityModel.Tokens;
using Parleo.BLL.Models.Entities;
using ParleoBackend.Contracts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParleoBackend.Services
{
    public class JwtService: IJwtService
    {
        private readonly IJwtSettings _jwtSettings;
        private readonly IClaimsService _claimsService;

        public JwtService(
            IJwtSettings jwtSettings,
            IClaimsService claimsService
        )
        {
            _jwtSettings = jwtSettings;
            _claimsService = claimsService;
        }

        public string GetJWTToken(UserModel user)
        {
            var token = new JwtSecurityToken(
                claims: _claimsService.GetClaims(user),
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JWTKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetUserIdFromToken(string token)
        {
            Claim userClaim = _claimsService.GetClaimsFromToken(token);

            if (userClaim == null)
            {
                return "";
            }

            return userClaim.Value;
        }
    }
}
