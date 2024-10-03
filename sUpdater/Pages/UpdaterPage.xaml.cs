using sUpdater.Controllers;
using Dasync.Collections;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Application = sUpdater.Models.Application;
using System.Linq;
using System.Diagnostics;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for UpdaterPage.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        public UpdaterPage()
        {
            InitializeComponent();

            updateListView.ItemsSource = AppController.Updates;
            if (AppController.Updates.Count > 0)
            {
                updateListView.SelectAll();
            }
            else
            {
                SetupDetailsMode();
            }
        }

        /// <summary>
        /// Sets up some visual tweaks for the details mode
        /// </summary>
        private void SetupDetailsMode()
        {
            var detailApps = AppController.Apps.Select(x => x.Clone()).ToList();

            foreach (Application app in detailApps)
            {
                app.Checkbox = false;
                app.Name = $"{app.Name} {app.LatestVersion}";

                if (app.LocalVersion != null)
                {
                    app.DisplayedVersion = $"Installed: {app.LocalVersion}{(app.NoUpdate ? " (using own updater)" : "")}";
                }
                else
                {
                    app.DisplayedVersion = "Not Found";
                }
            }

            updateListView.ItemsSource = detailApps;
            Title = "Details";

            // Do not allow the user to select items
            updateListView.SelectionChanged += PreventSelectionHandler;

            // Hide select all checkbox and bottom buttons for details view
            selectAllCheckBox.Visibility = Visibility.Collapsed;
            selectAllRow.Height = new GridLength(0);
            installButton.Visibility = Visibility.Collapsed;
        }

        private void PreventSelectionHandler(object sender, EventArgs e)
        {
            updateListView.SelectedItems.Clear();
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Append("Update installation started...", Log.LogLevel.INFO);
            bool installSuccess = true;
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            selectAllCheckBox.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (updateListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any updates to install.", "sUpdater",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Append("No updates selected to install, aborting...", Log.LogLevel.ERROR);

                refreshButton.IsEnabled = true;
                installButton.IsEnabled = true;
                selectAllCheckBox.IsEnabled = true;
            }
            else
            {
                // Remove all not selected apps from the list and remove the checkbox from all selected apps
                List<Application> selectedApps = new List<Application>();
                foreach (Application a in updateListView.ItemsSource)
                {
                    if (updateListView.SelectedItems.Contains(a))
                    {
                        a.Checkbox = false;
                        selectedApps.Add(a);
                    }
                }
                updateListView.ItemsSource = selectedApps;

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
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, updateListView.SelectedItems.Count), Log.LogLevel.INFO);
                        bool success = await app.Install();

                        if (!success) installSuccess = false;
                    }
                }

                if (installSuccess)
                {
                    await AppController.CheckForUpdates();
                    updateListView.ItemsSource = AppController.Updates;

                    if (AppController.Updates.Count == 0)
                    {
                        noUpdatesAvailablePanel.Visibility = Visibility.Visible;
                        installButton.Visibility = Visibility.Collapsed;
                        selectAllCheckBox.Visibility = Visibility.Collapsed;
                        updateListView.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    // Only show the failed apps
                    List<Application> failedApps = new List<Application>();
                    foreach (Application app in updateListView.SelectedItems)
                    {
                        if (app.Status != "Install complete")
                        {
                            failedApps.Add(app);
                        }
                    }
                    updateListView.ItemsSource = failedApps;
                    statusLabel.Foreground = Brushes.Red;
                    statusLabel.Content = "Some applications failed to install.";
                    statusLabel.Visibility = Visibility.Visible;
                }

                selectAllCheckBox.IsEnabled = true;
                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                updateListView.SelectAll();
            }
            else
            {
                updateListView.UnselectAll();
            }
        }

        private void ListViewItem_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get app associated with the listview item
            Application app = ((ListViewItem)sender).Content as Application;

            if (app.Checkbox == true)
            {
                if (updateListView.SelectedItems.Contains(app))
                {
                    updateListView.SelectedItems.Remove(app);
                }
                else
                {
                    updateListView.SelectedItems.Add(app);
                }
            }
        }

        private void UpdateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateListView.SelectedItems.Count == AppController.Updates.Count && selectAllCheckBox.IsChecked == false)
            {
                selectAllCheckBox.IsChecked = true;
            }
            else if (updateListView.SelectedItems.Count != AppController.Updates.Count && selectAllCheckBox.IsChecked == true)
            {
                selectAllCheckBox.IsChecked = false;
            }
        }

        private void MenuItemReleaseNotes_Click(object sender, RoutedEventArgs e)
        {
            Application app = Utilities.GetApplicationFromControl(sender);
            Process.Start(app.ReleaseNotesUrl);
        }

        private void MenuItemWebsite_Click(object sender, RoutedEventArgs e)
        {
            Application app = Utilities.GetApplicationFromControl(sender);
            Process.Start(app.WebsiteUrl);
        }

        private async void MenuItemForceInstall_Click(object sender, RoutedEventArgs e)
        {
            Application app = Utilities.GetApplicationFromControl(sender);

            await app.Download();
            await app.Install();
        }

        private void DetailsLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            noUpdatesAvailablePanel.Visibility = Visibility.Collapsed;
            updateListView.Visibility = Visibility.Visible;
            SetupDetailsMode();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await AppController.CheckForUpdates();
            updateListView.ItemsSource = AppController.Updates;

            if (AppController.Updates.Count > 0 && selectAllRow.Height == new GridLength(0))
            {
                // If the selectAllRow height is 0, the details mode is shown so restore the normal view
                selectAllCheckBox.Visibility = Visibility.Visible;
                installButton.Visibility = Visibility.Visible;
                selectAllRow.Height = new GridLength(25);
                buttonsRow.Height = new GridLength(50);
                Title = "Updates";

                // Allow the user to select items again (not possible in details mode)
                updateListView.SelectionChanged -= PreventSelectionHandler;

                updateListView.SelectAll();
            }
            else
            {
                SetupDetailsMode();
            }
        }
    }
}
