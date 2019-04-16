using System;

namespace Parleo.DAL.Models.Entities
{
    public class UserLanguage
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public string LanguageCode { get; set; }

        public Language Language { get; set; }

        public byte Level { get; set; }
    }
}
