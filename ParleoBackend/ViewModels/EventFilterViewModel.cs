using ParleoBackend.ViewModels;
using System;
using System.Collections.Generic;

namespace ParleoBackend.ViewModels
{
    public class EventFilterViewModel : PageRequestViewModel
    {
        public int MinNumberOfParticipants { get; set; }

        public int MaxNumberOfParticipants { get; set; }

        public int MinDistance { get; set; }

        public int MaxDistance { get; set; }

        public List<LanguageViewModel> Languages { get; set; }


        // Will be possibly used in the future
        public DateTimeOffset MinStartDate { get; set; }

        public DateTimeOffset MaxStartDate { get; set; }
    }
}