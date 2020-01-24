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
    public partial class InstalledPortableAppPage : Page
    {
        private List<PortableApp> installedPortableApps = new List<PortableApp>();
        private List<PortableApp> portableApps = PortableAppsPage.GetPortableApps();

        public InstalledPortableAppPage()
        {
            InitializeComponent();
            portableAppsListView.ItemsSource = installedPortableApps;
            GetInstalledPortableApps();     
            portableAppsListView.Focus();
        }

        /// <summary>
        /// Fills the installedPortableApps list with portable apps that are installed
        /// </summary>
        public void GetInstalledPortableApps()
        {
            string[] installedAppPaths = null;
            if (Settings.PortableAppDir != null)
            {
                installedAppPaths = Directory.GetDirectories(Settings.PortableAppDir);
            }

            if (installedAppPaths.Length == 0)
            {
                noAppsInstalledLabel.Visibility = Visibility.Visible;
            }
            else
            {
                foreach (string appDirPath in installedAppPaths)
                {
                    PortableApp pAppListItem = new PortableApp(Path.GetFileName(appDirPath), null);
                    installedPortableApps.Add(pAppListItem);

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

                portableAppsListView.Items.Refresh();
            }
        }

        private void GetPortableAppsLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PortableAppsPage portableAppsPage = new PortableAppsPage();
            NavigationService.Navigate(portableAppsPage);
        }
    }
}
