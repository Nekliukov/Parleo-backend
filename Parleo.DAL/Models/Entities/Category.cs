using System;
using System.Collections.Generic;

namespace Parleo.DAL.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<Hobby> Hobbies { get; set; }
    }
}
