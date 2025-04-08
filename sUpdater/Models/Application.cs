using sUpdater.Models.DTO;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace sUpdater.Models
{
    public class Application : BaseApplication, INotifyPropertyChanged
    {
        public int Id { get; }
        public string LocalVersion { get; set; }
        public string WebsiteUrl { get; }
        public string ReleaseNotesUrl { get; }
        public bool NoUpdate { get; }
        public DetectInfo DetectInfo { get; }
        public Installer Installer { get; }

        public Application(ApplicationDTO applicationDTO, DetectInfoDTO detectInfoDTO, InstallerDTO installerDTO)
        {
            Id = applicationDTO.Id;
            Name = applicationDTO.Name;
            LatestVersion = applicationDTO.Version;
            NoUpdate = applicationDTO.NoUpdate;
            WebsiteUrl = applicationDTO.WebsiteUrl;
            ReleaseNotesUrl = applicationDTO.ReleaseNotesUrl;

            if (detectInfoDTO != null) DetectInfo = new DetectInfo(detectInfoDTO);
            if (installerDTO != null) Installer = new Installer(installerDTO);
        }

        public async Task<bool> Download()
        {
            if (!Directory.Exists(Utilities.Settings.DataDir))
            {
                try
                {
                    Directory.CreateDirectory(Utilities.Settings.DataDir);
                }
                catch (Exception ex)
                {
                    Log.Append($"Could not create data dir: {ex.Message}", Log.LogLevel.ERROR);
                    Status = ex.Message;
                    return false;
                }
            }

            string fileName = Path.GetFileName(Installer.DownloadLink);
            SavePath = Path.Combine(Utilities.Settings.DataDir, fileName);

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
                        if (Progress == 0) Progress = 1; // Make sure the progress bar is always visible

                        Status = string.Format("Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                    };
                    wc.DownloadFileCompleted += (s, args) =>
                    {
                        Status = "Download complete";
                    };

                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(Installer.DownloadLink), SavePath);
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
                        return false;
                    }
                }
            }
            else
            {
                Progress = 50;
                Status = "Already downloaded, starting install...";
                Log.Append("Found existing installer", Log.LogLevel.INFO);
            }

            return true;
        }

        private Task<string> Extract()
        {
            return Task.Run(() =>
            {
                Status = "Extracting...";
                IsWaiting = true;
                string searchFileName = $"{Path.GetFileNameWithoutExtension(SavePath)}.exe";

                if (File.Exists(Path.Combine(Utilities.Settings.DataDir, searchFileName))) return searchFileName;

                using (ZipArchive archive = ZipFile.OpenRead(SavePath))
                {
                    ZipArchiveEntry entry = archive.GetEntry(searchFileName);

                    string extractPath = Path.Combine(Utilities.Settings.DataDir, entry.Name);
                    entry.ExtractToFile(extractPath);
                }

                IsWaiting = false;
                return searchFileName;
            });
        }

        public async Task<bool> Install()
        {
        launchInstaller:
            using (var p = new Process())
            {
                if (!SavePath.EndsWith(".msi"))
                {
                    p.StartInfo.FileName = SavePath;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = Installer.LaunchArgs;
                }
                else
                {
                    p.StartInfo.FileName = "msiexec";
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = $@"/i ""{SavePath}"" {Installer.LaunchArgs}";
                }

                if (SavePath.EndsWith(".zip"))
                {
                    try
                    {
                        string extractPath = await Extract();
                        p.StartInfo.FileName = Path.Combine(Utilities.Settings.DataDir, extractPath);
                    }
                    catch (Exception e)
                    {
                        Status = "Extracting failed";
                        Progress = 100;
                        Log.Append($"Extracting failed: {e.Message}", Log.LogLevel.ERROR);
                        return false;
                    }
                }

                try
                {
                    p.Start();
                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show("Lauching the installer failed. \nWould you like to try again?",
                        "Error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Log.Append($"Launching the installer failed: {ex.Message}, filename: {p.StartInfo.FileName}", Log.LogLevel.WARN);
                    if (result == MessageBoxResult.Yes)
                    {
                        Log.Append("Relaunching installer...", Log.LogLevel.INFO);
                        goto launchInstaller;
                    }
                    else
                    {
                        Log.Append("User chose not to relaunch installer. Skipping this app.", Log.LogLevel.INFO);
                        Progress = 0;
                        Status = "";
                        Checkbox = true;
                        return false;
                    }
                }

                // Restore back focus to the MainWindow
                Utilities.GetMainWindow().Focus();

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

                    return true;
                }
                else
                {
                    Log.Append(string.Format("Installation failed. Installer exited with exit code {0}.", p.ExitCode), Log.LogLevel.ERROR);
                    Status = string.Format("Install failed. Exit code: {0}", p.ExitCode);
                    Progress = 0;
                    IsWaiting = false;
                }
            }

            return false;
        }

        public Application Clone()
        {
            return (Application)MemberwiseClone();
        }
    }
}
