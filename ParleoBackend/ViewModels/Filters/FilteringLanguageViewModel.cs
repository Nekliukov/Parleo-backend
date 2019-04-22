namespace ParleoBackend.ViewModels.Filters
{
    public class FilteringLanguageViewModel
    {
        public string LanguageCode { get; set; }

        public byte? MinLevel { get; set; }

        public byte? MaxLevel { get; set; }
    }
}
