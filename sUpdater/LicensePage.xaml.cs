using System.Windows.Controls;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for LicensePage.xaml
    /// </summary>
    public partial class LicensePage : Page
    {
        public LicensePage(string creditName)
        {
            InitializeComponent();

            Title = $"{creditName} License";

            if (creditName == "7-Zip")
            {
                licenseTextBox.AppendText(Properties.Resources.SevenZipLicense);
            }
            else if (creditName == "AutoUpdater.NET")
            {
                licenseTextBox.AppendText(Properties.Resources.AutoUpdaterNETLicense);
            }
            else if (creditName == "Inno Setup")
            {
                licenseTextBox.AppendText(Properties.Resources.InnoSetupLicense);
            }
            else if (creditName == "WPF NotifyIcon")
            {
                licenseTextBox.AppendText(Properties.Resources.WPFNotifyIconLicense);
            }
        }
    }
}
