using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailConfirmationLink(string to, string token);
    }
}
