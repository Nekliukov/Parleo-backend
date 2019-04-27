using System;
using System.Collections.Generic;

namespace Parleo.BLL.Models.Pages
{
    // This goes from back-end
    public class PageModel<T>
    {
        public IEnumerable<T> Entities { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalAmount { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }
    }
}
