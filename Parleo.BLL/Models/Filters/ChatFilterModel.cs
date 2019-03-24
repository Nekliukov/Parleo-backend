using Parleo.BLL.Models.Pages;

namespace Parleo.BLL.Models.Filters
{
    public class ChatFilterModel : PageRequestModel
    {
        public string Keywords { get; set; }
    }
}