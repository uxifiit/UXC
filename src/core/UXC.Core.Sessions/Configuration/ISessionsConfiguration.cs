using UXC.Core.Configuration;

namespace UXC.Sessions
{
    public interface ISessionsConfiguration : IConfiguration
    {
        string TargetPath { get; }
    }
}
