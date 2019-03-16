using System;

namespace Parleo.BLL.Models
{
    public class UserLanguageModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }
    }
}
