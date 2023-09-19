using Microsoft.Win32;
using sUpdater.Models;
using sUpdater.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Application = sUpdater.Models.Application;
using Colors = sUpdater.Models.Colors;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
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
            Log.Append("Reading XML", Log.LogLevel.INFO);
            Apps.Regular = new List<Application>();
            XDocument appXML;

            // Load XML File
            try
            {
                appXML = XDocument.Load($"{Utilities.GetAppServerURL()}/apps");
            }
            catch (Exception)
            {
                return false;
            }

            foreach (XElement appElement in appXML.Descendants("app"))
            {
                ImageSource icon = null;

                // Get content from XML nodes
                XAttribute nameAttribute = appElement.Attribute("name");
                XElement idElement = appElement.Element("id");
                XElement versionElement = appElement.Element("version");
                XElement archElement = appElement.Element("arch");
                XElement typeElement = appElement.Element("type");
                XElement switchElement = appElement.Element("switch");
                XElement dlElement = appElement.Element("dl");
                XElement regkeyElement = appElement.Element("regkey");
                XElement regvalueElement = appElement.Element("regvalue");
                XElement exePathElement = appElement.Element("exePath");
                XElement hasWebsiteElement = appElement.Element("hasWebsite");
                XElement hasChangelogElement = appElement.Element("hasChangelog");

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

                string exePath = null;
                if (exePathElement?.Value != null)
                {
                    exePath = exePathElement.Value;
                    if (exePath.Contains("%pf64%") && !Environment.Is64BitOperatingSystem)
                    {
                        // Do not add the app to the list because it is a 64 bit app on a 32 bit system
                        continue;
                    }
                    exePath = Utilities.ParseExePath(exePath);

                    if (File.Exists(exePath))
                    {
                        if (regkeyElement?.Value == null)
                        {
                            localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                        }
                    }
                }

                int id = Convert.ToInt32(idElement.Value);
                Application appToAdd = new Application(id, nameAttribute.Value, versionElement.Value,
                    localVersion, exePath, archElement.Value, typeElement.Value,
                    switchElement.Value, dlElement.Value);
                appToAdd.HasChangelog = hasChangelogElement?.Value == "1";
                appToAdd.HasWebsite = hasWebsiteElement?.Value == "1";
                appToAdd.Icon = icon;

                Apps.Regular.Add(appToAdd);
            }

            return true;
        }
        #endregion

        #region CheckForUpdates()
        public static void CheckForUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);
            Apps.Updates = new List<Application>(Apps.Regular);

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
        public void UpdateGUI()
        {
            MainWindow mainWindow = Utilities.GetMainWindow();
            if (mainWindow.ConnectedToServer)
            {
                if (Apps.Updates != null)
                {
                    if (updaterTile.Background == Colors.normalGreyBrush)
                    {
                    // Update state when connection is available again
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

                int updateCount = AppController.GetUpdateCount();
                if (AppController.GetUpdateCount() > 0)
                    {
                        updaterTile.Background = Colors.normalOrangeBrush;
                    
                        if (Apps.Updates.Count > 1)
                        {
                            updaterTile.Title = $"{updateCount} updates available";                   
                        }
                        else
                        {
                            updaterTile.Title = "1 update available";
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

        private async void OfflineRetryLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await AppController.CheckForUpdates();
            UpdateGUI();
            mainWindow.UpdateTaskbarIcon();
        }
    }
}
