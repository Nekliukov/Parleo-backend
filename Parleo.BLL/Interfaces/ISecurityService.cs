using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.BLL.Interfaces
{
    public interface ISecurityHepler
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
