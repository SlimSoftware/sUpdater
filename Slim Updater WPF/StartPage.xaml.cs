using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            // Only read the defenitions and check for updates the first time 
            // Slim Updater is started (initially the Apps.Regular property is unassigned)
            if (Apps.Regular == null)
            {
                ReadDefenitions();
                bool updatesAvailable = CheckForUpdates();

                // Change updaterTile accordingly
                if (updatesAvailable)
                {
                    string notifiedUpdates = "";
                    // trayIcon.Icon = Properties.Resources.SlimUpdaterIcon_Orange;
                    updaterTile.Background = Colors.normalOrangeBrush;

                    if (Apps.Updates.Count > 1)
                    {
                        updaterTile.Title = string.Format("{0} updates available", Apps.Updates.Count);

                        foreach (Application update in Apps.Updates)
                        {
                            if (update.Equals(Apps.Updates.Last()))
                            {
                                // Do not add a space after the last app name
                                notifiedUpdates += update.Name;
                            }
                            else
                            {
                                notifiedUpdates += update.Name + " ";
                            }
                        }

                        //if (notifiedUpdates != settings.NotifiedUpdates && this.Visible == false)
                        //{
                        //    trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        //    trayIcon.BalloonTipTitle = "Slim Updater";
                        //    trayIcon.BalloonTipText = string.Format(
                        //        "{0} updates available. Click for details.", AppInfo.UpdateList.Count);
                        //    trayIcon.ShowBalloonTip(5000);
                        //}
                        //settings.NotifiedUpdates = notifiedUpdates;
                        //settings.Save();

                        Log.Append(string.Format("{0} updates available", Apps.Updates.Count),
                            Log.LogLevel.INFO);
                    }
                    else
                    {
                        updaterTile.Title = "1 update available";

                        //notifiedUpdates = AppInfo.UpdateList[0].Name;
                        //if (this.ShowInTaskbar == false &&
                        //    notifiedUpdates != settings.NotifiedUpdates)
                        //{
                        //    trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        //    trayIcon.BalloonTipTitle = "Slim Updater";
                        //    trayIcon.BalloonTipText = string.Format("An update for {0} is available",
                        //        AppInfo.UpdateList[0].Name);
                        //    trayIcon.ShowBalloonTip(5000);
                        //}
                        //settings.NotifiedUpdates = notifiedUpdates;
                        //settings.Save();

                        Log.Append("1 update available", Log.LogLevel.INFO);
                    }
                }
                else
                {
                    //trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
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
            XDocument defenitions = new XDocument();

            // Load XML File
            //try
            //{
            //    if (settings.DefenitionURL != null)
            //    {
            //        Log.Append("Using custom definition file from " + settings.DefenitionURL,
            //            Log.LogLevel.INFO);
            //        defenitions = XDocument.Load(settings.DefenitionURL);
            //    }
            //    else
            //    {
            Log.Append("Using official definitions", Log.LogLevel.INFO);
            defenitions = XDocument.Load("defenitions.xml");
            //    }
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

                // Add app to AppList
                Apps.Regular.Add(new Application(nameAttribute.Value.ToString(), versionElement.Value,
                    localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                    dlElement.Value, null));
            }
        }
        #endregion

        #region CheckForUpdates()
        public static bool CheckForUpdates()
        {
            if (Apps.Regular.Count == 0)
            {
                return false;
            }

            //logger.Log("Checking for updates...", Logger.LogLevel.INFO, logTextBox);
            Apps.Updates = new ObservableCollection<Application>(Apps.Regular);

            foreach (Application app in Apps.Updates.ToArray())
            {
                // Remove non-installed apps or apps with the noupdate type from the AppInfo.UpdateList
                if (app.LocalVersion == null || app.Type == "noupdate")
                {
                    Apps.Updates.Remove(app);
                    continue;
                }
                else
                {
                    // Remove up to date apps from the AppInfo.UpdateList
                    if (Utilities.IsUpToDate(app.LatestVersion, app.LocalVersion))
                    {
                        Apps.Updates.Remove(app);
                        continue;
                    }
                }
            }

            if (Apps.Updates.Count != 0)
            {
                return true;
            }
            else
            {
                return false;            
            }
        }
        #endregion

        private void UpdaterTile_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdaterPage updaterPage = new UpdaterPage();
            NavigationService.Navigate(updaterPage);
        }


        private void GetAppsTile_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetAppsPage getAppsPage = new GetAppsPage();
            NavigationService.Navigate(getAppsPage);
        }
    }
}
