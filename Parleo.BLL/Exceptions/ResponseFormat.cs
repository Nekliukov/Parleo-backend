namespace Parleo.BLL.Exceptions
{
    public class ErrorResponseFormat
    {
        public string Error { get; set; }

        public ErrorResponseFormat(string errorMessage)
        {
            Error = errorMessage;
        }
    }
}
