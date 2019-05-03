using Newtonsoft.Json;

namespace Parleo.BLL.Exceptions
{
    public class ErrorResponseFormat
    {
        public string Error { get; set; }

        public ErrorResponseFormat(string errorMessage)
        {
            Error = errorMessage;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
