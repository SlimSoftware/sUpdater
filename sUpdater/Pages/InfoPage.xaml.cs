using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        public enum InfoType { Changelog, Description };

        public InfoPage(string text, InfoType infoType)
        {
            InitializeComponent();

            if (infoType == InfoType.Changelog)
            {
                Title = "Changelog";
            }
            else if (infoType == InfoType.Description)
            {
                Title = "Description";
            }

            infoTextBox.AppendText(text);
        }
    }
}
