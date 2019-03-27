using System;

namespace ParleoBackend.ViewModels.Entities
{
    public class CreateOrUpdateEventViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxParticipants { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsFinished { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public Guid CreatorId { get; set; }

        public Guid LanguageId { get; set; }        
    }
}
