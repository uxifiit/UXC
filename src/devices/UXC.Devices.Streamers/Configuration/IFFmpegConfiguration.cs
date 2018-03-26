using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    public interface IFFmpegConfiguration
    {
        string FFmpegPath { get; }
        string EnumerateDevicesArgs { get; }
        string DevicesListHeaderTemplate { get; }
    }
}
