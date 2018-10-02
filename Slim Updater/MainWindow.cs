using System.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Xml;
using System.Linq;
using AutoUpdaterDotNET;

namespace SlimUpdater
{
    public partial class MainWindow : Form
    {
        List<App> appList = new List<App>();
        List<App> updateList;
        List<App> notInstalledApps = new List<App>();
        List<PortableApp> portableAppList = new List<PortableApp>();
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);
        Color normalGrey = Color.FromArgb(141, 141, 141);
        bool justInstalledUpdates = false;

        public MainWindow()
        {
            InitializeComponent();
            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("/tray"))
            {
                Utilities.MinimizeToTray(this);
            }
            Settings.Load();
        }

        #region MainWindow Events
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Log.Append("Slim Updater v" + ProductVersion + " " + "started on " +
                Utilities.GetFriendlyOSName(), Log.LogLevel.INFO, logTextBox);
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (this.Controls[0] == updatePage | this.Controls[0] == getNewAppsPage |
                    this.Controls[0] == installedPortableAppsPage |
                    this.Controls[0] == getPortableAppsPage)
            {
                topBar.BorderStyle = BorderStyle.None;
            }

            ReadDefenitions();
            CheckForUpdates();    
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.MinimizeToTray == true && (e.CloseReason != CloseReason.TaskManagerClosing &&
                e.CloseReason != CloseReason.ApplicationExitCall &&
                e.CloseReason != CloseReason.FormOwnerClosing))
            {
                e.Cancel = true;
                Utilities.MinimizeToTray(this);
            }
        }
        #endregion

        #region ReadDefenitions()
        public void ReadDefenitions()
        {
            Log.Append("Loading definitions file", Log.LogLevel.INFO, logTextBox);
            appList = new List<App>();
            XDocument defenitions = new XDocument();

            // Load XML File
            try
            {
                if (Settings.DefenitionURL != null)
                {
                    Log.Append("Using custom definition file from " + Settings.DefenitionURL,
                        Log.LogLevel.INFO, logTextBox);
                    defenitions = XDocument.Load(Settings.DefenitionURL);
                }
                else
                {
                    Log.Append("Using official definitions", Log.LogLevel.INFO, logTextBox);
                    defenitions = XDocument.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");
                }
            }
            catch (Exception e)
            {
                Log.Append("Cannot check for updates: " + e.Message, 
                    Log.LogLevel.ERROR, logTextBox);
                trayIcon.Icon = Properties.Resources.Slim_UpdaterIcon_Grey;
                trayIcon.Text = e.Message;
                updaterTile.BackColor = normalGrey;
                getNewAppsTile.BackColor = normalGrey;
                portableAppsTile.BackColor = normalGrey;
                updaterTile.Text = "Cannot check for updates";
                offlineLabel.Visible = true;
                offlineRetryLink.Visible = true;
                return;
            }

            if (updaterTile.BackColor == normalGrey)
            {
                trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
                trayIcon.Text = "Slim Updater";
                updaterTile.BackColor = normalGreen;
                getNewAppsTile.BackColor = normalGreen;
                portableAppsTile.BackColor = normalGreen;
                offlineLabel.Visible = false;
                offlineRetryLink.Visible = false;
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
                appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value,
                    localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                    dlElement.Value, null));   
            }
        }
        #endregion

        #region Check Methods

        #region CheckForUpdates()
        public bool CheckForUpdates()
        {
            if (updaterTile.BackColor == normalGrey)
            {
                return false;
            }

            Log.Append("Checking for updates...", Log.LogLevel.INFO, logTextBox);
            updateList = new List<App>(appList);
            string notifiedUpdates = null;

            foreach (App app in updateList.ToArray())
            {
                // Remove not installed apps from the updateList so it doesn't get added
                if (app.LocalVersion == null | app.Type == "noupdate")
                {
                    updateList.Remove(app);
                    continue;
                }
                else
                {
                    // Remove up to date apps from the updateList and don't its add AppItem to panel
                    if (Utilities.IsUpToDate(app.LatestVersion, app.LocalVersion))
                    {
                        updateList.Remove(app);
                        continue;
                    }
                }
            }

            // Change updaterTile on the startpage accordingly
            if (updateList.Count != 0)
            {
                trayIcon.Icon = Properties.Resources.SlimUpdaterIcon_Orange;
                updaterTile.BackColor = normalOrange;

                if (updateList.Count > 1)
                {
                    updaterTile.Text = string.Format("{0} updates available", updateList.Count);
                    trayIcon.Text = string.Format("Slim Updater\n{0} updates available",
                        updateList.Count);

                    foreach (App update in updateList)
                    {
                        if (update.Equals(updateList.Last()))
                        {
                            // Do not add a space after the last app name
                            notifiedUpdates += update.Name;
                        }
                        else
                        {
                            notifiedUpdates += update.Name + " ";
                        }                        
                    }
                    if (notifiedUpdates != Settings.NotifiedUpdates && this.Visible == false)
                    {
                        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        trayIcon.BalloonTipTitle = "Slim Updater";
                        trayIcon.BalloonTipText = string.Format(
                            "{0} updates available. Click for details.", updateList.Count);
                        trayIcon.ShowBalloonTip(5000);
                    }
                    Settings.NotifiedUpdates = notifiedUpdates;
                    Settings.Save();

                    Log.Append(string.Format("{0} updates available", updateList.Count),
                        Log.LogLevel.INFO, logTextBox);
                }
                else
                {
                    updaterTile.Text = "1 update available";
                    trayIcon.Text = "Slim Updater\n1 update available";

                    notifiedUpdates = updateList[0].Name;
                    if (this.ShowInTaskbar == false &&
                        notifiedUpdates != Settings.NotifiedUpdates)
                    {
                        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        trayIcon.BalloonTipTitle = "Slim Updater";
                        trayIcon.BalloonTipText = string.Format("An update for {0} is available",
                            updateList[0].Name);
                        trayIcon.ShowBalloonTip(5000);
                    }
                    Settings.NotifiedUpdates = notifiedUpdates;
                    Settings.Save();

                    Log.Append(string.Format("1 update available", updateList.Count),
                        Log.LogLevel.INFO, logTextBox);
                }

                if (updateContentPanel.VerticalScroll.Visible == true)
                {
                    Utilities.FixScrollbars(updateContentPanel.Controls);
                }
                return true;
            }
            else
            {
                trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
                trayIcon.Text = "Slim Updater\nNo updates available";
                updaterTile.BackColor = normalGreen;
                updaterTile.Text = "No updates available";
                Log.Append("No updates available", Log.LogLevel.INFO, logTextBox);

                // Add all apps to updatecontentPanel for details view
                // Only if page is actually visible and updates are not just installed
                if (this.Controls[0] == updatePage && justInstalledUpdates == false)
                {
                    updateContentPanel.Controls.Clear();
                    foreach (App app in appList)
                    {
                        AppItem appItem = new AppItem();
                        Separator separator = new Separator();

                        appItem.Checkbox = false;
                        appItem.Click += (sender, e) =>
                        {
                            ShowDetails(app.Name, false, false);
                        };

                        if (app.LocalVersion != null)
                        {
                            appItem.Name = app.Name + " " + app.LatestVersion;
                            if (app.Type == "noupdate")
                            {
                                appItem.Version = "Installed: " + app.LocalVersion + " (Using own updater)";
                            }
                            else
                            {
                                appItem.Version = "Installed: " + app.LocalVersion;
                            }
                        }
                        else
                        {
                            appItem.Name = app.Name + " " + app.LatestVersion;
                            appItem.Version = "Not Found";
                        }
                        Utilities.AddAppItem(appItem, updateContentPanel);
                    }

                    if (updateContentPanel.VerticalScroll.Visible == true)
                    {
                        Utilities.FixScrollbars(updateContentPanel.Controls);
                    }

                    // Hide select all checkbox and bottom buttons for details view
                    updateContentPanel.Location = new Point(0, 0);
                    updateContentPanel.Size = new Size(updateContentPanel.Size.Width, 425);
                    selectAllUpdatesCheckBox.Visible = false;
                    installUpdatesButton.Visible = false;
                    refreshUpdatesButton.Visible = false;
                }
                else if (justInstalledUpdates == true)
                {
                    updateContentPanel.Controls.Clear();
                    justInstalledUpdates = false;
                    selectAllUpdatesCheckBox.Visible = false;
                    installUpdatesButton.Enabled = false;

                    Label noticeLabel = new Label();
                    noticeLabel.Text = "No updates available.";
                    noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                    // Center
                    noticeLabel.AutoSize = true;
                    noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                    updateContentPanel.Controls.Add(noticeLabel);
                    Utilities.CenterControl(noticeLabel, updateContentPanel, 
                        Utilities.CenterMode.Both);
                    // Compensate for topBar height
                    noticeLabel.Location = new Point(noticeLabel.Location.X,
                        noticeLabel.Location.Y - topBar.Height);
                }                
                return false;
            }
        }
        #endregion

        #region CheckForNewApps()
        public void CheckForNewApps()
        {
            getNewAppsContentPanel.Controls.Clear();
            notInstalledApps.Clear();

            foreach (App app in appList.ToArray())
            {
                AppItem appItem = new AppItem();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(app.Name, false, false);
                };
                appItem.Checked = false;

                // Make sure installed apps are not included
                if (app.LocalVersion == null)
                {
                    notInstalledApps.Add(app);
                    // Add app to the content panel
                    appItem.Name = app.Name;
                    appItem.Version = app.LatestVersion;
                    Utilities.AddAppItem(appItem, getNewAppsContentPanel);

                    app.AppItem = appItem;
                }
            }

            if (getNewAppsContentPanel.VerticalScroll.Visible == true)
            {
                Utilities.FixScrollbars(getNewAppsContentPanel.Controls);
            }

            if (notInstalledApps.Count == 0)
            {
                selectAllAppsCheckBox.Visible = false;
                installAppsButton.Enabled = false;

                Label noticeLabel = new Label();
                noticeLabel.Text = "No new applications to install found.";
                noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                // Center
                noticeLabel.AutoSize = true;
                noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                getNewAppsContentPanel.Controls.Add(noticeLabel);
                Utilities.CenterControl(noticeLabel, getNewAppsContentPanel,
                    Utilities.CenterMode.Both);
                // Conpensate for topBar height
                noticeLabel.Location = new Point(noticeLabel.Location.X,
                    noticeLabel.Location.Y - topBar.Height);
            }
            else if(selectAllAppsCheckBox.Visible == false)
            {              
                selectAllAppsCheckBox.Visible = true;
                installAppsButton.Enabled = true;
            }
        }
        #endregion

        #region CheckForPortableApps()
        private void CheckForPortableApps()
        {
            portableAppList.Clear();
            getPortableContentPanel.Controls.Clear();

            // Load XML File
            XElement defenitions = XElement.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");

            foreach (XElement portableAppElement in defenitions.Elements("portable"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = portableAppElement.Attribute("name");
                XElement versionElement = portableAppElement.Element("version");
                XElement changelogElement = portableAppElement.Element("changelog");
                XElement descriptionElement = portableAppElement.Element("description");
                XElement archElement = portableAppElement.Element("arch");
                XElement launchElement = portableAppElement.Element("launch");
                XElement dlElement = portableAppElement.Element("dl");
                XElement savePathElement = portableAppElement.Element("savepath");
                XElement extractModeElement = portableAppElement.Element("extractmode");

                // Check if Portable App is already installed
                // TODO: Get local version of portable app if installed
                string localVersion = "-";

                // Add app to portableAppList
                portableAppList.Add(new PortableApp(nameAttribute.Value, versionElement.Value,
                    localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                    extractModeElement.Value));

                foreach (PortableApp portableApp in portableAppList.ToArray())
                {
                    AppItem appItem = new AppItem();
                    appItem.Click += (sender, e) =>
                    {
                        ShowDetails(portableApp.Name, false, true);
                    };
                    appItem.Link2Clicked += (sender, e) =>
                    {
                        List<PortableApp> selectedAppList = new List<PortableApp>();
                        selectedAppList.Add(portableAppList.Find(x => x.Name == appItem.Name));
                        InstallPortableApps(selectedAppList, true);
                    };
                    appItem.Checked = false;

                    // Make sure only not installed apps are included
                    if (portableApp.LocalVersion == "-")
                    {
                        // Add app to the content panel
                        appItem.Name = portableApp.Name;
                        appItem.Version = portableApp.LatestVersion;
                        appItem.Link2Text = "Run";
                        appItem.ShowLink2 = true;
                        Utilities.AddAppItem(appItem, getPortableContentPanel);
                        
                        portableApp.AppItem = appItem;
                    }
                }
                if (getPortableContentPanel.VerticalScroll.Visible == true)
                {
                    Utilities.FixScrollbars(getPortableContentPanel.Controls);
                }

                if (portableAppList.Count == 0)
                {
                    Label noticeLabel = new Label();
                    noticeLabel.Text = "No new Portable Apps to install found.";
                    noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                    // Center
                    noticeLabel.AutoSize = true;
                    noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                    getPortableContentPanel.Controls.Add(noticeLabel);
                    Utilities.CenterControl(noticeLabel, getPortableContentPanel, 
                        Utilities.CenterMode.Both);
                    // Conpensate for topBar height
                    noticeLabel.Location = new Point(noticeLabel.Location.X,
                        noticeLabel.Location.Y - topBar.Height);
                }
                else if (selectAllPortableCheckBox.Visible == false)
                {
                    selectAllPortableCheckBox.Visible = true;
                    downloadPortableButton.Enabled = true;
                }
            }
        }
        #endregion

        #region CheckForInstalledPortableApps()
        private void CheckForInstalledPortableApps()
        {
            CheckForPortableApps();
            installedPortableContentPanel.Controls.Clear();

            string[] installedAppPaths = null;
            if (Settings.PortableAppDir != null)
            {
                installedAppPaths = Directory.GetDirectories(Settings.PortableAppDir);
            }

            if (installedAppPaths.Length == 0)
            {
                Label noticeLabel = new Label();
                noticeLabel.Text = "It appears that you don't have any Portable Apps " +
                                    "installed. \n" +
                                    "Click the Get Portable Apps button on the top right " +
                                    "of the window to install some.";
                noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                // Center
                noticeLabel.AutoSize = true;                            
                noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                installedPortableContentPanel.Controls.Add(noticeLabel);
                Utilities.CenterControl(noticeLabel, installedPortableContentPanel, 
                    Utilities.CenterMode.Both);
                return;
            }

            if (installedAppPaths.Length > 0)
            foreach (string appDirPath in installedAppPaths)
            {
                // TODO: Way to retrive app version here to set appItem.Version
                AppItem appItem = new AppItem();
                appItem.Name = Path.GetFileName(appDirPath);
                appItem.Version = null;
                appItem.Link1Text = "Run";
                appItem.ShowLink1 = true;
                appItem.Link2Text = "Delete";
                appItem.ShowLink2 = true;

                // Find associated PortableApp
                PortableApp portableApp = portableAppList.Find(x => x.Name == appItem.Name);

                appItem.Link1Clicked += (sender, e) =>
                {
                    using (Process p = new Process())
                    {
                        // TODO: Add support for optional arguments and use shell execute here
                        try
                        {
                            p.StartInfo.FileName = Path.Combine(
                                Settings.PortableAppDir, portableApp.Name, portableApp.Launch);
                            p.Start();
                        }
                        catch
                        {
                            MessageBox.Show("Could not run the application executable." +
                                Environment.NewLine + "Try reinstalling the app.",
                                "Slim Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };
                appItem.Link2Clicked += (sender, e) =>
                {
                    DialogResult result = MessageBox.Show(string.Format("Are you sure that you " +
                        "want to delete {0}," + Environment.NewLine +
                        "including its Settings and other data?", appItem.Name), "Slim Updater", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        appItem.Status = "Deleting...";
                        Directory.Delete(Path.Combine(Settings.PortableAppDir, appItem.Name), true);
                        CheckForInstalledPortableApps();
                    }
                };
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(portableApp.Name, false, true);
                };
                Utilities.AddAppItem(appItem, installedPortableContentPanel);

                portableApp.AppItem = appItem;
            }

            if (installedPortableContentPanel.VerticalScroll.Visible == true)
            {
                Utilities.FixScrollbars(installedPortableContentPanel.Controls);
            }
        }
        #endregion

        #endregion

        #region AddUpdatesToContentPanel()
        private void AddUpdatesToContentPanel()
        {
            updateContentPanel.Controls.Clear();
            foreach (App app in updateList)
            {
                AppItem appItem = new AppItem();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(app.Name, true, false);
                };
                appItem.Name = app.Name + " " + app.LatestVersion;
                appItem.Version = "Installed: " + app.LocalVersion;
                Utilities.AddAppItem(appItem, updateContentPanel);
                app.AppItem = appItem;
            }
        }
        #endregion

        #region Install Methods

        #region InstallUpdates()
        public async void InstallUpdates(List<App> updateList)
        {
            bool updateFailed = false;
            Log.Append("Update started...", Log.LogLevel.INFO, logTextBox);
            refreshUpdatesButton.Enabled = false;
            installUpdatesButton.Enabled = false;
            List<App> selectedUpdateList = new List<App>();

            foreach (App update in updateList)
            {
                if (update.AppItem.Checked == true)
                {
                    selectedUpdateList.Add(update);
                }
                else
                {
                    updateContentPanel.Controls.Remove(update.AppItem);
                }
            }

            if (selectedUpdateList.Count == 0)
            {
                MessageBox.Show("You have not selected any updates.");
                Log.Append("No updates selected to install, aborting...",
                    Log.LogLevel.WARN, logTextBox);
            }

            // Download
            List<Task> tasks = new List<Task>();
            int currentUpdate = 0;

            foreach (App update in selectedUpdateList)
            {
                currentUpdate++;
                Task downloadTask = Task.Run(async () =>
                {
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...", 
                        update.Name, currentUpdate, selectedUpdateList.Count),
                        Log.LogLevel.INFO, logTextBox);

                    string fileName = Path.GetFileName(update.DL);
                    update.SavePath = Path.Combine(Settings.DataDir, fileName);
                    Log.Append("Saving to: " + update.SavePath, Log.LogLevel.INFO, logTextBox);

                    // Check if installer is already downloaded
                    if (!File.Exists(update.SavePath))
                    {
                        using (var wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += (s, e) =>
                            {
                                // Convert download size to mb
                                double recievedSize = Math.Round(e.BytesReceived / 1024d / 1024d, 1);
                                double totalSize = Math.Round(e.TotalBytesToReceive / 1024d / 1024d, 1);

                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        update.AppItem.Progress = e.ProgressPercentage / 2;
                                        update.AppItem.Status = string.Format(
                                            "Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                                    }));
                                }
                            };
                            wc.DownloadFileCompleted += (s, e) =>
                            {
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        update.AppItem.Status = "Download complete";                                       
                                    }));
                                }
                            };
                            try
                            {
                                await wc.DownloadFileTaskAsync(new Uri(update.DL), update.SavePath);
                            }
                            catch (Exception e)
                            {
                                updateFailed = true;
                                Log.Append("An error occurred when attempting to download " +
                                    "the update." + e.Message, Log.LogLevel.ERROR, logTextBox);
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        update.AppItem.Status = e.Message;
                                    }));
                                }
                                
                                if (File.Exists(update.SavePath))
                                {
                                    File.Delete(update.SavePath);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                update.AppItem.Progress = 50;                                
                            }));
                        }
                    }
                });
                
                // Do not allow more than 3 downloads at once
                // TODO: Let the user decide this with a setting
                while (tasks.Count > 2)
                {
                    await Task.Delay(1000);
                }
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Install
            if (!updateFailed)
            {
                currentUpdate = 0;
                foreach (App update in selectedUpdateList)
                {
                    currentUpdate++;
                    launchInstaller:
                    if (File.Exists(update.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", update.Name,
                            currentUpdate, selectedUpdateList.Count), Log.LogLevel.INFO, logTextBox);
                        using (var p = new Process())
                        {
                            if (update.DL.EndsWith(".exe"))
                            {
                                p.StartInfo.FileName = update.SavePath;
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = update.InstallSwitch;
                            }
                            else if (update.DL.EndsWith(".msi"))
                            {
                                p.StartInfo.FileName = "msiexec";
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = "\"" + update.InstallSwitch + "\""
                                    + " " + update.SavePath;
                            }
                            try
                            {
                                p.Start();
                            }
                            catch (Exception)
                            {
                                var result = MessageBox.Show(
                                    "Launching the installer failed. \nWould you like to try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                Log.Append("Launching the installer failed. Asking user for retry.",
                                    Log.LogLevel.INFO, logTextBox);
                                if (result == DialogResult.Yes)
                                {
                                    Log.Append("User chose yes.", Log.LogLevel.INFO, logTextBox);
                                    goto launchInstaller;
                                }
                                else
                                {
                                    Log.Append("User chose no. Skipping this update.", Log.LogLevel.INFO, logTextBox);
                                    continue;
                                }
                            }
                            update.AppItem.Status = "Installing...";
                            update.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                            // Wait on a separate thread so the GUI thread does not get blocked
                            await Task.Run(() =>
                            {
                                p.WaitForExit();
                            });

                            if (p.ExitCode == 0)
                            {
                                Log.Append("Installer exited with exit code 0.",
                                    Log.LogLevel.INFO, logTextBox);
                                File.Delete(update.SavePath);
                                update.AppItem.Status = "Install complete";
                                update.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                                update.AppItem.Progress = 100;
                            }
                            if (p.ExitCode != 0)
                            {
                                updateFailed = true;
                                Log.Append(string.Format("Installation failed. Installer exited with " +
                                    "exit code {0}.", p.ExitCode), Log.LogLevel.ERROR, logTextBox);
                                update.AppItem.Status = string.Format(
                                    "Install failed. Exit code: {0}", p.ExitCode);
                                update.AppItem.Progress = 0;
                                update.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                            }
                        }
                    }
                }

                // Cleanup any leftover exe's in appdata dir
                if (Directory.GetFiles(Settings.DataDir, "*.exe").Length > 0)
                {
                    Log.Append("Cleaning up leftover installers...", Log.LogLevel.INFO, logTextBox);
                    foreach (string exePath in Directory.GetFiles(Settings.DataDir, "*.exe"))
                    {
                        File.Delete(exePath);
                    }
                }

                installUpdatesButton.Enabled = true;
                justInstalledUpdates = true;
                ReadDefenitions();
                bool updatesAvailable = CheckForUpdates();
                if (updatesAvailable)
                {
                    AddUpdatesToContentPanel();
                }
            }
            else
            {
                // Only show the failed updates
                List<AppItem> failedUpdates = new List<AppItem>(); 
                foreach (Control control in updateContentPanel.Controls)
                {
                    if (control is AppItem appItem)
                    {
                        if (appItem.Status != "Install complete")
                        {
                            failedUpdates.Add(appItem);
                        }
                    }
                }
                updateContentPanel.Controls.Clear();
                Utilities.AddAppItems(failedUpdates, updateContentPanel);
                updatesStatusLabel.ForeColor = Color.Red;
                updatesStatusLabel.Text = "Some updates failed to install.";
                Utilities.CenterControl(updatesStatusLabel, updatesStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                updatesStatusLabel.Visible = true;
                installUpdatesButton.Enabled = true;
            }

            refreshUpdatesButton.Enabled = true;           
        }
        #endregion

        #region InstallNewApps()
        public async void InstallNewApps(List<App> appList)
        {
            bool installFailed = false;
            Log.Append("New app installation started...", Log.LogLevel.INFO, logTextBox);
            refreshAppsButton.Enabled = false;
            installAppsButton.Enabled = false;
            newAppsStatusLabel.Visible = false;
            List<App> selectedAppList = new List<App>();

            foreach (App app in appList)
            {
                if (app.AppItem != null)
                {
                    if (app.AppItem.Checked == true)
                    {
                        selectedAppList.Add(app);
                    }
                }
                else
                {
                    getNewAppsContentPanel.Controls.Remove(app.AppItem);
                }
            }

            if (selectedAppList.Count == 0)
            {
                MessageBox.Show("You have not selected any applications.");
                Log.Append("No applications selected to install, aborting...",
                    Log.LogLevel.WARN, logTextBox);
            }

            // Download
            List<Task> tasks = new List<Task>();
            int currentUpdate = 0;

            foreach (App app in selectedAppList)
            {
                currentUpdate++;
                Task downloadTask = Task.Run(async () =>
                {
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentUpdate, selectedAppList.Count),
                        Log.LogLevel.INFO, logTextBox);

                    string fileName = Path.GetFileName(app.DL);
                    app.SavePath = Path.Combine(Settings.DataDir, fileName);
                    Log.Append("Saving to: " + app.SavePath, Log.LogLevel.INFO, logTextBox);

                    // Check if installer is already downloaded
                    if (!File.Exists(app.SavePath))
                    {
                        using (var wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += (s, e) =>
                            {
                                // Convert download size to mb
                                double recievedSize = Math.Round(e.BytesReceived / 1024d / 1024d, 1);
                                double totalSize = Math.Round(e.TotalBytesToReceive / 1024d / 1024d, 1);

                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        app.AppItem.Progress = e.ProgressPercentage / 2;
                                        app.AppItem.Status = string.Format(
                                            "Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                                    }));
                                }
                            };
                            wc.DownloadFileCompleted += (s, e) =>
                            {
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        app.AppItem.Status = "Download complete";
                                        app.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                                    }));
                                }
                            };
                            try
                            {
                                await wc.DownloadFileTaskAsync(new Uri(app.DL), app.SavePath);
                            }
                            catch (Exception e)
                            {
                                installFailed = true;
                                Log.Append("An error occurred when attempting to download " +
                                    "the installer." + e.Message, Log.LogLevel.ERROR, logTextBox);
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        app.AppItem.Status = e.Message;
                                    }));
                                }

                                if (File.Exists(app.SavePath))
                                {
                                    File.Delete(app.SavePath);
                                }                               
                            }
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                app.AppItem.Progress = 50;                                
                            }));
                        }
                    }
                });

                // Do not allow more than 3 downloads at once
                while (tasks.Count > 2)
                {
                    await Task.Delay(1000);
                }
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Install
            if (!installFailed)
            {
                currentUpdate = 0;
                foreach (App app in selectedAppList)
                {
                    currentUpdate++;
                    launchInstaller:
                    if (File.Exists(app.SavePath))
                    {
                        Log.Append(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                            currentUpdate, selectedAppList.Count), Log.LogLevel.INFO, logTextBox);
                        using (var p = new Process())
                        {
                            if (app.DL.EndsWith(".exe"))
                            {
                                p.StartInfo.FileName = app.SavePath;
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = app.InstallSwitch;
                            }
                            else if (app.DL.EndsWith(".msi"))
                            {
                                p.StartInfo.FileName = "msiexec";
                                p.StartInfo.UseShellExecute = true;
                                p.StartInfo.Verb = "runas";
                                p.StartInfo.Arguments = "\"" + app.InstallSwitch + "\""
                                    + " " + app.SavePath;
                            }
                            try
                            {
                                p.Start();
                            }
                            catch (Exception)
                            {
                                var result = MessageBox.Show(
                                    "Lauching the installer failed. \nWould you like to try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                Log.Append("Launching the installer failed. Asking user for retry.",
                                    Log.LogLevel.INFO, logTextBox);
                                if (result == DialogResult.Yes)
                                {
                                    Log.Append("User chose yes.", Log.LogLevel.INFO, logTextBox);
                                    goto launchInstaller;
                                }
                                else
                                {
                                    Log.Append("User chose no. Skipping this app.", Log.LogLevel.INFO, logTextBox);
                                    continue;
                                }
                            }
                            app.AppItem.Status = "Installing...";
                            app.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                            // Wait on a separate thread so the GUI thread does not get blocked
                            await Task.Run(() =>
                            {
                                p.WaitForExit();
                            });

                            if (p.ExitCode == 0)
                            {
                                Log.Append("Installer exited with exit code 0.",
                                    Log.LogLevel.INFO, logTextBox);
                                File.Delete(app.SavePath);
                                app.AppItem.Status = "Install complete";
                                app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                                app.AppItem.Progress = 100;
                            }
                            if (p.ExitCode != 0)
                            {
                                installFailed = true;
                                Log.Append(string.Format("Installation failed. Installer exited with " +
                                    "exit code {0}.", p.ExitCode), Log.LogLevel.ERROR, logTextBox);
                                app.AppItem.Status = string.Format(
                                    "Install failed. Exit code: {0}", p.ExitCode);
                                app.AppItem.Progress = 0;
                                app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                            }
                        }
                    }
                }

                // Cleanup any leftover exe's in appdata dir
                if (Directory.GetFiles(Settings.DataDir, "*.exe").Length > 0)
                {
                    Log.Append("Cleaning up leftover installers...", Log.LogLevel.INFO, logTextBox);
                    foreach (string exePath in Directory.GetFiles(Settings.DataDir, ".exe"))
                    {
                        File.Delete(exePath);
                    }
                }

                Utilities.CenterControl(newAppsStatusLabel, newAppsStatusLabel.Parent,
                    Utilities.CenterMode.Horizontal);
                installAppsButton.Enabled = true;
                ReadDefenitions();
                CheckForNewApps();
            }
            else
            {
                newAppsStatusLabel.ForeColor = Color.Red;
                newAppsStatusLabel.Text = "Some applications failed to install.";
                Utilities.CenterControl(newAppsStatusLabel, newAppsStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                newAppsStatusLabel.Visible = true;
                installAppsButton.Enabled = true;
            }

            refreshAppsButton.Enabled = true;            
        }
        #endregion

        #region InstallPortableApps()
        public async void InstallPortableApps(List<PortableApp> portableAppList, bool runOnce)
        {
            bool installFailed = false;
            if (runOnce == true)
            {
                Log.Append("Run Once Portable App install started...", 
                    Log.LogLevel.INFO, logTextBox);
            }
            else
            {
                Log.Append("Portable App installation started...",
                    Log.LogLevel.INFO, logTextBox);
            }
            refreshPortableButton.Enabled = false;
            downloadPortableButton.Enabled = false;
            List<PortableApp> selectedAppList = new List<PortableApp>();

            if (runOnce != true)
            {
                foreach (PortableApp app in portableAppList)
                {
                    app.AppItem.ShowLink2 = false;
                    if (app.AppItem.Checked == true)
                    {
                        selectedAppList.Add(app);
                    }
                    else
                    {
                        getPortableContentPanel.Controls.Remove(app.AppItem);
                    }
                }

                if (selectedAppList.Count == 0)
                {
                    MessageBox.Show("You have not selected any Portable Apps.");
                    Log.Append("No Portable Apps selected to install, aborting...",
                        Log.LogLevel.WARN, logTextBox);
                }
            }
            else
            {
                selectedAppList = new List<PortableApp>(portableAppList);
                portableAppList = null;

                if (selectedAppList.Count == 0)
                {
                    MessageBox.Show("You have not selected any Portable Apps.");
                    Log.Append("No Portable Apps selected to install, aborting...",
                        Log.LogLevel.WARN, logTextBox);
                }
            }

            // Download
            List<Task> tasks = new List<Task>();
            int currentApp = 0;

            foreach (PortableApp app in selectedAppList)
            {
                currentApp++;
                Task downloadTask = Task.Run(async () =>
                {
                    Log.Append(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, selectedAppList.Count), 
                        Log.LogLevel.INFO, logTextBox);

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            app.AppItem.ShowLink2 = false;
                        }));
                    }

                    Directory.CreateDirectory(Path.Combine(Settings.PortableAppDir, app.Name));
                    string fileName = @Path.GetFileName(app.DL);
                    app.SavePath = @Path.Combine(Settings.PortableAppDir, app.Name, fileName);
                    Log.Append("Saving to: " + app.SavePath, Log.LogLevel.INFO, logTextBox);

                    // Check if portable app is already downloaded
                    if (!File.Exists(app.SavePath))
                    {
                        using (var wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += (s, e) =>
                            {
                                // Convert download size to MB
                                double recievedSize = Math.Round(e.BytesReceived / 1024d / 1024d, 1);
                                double totalSize = Math.Round(e.TotalBytesToReceive / 1024d / 1024d, 1);

                                // Set the progress
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        if (app.ExtractMode == "single")
                                        {
                                            app.AppItem.Progress = e.ProgressPercentage;
                                        }
                                        else
                                        {
                                            app.AppItem.Progress = e.ProgressPercentage / 2;
                                        }
                                        app.AppItem.Status = string.Format(
                                            "Downloading... {0:0.0} MB/{1:0.0} MB",
                                            recievedSize, totalSize);
                                    }));
                                }
                            };
                            wc.DownloadFileCompleted += (s, e) =>
                            {
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        app.AppItem.Status = "Download complete";                                        
                                    }));
                                }
                            };
                            try
                            {
                                await wc.DownloadFileTaskAsync(new Uri(app.DL), app.SavePath);
                            }
                            catch (Exception e)
                            {
                                installFailed = true;
                                Log.Append("An error occurred when attempting to download " +
                                    "the Portable App." + e.Message, Log.LogLevel.ERROR, logTextBox);
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        app.AppItem.Status = e.Message;
                                    }));
                                }

                                if (File.Exists(app.SavePath))
                                {
                                    File.Delete(app.SavePath);
                                }                               
                            }
                        }
                    }
                    else
                    {
                        // App has already been downloaded before
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                app.AppItem.Progress = 50;
                            }));
                        }
                    }
                });

                // Do not allow more than 3 downloads at once
                while (tasks.Count > 2)
                {
                    await Task.Delay(1000);
                }
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Extract
            if (!installFailed)
            {
                currentApp = 0;
                string sevenZipPath = null;
#if DEBUG
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles), "7-Zip")))
                {
                    sevenZipPath = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
                }
