using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for LicensePage.xaml
    /// </summary>
    public partial class LicensePage : Page
    {
        public LicensePage()
        {
            InitializeComponent();
            licenseTextBox.AppendText(Properties.Resources.ThirdPartyNotices);
        }
    }
}
