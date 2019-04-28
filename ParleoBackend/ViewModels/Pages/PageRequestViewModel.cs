using System;

namespace ParleoBackend.ViewModels.Pages
{
    // This goes to back-end
    public class PageRequestViewModel
    {
        public int PageNumber { get; set; }

        public int? PageSize { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }
    }
}