using System;

namespace ParleoBackend.ViewModels.Entities
{
    public class UserLanguageViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
	 
        public byte Level { get; set; }
    }
}