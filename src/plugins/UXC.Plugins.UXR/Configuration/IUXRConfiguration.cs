namespace UXC.Plugins.UXR.Configuration
{
    public interface IUXRConfiguration
    {
        string NodeName { get; set; }

        string EndpointAddress { get; set; }

        int StatusUpdateIntervalSeconds { get; set; }

        int UploadTimeoutSeconds { get; set; }
    }
}
