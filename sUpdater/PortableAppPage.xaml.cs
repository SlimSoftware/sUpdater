using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for PortableAppPage.xaml
    /// </summary>
    public partial class PortableAppPage : Page
    {
        private List<PortableApp> installedPortableApps = new List<PortableApp>();

        public PortableAppPage()
        {
            InitializeComponent();
            GetInstalledPortableApps();
            portableAppsListView.ItemsSource = installedPortableApps;
            portableAppsListView.Focus();
        }

        /// <summary>
        /// Fills the installedPortableApps list with portable apps that are installed
        /// </summary>
        public void GetInstalledPortableApps()
        {
            installedPortableApps.Clear();
            XDocument defenitions;

            // Load XML File
            if (Settings.DefenitionURL != null)
            {
                defenitions = XDocument.Load(Settings.DefenitionURL);
            }
            else
            {
                Log.Append("Using official definitions", Log.LogLevel.INFO);
                defenitions = XDocument.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");
            }

            foreach (XElement portableAppElement in defenitions.Elements("portable"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = portableAppElement.Attribute("name");
                XElement versionElement = portableAppElement.Element("version");
                XElement changelogElement = portableAppElement.Element("changelog");
                XElement descriptionElement = portableAppElement.Element("description");
                XElement archElement = portableAppElement.Element("arch");
                XElement launchElement = portableAppElement.Element("launch");
                XElement dlElement = portableAppElement.Element("dl");
                XElement savePathElement = portableAppElement.Element("savepath");
                XElement extractModeElement = portableAppElement.Element("extractmode");

                // Check if Portable App is already installed
                // TODO: Get local version of portable app if installed
                string localVersion = "-";

                // Add app to portableAppList
                installedPortableApps.Add(new PortableApp(nameAttribute.Value, versionElement.Value,
                    localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                    extractModeElement.Value));
            }

            portableAppsListView.Items.Refresh();
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StartPage.ReadDefenitions();
            GetInstalledPortableApps();
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            bool installFailed = false;
            Log.Append("New app installation started...", Log.LogLevel.INFO);
            refreshButton.IsEnabled = false;
            installButton.IsEnabled = false;
            statusLabel.Visibility = Visibility.Hidden;

            if (portableAppsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("You have not selected any applications.");
                Log.Append("No application(s) selected to install, aborting...", Log.LogLevel.WARN);
            }
            else
            {
                // Download
                List<Task> tasks = new List<Task>();
                int currentApp = 0;

                foreach (PortableApp app in portableAppsListView.SelectedItems)
                {
                    currentApp++;
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, portableAppsListView.SelectedItems.Count), Log.LogLevel.INFO);

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
                foreach (PortableApp app in portableAppsListView.SelectedItems)
                {
                    currentApp++;
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentApp, portableAppsListView.SelectedItems.Count), Log.LogLevel.INFO);
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
                    statusLabel.Content = "Some applications failed to install.";
                    statusLabel.Visibility = Visibility.Visible;
                }
                if (installFailed == false)
                {
                    installButton.IsEnabled = true;
                    StartPage.ReadDefenitions();
                    GetInstalledPortableApps();
                }

                refreshButton.IsEnabled = true;
            }
        }
    }
}
