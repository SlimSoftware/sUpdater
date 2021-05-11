using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for LicensePage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        public enum InfoType { Changelog, Description };
        private int appId { get; set; }
        private InfoType infoType { get; set; }

        public InfoPage(int appId, InfoType infoType)
        {
            InitializeComponent();
            this.appId = appId;
            this.infoType = infoType;

            if (infoType == InfoType.Changelog)
            {
                Title = "Changelog";
            }
            else if (infoType == InfoType.Description)
            {
                Title = "Description";
            }
        }

        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (infoType == InfoType.Changelog)
            {
                Changelog changelog = await AppController.GetChangelog(appId);
                if (changelog.Text != null)
                {
                    infoTextBox.Document.Blocks.Clear();
                    infoTextBox.AppendText(changelog.Text);
                }
                else if (changelog.URL != null)
                {
                    OpenURL(changelog.URL);
                }
            }
            else if (infoType == InfoType.Description)
            {
                Description description = await AppController.GetDescription(appId);
                if (description.Text != null)
                {
                    infoTextBox.Document.Blocks.Clear();
                    infoTextBox.AppendText(description.Text);
                }
                else if (description.URL != null)
                {
                    OpenURL(description.URL);
                }
            }         
        }

        private void OpenURL(string url)
        {
            infoTextBox.Document.Blocks.Clear();
            infoTextBox.AppendText("The webpage has opened in your default browser. Click here to open it manually:");
            Hyperlink link = new Hyperlink();
            link.Inlines.Add(url);
            link.NavigateUri = new Uri(url);
            link.RequestNavigate += (sender, args) => 
            { 
                Process.Start(url); 
                args.Handled = true; // Prevent the textbox navigating to the url as well
            };
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(link);
            infoTextBox.Document.Blocks.Add(paragraph);
            Process.Start(url);
        }
    }
}
