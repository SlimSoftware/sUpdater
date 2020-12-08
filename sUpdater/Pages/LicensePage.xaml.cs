using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for LicensePage.xaml
    /// </summary>
    public partial class LicensePage : Page
    {
        public LicensePage(string license)
        {
            InitializeComponent();

            if (license == "7-Zip")
            {
                Title = "7-Zip License";
                licenseTextBox.AppendText(Properties.Resources.SevenZipLicense);
            }
            else if (license == "AutoUpdater.NET")
            {
                Title = "AutoUpdater.NET License";
                licenseTextBox.AppendText(Properties.Resources.AutoUpdaterNETLicense);
            }
            else if (license == "Inno Setup")
            {
                Title = "Inno Setup License";
                licenseTextBox.AppendText(Properties.Resources.InnoSetupLicense);
            }
        }
    }
}
