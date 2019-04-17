using Parleo.BLL.Models.Entities;
using System.Security.Claims;

namespace ParleoBackend.Contracts
{
    public interface IClaimsService
    {
        Claim[] GetClaims(UserModel user);
        Claim GetClaimsFromToken(string token);
    }
}
