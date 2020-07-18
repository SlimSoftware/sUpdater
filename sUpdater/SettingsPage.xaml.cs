using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using FolderBrowser = System.Windows.Forms.FolderBrowserDialog;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run") ??
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key.GetValue("sUpdater") != null)
                {
                    autoStartCheckBox.IsChecked = true;
                }
            }

            if (Settings.MinimizeToTray == true)
            {
                minimizeToTrayCheckBox.IsChecked = true;
            }

            if (Settings.DataDir != null)
            {
                dataFolderTextBox.Text = Settings.DataDir;
            }

            if (Settings.PortableAppDir != null)
            {
                portableAppsFolderTextBox.Text = Settings.PortableAppDir;
            }

            if (Settings.DefenitionURL != null)
            {
                customDefinitionsTextBox.Text = Settings.DefenitionURL;
                customDefinitionsTextBox.IsEnabled = true;
                officialDefinitionsRadioButton.IsChecked = false;
                customDefinitionsRadioButton.IsChecked = true;
            }
            else
            {
                officialDefinitionsRadioButton.IsChecked = true;
            }
        }

        private void DataFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowser fbd = new FolderBrowser();         
            fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                dataFolderTextBox.Text = fbd.SelectedPath;
            }
        }

        private void OpenDataFolderButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(dataFolderTextBox.Text))
            {
                Process.Start("explorer.exe", dataFolderTextBox.Text);
            }
        }

        private void PortableAppsFolderBrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowser fbd = new FolderBrowser())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    portableAppsFolderTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void OpenPortableAppsFolderButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(portableAppsFolderTextBox.Text))
            {
                Process.Start("explorer.exe", portableAppsFolderTextBox.Text);
            }
        }

        private void OfficialDefinitionsRadioButton_Click(object sender, EventArgs e)
        {
            customDefinitionsTextBox.Text = "";
            customDefinitionsTextBox.IsEnabled = false;
        }

        private void CustomDefinitionsRadioButton_Click(object sender, EventArgs e)
        {
            customDefinitionsTextBox.IsEnabled = true;
        }

        public void SaveSettings()
        {
            // Check if the specified portable apps folder already exists
            if (!Directory.Exists(portableAppsFolderTextBox.Text) && portableAppsFolderTextBox.Text != "")
            {
                try
                {
                    Directory.CreateDirectory(portableAppsFolderTextBox.Text);
                }
                catch
                {
                    portableAppsFolderNotWritableLabel.Visibility = System.Windows.Visibility.Visible;
                    return;
                }
            }

            if (portableAppsFolderTextBox.Text != "")
            {
                if (!IsFolderWriteable(portableAppsFolderTextBox.Text))
                {
                    portableAppsFolderNotWritableLabel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    // Hide previously shown error label if the folder is writeable
                    portableAppsFolderNotWritableLabel.Visibility = System.Windows.Visibility.Collapsed;
                    Settings.PortableAppDir = portableAppsFolderTextBox.Text;
                }
            }
            else
            {
                Settings.PortableAppDir = null;
            }

            // Check if the specified data folder already exists
            if (!Directory.Exists(dataFolderTextBox.Text) && dataFolderTextBox.Text != "")
            {
                try
                {
                    Directory.CreateDirectory(dataFolderTextBox.Text);
                }
                catch
                {
                    dataFolderNotWriteableLabel.Visibility = System.Windows.Visibility.Visible;
                    return;
                }
            }

            if (dataFolderTextBox.Text != "")
            {
                if (!IsFolderWriteable(dataFolderTextBox.Text))
                {
                    dataFolderNotWriteableLabel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    // Hide previously shown error label if the folder is writeable
                    dataFolderNotWriteableLabel.Visibility = System.Windows.Visibility.Collapsed;
                    Settings.DataDir = dataFolderTextBox.Text;
                }
            }
            else
            {
                Settings.DataDir = null;
            }

            if (customDefinitionsRadioButton.IsChecked == true && customDefinitionsTextBox.Text != "")
            {
                Settings.DefenitionURL = customDefinitionsTextBox.Text;
            }
            if (customDefinitionsRadioButton.IsChecked == true && customDefinitionsTextBox.Text == "")
            {
                MessageBox.Show("You must specify a custom definition URL or use the official defentions");
            }
            if (officialDefinitionsRadioButton.IsChecked == true)
            {
                Settings.DefenitionURL = null;
            }

            if (autoStartCheckBox.IsChecked == true)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    key.SetValue("sUpdater", "\"" +
                        System.Reflection.Assembly.GetExecutingAssembly().Location.ToString()
                        + "\"" + " /tray");
                }
            }

            if (minimizeToTrayCheckBox.IsChecked == true)
            {
                Settings.MinimizeToTray = true;
            }
            else
            {
                Settings.MinimizeToTray = false;
            }

            Settings.Save();
        }

        /// <summary>
        /// Tests if the specified folder is writeable
        /// </summary>
        /// <param name="path">The folder to check</param>
        private bool IsFolderWriteable(string path)
        {
            try
            {
                File.Create(Path.Combine(path, "sUpdater Tempfile")).Close();
                File.Delete(Path.Combine(path, "sUpdater Tempfile"));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveSettings();
        }
    }
}
