using System;

namespace ParleoBackend.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxParticipants { get; set; }    
        
        public decimal Latitude { get; set; }  
        
        public decimal Longitude { get; set; }

        public bool IsFinished { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid CreatorId { get; set; } 

        public UserLanguageViewModel Language { get; set; }

		//PARTICIPANTS??
    }
}
