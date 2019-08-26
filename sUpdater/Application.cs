using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace sUpdater
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
        public bool Checkbox { get; set; } = true;

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

        public async Task Download()
        {
            string fileName = Path.GetFileName(DownloadLink);
            SavePath = Path.Combine(Settings.DataDir, fileName);
            Log.Append("Saving to: " + SavePath, Log.LogLevel.INFO);

            // Check if installer is already downloaded
            if (!File.Exists(SavePath))
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, args) =>
                    {
                        // Convert download size to mb
                        double recievedSize = Math.Round(args.BytesReceived / 1024d / 1024d, 1);
                        double totalSize = Math.Round(args.TotalBytesToReceive / 1024d / 1024d, 1);

                        Progress = args.ProgressPercentage / 2;
                        Status = string.Format(
                            "Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                    };
                    wc.DownloadFileCompleted += (s, args) =>
                    {
                        Status = "Download complete";
                    };
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(DownloadLink), SavePath);
                    }
                    catch (Exception ex)
                    {
                        Log.Append("An error occurred when attempting to download " +
                            "the installer." + ex.Message, Log.LogLevel.ERROR);
                        Status = ex.Message;

                        if (File.Exists(SavePath))
                        {
                            File.Delete(SavePath);
                        }
                        return;
                    }
                }
            }
            else
            {
                Progress = 50;
            }
        }

        public async Task Install()
        {
            launchInstaller:
            using (var p = new Process())
            {
                if (DownloadLink.EndsWith(".exe"))
                {
                    p.StartInfo.FileName = SavePath;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = InstallSwitch;
                }
                else if (DownloadLink.EndsWith(".msi"))
                {
                    p.StartInfo.FileName = "msiexec";
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = "\"" + InstallSwitch + "\""
                        + " " + SavePath;
                }
                try
                {
                    p.Start();
                }
                catch (Exception)
                {
                    var result = MessageBox.Show(
                        "Lauching the installer failed. \nWould you like to try again?",
                        "Error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Log.Append("Launching the installer failed. Asking user for retry.",
                        Log.LogLevel.INFO);
                    if (result == MessageBoxResult.Yes)
                    {
                        Log.Append("User chose yes.", Log.LogLevel.INFO);
                        goto launchInstaller;
                    }
                    else
                    {
                        Log.Append("User chose no. Skipping this app.", Log.LogLevel.INFO);
                        return;
                    }
                }

                // Restore back focus to the MainWindow
                System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Focus();

                Status = "Installing...";
                IsWaiting = true;

                // Wait on a separate thread for the process to exit so the GUI thread does not get blocked
                await Task.Run(() =>
                {
                    p.WaitForExit();
                });

                if (p.ExitCode == 0)
                {
                    Log.Append("Installer exited with exit code 0.", Log.LogLevel.INFO);
                    File.Delete(SavePath);
                    Status = "Install complete";
                    IsWaiting = false;
                    Progress = 100;
                }
                if (p.ExitCode != 0)
                {
                    Log.Append(string.Format("Installation failed. Installer exited with " +
                        "exit code {0}.", p.ExitCode), Log.LogLevel.ERROR);
                    Status = String.Format(
                        "Install failed. Exit code: {0}", p.ExitCode);
                    Progress = 0;
                    IsWaiting = false;
                }
            }
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
