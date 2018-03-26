namespace UXC.Configuration
{
    public interface IAppConfiguration
    {
        bool HideOnClose { get; set; }

        bool Experimental { get; set; }
    }
}
