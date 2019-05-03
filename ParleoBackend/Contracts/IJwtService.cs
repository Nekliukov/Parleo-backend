using Parleo.BLL.Models.Entities;

namespace ParleoBackend.Contracts
{
    public interface IJwtService
    {
        string GetJWTToken(UserModel user, IClaimsService claimsService);
        string GetUserIdFromToken(string token);
    }
}
