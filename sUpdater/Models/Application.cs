using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using sUpdater.Controllers;

namespace sUpdater
{   
    public class Application : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public bool HasChangelog { get; set; }
        public bool HasDescription { get; set; }
        public bool NoUpdate { get; set; }
        public string DownloadLink { get; set; }
        public string SavePath { get; set; }
        public string LaunchArgs { get; set; }
        public bool Checkbox { get; set; } = true;
        public string LinkText { get; set; }
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

        public async Task Download()
        {
            if (!Directory.Exists(Settings.DataDir))
            {
                try
                {
                    Directory.CreateDirectory(Settings.DataDir);
                } 
                catch (Exception ex)
                {
                    Log.Append($"Could not create data dir: {ex.Message}", Log.LogLevel.ERROR);
                    Status = ex.Message;
                    return;
                }
            }

            string fileName = Path.GetFileName(DownloadLink);
            SavePath = Path.Combine(Settings.DataDir, fileName);        

            // Check if installer is already downloaded
            if (!File.Exists(SavePath))
            {
                Log.Append("Saving to: " + SavePath, Log.LogLevel.INFO);

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
                Status = "Already downloaded, starting install...";
                Log.Append("Found existing installer", Log.LogLevel.INFO);
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
                    p.StartInfo.Arguments = LaunchArgs;
                }
                else if (DownloadLink.EndsWith(".msi"))
                {
                    p.StartInfo.FileName = "msiexec";
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = "\"" + LaunchArgs + "\""
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

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
