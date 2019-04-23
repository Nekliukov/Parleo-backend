using System;
using System.Collections.Generic;
using System.Text;

namespace Parleo.BLL.Models.Entities
{
    public class CreateOrUpdateEventModel
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

        public string LanguageCode { get; set; }
    }
}
