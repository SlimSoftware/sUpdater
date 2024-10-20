using sUpdater.Controllers;
using sUpdater.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for INstalledPortableAppsPage.xaml
    /// </summary>
    public partial class InstalledPortableAppsPage : Page
    {
        private List<PortableApp> installedPortableApps = new List<PortableApp>();
        private bool navigateHandlerAttached = false;

        public InstalledPortableAppsPage()
        {
            InitializeComponent();
            PopulateInstalledAppsList();
        }

        // (Re)loads the list of installed portable apps
        private async void PopulateInstalledAppsList()
        {
            await PortableAppController.CheckForInstalledPortableApps();

            installedPortableApps = await PortableAppController.GetInstalledPortableApps();
            foreach (PortableApp portableApp in installedPortableApps)
            {
                portableApp.Checkbox = false;
                portableApp.Progress = 0;
                portableApp.Status = "";
            }

            portableAppsListView.ItemsSource = null;
            portableAppsListView.ItemsSource = installedPortableApps;
            noAppsInstalledLabel.Visibility = installedPortableApps.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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
                string appDir = Path.Combine(Utilities.Settings.PortableAppDir, app.Name);
                if (Directory.Exists(Path.Combine(appDir)))
                {
                    Directory.Delete(appDir, true);
                }
            }

            PopulateInstalledAppsList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!navigateHandlerAttached)
            {
                NavigationService.Navigating += NavigationService_Navigating;
                navigateHandlerAttached = true;
            }
        }

        private void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                // Refresh list when going back from get portable apps page
                PopulateInstalledAppsList();
            }
        }
    }
}
