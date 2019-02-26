namespace UXC.Sessions.Timeline.Actions
{
    public abstract class ContentActionSettingsBase : SessionStepActionSettings
    {
        public string Background { get; set; }
        public string Foreground { get; set; }
        public int? FontSize { get; set; }
        public virtual bool? ShowCursor { get; set; }
    }
}
