using System.ComponentModel;
using System.Windows.Media;

namespace sUpdater.Models
{
    public class BaseApplication : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public string LatestVersion { get; set; }
        public bool Installed { get; set; }

        /// <summary>The version displayed under the app's name</summary>
        public string DisplayedVersion { get; set; }

        public bool Checkbox { get; set; } = true;
        public string SavePath { get; set; }

        private int progress;
        public int Progress
        {
            get => progress;
            set { progress = value; OnPropertyChanged(nameof(Progress)); }
        }

        private string status;
        public string Status
        {
            get => status;
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        private bool isWaiting;
        public bool IsWaiting
        {
            get => isWaiting;
            set { isWaiting = value; OnPropertyChanged(nameof(IsWaiting)); }
        }

        public LinkClickCommand LinkClickCommand { get; set; }

        private string linkText;
        public string LinkText
        {
            get => linkText;
            set { linkText = value; OnPropertyChanged(nameof(LinkText)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
