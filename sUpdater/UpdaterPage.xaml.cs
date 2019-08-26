using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        private ObservableCollection<Application> detailsList;

        public UpdaterPage()
        {
            InitializeComponent();
            
            if (Apps.Updates.Count != 0)
            {
                updateListView.ItemsSource = Apps.Updates;
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
            detailsList = new ObservableCollection<Application>();

            foreach (Application a in Apps.Regular)
            {
                // Create a copy of the app so that the app in the list does not get modified
                Application app = a.Clone();
                app.Checkbox = false;

                if (app.LocalVersion != null)
                {
                    app.Name = app.Name + " " + app.LatestVersion;
                    if (app.Type == "noupdate")
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

                detailsList.Add(app);
            }

            updateListView.ItemsSource = detailsList;
            Title = "Details";

            // Do not allow the user to select items by clicking on them
            updateListView.SelectionChanged += (sender, e) =>
            {
                updateListView.SelectedItems.Clear();
            };

            // Hide select all checkbox and bottom buttons for details view
            selectAllCheckBox.Visibility = Visibility.Hidden;
            installButton.Visibility = Visibility.Hidden;
            refreshButton.Visibility = Visibility.Hidden;

            // Set height of the other rows to 0 so that the row of the listview 
            // takes up all the available space
            selectAllRow.Height = new GridLength(0);
            buttonsRow.Height = new GridLength(0);
            listViewRow.Height = new GridLength(1, GridUnitType.Star);
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installFailed = false;
            Log.Append("Update installation started...", Log.LogLevel.INFO);
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (updateListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any updates.");
                Log.Append("No update(s) selected to install, aborting...", Log.LogLevel.WARN);
            }
            else
            {
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

                if (installFailed == true)
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
                if (installFailed == false)
                {
                    installButton.IsEnabled = true;
                    StartPage.ReadDefenitions();
                    StartPage.CheckForUpdates();

                    if (updateListView.ItemsSource != Apps.Updates)
                    {
                        updateListView.ItemsSource = Apps.Updates;
                    }
                }

                refreshButton.IsEnabled = true;
            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in updateListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!updateListView.SelectedItems.Contains(app))
                    {
                        updateListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                updateListView.SelectedItems.Clear();    
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
            if (updateListView.SelectedItems.Count == Apps.Updates.Count)
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
    }
}
