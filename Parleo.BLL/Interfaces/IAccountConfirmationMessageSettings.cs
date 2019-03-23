namespace Parleo.BLL.Interfaces
{
    public interface IAccountConfirmationMessageSettings
    {
        string InvitationUrl { get; }

        string Message { get; }

        string Subject { get; }
    }
}
