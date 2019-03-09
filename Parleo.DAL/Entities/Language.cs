using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Entities
{
    public class Language
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
