namespace sUpdater.Models
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; } = true;
        public string DefenitionURL { get; set; }
        public string DataDir { get; set; }
        public string PortableAppDir { get; set; }
        public string NotifiedUpdates { get; set; }
    }
}
