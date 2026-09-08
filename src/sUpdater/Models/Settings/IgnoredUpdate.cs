using sUpdater.Helpers;

namespace sUpdater.Models.Settings
{
    public enum AppType { sUpdater, WinGet }

    public class IgnoredUpdate
    {
        public string Id { get; set; }
        public AppType Type { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
           var app = UpdateHelper.GetAppFromIgnoredUpdate(this);
           return Version == null ? app.Name : $"{app.Name} {app.LatestVersion}";
        }
    }
}
