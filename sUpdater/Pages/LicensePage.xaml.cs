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

            if (creditName == "AutoUpdater.NET")
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
            else if (creditName == "AsyncEnumerable")
            {
                licenseTextBox.AppendText(Properties.Resources.AsyncEnumerableLicense);
            }
            else if (creditName == "AsyncEnumerable")
            {
                licenseTextBox.AppendText(Properties.Resources.AsyncEnumerableLicense);
            }
            else if (creditName == "DotNetZip")
            {
                licenseTextBox.AppendText(Properties.Resources.DotNetZipLicense);
            }
        }
    }
}
