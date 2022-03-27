using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace sUpdater.Models
{
    public class PortableApp : INotifyPropertyChanged
    {
        public int Id { get; }
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public bool Checkbox { get; set; } = true;
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }
        public LinkClickCommand LinkClickCommand { get; set; }

        private string linkText = "Run";

        public string LinkText
        {
            get { return linkText; }
            set
            {
                linkText = value;
                OnPropertyChanged("LinkText");
            }
        }

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

        public PortableApp(int id, string name, string latestVersion, string localVersion, string arch,
            string launch, string dl, string extractMode, string savePath = null)
        {
            Id = id;
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
            if (!Directory.Exists(Utilities.Settings.PortableAppDir))
            {
                try
                {
                    Directory.CreateDirectory(Utilities.Settings.PortableAppDir);
                }
                catch (Exception ex)
                {
                    Log.Append($"Could not create portable app dir: {ex.Message}", Log.LogLevel.ERROR);
                    Status = ex.Message;
                    return;
                }
            }

            LinkText = "";
            Directory.CreateDirectory(Path.Combine(Utilities.Settings.PortableAppDir, Name));
            string fileName = @Path.GetFileName(DL);
            SavePath = @Path.Combine(Utilities.Settings.PortableAppDir, Name, fileName);

            // Check if portable app is already downloaded
            if (!File.Exists(SavePath))
            {
                Log.Append("Saving to: " + SavePath, Log.LogLevel.INFO);

                using (var wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        // Convert download size to MB
                        double recievedSize = Math.Round(e.BytesReceived / 1024d / 1024d, 1);
                        double totalSize = Math.Round(e.TotalBytesToReceive / 1024d / 1024d, 1);

                        // Set the progress
                        if (ExtractMode == "single")
                        {
                            Progress = e.ProgressPercentage;
                        }
                        else
                        {
                            Progress = e.ProgressPercentage / 2;
                        }
                        Status = string.Format("Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                    };
                    wc.DownloadFileCompleted += (s, e) =>
                    {
                        Status = "Download complete";
                        Progress = 50;
                    };
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(DL), SavePath);
                    }
                    catch (Exception e)
                    {
                        Log.Append($"An error occurred while downloading {e.Message}", Log.LogLevel.ERROR);
                            Status = $"Download failed: {e.Message}";

                        if (File.Exists(SavePath))
                        {
                            File.Delete(SavePath);
                        }
                    }
                }
            }
            else
            {
                Progress = 50;
                Status = "Already downloaded, starting install...";
                Log.Append("Found existing download", Log.LogLevel.INFO);
            }
        }

        public async Task Install()
        {
            if (File.Exists(SavePath))
            {
                // If ExtractMode is single, we do not need to extract
                if (ExtractMode == "folder")
                {
                    using (ZipFile zip = ZipFile.Read(SavePath))
                    {
                        zip.ExtractProgress += (sender, e) =>
                        {
                            if (e.TotalBytesToTransfer > 0)
                            {
                                Progress = Convert.ToInt32(50 * e.BytesTransferred / e.TotalBytesToTransfer);
                            }
                        };

                        string extractPath = Path.Combine(Utilities.Settings.PortableAppDir, Name);
                        try
                        {
                            zip.ExtractAll(extractPath);
                        }
                        catch (Exception e)
                        {
                            Status = $"Extracting failed: {e.Message}";
                            Log.Append($"Extracting failed: {e.Message}", Log.LogLevel.ERROR);
                            return;
                        }
                    }
            
                    Log.Append("Succesfully extracted", Log.LogLevel.INFO);
                    File.Delete(SavePath);
                    Status = "Extracting complete";
                    Progress = 100;
                    await Task.Delay(1000);                       
                }
            }
        }

        public async Task Run()
        {
            Log.Append($"Launching {Name}", Log.LogLevel.INFO);
            if (Progress != 0)
            {
                Progress = 0;
            }

            Status = "Running...";

            using (var p = new Process())
            {
                p.StartInfo.FileName = Path.Combine(Utilities.Settings.PortableAppDir, Name, Launch);
                // TODO: Add support for optional arguments and using shell execute here
                try
                {
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred when trying to run the app:\n{ex.Message}", "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Append($"An error occurred when trying to run the Portable App {Name}: {ex.Message}", Log.LogLevel.ERROR);
                    Status = "";
                }
            }
            await Task.Run(() =>
            {
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Launch));
                while (processes == null | processes.Length != 0)
                {
                    Thread.Sleep(1000);
                    processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Launch));
                }
            });
            Status = "";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            return obj is PortableApp app &&
                   Name == app.Name &&
                   LatestVersion == app.LatestVersion &&
                   Arch == app.Arch;
        }

        public override int GetHashCode()
        {
            int hashCode = 349623337;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LatestVersion);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Arch);
            return hashCode;
        }
    }
}
