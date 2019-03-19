using System;
namespace Parleo.BLL.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxParticipants { get; set; }        
       
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }

        public bool IsFinished { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public Guid CreatorId { get; set; }

        public LanguageModel Language { get; set; }

        public int ParticipantsCount { get; set; }
    }
}