#else
                if (File.Exists(Path.Combine(Application.StartupPath), "7-Zip")))
                {
                    sevenZipPath = Path.Combine(Application.StartupPath, "7z.exe");
                }
#endif
                if (sevenZipPath == null)
                {
                    Log.Append("7-Zip not present under: " + sevenZipPath + ". Cancelling...",
                        Log.LogLevel.ERROR, logTextBox);
                    return;
                }
                else
                {
                    Log.Append("7-Zip path: " + sevenZipPath, Log.LogLevel.INFO, logTextBox);
                }

                foreach (PortableApp app in selectedAppList)
                {
                    if (File.Exists(app.SavePath))
                    {
                        if (runOnce == true && app.ExtractMode == "single")
                        {
                            Log.Append("Launching " + app.Name, Log.LogLevel.INFO, logTextBox);
                            app.AppItem.Status = "Running";
                            using (var ro = new Process())
                            {
                                ro.StartInfo.FileName = Path.Combine(
                                    Settings.PortableAppDir, app.Name, app.Launch);
                                // TODO: Add support for optional arguments and use shell execute here
                                ro.Start();
                            }
                            await Task.Run(() =>
                            {
                                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                while (processes == null | processes.Length != 0)
                                {
                                    Thread.Sleep(1000);
                                    processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                }
                            });
                            Log.Append("All processes exited. Cleaning up...", Log.LogLevel.INFO, logTextBox);

                            // Cleanup
                            Directory.Delete(Path.Combine(Settings.PortableAppDir, app.Name), true);
                        }

                        if (app.ExtractMode == "folder")
                        {
                            using (var p = new Process())
                            {
                                p.StartInfo.FileName = sevenZipPath;
                                p.StartInfo.Arguments = "e \"" + app.SavePath + "\" -o\""
                                    + Path.Combine(Settings.PortableAppDir, app.Name) + "\" -aoa";
                                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                try
                                {
                                    p.Start();
                                }
                                catch (Exception e)
                                {
                                    installFailed = true;
                                    app.AppItem.Status = "Extracting failed: " + e.InnerException;
                                    Log.Append("Extracting failed" + e.Message,
                                        Log.LogLevel.ERROR, logTextBox);
                                }

                                app.AppItem.Status = "Extracting...";
                                app.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                                Log.Append(string.Format("Extracting {0} ({1} of {2}) ...",
                                    app.Name, currentApp, selectedAppList.Count),
                                    Log.LogLevel.INFO, logTextBox);

                                // Wait on a separate thread so the GUI thread does not get blocked
                                await Task.Run(() =>
                                {
                                    p.WaitForExit();
                                });
                                if (p.ExitCode == 0)
                                {
                                    Log.Append("Extract succesful.", Log.LogLevel.INFO, logTextBox);
                                    File.Delete(app.SavePath);
                                    app.AppItem.Status = "Install complete";
                                    app.AppItem.Progress = 100;
                                    app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                                    await Task.Delay(1000);

                                    if (runOnce == true)
                                    {
                                        Log.Append("Launching " + app.Name, Log.LogLevel.INFO, logTextBox);
                                        app.AppItem.Status = "Running";
                                        using (var ro = new Process())
                                        {
                                            ro.StartInfo.FileName = Path.Combine(
                                                Settings.PortableAppDir, app.Name, app.Launch);
                                            // TODO: Add support for optional arguments and use shell execute here
                                            ro.Start();
                                        }
                                        await Task.Run(() =>
                                        {
                                            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                            while (processes == null | processes.Length != 0)
                                            {
                                                Thread.Sleep(1000);
                                                processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                            }
                                        });
                                        Log.Append("All processes exited. Cleaning up...",
                                            Log.LogLevel.INFO, logTextBox);
                                        Directory.Delete(Path.Combine(Settings.PortableAppDir, app.Name), true);

                                        downloadPortableButton.Enabled = true;
                                        ReadDefenitions();
                                        CheckForPortableApps();
                                    }
                                }
                                if (p.ExitCode != 0)
                                {
                                    installFailed = true;
                                    app.AppItem.Status = string.Format(
                                        "Extract failed. Exit code: {0}", p.ExitCode);
                                    app.AppItem.Progress = 0;
                                    Log.Append("Extract failed. Exit code: " + p.ExitCode,
                                        Log.LogLevel.ERROR, logTextBox);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                portableStatusLabel.ForeColor = Color.Red;
                portableStatusLabel.Text = "Some Portable Apps failed to install.";
                Utilities.CenterControl(portableStatusLabel, portableStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                portableStatusLabel.Visible = true;
                downloadPortableButton.Enabled = false;
            }

            refreshPortableButton.Enabled = true;
        }
        #endregion

        #endregion

        #region ShowDetails()
        public void ShowDetails(string appName, bool changelog, bool portable)
        {
            string changelogText = null;
            string descriptionText = null;
            string defenitionURL = null;
            if (Settings.DefenitionURL != null)
            {
                defenitionURL = Settings.DefenitionURL;
            }
            else
            {
                defenitionURL = "https://www.slimsoft.tk/slimupdater/defenitions.xml";
            }

            using (XmlReader xmlReader = XmlReader.Create(defenitionURL))
            {
                while (xmlReader.Read())
                {
                    if (portable == true)
                    {
                        xmlReader.ReadToFollowing("portable");
                    }
                    else
                    {
                        xmlReader.ReadToFollowing("app");
                    }
                    //xmlReader.MoveToFirstAttribute();
                    xmlReader.MoveToNextAttribute();
                    string appAttribute = xmlReader.Value;
                    if (appAttribute == appName)
                    {
                        if (changelog == true)
                        {
                            xmlReader.MoveToElement();
                            xmlReader.ReadToDescendant("changelog");
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                changelogText = xmlReader.ReadElementContentAsString();
                            }
                        }
                        else
                        {
                            xmlReader.MoveToElement();
                            xmlReader.ReadToDescendant("description");
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                descriptionText = xmlReader.ReadElementContentAsString();
                            }
                        }
                        break;
                    }
                }
            }

            
            if (changelog == true)
            {
                if (changelogText != null)
                {
                    // Remove first newline and/or any tabs
                    if (changelogText.StartsWith("\n"))
                    {
                        changelogText = changelogText.TrimStart("\n".ToCharArray());
                    }
                    if (changelogText.Contains("\t"))
                    {
                        changelogText = changelogText.Replace("\t", String.Empty);
                    }

                    if (detailLabel.Text != "Changelog")
                    {
                        detailLabel.Text = "Changelog";
                    }
                    detailText.Text = changelogText;
                    detailsPage.BringToFront();
                    titleButtonLeft.Text = "Details";
                }
            }
            else
            {
                if (descriptionText != null)
                {
                    if (descriptionText.StartsWith("\n"))
                    {
                        descriptionText = descriptionText.TrimStart("\n".ToCharArray());
                    }
                    if (descriptionText.Contains("\t"))
                    {
                        descriptionText = descriptionText.Replace("\t", String.Empty);
                    }

                    if (detailLabel.Text != "Description")
                    {
                        detailLabel.Text = "Description";
                    }
                    detailText.Text = descriptionText;
                    detailsPage.BringToFront();
                    titleButtonLeft.Text = "Details";
                }
            }
        }
        #endregion

        #region startPage/topBar Mouse Events
        private void TitleButtonLeft_Click(object sender, EventArgs e)
        {
            if (topBar.Size.Height != 36)
            {
                topBar.Size = new Size(topBar.Size.Width, 36);
            }

            if (topBar.BorderStyle == BorderStyle.None)
            {
                topBar.BorderStyle = BorderStyle.FixedSingle;
            }

            if (titleButtonLeft.Text == "Details")
            {
                if (this.Controls[0] == updatePage)
                {
                    updatePage.SendToBack();
                    titleButtonLeft.ArrowLeft = false;
                    titleButtonLeft.Text = "Home";
                    updateContentPanel.Location = new Point(0, 20);
                    updateContentPanel.Size = new Size(updateContentPanel.Size.Width, 365);
                    selectAllUpdatesCheckBox.Visible = true;
                    installUpdatesButton.Visible = true;
                    refreshUpdatesButton.Visible = true;
                    updatesStatusLabel.Visible = false;
                }
                else
                {
                    detailsPage.SendToBack();
                    if (this.Controls[0] == updatePage)
                    {
                        titleButtonLeft.Text = "Updates";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0] == getNewAppsPage)
                    {
                        titleButtonLeft.Text = "Get New Applications";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0] == installedPortableAppsPage)
                    {
                        titleButtonLeft.Text = "Portable Apps";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0] == getPortableAppsPage)
                    {
                        topBar.BorderStyle = BorderStyle.None;
                        titleButtonLeft.Text = "Get Portable Apps";
                    }
                }
            }
            else
            {
                if (titleButtonLeft.ArrowLeft == true)
                {
                    if (this.Controls[0] == getPortableAppsPage)
                    {
                        installedPortableAppsPage.BringToFront();
                        titleButtonLeft.Text = "Portable Apps";
                        topBar.BorderStyle = BorderStyle.None;
                        aboutButton.Visible = false;
                        logButton.Visible = false;
                        titleButtonRight.Visible = true;
                    }
                    else if (this.Controls[0] == installedPortableAppsPage)
                    {
                        if (updaterTile.Text != "No updates available")
                        {
                            updaterTile.BackColor = normalOrange;
                        }
                        startPage.BringToFront();                      
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonLeft.Text = "Home";
                        titleButtonRight.Visible = false;
                        updatesStatusLabel.Visible = false;
                        newAppsStatusLabel.Visible = false;
                        portableStatusLabel.Visible = false;
                        topBar.BorderStyle = BorderStyle.FixedSingle;
                        aboutButton.Visible = true;
                        logButton.Visible = true;
                    }
                    else
                    {
                        if (updaterTile.Text != "No updates available")
                        {
                            updaterTile.BackColor = normalOrange;
                        }
                        startPage.BringToFront();                        
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonLeft.Text = "Home";
                        titleButtonRight.Visible = false;
                        updatesStatusLabel.Visible = false;
                        newAppsStatusLabel.Visible = false;
                        portableStatusLabel.Visible = false;
                        topBar.BorderStyle = BorderStyle.FixedSingle;
                        aboutButton.Visible = true;
                        logButton.Visible = true;
                    }
                }
            }
        }

        private void TitleButtonRight_Click(object sender, EventArgs e)
        {
            getPortableAppsPage.BringToFront();            
            titleButtonLeft.ArrowLeft = true;
            titleButtonLeft.Text = "Portable Apps";
            titleButtonRight.Visible = false;
            topBar.BorderStyle = BorderStyle.None;
            CheckForPortableApps();
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            logPage.BringToFront();            
            titleButtonLeft.ArrowLeft = true;
            titleButtonLeft.Text = "Log";
            titleButtonLeft.ArrowRight = false;
            titleButtonLeft.Visible = true;
        }

        private void LogButton_MouseEnter(object sender, EventArgs e)
        {
            logButton.ForeColor = Color.White;
            logButton.BackColor = normalGreen;
        }

        private void LogButton_MouseLeave(object sender, EventArgs e)
        {
            logButton.ForeColor = normalGreen;
            logButton.BackColor = Color.White;
        }

        private void AboutLabel_MouseEnter(object sender, EventArgs e)
        {
            aboutButton.ForeColor = Color.White;
            aboutButton.BackColor = normalGreen;
        }

        private void AboutLabel_MouseLeave(object sender, EventArgs e)
        {
            aboutButton.ForeColor = normalGreen;
            aboutButton.BackColor = Color.White;
        }

        private void AboutLabel_Click(object sender, EventArgs e)
        {
            topBar.Size = new Size(topBar.Size.Width, 36);
            aboutPage.BringToFront();           
            titleButtonLeft.ArrowLeft = true;
            titleButtonLeft.Text = "About";
        }

        private void SiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.slimsoft.tk");
        }

        private void UpdaterTile_Click(object sender, EventArgs e)
        {
            ReadDefenitions();
            if (trayIcon.Icon != Properties.Resources.Slim_UpdaterIcon_Grey)
            {
                updatesStatusLabel.Visible = false;
                updatePage.BringToFront();
                titleButtonLeft.ArrowLeft = true;
                bool updatesAvailable = CheckForUpdates();
                if (updatesAvailable == false)
                {
                    // No updates are available and therefore the details view is active
                    titleButtonLeft.Text = "Details";
                }
                else
                {
                    titleButtonLeft.Text = "Updates";
                    if (installUpdatesButton.Enabled == false)
                    {
                        installUpdatesButton.Enabled = true;
                    }
                    if (selectAllUpdatesCheckBox.Visible == false)
                    {
                        selectAllUpdatesCheckBox.Visible = true;
                    }
                    AddUpdatesToContentPanel();
                }                             
                topBar.BorderStyle = BorderStyle.None;
            }         
        }

        private void GetNewAppsTile_Click(object sender, EventArgs e)
        {
            newAppsStatusLabel.Visible = false;
            getNewAppsPage.BringToFront();
            titleButtonLeft.ArrowLeft = true;
            titleButtonLeft.Text = "Get New Applications";            
            topBar.BorderStyle = BorderStyle.None;
            CheckForNewApps();
        }

        private void PortableAppsTile_Click(object sender, EventArgs e)
        {
            if (Settings.PortableAppDir != null)
            {                
                installedPortableAppsPage.BringToFront();
                titleButtonLeft.Text = "Portable Apps";
                titleButtonLeft.ArrowLeft = true;
                titleButtonRight.ArrowRight = true;
                aboutButton.Visible = false;
                logButton.Visible = false;
                titleButtonRight.Text = "Get Portable Apps";
                titleButtonRight.Visible = true;
                topBar.BorderStyle = BorderStyle.None;
                CheckForInstalledPortableApps();

                if (downloadPortableButton.Enabled == false)
                {
                    downloadPortableButton.Enabled = true;
                }
            }
            else
            {
                portableStatusLabel.Visible = false;
                setPortableAppFolderPage.BringToFront();
                locationBox2.Text = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments), "Portable Apps");
                titleButtonLeft.ArrowLeft = true;
                titleButtonLeft.Text = "Portable Apps";
                topBar.BorderStyle = BorderStyle.None;
            }
        }

        private void SettingsTile_Click(object sender, EventArgs e)
        {
            Settings.Load();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run") ??
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key.GetValue("Slim Updater") != null)
                {
                    autoStartCheckBox.Checked = true;
                }
            }
            if (Settings.MinimizeToTray == true)
            {
                minimizeToTrayCheckBox.Checked = true;
            }
            if (Settings.DataDir != null)
            {
                dataLocationBox.Text = Settings.DataDir;
            }
            if (Settings.PortableAppDir != null)
            {
                paLocationBox.Text = Settings.PortableAppDir;
            }
            if (Settings.DefenitionURL != null)
            {
                customURLTextBox.Text = Settings.DefenitionURL;
                officialDefenRadioBtn.Checked = false;
                customDefenRadioBtn.Checked = true;
            }                
            settingsPage.BringToFront();
            titleButtonLeft.ArrowLeft = true;
            titleButtonLeft.Text = "Settings";            
            topBar.BorderStyle = BorderStyle.None;           
        }

        private void OfflineRetryLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            offlineLabel.Visible = false;
            offlineRetryLink.Visible = false;
            ReadDefenitions();
            CheckForUpdates();
        }
        #endregion

        #region updatePage Mouse Events
        private void SelectAllUpdatesCheckBox_Click(object sender, EventArgs e)
        {
            if (selectAllUpdatesCheckBox.Checked == true)
            {
                foreach (Control ctl in updateContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = true;
                        selectAllUpdatesCheckBox.Text = "Unselect All";
                    }
                }
            }

            else
            {
                foreach (Control ctl in updateContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = false;
                        selectAllUpdatesCheckBox.Text = "Select All";
                    }
                }
            }
        }

        private void RefreshUpdatesButton_Click(object sender, EventArgs e)
        {
            updatesStatusLabel.Visible = false;
            refreshUpdatesButton.Enabled = false;
            ReadDefenitions();
            bool updatesAvailable = CheckForUpdates();
            if (updatesAvailable)
            {
                AddUpdatesToContentPanel();
            }            
            refreshUpdatesButton.Enabled = true;
        }

        private void InstallUpdatesButton_Click(object sender, EventArgs e)
        {
            InstallUpdates(updateList);
        }
        #endregion

        #region getNewAppsPage Mouse Events
        private void SelectAllAppsCheckBox_Click(object sender, EventArgs e)
        {
            if (selectAllAppsCheckBox.Checked == true)
            {
                foreach (Control ctl in getNewAppsContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = true;
                        selectAllAppsCheckBox.Text = "Unselect All";
                    }
                }
            }
            else
            {
                foreach (Control ctl in getNewAppsContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = false;
                        selectAllAppsCheckBox.Text = "Select All";
                    }
                }
            }
        }

        private void InstallAppsButton_Click(object sender, EventArgs e)
        {
            InstallNewApps(appList);
        }

        private void RefreshAppsButton_Click(object sender, EventArgs e)
        {
            newAppsStatusLabel.Visible = false;
            refreshAppsButton.Enabled = false;
            ReadDefenitions();
            CheckForNewApps();
            refreshAppsButton.Enabled = true;
        }
        #endregion

        #region installedPortableAppsPage Mouse Events
        private void LocationBox2_TextChanged(object sender, EventArgs e)
        {
            if (setPortableAppFolderButton.Enabled == false)
            {
                setPortableAppFolderButton.Enabled = true;
            }
        }

        private void BrowseButton2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    locationBox2.Text = fbd.SelectedPath;
                }
            }

            if (setPortableAppFolderButton.Enabled == false)
            {
                setPortableAppFolderButton.Enabled = true;
            }
        }

        private void SetPortableAppFolderButton_Click(object sender, EventArgs e)
        {
            // Check if selected folder exists
            if (locationBox2.Text == null)
            {
                MessageBox.Show("You must specify a Portable Apps Folder to continue");
            }
            else
            {
                // Test if the folder is accessible without admin rights
                try
                {
                    if (!Directory.Exists(locationBox2.Text))
                    {
                        Directory.CreateDirectory(locationBox2.Text);
                    }

                    File.Create(Path.Combine(locationBox2.Text, "Slim Updater Tempfile")).Close();
                    // Enable ok button and hide error label if the folder is writeable
                    if (setPortableAppFolderButton.Enabled == false)
                    {
                        setPortableAppFolderButton.Enabled = true;
                        paFolderNotWriteableLabel2.Visible = false;
                    }

                    File.Delete(Path.Combine(locationBox2.Text, "Slim Updater Tempfile"));
                    Settings.PortableAppDir = locationBox2.Text;
                    Settings.Save();
                }
                catch (Exception)
                {
                    setPortableAppFolderButton.Enabled = false;
                    paFolderNotWriteableLabel2.Visible = true;
                }

                CheckForInstalledPortableApps();
                installedPortableAppsPage.BringToFront();
                logButton.Visible = false;
                aboutButton.Visible = false;
                titleButtonLeft.Text = "Portable Apps";
                titleButtonLeft.ArrowLeft = true;
                titleButtonRight.Visible = true;
                titleButtonRight.Text = "Get Portable Apps";
                topBar.BorderStyle = BorderStyle.None;
            }
        }

        private void RefreshPortableButton_Click(object sender, EventArgs e)
        {
            CheckForInstalledPortableApps();
        }
        #endregion

        #region getPortableAppsPage Mouse Events
        private void SelectAllPortableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAllPortableCheckBox.Checked == true)
            {
                foreach (Control ctl in getPortableContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = true;
                        selectAllPortableCheckBox.Text = "Unselect All";
                    }
                }
            }
            else
            {
                foreach (Control ctl in getPortableContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = false;
                        selectAllPortableCheckBox.Text = "Select All";
                    }
                }
            }
        }

        private void DownloadPortableButton_Click(object sender, EventArgs e)
        {
            InstallPortableApps(portableAppList, false);
        }

        private void RefreshPortableButton2_Click(object sender, EventArgs e)
        {
            portableStatusLabel.Visible = false;
            CheckForPortableApps();
        }
        #endregion

        #region SettingsPage Events
        private void DataBrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    dataLocationBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void OpenDataDirButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(dataLocationBox.Text))
            {
                Process.Start("explorer.exe", dataLocationBox.Text);
            }
        }

        private void PA_BrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    paLocationBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void OpenPAFolderButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(paLocationBox.Text))
            {
                Process.Start("explorer.exe", paLocationBox.Text);
            }
        }

        private void OfficialDefenRadioBtn_Click(object sender, EventArgs e)
        {
            customURLTextBox.Enabled = false;
        }

        private void CustomDefenRadioBtn_Click(object sender, EventArgs e)
        {
            customURLTextBox.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(paLocationBox.Text) && paLocationBox.Text != "")
            {
                try
                {
                    Directory.CreateDirectory(paLocationBox.Text);
                }
                catch
                {
                    paFolderNotWriteableLabel.Visible = true;
                    return;
                }
            }

            // Test if the portable apps folder is accessible without admin rights
            if (paLocationBox.Text != "")
            {
                try
                {
                    File.Create(Path.Combine(paLocationBox.Text, "Slim Updater Tempfile")).Close();                  
                }
                catch (Exception)
                {
                    paFolderNotWriteableLabel.Visible = true;
                    return;
                }
                // Hide previously shown error label if the folder is writeable
                paFolderNotWriteableLabel.Visible = false;
                File.Delete(Path.Combine(paLocationBox.Text, "Slim Updater Tempfile"));
                Settings.PortableAppDir = paLocationBox.Text;
            }
            else
            {
                Settings.PortableAppDir = null;
            }

            if (!Directory.Exists(dataLocationBox.Text) && dataLocationBox.Text != "")
            {
                try
                {
                    Directory.CreateDirectory(dataLocationBox.Text);
                }
                catch
                {
                    dataFolderNotWriteableLabel.Visible = true;
                    return;
                }
            }

            // Test if the data folder is accessible without admin rights
            if (dataLocationBox.Text != "")
            {
                try
                {
                    File.Create(Path.Combine(dataLocationBox.Text, "Slim Updater Tempfile")).Close();
                }
                catch (Exception)
                {
                    dataFolderNotWriteableLabel.Visible = true;
                    return;
                }
                // Hide previously shown error label if the folder is writeable
                dataFolderNotWriteableLabel.Visible = false;
                File.Delete(Path.Combine(dataLocationBox.Text, "Slim Updater Tempfile"));
                Settings.DataDir = dataLocationBox.Text;
            }
            else
            {
                Settings.DataDir = null;
            }            
            
            if (customDefenRadioBtn.Checked == true && customURLTextBox.Text != null)
            {
                Settings.DefenitionURL = customURLTextBox.Text;
            }
            if (customDefenRadioBtn.Checked == true && customURLTextBox == null)
            {
                MessageBox.Show("You must specify a custom Defenition URL or use the official Defentions");
            }

            if (autoStartCheckBox.Checked == true)
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    key.SetValue("Slim Updater", "\"" + 
                        System.Reflection.Assembly.GetExecutingAssembly().Location.ToString()
                        + "\"" + " /tray");
                }
            }

            if (minimizeToTrayCheckBox.Checked == true)
            {
                Settings.MinimizeToTray = true;
            }
            else
            {
                Settings.MinimizeToTray = false;
            }
            Settings.Save();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Settings.CreateXMLFile();
            // Update control states with the default Settings from xml
            SettingsTile_Click(sender, e);
        }
        #endregion

        #region Tray Icon Click Events
        private void TrayIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            OpenTrayIconMenuItem_Click(sender, e);
            updatePage.BringToFront();
            AddUpdatesToContentPanel();
            titleButtonLeft.ArrowLeft = true;
            topBar.BorderStyle = BorderStyle.None;
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OpenTrayIconMenuItem_Click(sender, e);
            }
        }

        private void CheckUpdatesTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            if (trayIcon.Icon != Properties.Resources.Slim_UpdaterIcon_Grey)
            {
                ReadDefenitions();
                CheckForUpdates();
            }
        }

        private void OpenTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            ReadDefenitions();
            Utilities.ShowFromTray(this);
            if (trayIcon.Icon != Properties.Resources.Slim_UpdaterIcon_Grey)
            {
                CheckForUpdates();
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.Start("https://www.slimsoft.tk/slimupdater/update.xml");
            }
        }

        private void SettingsTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visible == false | this.WindowState == FormWindowState.Minimized)
            {
                Utilities.ShowFromTray(this);
            }
            SettingsTile_Click(sender, e);
        }

        private void ExitTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}