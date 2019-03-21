using Microsoft.IdentityModel.Tokens;
using Parleo.BLL.Models;
using ParleoBackend.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ParleoBackend.Extensions
{
    public class JWTService
    {
        private readonly IJWTSettings _jwtSettings;

        public JWTService(
            IJWTSettings jwtSettings
        )
        {
            _jwtSettings = jwtSettings;
        }

        public string GetJWTToken(UserModel user, string secretKey)
        {
            var token = new JwtSecurityToken(
                claims: ClaimsService.GetClaims(user),
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
