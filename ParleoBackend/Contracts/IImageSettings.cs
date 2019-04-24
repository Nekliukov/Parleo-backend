namespace ParleoBackend.Contracts
{
    public interface IImageSettings
    {
        string EventSourceUrl { get; }
        string EventDestPath { get; }
        string AccountSourceUrl { get; }
        string AccountDestPath { get; }
    }
}
