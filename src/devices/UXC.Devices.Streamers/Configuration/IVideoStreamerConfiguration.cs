using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    public interface IVideoStreamerConfiguration 
    {
        bool AutoSelectDevice { get; }
        string FFmpegCodecArgs { get; }
        string DeviceName { get; }
        string Extension { get; }
        int StopRecordingTimeoutMilliseconds { get; }
    }
}
