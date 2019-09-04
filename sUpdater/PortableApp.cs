namespace sUpdater
{
    class PortableApp
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Status { get; set; }
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }

        public PortableApp(string name, string latestVersion, string localVersion, string arch,
            string launch, string dl, string extractMode, string savePath = null)
        {
            Name = name;
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
