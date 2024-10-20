using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;

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

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key.GetValue("sUpdater") != null)
                {
                    autoStartCheckBox.IsChecked = true;
                }
            }

            if (Utilities.Settings.MinimizeToTray == true)
            {
                minimizeToTrayCheckBox.IsChecked = true;
            }

            if (Utilities.Settings.DataDir != null)
            {
                dataFolderTextBox.Text = Utilities.Settings.DataDir;
            }

            if (Utilities.Settings.PortableAppDir != null)
            {
                portableAppsFolderTextBox.Text = Utilities.Settings.PortableAppDir;
            }

            if (Utilities.Settings.AppServerURL != null)
            {
                customAppServerTextBox.Text = Utilities.Settings.AppServerURL;
                customAppServerTextBox.IsEnabled = true;
                officialAppServerRadioButton.IsChecked = false;
                customAppServerRadioButton.IsChecked = true;
            }
            else
            {
                officialAppServerRadioButton.IsChecked = true;
            }
        }

        private void DataFolderBrowseButton_Click(object sender, EventArgs e)
        {
            string selectedPath = Utilities.BrowseForFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            if (selectedPath != null)
            {
                dataFolderTextBox.Text = selectedPath;
            }
        }

        private void OpenDataFolderButton_Click(object sender, EventArgs e)
        {
            OpenDirInExplorer(dataFolderTextBox.Text);
        }

        private void PortableAppsFolderBrowseButton_Click(object sender, EventArgs e)
        {
            string selectedPath = Utilities.BrowseForFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            if (selectedPath != null)
            {
                portableAppsFolderTextBox.Text = selectedPath;
            }
        }

        private void OpenPortableAppsFolderButton_Click(object sender, EventArgs e)
        {
            OpenDirInExplorer(portableAppsFolderTextBox.Text);
        }

        private void OfficialAppServerRadioButton_Click(object sender, EventArgs e)
        {
            customAppServerTextBox.Text = "";
            customAppServerTextBox.IsEnabled = false;
        }

        private void CustomAppServerRadioButton_Click(object sender, EventArgs e)
        {
            customAppServerTextBox.IsEnabled = true;
        }

        public void SaveSettings()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (autoStartCheckBox.IsChecked == true && key.GetValue("sUpdater") == null)
                {
                    key.SetValue("sUpdater",
                        $"\"{System.Reflection.Assembly.GetExecutingAssembly().Location}\" /tray");
                }

                if (autoStartCheckBox.IsChecked == false && key.GetValue("sUpdater") != null)
                {
                    key.DeleteValue("sUpdater");
                }
            }

            if (minimizeToTrayCheckBox.IsChecked == true)
            {
                Utilities.Settings.MinimizeToTray = true;
            }
            else
            {
                Utilities.Settings.MinimizeToTray = false;
            }

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
                    Utilities.Settings.PortableAppDir = portableAppsFolderTextBox.Text;
                }
            }
            else
            {
                Utilities.Settings.PortableAppDir = null;
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
                    Utilities.Settings.DataDir = dataFolderTextBox.Text;
                }
            }
            else
            {
                Utilities.Settings.DataDir = null;
            }

            if (officialAppServerRadioButton.IsChecked == true)
            {
                Utilities.Settings.AppServerURL = null;
            }
            else if (customAppServerRadioButton.IsChecked == true)
            {
                if (customAppServerTextBox.Text != "")
                {
                    Utilities.Settings.AppServerURL = customAppServerTextBox.Text;
                }
                else
                {
                    MessageBox.Show("You must specify a custom App Server URL or use the official App Server");
                    return;
                }
            }

            Utilities.InitHttpClient();
            Utilities.SaveSettings();
        }

        /// <summary>
        /// Opens a folder in File Explorer if the folder exists
        /// </summary>
        /// <param name="path">The path to the folder to open</param>
        private void OpenDirInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }

        /// <summary>
        /// Tests if the specified folder is writeable
        /// </summary>
        /// <param name="path">The path to the folder to check</param>
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
