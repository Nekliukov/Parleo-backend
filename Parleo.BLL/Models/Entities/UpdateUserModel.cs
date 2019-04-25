using System;

namespace Parleo.BLL.Models.Entities
{
    public class UpdateUserModel
    {
        public string Name { get; set; }

        public string About { get; set; }

        public DateTime Birthdate { get; set; }

        public bool Gender { get; set; }

        public UserLanguageModel[] Languages { get; set; }

        public string[] Hobbies { get; set; }
    }
}