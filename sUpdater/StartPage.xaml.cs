using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            if (Apps.Updates != null)
            {
                if (Apps.Updates.Count > 0)
                {
                    updaterTile.Background = Colors.normalOrangeBrush;
                    updaterTile.Title = string.Format("{0} updates available", Apps.Updates.Count);
                }
                else
                {
                    updaterTile.Background = Colors.normalGreenBrush;
                    updaterTile.Title = "No updates available";
                    Log.Append("No updates available", Log.LogLevel.INFO);
                }
            }
        }

        #region ReadDefenitions()
        public static void ReadDefenitions()
        {
            Log.Append("Reading definitions file", Log.LogLevel.INFO);
            Apps.Regular = new ObservableCollection<Application>();
            XDocument defenitions;

            // Load XML File
            //try
            //{
            if (Settings.DefenitionURL != null)
            {
                Log.Append("Using custom definition file from " + Settings.DefenitionURL,
                    Log.LogLevel.INFO);
                defenitions = XDocument.Load(Settings.DefenitionURL);
            }
            else
            {
                Log.Append("Using official definitions", Log.LogLevel.INFO);
                defenitions = XDocument.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");
            }
            //}
            //catch (Exception e)
            //{
            //    logger.Log("Cannot check for updates: " + e.Message,
            //        Logger.LogLevel.ERROR, logTextBox);
            //    trayIcon.Icon = Properties.Resources.Slim_UpdaterIcon_Grey;
            //    trayIcon.Text = e.Message;
            //    updaterTile.BackColor = normalGrey;
            //    getNewAppsTile.BackColor = normalGrey;
            //    portableAppsTile.BackColor = normalGrey;
            //    updaterTile.Text = "Cannot check for updates";
            //    offlineLabel.Visible = true;
            //    offlineRetryLink.Visible = true;
            //    return;
            //}

            //if (updaterTile.BackColor == normalGrey)
            //{
            //    trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
            //    trayIcon.Text = "Slim Updater";
            //    updaterTile.BackColor = normalGreen;
            //    getNewAppsTile.BackColor = normalGreen;
            //    portableAppsTile.BackColor = normalGreen;
            //    offlineLabel.Visible = false;
            //    offlineRetryLink.Visible = false;
            //}

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
                    var regValue = Registry.GetValue(regkeyElement.Value,
                        regvalueElement.Value, null);
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
                            exePath = exePath.Replace("%pf64%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
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
                    if (Utilities.IsUpToDate(app.LatestVersion, app.LocalVersion))
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
    }
}
