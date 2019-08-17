using System.ComponentModel;

namespace SlimUpdater
{
    public class Application : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string DownloadLink { get; set; }
        public string SavePath { get; set; }
        public string InstallSwitch { get; set; }
        public bool Checkbox { get; set; }

        private int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private bool isWaiting;
        public bool IsWaiting
        {
            get { return isWaiting; }
            set
            {
                isWaiting = value;
                OnPropertyChanged("IsWaiting");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Application(string name, string latestVersion, string localVersion, string arch,
            string type, string installSwitch, string downloadLink, string savePath = null)
        {
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DownloadLink = downloadLink;
            SavePath = savePath;
        }

        public Application Clone()
        {
            return (Application)MemberwiseClone();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
