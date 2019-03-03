using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parleo.DAL.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(11, 8)")]
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserAuth UserAuth { get; set; }
        public ICollection<UserLanguage> UserLanguages { get; set; }
    }
}
