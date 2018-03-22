namespace SlimUpdater
{
    public class PortableApp
    {
        public string Name { get; set; }
        public AppItem AppItem { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }

        public PortableApp(string name, string latestVersion, string localVersion, string arch,
            string launch, string dl, string extractMode, AppItem appItem = null,
            string savePath = null)
        {
            Name = name;
            AppItem = appItem;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Launch = launch;
            DL = dl;
            ExtractMode = extractMode;
            SavePath = savePath;
        }
    }
}
