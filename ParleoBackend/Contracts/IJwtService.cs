using Parleo.BLL.Models;
using System;

namespace ParleoBackend.Contracts
{
    public interface IJwtService
    {
        string GetJWTToken(UserModel user);
        string GetUserIdFromToken(string token);
    }
}
