using sUpdater.Models.DTO;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace sUpdater.Models
{
    public class PortableApp : BaseApplication, INotifyPropertyChanged
    {
        public int Id { get; }

        public Archive Archive { get; set; }

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
            string fileName = Path.GetFileName(Archive.DownloadLinkParsed);

            if (fileName.Contains('?') && Archive.LaunchFile != null)
            {
                // Filename contains invalid character so we'll have to use the launch property as fallback filename
                fileName = Archive.LaunchFile;
            }

            SavePath = Path.Combine(Utilities.Settings.PortableAppDir, Name, fileName);

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

        protected Task Extract()
        {
            return Task.Run(() =>
            {
                Status = "Extracting...";
                IsWaiting = true;
                string extractPath = Path.GetDirectoryName(SavePath);

                if (Archive.ExtractMode == ExtractMode.Folder)
                {
                    ZipFile.ExtractToDirectory(SavePath, extractPath);
                }
                else if (Archive.ExtractMode == ExtractMode.Single)
                {
                    using ZipArchive archive = ZipFile.OpenRead(SavePath);
                    string searchFileName = $"{Path.GetFileNameWithoutExtension(SavePath)}.exe";
                    ZipArchiveEntry entry = archive.GetEntry(searchFileName);

                    extractPath = Path.Combine(extractPath, entry.Name);
                    entry.ExtractToFile(extractPath);
                }

                IsWaiting = false;
            });
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
    }
}
