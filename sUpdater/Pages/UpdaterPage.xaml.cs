using sUpdater.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        public UpdaterPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await PopulateListAsync();
            updateListView.SelectAll();
        }

        private async Task PopulateListAsync()
        {
            updateListView.ItemsSource = await AppController.GetUpdateInfo();

            if (AppController.GetUpdateCount() == 0)
            {
                await SetupDetailsMode();
            }
        }

        /// <summary>
        /// Sets up some visual tweaks for the details mode
        /// </summary>
        private async Task SetupDetailsMode()
        {
            updateListView.ItemsSource = await AppController.GetAppInfo();

            foreach (Application app in updateListView.ItemsSource)
            {
                app.Checkbox = false;

                if (app.LocalVersion != null)
                {
                    app.Name = app.Name + " " + app.LatestVersion;
                    if (app.Type == Type.NoUpdate)
                    {
                        app.DisplayedVersion = "Installed: " + app.LocalVersion + " (Using own updater)";
                    }
                    else
                    {
                        app.DisplayedVersion = "Installed: " + app.LocalVersion;
                    }
                }
                else
                {
                    app.Name = app.Name + " " + app.LatestVersion;
                    app.DisplayedVersion = "Not Found";
                }
            }

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
            bool installFailed = false;
            Log.Append("Update installation started...", Log.LogLevel.INFO);
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            selectAllCheckBox.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (updateListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any updates to install.", "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Append("No updates selected to install, aborting...", Log.LogLevel.ERROR);

                refreshButton.IsEnabled = true;
                installButton.IsEnabled = true;
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
                List<Task> tasks = new List<Task>();
                int currentApp = 0;

                foreach (Application app in updateListView.SelectedItems)
                {
                    currentApp++;
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, updateListView.SelectedItems.Count), Log.LogLevel.INFO);

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
                foreach (Application app in updateListView.SelectedItems)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, updateListView.SelectedItems.Count), Log.LogLevel.INFO);
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

                if (installFailed)
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
                else
                {
                    updateListView.ItemsSource = await AppController.GetUpdateInfo();

                    if (AppController.GetUpdateCount() == 0)
                    {
                        noUpdatesAvailablePanel.Visibility = Visibility.Visible;
                        installButton.Visibility = Visibility.Collapsed;
                        selectAllCheckBox.Visibility = Visibility.Collapsed;
                        updateListView.Visibility = Visibility.Hidden;
                    }
                }

                selectAllCheckBox.IsEnabled = true;
                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;

                MainWindow mainWindow = Utilities.GetMainWindow();
                mainWindow.UpdateTaskbarIcon();
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
            if (updateListView.SelectedItems.Count == AppController.GetUpdateCount() && selectAllCheckBox.IsChecked == false)
            {
                selectAllCheckBox.IsChecked = true;
            }
            else if (updateListView.SelectedItems.Count != AppController.GetUpdateCount() && selectAllCheckBox.IsChecked == true)
            {
                selectAllCheckBox.IsChecked = false;
            }
        }

        private void ItemChangelog_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Application app = (Application)menuItem.DataContext;

            string changelog = app.GetChangelog();
            if (changelog != "")
            {
                InfoPage infoPage = new InfoPage(changelog, InfoPage.InfoType.Changelog);
                NavigationService.Navigate(infoPage);
            }
            else
            {
                MessageBox.Show("No changelog is available for this application");
            }
        }

        private async void DetailsLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            noUpdatesAvailablePanel.Visibility = Visibility.Collapsed;
            updateListView.Visibility = Visibility.Visible;
            await SetupDetailsMode();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            updateListView.ItemsSource = await AppController.GetUpdateInfo();
            MainWindow mainWindow = Utilities.GetMainWindow();
            mainWindow.UpdateTaskbarIcon();

            if (AppController.GetUpdateCount() != 0 && selectAllRow.Height == new GridLength(0))
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
            else if (AppController.GetUpdateCount() == 0)
            {
                await SetupDetailsMode();
            }
        }
    }
}
