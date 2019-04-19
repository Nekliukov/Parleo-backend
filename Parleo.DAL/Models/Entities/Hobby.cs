using System;
using System.Collections.Generic;

namespace Parleo.DAL.Models.Entities
{
    public class Hobby
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public ICollection<UserHobby> Users { get; set; }
    }
}
