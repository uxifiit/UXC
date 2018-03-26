namespace UXC.Configuration
{
    public interface IServerConfiguration
    {
        uint ServerPort { get; set; }
        bool SslEnabled { get; set; }
        bool SignalREnabled { get; set; }
        string CustomHostName { get; set; }
    }
}
