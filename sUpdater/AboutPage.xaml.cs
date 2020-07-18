using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            versionLabel.Content += Utilities.GetFriendlyVersion(Assembly.GetEntryAssembly().GetName().Version);
        }

        private void CreditsLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CreditsPage creditsPage = new CreditsPage();
            NavigationService.Navigate(creditsPage);
        }

        private void SiteLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://www.slimsoft.tk");
        }
    }
}
