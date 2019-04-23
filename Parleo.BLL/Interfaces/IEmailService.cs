using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationLinkAsync(string to, string token);
    }
}
