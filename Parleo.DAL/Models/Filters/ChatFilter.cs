using Parleo.DAL.Models.Pages;

namespace Parleo.DAL.Models.Filters
{
    public class ChatFilter : PageRequest
    {
        public string Keywords { get; set; }
    }
}