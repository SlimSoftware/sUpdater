using sUpdater.Models.Apps;

namespace sUpdater.Models
{
    public class AppUpdateInfo
    {
        public bool UpdateAvailable { get; set; }
        public string ChangelogRawText { get; set; }
        public SUpdaterApp App { get; set; }
    }
}
