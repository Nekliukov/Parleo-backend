using System.Collections.Generic;

namespace ParleoBackend.ViewModels
{
    // This goes from back-end
    public class PageViewModel<T>
    {
        public IEnumerable<T> Entities { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalAmount { get; set; }
    }
}
