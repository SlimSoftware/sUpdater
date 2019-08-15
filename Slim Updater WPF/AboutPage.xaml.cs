using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            versionLabel.Content += Utilities.GetFriendlyVersion();
        }

        private void CreditsLink_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CreditsPage creditsPage = new CreditsPage();
            NavigationService.Navigate(creditsPage);
        }
    }
}
