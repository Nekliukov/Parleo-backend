namespace Parleo.BLL.Interfaces
{
    public interface IEmailClientSettings
    {
        string Sender { get; }

        string UserName { get; }

        string Password { get; }

        string Host { get; }

        int Port { get; }
    }
}
