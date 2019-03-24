using Parleo.BLL.Models.Pages;

namespace Parleo.BLL.Models.Filters
{
    public class ChatFilterViewModel : PageRequestModel
    {
        public string Keywords { get; set; }
    }
}