namespace sUpdater.Models
{
    public class AppUpdateInfo
    {
        public bool UpdateAvailable { get; set; }
        public string ChangelogRawText { get; set; }
        public Application App { get; set; }
    }
}
