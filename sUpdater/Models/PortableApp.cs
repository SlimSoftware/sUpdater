using Ionic.Zip;
using sUpdater.Models.DTO;
using System;
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
        public bool Installed { get; set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public bool Checkbox { get; set; } = true;
        public Archive Archive { get; set; }
        public string SavePath { get; set; }
        public LinkClickCommand LinkClickCommand { get; set; }

        private string linkText;

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

        public PortableApp(PortableAppDTO portableAppDTO, ArchiveDTO archiveDTO)
        {
            Id = portableAppDTO.Id;
            Name = portableAppDTO.Name;
            LatestVersion = portableAppDTO.Version;
            Archive = new Archive(archiveDTO);
        }

        public async Task<bool> Download()
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
                    return false;
                }
            }

            LinkText = "";
            Directory.CreateDirectory(Path.Combine(Utilities.Settings.PortableAppDir, Name));
            string fileName = @Path.GetFileName(Archive.DownloadLinkParsed);

            if (fileName.Contains("?") && Archive.LaunchFile != null)
            {
                // Filename contains invalid character so we'll have to use the launch property as fallback filename
                fileName = Archive.LaunchFile;
            }

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

                        Progress = Archive.ExtractMode == ExtractMode.Folder ? e.ProgressPercentage / 2 : e.ProgressPercentage;
                        Status = string.Format("Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                    };
                    wc.DownloadFileCompleted += (s, e) =>
                    {
                        Status = "Download complete";
                        Progress = 50;
                    };
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(Archive.DownloadLinkParsed), SavePath);
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

            return true;
        }

        public async Task<bool> Install()
        {
            if (File.Exists(SavePath))
            {
                if (Archive.ExtractMode == ExtractMode.Folder)
                {
                    try
                    {
                        await Extract();
                    }
                    catch (Exception e)
                    {
                        Status = "Extracting failed";
                        Progress = 100;
                        Log.Append($"Extracting failed: {e.Message}", Log.LogLevel.ERROR);
                        return false;
                    }

                    Log.Append("Succesfully extracted", Log.LogLevel.INFO);
                    File.Delete(SavePath);
                    Status = "Extracting complete";
                    Progress = 100;
                    await Task.Delay(1000);
                }

                return true;
            }

            return false;
        }

        private Task Extract()
        {
            return Task.Run(() =>
            {
                using (ZipFile zip = ZipFile.Read(SavePath))
                {
                    zip.ExtractProgress += (sender, e) =>
                    {
                        if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                        {
                            Progress = 50 + 50 * e.EntriesExtracted / e.EntriesTotal;
                            Status = $"Extracting ({e.EntriesExtracted}/{e.EntriesTotal}) ...";
                        }
                    };

                    string extractPath = Path.Combine(Utilities.Settings.PortableAppDir, Name);
                    zip.ExtractAll(extractPath);
                }
            });
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
                p.StartInfo.FileName = Path.Combine(Utilities.Settings.PortableAppDir, Name, Archive.LaunchFile);
                // TODO: Add support for optional arguments and using shell execute here
                try
                {
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred when trying to run the app:\n{ex.Message}", "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Append($"An error occurred when trying to run the Portable App {Name}: {ex.Message} {p.StartInfo.FileName}", Log.LogLevel.ERROR);
                    Status = "";
                }
            }
            await Task.Run(() =>
            {
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Archive.LaunchFile));
                while (processes == null | processes.Length != 0)
                {
                    Thread.Sleep(1000);
                    processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Archive.LaunchFile));
                }
            });
            Status = "";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
