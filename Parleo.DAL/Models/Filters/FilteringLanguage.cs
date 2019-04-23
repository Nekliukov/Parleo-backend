namespace Parleo.DAL.Models.Filters
{
    public class FilteringLanguage
    {
        public string LanguageCode { get; set; }

        public byte? MinLevel { get; set; }

        public byte? MaxLevel { get; set; }
    }
}