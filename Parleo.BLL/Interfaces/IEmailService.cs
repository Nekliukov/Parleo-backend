using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirmationLinkAsync(string to, string token);
    }
}
