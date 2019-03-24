using System;
using System.Collections.Generic;

namespace Parleo.DAL.Models.Entities
{
    public class Language
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
