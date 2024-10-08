using sUpdater.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
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
            versionLabel.Content += Utilities.GetFriendlyVersion(new Version(appUpdateInfo.App.LocalVersion));
            updateInfo = appUpdateInfo;

            using (var stringReader = new StringReader(appUpdateInfo.ChangelogRawText))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    FlowDocument flowDoc = (FlowDocument)XamlReader.Load(xmlReader);
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
            await updateInfo.App.Download();
            await updateInfo.App.Install();
        }
    }
}
