using AutoUpdaterDotNET;
using System;
using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AppUpdatePage : Page
    {
        private UpdateInfoEventArgs updateInfo;

        public AppUpdatePage(UpdateInfoEventArgs updateInfo)
        {
            InitializeComponent();
            this.updateInfo = updateInfo;
            versionLabel.Content += Utilities.GetFriendlyVersion(new Version(updateInfo.CurrentVersion));
            changelogBrowser.Navigate(updateInfo.ChangelogURL);
        }

        private void IgnoreUpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void InstallUpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AutoUpdater.DownloadUpdate(updateInfo);
        }
    }
}
