using Dasync.Collections;
using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for GetPortableAppsPage.xaml
    /// </summary>
    public partial class GetPortableAppsPage : Page
    {
        private List<PortableApp> notInstalledPortableApps = new List<PortableApp>();

        public GetPortableAppsPage()
        {
            InitializeComponent();
            PopluatePortableAppsList();
        }

        // (Re)loads the list of not installed portable apps
        private async void PopluatePortableAppsList()
        {
            notInstalledPortableApps = await PortableAppController.GetNotInstalledPortableApps();

            foreach (PortableApp app in notInstalledPortableApps)
            {
                app.DisplayedVersion = app.LatestVersion;

                app.LinkClickCommand = new LinkClickCommand(new Action(async () =>
                {
                    Log.Append($"Starting run once installation for {app.Name}", Log.LogLevel.INFO);

                    installButton.IsEnabled = false;
                    refreshButton.IsEnabled = false;
                    string linkText = app.LinkText;
                    app.LinkText = "";

                    await app.Download();
                    await app.Install();
                    await app.Run();

                    Log.Append("All processes exited. Cleaning up...", Log.LogLevel.INFO);
                    string appDir = Path.Combine(Utilities.Settings.PortableAppDir, app.Name);
                    if (Directory.Exists(Path.Combine(appDir)))
                    {
                        Directory.Delete(appDir, true);
                    }

                    app.LinkText = linkText;
                    installButton.IsEnabled = true;
                    refreshButton.IsEnabled = true;
                }));
            }

            noAppsAvailableLabel.Visibility = notInstalledPortableApps.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            portableAppsListView.ItemsSource = notInstalledPortableApps;
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (PortableApp app in portableAppsListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!portableAppsListView.SelectedItems.Contains(app))
                    {
                        portableAppsListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                portableAppsListView.SelectedItems.Clear();
            }
        }

        private void GetAppsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (portableAppsListView.SelectedItems.Count == notInstalledPortableApps.Count)
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
            await PortableAppController.CheckForPortableApps();
            await PortableAppController.CheckForInstalledPortableApps();
            PopluatePortableAppsList();
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installSucces = true;
            Log.Append("Portable App installation started...", Log.LogLevel.INFO);
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            selectAllCheckBox.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (portableAppsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any Portable Apps to install.", "sUpdater",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Append("No Portable Apps selected to install, aborting...", Log.LogLevel.WARN);

                refreshButton.IsEnabled = true;
                installButton.IsEnabled = true;
                selectAllCheckBox.IsEnabled = true;
            }
            else
            {
                // Remove all not selected apps from the list and remove the checkbox from all selected apps
                List<PortableApp> selectedApps = new List<PortableApp>();
                foreach (PortableApp app in notInstalledPortableApps)
                {
                    if (portableAppsListView.SelectedItems.Contains(app))
                    {
                        app.Checkbox = false;
                        selectedApps.Add(app);
                    }
                }
                portableAppsListView.ItemsSource = selectedApps;

                // Download
                int currentApp = 0;
                await selectedApps.ParallelForEachAsync(async (app) =>
                {
                    currentApp++;
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, selectedApps.Count), Log.LogLevel.INFO);

                    bool success = await app.Download();
                    if (!success) installSucces = false;
                }, maxDegreeOfParallelism: 3);

                // Install
                currentApp = 0;
                foreach (PortableApp app in selectedApps)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, portableAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
                        bool success = await app.Install();

                        if (!success) installSucces = false;
                    }
                }

                if (installSucces)
                {
                    await PortableAppController.CheckForInstalledPortableApps();
                    portableAppsListView.ItemsSource = await PortableAppController.GetNotInstalledPortableApps();
                }
                else
                {
                    // Only show the failed apps
                    List<PortableApp> failedApps = new List<PortableApp>();
                    foreach (PortableApp app in portableAppsListView.SelectedItems)
                    {
                        if (app.Status != "Install complete")
                        {
                            failedApps.Add(app);
                        }
                    }
                    portableAppsListView.ItemsSource = failedApps;
                    statusLabel.Foreground = Brushes.Red;
                    statusLabel.Content = "Some Portable Apps failed to install.";
                    statusLabel.Visibility = Visibility.Visible;
                }

                selectAllCheckBox.IsEnabled = true;
                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            }
        }
    }
}
