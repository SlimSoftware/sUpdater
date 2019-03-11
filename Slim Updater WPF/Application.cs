namespace SlimUpdater
{
    public class Application
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string DL { get; set; }
        public string SavePath { get; set; }
        public string InstallSwitch { get; set; }
        public bool Checkbox { get; set; }

        public Application(string name, string latestVersion, string localVersion, string arch,
            string type, string installSwitch, string dl, string savePath = null)
        {
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DL = dl;
            SavePath = savePath;
        }
    }
}
