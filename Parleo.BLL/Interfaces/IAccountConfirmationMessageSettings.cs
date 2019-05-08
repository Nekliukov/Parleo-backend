namespace Parleo.BLL.Interfaces
{
    public interface IAccountConfirmationMessageSettings
    {
        string WebSiteInvitationUrl { get; }

        string MobileInvitationUrl { get; }

        string Message { get; }

        string Subject { get; }
    }
}
