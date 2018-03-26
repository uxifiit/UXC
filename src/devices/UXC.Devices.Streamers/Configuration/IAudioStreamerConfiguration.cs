using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    public interface IAudioStreamerConfiguration 
    {
        bool AutoSelectDevice { get; }
        string DeviceName { get; }
        string FFmpegCodecArgs { get; }
        string Extension { get; }
        int StopRecordingTimeoutMilliseconds { get; }
    }
}
