using System;
using System.Collections.Generic;

namespace Parleo.BLL.Models.Entities
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int MaxParticipants { get; set; }        
       
        public decimal Latitude { get; set; }
        
        public decimal Longitude { get; set; }

        public bool IsFinished { get; set; }

        public Guid? ChatId { get; set; }

        public ChatModel Chat { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public MiniatureModel Creator { get; set; }

        public LanguageModel Language { get; set; }

        public ICollection<MiniatureModel> Participants { get; set; }
    }
}