using Dasync.Collections;
using sUpdater.Controllers;
using sUpdater.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Application = sUpdater.Models.Application;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GetAppsPage : Page
    {
        public GetAppsPage()
        {
            InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetNotInstalledApps();

            if (getAppsListView.Items.Count == 0)
            {
                noAppsAvailableLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Fills the list with apps that are not installed
        /// </summary>
        public async Task GetNotInstalledApps(bool refresh = false)
        {
            if (refresh) await AppController.CheckForInstalledApps();

            getAppsListView.ItemsSource = await AppController.GetNotInstalledApps();
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
            if (getAppsListView.SelectedItems.Count == getAppsListView.Items.Count)
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

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await GetNotInstalledApps(true);

            if (getAppsListView.Items.Count == 0)
            {
                noAppsAvailableLabel.Visibility = Visibility.Visible;
            }
            else if (noAppsAvailableLabel.Visibility == Visibility.Visible)
            {
                noAppsAvailableLabel.Visibility = Visibility.Collapsed;
            }

            if (statusLabel.Visibility == Visibility.Visible)
            {
                statusLabel.Visibility = Visibility.Hidden;
            }
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Append("New app installation started...", Log.LogLevel.INFO);

            bool installSuccess = true;
            statusLabel.Visibility = Visibility.Hidden;

            if (getAppsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any applications to install.",
                    "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
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
                foreach (Application app in getAppsListView.ItemsSource)
                {
                    if (getAppsListView.SelectedItems.Contains(app))
                    {
                        app.Checkbox = false;
                        selectedApps.Add(app);
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
                    bool success = await app.Download();

                    if (!success) installSuccess = false;
                }, maxDegreeOfParallelism: 3);

                // Install
                currentApp = 0;
                foreach (Application app in selectedApps)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append($"Installing {app.Name} ({currentApp} of {getAppsListView.SelectedItems.Count}) ...",
                            Log.LogLevel.INFO);

                        bool success = await app.Install();
                        if (!success) installSuccess = false;
                    }
                }

                if (installSuccess)
                {
                    await GetNotInstalledApps(true);
                }
                else
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

                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
                selectAllCheckBox.IsEnabled = true;
            }
        }

        private void MenuItemWebsite_Click(object sender, RoutedEventArgs e)
        {
            Application app = Utilities.GetApplicationFromControl(sender);
            Utilities.OpenWebLink(app.WebsiteUrl);
        }
    }
}
