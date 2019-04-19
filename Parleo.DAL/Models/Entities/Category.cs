using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parleo.DAL.Models.Entities
{
    public class Category
    {
        [Key]
        public string Name { get; set; }

        public ICollection<Hobby> Hobbies { get; set; }
    }
}
