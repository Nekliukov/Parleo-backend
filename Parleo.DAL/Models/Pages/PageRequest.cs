using System;

namespace Parleo.DAL.Models.Pages
{
    // This goes to back-end
    public class PageRequest
    {
        public int Page { get; set; }

        public int? PageSize { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }
    }
}