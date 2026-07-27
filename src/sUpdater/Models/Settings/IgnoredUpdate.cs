namespace sUpdater.Models.Settings
{
    public enum AppType { sUpdater, WinGet }

    public class IgnoredUpdate
    {
        public string Id { get; set; }
        public AppType Type { get; set; }
        public string Version { get; set; }
    }
}
