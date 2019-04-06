using System;

namespace Parleo.BLL.Models.Entities
{
    public class UserLanguageModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }
    }
}
