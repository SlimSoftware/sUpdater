using sUpdater.Models;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for AppUpdatePage.xaml
    /// </summary>
    public partial class AppUpdatePage : Page
    {
        private readonly AppUpdateInfo updateInfo;

        public AppUpdatePage(AppUpdateInfo appUpdateInfo)
        {
            InitializeComponent();
            updateInfo = appUpdateInfo;
            DataContext = appUpdateInfo.App;

            using (var stringReader = new StringReader(appUpdateInfo.ChangelogRawText))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    FlowDocument flowDoc = (FlowDocument)XamlReader.Load(xmlReader);
                    flowDoc.FontFamily = new FontFamily("Segoe UI");
                    flowDoc.FontSize = 15;
                    releaseNotesTextBox.Document = flowDoc;

                    foreach (Hyperlink link in Utilities.FindLogicalChildren<Hyperlink>(flowDoc))
                    {
                        link.RequestNavigate += Hyperlink_RequestNavigate;
                    }
                }
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void IgnoreUpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void InstallUpdateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            buttonsStackPanel.Visibility = System.Windows.Visibility.Hidden;
            installStackPanel.Visibility = System.Windows.Visibility.Visible;

            await updateInfo.App.Download();
            bool succes = await updateInfo.App.Install();

            if (!succes)
            {
                buttonsStackPanel.Visibility = System.Windows.Visibility.Visible;
                installStackPanel.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
