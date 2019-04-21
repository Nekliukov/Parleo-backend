using System;

namespace Parleo.BLL.Models.Pages
{
    // This goes to back-end
    public class PageRequestModel
    {
        public int Page { get; set; }

        public int? PageSize { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }
    }
}