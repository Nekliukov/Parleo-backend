using System.Collections.Generic;

namespace Parleo.DAL.Models.Pages
{
    // This goes from back-end
    public class Page<T>
    {
        public IEnumerable<T> Entities { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalAmount { get; set; }
    }
}
