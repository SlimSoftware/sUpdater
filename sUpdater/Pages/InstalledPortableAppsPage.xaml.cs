using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for PortableAppPage.xaml
    /// </summary>
    public partial class InstalledPortableAppsPage : Page
    {
        private List<PortableApp> installedPortableApps = new List<PortableApp>();
        private static List<PortableApp> portableApps = GetPortableAppsPage.GetPortableApps();

        public InstalledPortableAppsPage()
        {
            InitializeComponent();

            installedPortableApps = GetInstalledPortableApps();
            portableAppsListView.ItemsSource = installedPortableApps;

            if (installedPortableApps.Count == 0)
            {
                noAppsInstalledLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Fills the installedPortableApps list with portable apps that are installed
        /// </summary>
        public static List<PortableApp> GetInstalledPortableApps()
        {
            Log.Append("Checking for installed Portable Apps", Log.LogLevel.INFO);
            List<PortableApp> apps = new List<PortableApp>();

            string[] installedAppPaths = null;
            if (Settings.PortableAppDir != null)
            {
                installedAppPaths = Directory.GetDirectories(Settings.PortableAppDir);
            }

            foreach (string appDirPath in installedAppPaths)
            {
                PortableApp pAppListItem = new PortableApp(Path.GetFileName(appDirPath), null);
                pAppListItem.Checkbox = false;
                apps.Add(pAppListItem);

                // Find associated PortableApp
                PortableApp app = portableApps.Find(x => x.Name == pAppListItem.Name);

                pAppListItem.LinkClickCommand = new LinkClickCommand(new Action(async () =>
                {
                    string linkText = pAppListItem.LinkText;
                    pAppListItem.LinkText = "";
                    pAppListItem.Status = "Running...";
                    await app.Run();

                    pAppListItem.Status = "";
                    pAppListItem.LinkText = linkText;
                }));               
            }

            return apps; 
        }

        private void GetPortableAppsLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetPortableAppsPage portableAppsPage = new GetPortableAppsPage();
            NavigationService.Navigate(portableAppsPage);
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            PortableApp app = (PortableApp)menuItem.DataContext;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {app.Name}, including all of its settings and data?", 
                "sUpdater", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Log.Append($"Deleting Portable App: {app.Name}", Log.LogLevel.INFO);
                string appDir = Path.Combine(Settings.PortableAppDir, app.Name);
                if (Directory.Exists(Path.Combine(appDir)))
                {
                    Directory.Delete(appDir, true);
                }
            }

            installedPortableApps = GetInstalledPortableApps();
            // TODO: Properly refresh
            portableAppsListView.ItemsSource = null;
            portableAppsListView.ItemsSource = installedPortableApps;

            if (installedPortableApps.Count == 0)
            {
                noAppsInstalledLabel.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigating += NavigationService_Navigating;
        }

        private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            
            if (e.NavigationMode == NavigationMode.Back)
            {
                // Refresh list when going back from other page
                installedPortableApps = GetInstalledPortableApps();
                portableAppsListView.ItemsSource = installedPortableApps;
            }
        }
    }
}
