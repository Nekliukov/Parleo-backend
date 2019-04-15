using System;

namespace Parleo.BLL.Models.Entities
{
    public class AccountTokenModel
    {
        public Guid UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
