using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.DAL.Entities
{
    public class UserLanguage
    {
        public Guid UserId { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public Guid LanguageId { get; set; }

        public virtual Language Language { get; set; }

        public byte Level { get; set; }
    }
}
