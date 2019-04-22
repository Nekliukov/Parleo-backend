using Parleo.DAL.Models.Pages;
using System;

namespace Parleo.DAL.Models.Filters
{
    public class EventFilter : PageRequest
    {
        public int? MinNumberOfParticipants { get; set; }

        public int? MaxNumberOfParticipants { get; set; }

        public int? MinDistance { get; set; }

        public int? MaxDistance { get; set; }

        public string[] Languages { get; set; }


        // Will be possibly used in the future
        public DateTimeOffset? MinStartDate { get; set; }

        public DateTimeOffset? MaxStartDate { get; set; }
    }
}