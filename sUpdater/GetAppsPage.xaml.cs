using Dasync.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sUpdater
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

            if (notInstalledApps.Count == 0)
            {
                noAppsAvailableLabel.Visibility = Visibility.Visible;
            }
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

            if (notInstalledApps.Count == 0)
            {
                noAppsAvailableLabel.Visibility = Visibility.Visible;
            }
            else if (noAppsAvailableLabel.Visibility == Visibility.Visible)
            {
                noAppsAvailableLabel.Visibility = Visibility.Collapsed;
            }
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installFailed = false;
            Log.Append("New app installation started...", Log.LogLevel.INFO);

            statusLabel.Visibility = Visibility.Hidden;

            if (getAppsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any applications to install.", "sUpdater", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Append("No applications selected to install, aborting...", Log.LogLevel.ERROR);

                refreshButton.IsEnabled = true;
                installButton.IsEnabled = true;
                selectAllCheckBox.IsEnabled = true;
            }
            else
            {
                refreshButton.IsEnabled = false;
                installButton.IsEnabled = false;
                selectAllCheckBox.IsEnabled = false;

                // Remove all not selected apps from the list and remove the checkbox from all selected apps
                List<Application> selectedApps = new List<Application>();
                foreach (Application a in notInstalledApps)
                {
                    if (getAppsListView.SelectedItems.Contains(a))
                    {
                        a.Checkbox = false;
                        selectedApps.Add(a);
                    }
                }
                getAppsListView.ItemsSource = selectedApps;

                // Download
                int currentApp = 0;
                await selectedApps.ParallelForEachAsync(async (app) =>
                {
                    currentApp++;
                    Dispatcher.Invoke(() =>
                    {
                        Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, selectedApps.Count), Log.LogLevel.INFO);
                    });
                    await app.Download();
                }, maxDegreeOfParallelism: 3);                

                // Install
                currentApp = 0;
                foreach (Application app in selectedApps)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, getAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
                        await app.Install();
                    }
                }

                Utilities.CleanupInstallers();

                if (installFailed)
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
                else
                {
                    StartPage.ReadDefenitions();
                    GetNotInstalledApps();
                    getAppsListView.ItemsSource = notInstalledApps;
                }

                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
                selectAllCheckBox.IsEnabled = true;
            }
        }

        private void ItemDescription_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Application app = (Application)menuItem.DataContext;

            string description = app.GetDescription();
            if (description != "")
            {
                InfoPage infoPage = new InfoPage(description, InfoPage.InfoType.Changelog);
                NavigationService.Navigate(infoPage);
            }
            else
            {
                MessageBox.Show("No description is available for this application");
            }
        }
    }
}
