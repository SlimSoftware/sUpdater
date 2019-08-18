using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
                    tasks.Add(app.Download());
                }
                await Task.WhenAll(tasks);

                // Install
                currentApp = 0;
                foreach (Application app in getAppsListView.SelectedItems)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, getAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
                        await app.Install();
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
