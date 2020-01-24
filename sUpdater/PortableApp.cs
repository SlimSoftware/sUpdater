using System.ComponentModel;
using System.Threading.Tasks;

namespace sUpdater
{
    public class PortableApp
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public bool Checkbox { get; set; } = true;
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }
        public string LinkText { get; set; } = "Run";
        public LinkClickCommand LinkClickCommand { get; set; }

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

        public PortableApp(string name, string displayedVersion)
        {
            Name = name;
            DisplayedVersion = displayedVersion;
        }

        public async Task Download()
        {

        }

        public async Task Install()
        {

        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
