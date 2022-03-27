using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using Ionic.Zip;

namespace sUpdater.Models
{
    public class Application : INotifyPropertyChanged
    {
        public int Id { get; }
        public ImageSource Icon { get; set; }
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string ExePath { get; private set; }
        public string DisplayedVersion { get; set; } // The version displayed under the app's name
        public bool HasChangelog { get; set; }
        public bool HasDescription { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string DownloadLink { get; set; }
        public string SavePath { get; set; }
        public string InstallSwitch { get; set; }
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

        public Application(int id, string name, string latestVersion, string localVersion, string exePath,
            string arch, string type, string installSwitch, string downloadLink, string savePath = null)
        {
            Id = id;
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            ExePath = exePath;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DownloadLink = downloadLink;
            SavePath = savePath;
        }

        public async Task Download()
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
                    return;
                }
            }

            string fileName = Path.GetFileName(DownloadLink);
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
                if (!SavePath.EndsWith(".msi"))
                {
                    p.StartInfo.FileName = SavePath;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = InstallSwitch;
                }
                else
                {
                    p.StartInfo.FileName = "msiexec";
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = $"/i {SavePath} {InstallSwitch}";
                }

                if (SavePath.EndsWith(".zip"))
                {
                    await Task.Run(() =>
                    {
                        using (ZipFile zip = ZipFile.Read(SavePath))
                        {
                            Status = "Extracting...";

                            foreach (ZipEntry entry in zip)
                            {
                                // Extract the entry if the filename is the same as the name of the zip file
                                if (Path.GetFileNameWithoutExtension(entry.FileName) == Path.GetFileNameWithoutExtension(SavePath))
                                {
                                    p.StartInfo.FileName = Path.Combine(Utilities.Settings.DataDir, entry.FileName);

                                    try
                                    {
                                        entry.Extract(Utilities.Settings.DataDir, ExtractExistingFileAction.DoNotOverwrite);
                                    }
                                    catch (Exception e)
                                    {
                                        Status = $"Extracting failed";
                                        Log.Append($"Extracting failed: {e.Message}", Log.LogLevel.ERROR);
                                        return;
                                    }
                                }
                            }
                        }
                    });
                }

                try
                {
                    p.Start();
                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show(
                        "Lauching the installer failed. \nWould you like to try again?",
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
                else
                {
                    Log.Append(string.Format("Installation failed. Installer exited with " +
                        "exit code {0}.", p.ExitCode), Log.LogLevel.ERROR);
                    Status = string.Format(
                        "Install failed. Exit code: {0}", p.ExitCode);
                    Progress = 0;
                    IsWaiting = false;
                }
            }
        }

        public string GetChangelog()
        {
            string changelogText = "";
            string definitionURL = Utilities.GetDefinitionURL();

            using (WebClient client = new WebClient())
            {
                try
                {
                    changelogText = client.DownloadString($"{definitionURL}/changelog?id={Id}");
                }
                catch (Exception ex)
                {
                    Log.Append($"Failed to fetch changelog for {Name}, id {Id}: {ex.Message}", Log.LogLevel.ERROR);
                }              
            }

            if (changelogText != null)
            {
                Utilities.RemoveLeadingNewLinesAndTabs(changelogText);
            }

            return changelogText;
        }

        public string GetDescription()
        {
            string descriptionText = null;
            string definitionURL = Utilities.GetDefinitionURL();

            using (WebClient client = new WebClient())
            {
                try
                {
                    descriptionText = client.DownloadString($"{definitionURL}/description?id={Id}");
                }
                catch (Exception ex)
                {
                    Log.Append($"Failed to fetch changelog for {Name}, id {Id}: {ex.Message}", Log.LogLevel.ERROR);
                }
            }

            if (descriptionText != null)
            {
                Utilities.RemoveLeadingNewLinesAndTabs(descriptionText);
            }

            return descriptionText;
        }

        public Application Clone()
        {
            return (Application)MemberwiseClone();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override bool Equals(object obj)
        {
            return obj is Application application &&
                   Name == application.Name &&
                   LatestVersion == application.LatestVersion &&
                   Arch == application.Arch;
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
