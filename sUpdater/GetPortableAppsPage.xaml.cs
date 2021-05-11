using Dasync.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

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
            notInstalledPortableApps = GetNotInstalledPortableApps();    

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

            portableAppsListView.ItemsSource = notInstalledPortableApps;
        }

        /// <summary>
        /// Fills the portableApps list with all Portable Apps from the definitions
        /// </summary>
        public static List<PortableApp> GetPortableApps()
        {
            Log.Append("Getting Portable Apps", Log.LogLevel.INFO);

            List<PortableApp> apps = new List<PortableApp>();
            XDocument definitions;

            // Load XML File
            if (Utilities.Settings.DefenitionURL != null)
            {
                definitions = XDocument.Load(Utilities.Settings.DefenitionURL);
            }
            else
            {            
                definitions = XDocument.Load("https://www.slimsoft.tk/supdater/definitions.xml");
            }

            foreach (XElement portableAppElement in definitions.Descendants("portable"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = portableAppElement.Attribute("name");
                XElement versionElement = portableAppElement.Element("version");
                XElement archElement = portableAppElement.Element("arch");
                XElement launchElement = portableAppElement.Element("launch");
                XElement dlElement = portableAppElement.Element("dl");
                XElement extractModeElement = portableAppElement.Element("extractmode");

                // Check if Portable App is already installed
                // TODO: Get local version of portable app if installed
                string localVersion = "-";

                apps.Add(new PortableApp(nameAttribute.Value, versionElement.Value,
                    localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                    extractModeElement.Value));             
            }

            return apps;
        }

        private List<PortableApp> GetNotInstalledPortableApps()
        {
            List<PortableApp> portableApps = GetPortableApps();
            List<PortableApp> installedPortableApps = InstalledPortableAppsPage.GetInstalledPortableApps();
            List<PortableApp> notInstalledPortableApps = portableApps.Where(
                a => installedPortableApps.All(b => a.Name != b.Name && a.LatestVersion != b.LatestVersion)).ToList();

            return notInstalledPortableApps;
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            notInstalledPortableApps = GetNotInstalledPortableApps();
            // TODO: Properly refresh
            portableAppsListView.ItemsSource = null;
            portableAppsListView.ItemsSource = notInstalledPortableApps;
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installFailed = false;
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
                foreach (PortableApp a in notInstalledPortableApps)
                {
                    if (portableAppsListView.SelectedItems.Contains(a))
                    {
                        a.Checkbox = false;
                        selectedApps.Add(a);
                    }
                }
                portableAppsListView.ItemsSource = selectedApps;

                // Download
                int currentApp = 0;
                await selectedApps.ParallelForEachAsync(async (app) =>
                {
                    currentApp++;
                    Dispatcher.Invoke(() =>
                    {
                        Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                            app.Name, currentApp, portableAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
                    });
                    await app.Download();
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
                        await app.Install();
                    }
                }

                if (installFailed == true)
                {
                    // Only show the failed apps
                    List<Application> failedApps = new List<Application>();
                    foreach (Application app in portableAppsListView.SelectedItems)
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
                else
                {                   
                    notInstalledPortableApps = GetNotInstalledPortableApps();
                    // TODO: Properly refresh
                    portableAppsListView.ItemsSource = null;
                    portableAppsListView.ItemsSource = notInstalledPortableApps;
                }

                selectAllCheckBox.IsEnabled = true;
                installButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            }
        }
    }
}
