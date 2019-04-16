using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Models.Entities
{
    public class Language
    {
        [Key]
        [Column(TypeName = "varchar(2)")]
        public string Code { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
