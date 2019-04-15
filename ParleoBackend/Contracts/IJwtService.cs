using Parleo.BLL.Models.Entities;

namespace ParleoBackend.Contracts
{
    public interface IJwtService
    {
        string GetJWTToken(UserModel user);
        string GetUserIdFromToken(string token);
    }
}
