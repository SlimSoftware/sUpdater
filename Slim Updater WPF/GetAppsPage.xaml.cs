using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GetAppsPage : Page
    {
        private List<Application> notInstalledApps = new List<Application>();

        public GetAppsPage()
        {
            InitializeComponent();
            GetNotInstalledApps();
            getAppsListView.ItemsSource = notInstalledApps;
        }

        /// <summary>
        /// Fills the notInstalledApps list with apps that are not installed
        /// </summary>
        public void GetNotInstalledApps()
        {
            notInstalledApps.Clear();

            foreach (Application a in Apps.Regular)
            {
                // Create a copy of the app so that the app in the list does not get modified
                Application app = a.Clone();

                // If the LocalVersion is null, then the app is not installed
                if (app.LocalVersion == null)
                {
                    // Ensure the app has a checkbox
                    if (app.Checkbox == false)
                    {
                        app.Checkbox = true;
                    }

                    app.DisplayedVersion = app.LatestVersion;                    
                    notInstalledApps.Add(app);
                }
            }

            getAppsListView.Items.Refresh();
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in getAppsListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!getAppsListView.SelectedItems.Contains(app))
                    {
                        getAppsListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                getAppsListView.SelectedItems.Clear();    
            }
        }

        private void GetAppsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (getAppsListView.SelectedItems.Count == notInstalledApps.Count)
            {
                if (selectAllCheckBox.IsChecked == false)
                {
                    selectAllCheckBox.IsChecked = true;
                }
            }
            else
            {
                if (selectAllCheckBox.IsChecked == true)
                {
                    selectAllCheckBox.IsChecked = false;
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StartPage.ReadDefenitions();
            GetNotInstalledApps();
        }

        private async Task DownloadUpdate(Application app)
        {
            string fileName = Path.GetFileName(app.DownloadLink);
            app.SavePath = Path.Combine(Settings.DataDir, fileName);
            Log.Append("Saving to: " + app.SavePath, Log.LogLevel.INFO);

            // Check if installer is already downloaded
            if (!File.Exists(app.SavePath))
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, args) =>
                    {
                        // Convert download size to mb
                        double recievedSize = Math.Round(args.BytesReceived / 1024d / 1024d, 1);
                        double totalSize = Math.Round(args.TotalBytesToReceive / 1024d / 1024d, 1);

                        app.Progress = args.ProgressPercentage / 2;
                        app.Status = string.Format(
                            "Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);                          
                    };
                    wc.DownloadFileCompleted += (s, args) =>
                    {
                        app.Status = "Download complete";
                    };
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(app.DownloadLink), app.SavePath);
                    }
                    catch (Exception ex)
                    {
                        Log.Append("An error occurred when attempting to download " +
                            "the installer." + ex.Message, Log.LogLevel.ERROR);
                        app.Status = ex.Message;

                        if (File.Exists(app.SavePath))
                        {
                            File.Delete(app.SavePath);
                        }
                        return;
                    }
                }
            }
            else
            {
                app.Progress = 50;              
            }
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installFailed = false;
            Log.Append("New app installation started...", Log.LogLevel.INFO);
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (getAppsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any applications.");
                Log.Append("No application(s) selected to install, aborting...", Log.LogLevel.WARN);
            }
            else
            {
                // Download
                List<Task> tasks = new List<Task>();
                int currentApp = 0;

                foreach (Application app in getAppsListView.SelectedItems)
                {
                    currentApp++;
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, getAppsListView.SelectedItems.Count), Log.LogLevel.INFO);

                    // Do not allow more than 3 downloads at once
                    while (tasks.Count > 2)
                    {
                        await Task.Delay(1000);
                    }
                    tasks.Add(DownloadUpdate(app));
                }
                await Task.WhenAll(tasks);

                // Install
                currentApp = 0;
                foreach (Application app in getAppsListView.SelectedItems)
                {
                    currentApp++;
                    launchInstaller:
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, getAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
                        using (var p = new Process())
                        {
                            if (app.DownloadLink.EndsWith(".exe"))
                            {
                                p.StartInfo.FileName = app.SavePath;
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = app.InstallSwitch;
                            }
                            else if (app.DownloadLink.EndsWith(".msi"))
                            {
                                p.StartInfo.FileName = "msiexec";
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = "\"" + app.InstallSwitch + "\""
                                    + " " + app.SavePath;
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
                                    continue;
                                }
                            }

                            // Restore back focus to the MainWindow
                            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().Focus();

                            app.Status = "Installing...";
                            app.IsWaiting = true;

                            // Wait on a separate thread for the process to exit so the GUI thread does not get blocked
                            await Task.Run(() =>
                            {
                                p.WaitForExit();
                            });

                            if (p.ExitCode == 0)
                            {
                                Log.Append("Installer exited with exit code 0.", Log.LogLevel.INFO);
                                File.Delete(app.SavePath);
                                app.Status = "Install complete";
                                app.IsWaiting = false;
                                app.Progress = 100;
                            }
                            if (p.ExitCode != 0)
                            {
                                Log.Append(string.Format("Installation failed. Installer exited with " +
                                    "exit code {0}.", p.ExitCode), Log.LogLevel.ERROR);
                                app.Status = String.Format(
                                    "Install failed. Exit code: {0}", p.ExitCode);
                                app.Progress = 0;
                                app.IsWaiting = false;
                            }
                        }
                    }
                }

                // Cleanup any leftover exe's in appdata dir
                if (Directory.GetFiles(Settings.DataDir, "*.exe").Length > 0)
                {
                    Log.Append("Cleaning up leftover installers...", Log.LogLevel.INFO);
                    foreach (string exePath in Directory.GetFiles(Settings.DataDir, ".exe"))
                    {
                        File.Delete(exePath);
                    }
                }

                if (installFailed == true)
                {
                    // Only show the failed apps
                    List<Application> failedApps = new List<Application>();
                    foreach (Application app in getAppsListView.SelectedItems)
                    {
                        if (app.Status != "Install complete")
                        {
                            failedApps.Add(app);
                        }
                    }
                    getAppsListView.ItemsSource = failedApps;
                    statusLabel.Foreground = Brushes.Red;
                    statusLabel.Content = "Some applications failed to install.";
                    statusLabel.Visibility = Visibility.Visible;
                }
                if (installFailed == false)
                {
                    installButton.IsEnabled = true;
                    StartPage.ReadDefenitions();
                    GetNotInstalledApps();
                }

                refreshButton.IsEnabled = true;
            }
        }
    }
}
