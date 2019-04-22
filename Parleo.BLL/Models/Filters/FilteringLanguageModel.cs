namespace Parleo.BLL.Models.Filters
{
    public class FilteringLanguageModel
    {
        public string LanguageCode { get; set; }

        public byte? MinLevel { get; set; }

        public byte? MaxLevel { get; set; }
    }
}

