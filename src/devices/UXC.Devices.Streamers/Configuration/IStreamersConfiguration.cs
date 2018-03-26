using UXI.Configuration;

namespace UXC.Devices.Streamers.Configuration
{
    public interface IStreamersConfiguration
    {
        bool AutoSelectDevice { get; }
        bool LogOutput { get; }
        bool ShowOutput { get; }
        int StopRecordingTimeoutMilliseconds { get; }
        string FFmpegStartRecordingArgs { get; }
    }
}
