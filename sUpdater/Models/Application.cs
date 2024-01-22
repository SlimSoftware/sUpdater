using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using Ionic.Zip;
using sUpdater.Models.DTO;

namespace sUpdater.Models
{
    public class Application : INotifyPropertyChanged
    {
        public int Id { get; }
        public ImageSource Icon { get; set; }
        public string Name { get; set; }
        public string LatestVersion { get; }
        public string LocalVersion { get; set; }

        /// <summary>
        /// The version displayed under the app's name
        /// </summary>
        public string DisplayedVersion { get; set; }

        public string WebsiteUrl { get; }
        public string ReleaseNotesUrl { get; }
        public bool NoUpdate { get; }
        public Installer Installer { get; }
        public string SavePath { get; set; }
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

        public Application(ApplicationDTO applicationDTO, Installer installer)
        {
            Id = applicationDTO.Id;
            Name = applicationDTO.Name;
            LatestVersion = applicationDTO.Version;
            NoUpdate = applicationDTO.NoUpdate;
            WebsiteUrl = applicationDTO.WebsiteUrl;
            ReleaseNotesUrl = applicationDTO.ReleaseNotesUrl;
            Installer = installer;
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
                        Status = string.Format(
                            "Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                    };
                    wc.DownloadFileCompleted += (s, args) =>
                    {
                        Status = "Download complete";
                    };
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(Installer.DownloadLinkParsed), SavePath);
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
                    string extractedFilename;
                    try
                    {
                        extractedFilename = await Extract();
                    } 
                    catch (Exception e)
                    {
                        Status = "Extracting failed";
                        Progress = 100;
                        Log.Append($"Extracting failed: {e.Message}", Log.LogLevel.ERROR);
                        return;
                    }

                    p.StartInfo.FileName = Path.Combine(Utilities.Settings.DataDir, extractedFilename);
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
                        return;
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

        private Task<string> Extract()
        {
            return Task.Run(() =>
            {
                string extractedFileName = "";
                Status = "Extracting...";
                IsWaiting = true;

                using (ZipFile zip = ZipFile.Read(SavePath))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        // Extract the entry if the file is an installer
                        string extention = Path.GetExtension(entry.FileName);
                        if (extention == ".exe" || extention == ".msi")
                        {
                            entry.Extract(Utilities.Settings.DataDir, ExtractExistingFileAction.DoNotOverwrite);
                            extractedFileName = entry.FileName;
                            break;
                        }
                    }
                }

                if (extractedFileName == "")
                {
                    throw new InvalidOperationException("Did not find an installer in the archive to extract");
                }

                IsWaiting = false;
                return extractedFileName;
            });
        }

        /// <summary>
        /// Opens the changelog of this app in the default browser
        /// </summary>
        public void OpenChangelog()
        {
            string changelogRedirectURL = $"{Utilities.GetAppServerURL()}/changelog?id={Id}";
            Process.Start(changelogRedirectURL);
        }

        /// <summary>
        /// Opens the website of this app in the default browser
        /// </summary>
        public void OpenWebsite()
        {
            string websiteRedirectURL = $"{Utilities.GetAppServerURL()}/website?id={Id}";
            Process.Start(websiteRedirectURL);
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
                   LatestVersion == application.LatestVersion;
        }

        public override int GetHashCode()
        {
            int hashCode = 349623337;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LatestVersion);
            return hashCode;
        }
    }
}
