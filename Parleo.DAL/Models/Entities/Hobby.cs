using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parleo.DAL.Models.Entities
{
    public class Hobby
    {
        [Key]
        public string Name { get; set; }

        public Category Category { get; set; }

        public ICollection<UserHobby> Users { get; set; }
    }
}
