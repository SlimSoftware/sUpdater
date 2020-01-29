using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
            portableAppsListView.Focus();

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

                pAppListItem.LinkClickCommand = new LinkClickCommand(new Action(() =>
                {
                    using (Process p = new Process())
                    {
                        // TODO: Add support for optional arguments and use shell execute here
                        try
                        {
                            p.StartInfo.FileName = Path.Combine(
                                Settings.PortableAppDir, pAppListItem.Name, app.Launch);
                            p.Start();
                        }
                        catch
                        {
                            MessageBox.Show("Could not run the application. Try reinstalling the app.",
                                "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }));               
            }

            return apps; 
        }

        private void GetPortableAppsLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetPortableAppsPage portableAppsPage = new GetPortableAppsPage();
            NavigationService.Navigate(portableAppsPage);
        }
    }
}
