using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private List<Application> AppList;
        private List<Application> UpdateList;

        public StartPage()
        {
            InitializeComponent();
            AppList = new List<Application>();
            ReadDefenitions();
            CheckForUpdates();
        }

        private void UpdaterTile_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdaterPage updaterPage = new UpdaterPage(AppList);
            NavigationService.Navigate(updaterPage);
        }

        #region ReadDefenitions()
        public void ReadDefenitions()
        {
            //logger.Log("Loading definitions file", Logger.LogLevel.INFO, logTextBox);
            AppList = new List<Application>();
            XDocument defenitions = new XDocument();

            // Load XML File
            //try
            //{
            //    if (settings.DefenitionURL != null)
            //    {
            //        logger.Log("Using custom definition file from " + settings.DefenitionURL,
            //            Logger.LogLevel.INFO, logTextBox);
            //        defenitions = XDocument.Load(settings.DefenitionURL);
            //    }
            //    else
            //    {
            //logger.Log("Using official definitions", Logger.LogLevel.INFO, logTextBox);
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
                        exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFilesX86));
                    }
                    if (exePath.Contains("%pf64%"))
                    {
                        exePath = exePath.Replace("%pf64%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
                    }

                    if (File.Exists(exePath))
                    {
                        localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                    }
                }

                // Add app to appList
                AppList.Add(new Application(nameAttribute.Value.ToString(), versionElement.Value,
                    localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                    dlElement.Value, null));
            }
        }
        #endregion

        #region CheckForUpdates()
        public bool CheckForUpdates()
        {
            //if (updaterTile.BackColor == normalGrey)
            //{
            //    return false;
            //}

            //logger.Log("Checking for updates...", Logger.LogLevel.INFO, logTextBox);
            UpdateList = new List<Application>(AppList);
            string notifiedUpdates = null;

            foreach (Application app in UpdateList.ToArray())
            {
                // Remove not installed apps from the UpdateList so it doesn't get added
                if (app.LocalVersion == null | app.Type == "noupdate")
                {
                    UpdateList.Remove(app);
                    continue;
                }
                else
                {
                    // Remove up to date apps from the UpdateList and don't its add AppItem to panel
                    if (Utilities.IsUpToDate(app.LatestVersion, app.LocalVersion))
                    {
                        UpdateList.Remove(app);
                        continue;
                    }
                }
            }

            // Change updaterTile on the startpage accordingly
            if (UpdateList.Count != 0)
            {
                //    trayIcon.Icon = Properties.Resources.SlimUpdaterIcon_Orange;
                //    updaterTile.BackColor = normalOrange;

                //    if (UpdateList.Count > 1)
                //    {
                //        updaterTile.Text = String.Format("{0} updates available", UpdateList.Count);

                //        foreach (App update in UpdateList)
                //        {
                //            if (update.Equals(UpdateList.Last()))
                //            {
                //                // Do not add a space after the last app name
                //                notifiedUpdates += update.Name;
                //            }
                //            else
                //            {
                //                notifiedUpdates += update.Name + " ";
                //            }
                //        }
                //        if (notifiedUpdates != settings.NotifiedUpdates && this.Visible == false)
                //        {
                //            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                //            trayIcon.BalloonTipTitle = "Slim Updater";
                //            trayIcon.BalloonTipText = string.Format(
                //                "{0} updates available. Click for details.", UpdateList.Count);
                //            trayIcon.ShowBalloonTip(5000);
                //        }
                //        settings.NotifiedUpdates = notifiedUpdates;
                //        settings.Save();

                //        logger.Log(string.Format("{0} updates available", UpdateList.Count),
                //            Logger.LogLevel.INFO, logTextBox);
                //    }
                //    else
                //    {
                //        updaterTile.Text = String.Format("1 update available");

                //        notifiedUpdates = UpdateList[0].Name;
                //        if (this.ShowInTaskbar == false &&
                //            notifiedUpdates != settings.NotifiedUpdates)
                //        {
                //            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                //            trayIcon.BalloonTipTitle = "Slim Updater";
                //            trayIcon.BalloonTipText = string.Format("An update for {0} is available",
                //                UpdateList[0].Name);
                //            trayIcon.ShowBalloonTip(5000);
                //        }
                //        settings.NotifiedUpdates = notifiedUpdates;
                //        settings.Save();

                //        logger.Log(string.Format("1 update available", UpdateList.Count),
                //            Logger.LogLevel.INFO, logTextBox);
                //    }

                return true;
            }
            else
            {
                //trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
                //updaterTile.BackColor = normalGreen;
                //updaterTile.Text = "No updates available";
                //logger.Log("No updates available", Logger.LogLevel.INFO, logTextBox);

                //// Add all apps to updatecontentPanel for details view
                //// Only if page is actually visible and updates are not just installed
                //if (this.Controls[0] == updatePage && justInstalledUpdates == false)
                //{
                //    foreach (App a in AppList)
                //    {
                //        Application app = a;
                //        app.Checkbox = false;

                //        if (app.LocalVersion != null)
                //        {
                //            app.Name = app.Name + " " + app.LatestVersion;
                //            if (app.Type == "noupdate")
                //            {
                //                app.Version = "Installed: " + app.LocalVersion + " (Using own updater)";
                //            }
                //            else
                //            {
                //                app.Version = "Installed: " + app.LocalVersion;
                //            }
                //        }
                //        else
                //        {
                //            app.Name = app.Name + " " + app.LatestVersion;
                //            app.Version = "Not Found";
                //        }
                //        UpdateList.Add(app);
                //    }

                    // Hide select all checkbox and bottom buttons for details view
                    //selectAllCheckBox.Visible = false;
                    //installUpdatesButton.Visible = false;
                    //refreshUpdatesButton.Visible = false;
             }
                //else if (justInstalledUpdates == true)
                //{
                //    justInstalledUpdates = false;
                //    selectAllUpdatesCheckBox.Visible = false;
                //    installUpdatesButton.Enabled = false;

                //    Label noticeLabel = new Label();
                //    noticeLabel.Text = "No updates available.";
                //    noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                //    // Center
                //    noticeLabel.AutoSize = true;
                //    noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                //    updateContentPanel.Controls.Add(noticeLabel);
                //    Utilities.CenterControl(noticeLabel, updateContentPanel,
                //        Utilities.CenterMode.Both);
                //}
                return false;
        }
        #endregion
    }
}
