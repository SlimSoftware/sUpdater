using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            UpdateGUI();            
        }

        #region ReadDefenitions()
        public static bool ReadDefenitions()
        {
            Log.Append("Reading definitions file", Log.LogLevel.INFO);
            Apps.Regular = new ObservableCollection<Application>();
            XDocument defenitions;

            // Load XML File
            try
            {
                if (Utilities.Settings.DefenitionURL != null)
                {
                    Log.Append("Using custom definition file from " + Utilities.Settings.DefenitionURL,
                        Log.LogLevel.INFO);
                    defenitions = XDocument.Load(Utilities.Settings.DefenitionURL);
                }
                else
                {
                    Log.Append("Using official definitions", Log.LogLevel.INFO);
                    defenitions = XDocument.Load("https://www.slimsoft.tk/supdater/definitions.xml");
                }
            }
            catch (Exception)
            {
                return false;
            }

            foreach (XElement appElement in defenitions.Descendants("app"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = appElement.Attribute("name");
                XElement versionElement = appElement.Element("version");
                XElement archElement = appElement.Element("arch");
                XElement typeElement = appElement.Element("type");
                XElement switchElement = appElement.Element("switch");
                XElement dlElement = appElement.Element("dl");
                XElement regkeyElement = appElement.Element("regkey");
                XElement regvalueElement = appElement.Element("regvalue");
                XElement exePathElement = appElement.Element("exePath");
                XElement descriptionElement = appElement.Element("description");
                XElement changelogElement = appElement.Element("changelog");

                // Check whether this app run on the system
                if (!Environment.Is64BitOperatingSystem && archElement?.Value == "x64")
                {
                    // This app cannot run on the system, so skip it
                    continue;
                }

                // Get local version if installed
                string localVersion = null;
                if (regkeyElement?.Value != null)
                {
                    RegistryKey baseKey;
                    if (regkeyElement.Value.StartsWith("HKEY_LOCAL_MACHINE"))
                    {
                        if (archElement?.Value == "x64")
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        }
                        else
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                        }
                    } 
                    else
                    {
                        if (archElement?.Value == "x64")
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                        }
                        else
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                        }
                    }

                    string keyPath = regkeyElement.Value;
                    keyPath = keyPath.Replace("HKEY_LOCAL_MACHINE\\", "");
                    keyPath = keyPath.Replace("HKEY_CURRENT_USER\\", "");
                    var key = baseKey.OpenSubKey(keyPath, false);

                    var regValue = key?.GetValue(regvalueElement.Value, null);
                    if (regValue == null && Environment.Is64BitOperatingSystem)
                    {
                        // In case we are on 64-bit we can check once more under the 64-bit registry
                        if (regkeyElement.Value.StartsWith("HKEY_LOCAL_MACHINE"))
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        }
                        else
                        {
                            baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                        }
                        key = baseKey.OpenSubKey(keyPath, false);
                        regValue = key?.GetValue(regvalueElement.Value, null);
                    }

                    if (regValue != null)
                    {
                        localVersion = regValue.ToString();
                    }
                }
                if (exePathElement?.Value != null)
                {
                    string exePath = exePathElement.Value;
                    if (exePath.Contains("%pf32%"))
                    {
                        if (Environment.Is64BitOperatingSystem)
                        {
                            exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                                Environment.SpecialFolder.ProgramFilesX86));
                        }
                        else
                        {
                            exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
                        }
                    }
                    else if (exePath.Contains("%pf64%"))
                    {
                        if (Environment.Is64BitOperatingSystem)
                        {
                            // We cannot use SpecialFolder.ProgramFiles here, because we are running as a 32-bit process
                            // SpecialFolder.ProgramFiles would return the 32-bit ProgramFiles here
                            exePath = exePath.Replace("%pf64%", Environment.GetEnvironmentVariable("ProgramW6432"));
                        }
                        else
                        {
                            // Do not add the app to the list because it is a 64 bit app
                            // on a 32 bit system
                            continue;
                        }
                    }

                    if (File.Exists(exePath))
                    {
                        localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                    }
                }

                Application appToAdd = new Application(nameAttribute.Value.ToString(), versionElement.Value,
                    localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                    dlElement.Value);
                appToAdd.HasChangelog = changelogElement?.Value != null;
                appToAdd.HasDescription = descriptionElement?.Value != null;

                Apps.Regular.Add(appToAdd);
            }

            return true;
        }
        #endregion

        #region CheckForUpdates()
        public static void CheckForUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);
            Apps.Updates = new ObservableCollection<Application>(Apps.Regular);

            foreach (Application app in Apps.Updates.ToArray())
            {
                // Remove non-installed apps or apps with the noupdate type from the AppInfo.UpdateList
                if (app.LocalVersion == null || app.Type == "noupdate")
                {
                    Apps.Updates.Remove(app);
                }
                else
                {
                    // Remove up to date apps from the AppInfo.UpdateList
                    if (!Utilities.UpdateAvailable(app.LatestVersion, app.LocalVersion))
                    {
                        Apps.Updates.Remove(app);
                    }
                    else
                    {
                        app.Name += $" {app.LatestVersion}";
                        app.DisplayedVersion = $"Installed: {app.LocalVersion}";
                    }
                }
            }

            if (Apps.Updates.Count > 0)
            {
                Log.Append(string.Format("{0} updates available", Apps.Updates.Count),
                    Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("1 update available", Log.LogLevel.INFO);
            }
        }
        #endregion

        /// <summary>
        /// Updates the GUI according to whether there are updates or there is a connection to the server
        /// </summary>
        private void UpdateGUI()
        {
            MainWindow mainWindow = Utilities.GetMainWindow();
            if (mainWindow.ConnectedToServer)
            {
                if (Apps.Updates != null)
                {
                    if (updaterTile.Background == Colors.normalGreyBrush)
                    {
                        getAppsTile.Background = Colors.normalGreenBrush;
                        portableAppsTile.Background = Colors.normalGreenBrush;
                        offlineNoticePanel.Visibility = Visibility.Hidden;

                        updaterTile.MouseLeftButtonDown += UpdaterTile_MouseLeftButtonDown;
                        updaterTile.MouseLeftButtonDown -= TileClickedWithNoConnection;
                        getAppsTile.MouseLeftButtonDown += GetAppsTile_MouseLeftButtonDown;
                        getAppsTile.MouseLeftButtonDown -= TileClickedWithNoConnection;
                        portableAppsTile.MouseLeftButtonDown += PortableAppsTile_MouseLeftButtonDown;
                        portableAppsTile.MouseLeftButtonDown -= TileClickedWithNoConnection;
                    }

                    if (Apps.Updates.Count > 0)
                    {
                        updaterTile.Background = Colors.normalOrangeBrush;
                        if (Apps.Updates.Count == 1)
                        {
                            updaterTile.Title = "1 update available";
                        }
                        else
                        {
                            updaterTile.Title = string.Format("{0} updates available", Apps.Updates.Count);
                        }
                        
                    }
                    else
                    {
                        updaterTile.Background = Colors.normalGreenBrush;
                        updaterTile.Title = "No updates available";
                        Log.Append("No updates available", Log.LogLevel.INFO);
                    }
                }
            }
            else
            {
                updaterTile.Background = Colors.normalGreyBrush;
                updaterTile.MouseLeftButtonDown -= UpdaterTile_MouseLeftButtonDown;
                updaterTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                getAppsTile.Background = Colors.normalGreyBrush;
                getAppsTile.MouseLeftButtonDown -= GetAppsTile_MouseLeftButtonDown;
                getAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                portableAppsTile.Background = Colors.normalGreyBrush;
                portableAppsTile.MouseLeftButtonDown -= PortableAppsTile_MouseLeftButtonDown;
                getAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                offlineNoticePanel.Visibility = Visibility.Visible;
            }
        }

        private void TileClickedWithNoConnection(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("This functionality is not available when there is no connection to the server.", 
                "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void UpdaterTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdaterPage updaterPage = new UpdaterPage();
            NavigationService.Navigate(updaterPage);
        }


        private void GetAppsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetAppsPage getAppsPage = new GetAppsPage();
            NavigationService.Navigate(getAppsPage);
        }

        private void SettingsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage();
            NavigationService.Navigate(settingsPage);
        }

        private void PortableAppsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InstalledPortableAppsPage portableAppPage = new InstalledPortableAppsPage();
            NavigationService.Navigate(portableAppPage);
        }

        private void OfflineRetryLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow mainWindow = Utilities.GetMainWindow();
            bool connectedToServer = ReadDefenitions();
            mainWindow.ConnectedToServer = connectedToServer;
            if (connectedToServer)
                CheckForUpdates();
            UpdateGUI();
        }
    }
}
