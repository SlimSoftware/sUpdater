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
        public List<App> appList = new List<App>();
        public List<App> updateList;
        public List<App> notInstalledApps = new List<App>();
        public List<PortableApp> portableAppList = new List<PortableApp>();
        public Settings settings = new Settings();
        public Logger logger = new Logger();
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);
        Color normalGrey = Color.FromArgb(141, 141, 141);

        public MainWindow()
        {
            InitializeComponent();           

            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("/tray"))
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
            settings.Load();
        }

        #region MainWindow Events
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (this.Controls[0] == updatePage | this.Controls[0] == getNewAppsPage |
                    this.Controls[0] == installedPortableAppsPage |
                    this.Controls[0] == getPortableAppsPage)
            {
                topBar.BorderStyle = BorderStyle.None;
            }

            ReadDefenitions();
            if (offlineLabel.Visible == false)
            {
                CheckForUpdates();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (settings.MinimizeToTray == true && (e.CloseReason != CloseReason.TaskManagerClosing &&
                e.CloseReason != CloseReason.ApplicationExitCall &&
                e.CloseReason != CloseReason.FormOwnerClosing))
            {
                e.Cancel = true;
                this.Hide();
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
        }
        #endregion

        #region ReadDefenitions()
        public void ReadDefenitions()
        {
            appList = new List<App>();
            XDocument defenitions = new XDocument();

            // Load XML File
            try
            {
                if (settings.DefenitionURL != null)
                {
                    defenitions = XDocument.Load(settings.DefenitionURL);
                }
                else
                {
                    defenitions = XDocument.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");
                }
            }
            catch (Exception e)
            {
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

            if (trayIcon.Icon == Properties.Resources.Slim_UpdaterIcon_Grey)
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

                // Get local version if installed
                string localVersion = null;
                if (regkeyElement.Value.StartsWith("HKEY"))
                {
                    var regValue = Registry.GetValue(regkeyElement.Value, 
                        regvalueElement.Value, null);
                    if (regValue != null)
                    {
                        localVersion = regValue.ToString();
                    }
                }
                else
                {
                    string exePath = regkeyElement.Value;
                    if (exePath.Contains("%pf32%"))
                    {
                        exePath.Replace("%pf64", Environment.ExpandEnvironmentVariables(
                            "%ProgramFiles(x86)%"));
                    }
                    if (exePath.Contains("%pf64%"))
                    {
                        exePath.Replace("%pf32%", Environment.ExpandEnvironmentVariables(
                            "%ProgramW6432%"));
                    }
                    localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
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
            logger.Log("Checking for updates...", Logger.LogLevel.INFO, logTextBox);
            updateList = new List<App>(appList);           

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
                // TODO: Balloon tip only once and if more updates are availble then last checked
                trayIcon.Icon = Properties.Resources.SlimUpdaterIcon_Orange;
                updaterTile.BackColor = normalOrange;
                if (updateList.Count > 1)
                {
                    updaterTile.Text = String.Format("{0} updates available", updateList.Count);
                    logger.Log(string.Format("{0} updates available", updateList.Count),
                        Logger.LogLevel.INFO, logTextBox);
                }
                else
                {
                    updaterTile.Text = String.Format("1 update available");
                    logger.Log(string.Format("1 update available", updateList.Count),
                        Logger.LogLevel.INFO, logTextBox);
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
                updaterTile.BackColor = normalGreen;
                updaterTile.Text = "No updates available";
                logger.Log("No updates available", Logger.LogLevel.INFO, logTextBox);

                // Add all apps to updatecontentPanel for details view
                // Only if page is actually visible and if updates were not just installed
                if (this.Controls[0] == updatePage)
                {
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
                else
                {
                    Label noticeLabel = new Label();
                    noticeLabel.Text = "No updates available.";
                    noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                    // Center
                    noticeLabel.AutoSize = true;
                    noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                    updateContentPanel.Controls.Add(noticeLabel);
                    Utilities.CenterControl(noticeLabel, updateContentPanel, 
                        Utilities.CenterMode.Both);
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
                Separator separator = new Separator();

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

                if (getNewAppsContentPanel.VerticalScroll.Visible == true)
                {
                    Utilities.FixScrollbars(getNewAppsContentPanel.Controls);
                }
            }

            if (notInstalledApps.Count == 0)
            {
                Label noticeLabel = new Label();
                noticeLabel.Text = "No new applications to install found.";
                noticeLabel.Font = new Font("Microsoft Sans Serif", 10);
                // Center
                noticeLabel.AutoSize = true;
                noticeLabel.TextAlign = ContentAlignment.MiddleCenter;
                getNewAppsContentPanel.Controls.Add(noticeLabel);
                Utilities.CenterControl(noticeLabel, getNewAppsContentPanel,
                    Utilities.CenterMode.Both);
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
                Separator separator = new Separator();

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
            if (settings.PortableAppDir != null)
            {
                installedAppPaths = Directory.GetDirectories(settings.PortableAppDir);
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
                                settings.PortableAppDir, portableApp.Name, portableApp.Launch);
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
                        "including its settings and other data?", appItem.Name), "Slim Updater", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        appItem.Status = "Deleting...";
                        Directory.Delete(Path.Combine(settings.PortableAppDir, appItem.Name), true);
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
            logger.Log("Update started...", Logger.LogLevel.INFO, logTextBox);
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
                logger.Log("No updates selected to install, aborting...",
                    Logger.LogLevel.WARN, logTextBox);
            }

            // Download
            List<Task> tasks = new List<Task>();
            int currentUpdate = 0;

            foreach (App update in selectedUpdateList)
            {
                currentUpdate++;
                Task downloadTask = Task.Run(async () =>
                {
                    logger.Log(string.Format("Downloading {0} ({1} of {2}) ...", 
                        update.Name, currentUpdate, selectedUpdateList.Count),
                        Logger.LogLevel.INFO, logTextBox);

                    string fileName = Path.GetFileName(update.DL);
                    update.SavePath = Path.Combine(
                        @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Slim Software\Slim Updater\" + fileName);
                    logger.Log("Saving to: " + update.SavePath, Logger.LogLevel.INFO, logTextBox);

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
                                        update.AppItem.Status = String.Format(
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
                                logger.Log("An error occurred when attempting to download " +
                                    "the update." + e.Message, Logger.LogLevel.ERROR, logTextBox);
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
                                return;
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
                while (tasks.Count > 2)
                {
                    await Task.Delay(1000);
                }
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Install
            currentUpdate = 0;
            foreach (App update in selectedUpdateList)
            {
                currentUpdate++;
                launchInstaller:
                if (File.Exists(update.SavePath))
                {
                    logger.Log(string.Format("Installing {0} ({1} of {2}) ...", update.Name,
                        currentUpdate, selectedUpdateList.Count), Logger.LogLevel.INFO, logTextBox);
                    using (var p = new Process())
                    {
                        p.StartInfo.FileName = update.SavePath;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Arguments = update.InstallSwitch;
                        try
                        {
                            p.Start();
                        }
                        catch (Exception)
                        {
                            var result = MessageBox.Show(
                                "Launching the installer failed. \nWould you like to try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            logger.Log("Launching the installer failed. Asking user for retry.",
                                Logger.LogLevel.INFO, logTextBox);
                            if (result == DialogResult.Yes)
                            {
                                logger.Log("User chose yes.", Logger.LogLevel.INFO, logTextBox);
                                goto launchInstaller;
                            }
                            else
                            {
                                logger.Log("User chose no. Skipping this update.", Logger.LogLevel.INFO, logTextBox);
                                continue;
                            }
                        }
                        update.AppItem.Status = "Installing...";
                        update.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                        p.WaitForExit();
                        if (p.ExitCode == 0)
                        {
                            logger.Log("Installer exited with exit code 0.",
                                Logger.LogLevel.INFO, logTextBox);
                            File.Delete(update.SavePath);
                            update.AppItem.Status = "Install complete";
                            update.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                            update.AppItem.Progress = 100;
                        }
                        if (p.ExitCode != 0)
                        {
                            logger.Log(string.Format("Installation failed. Installer exited with " +
                                "exit code {0}.", p.ExitCode), Logger.LogLevel.ERROR, logTextBox);
                            update.AppItem.Status = String.Format(
                                "Install failed. Exit code: {0}", p.ExitCode);
                            update.AppItem.Progress = 0;
                            update.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                        }
                    }
                }
            }

            // Cleanup any leftover exe's in appdata dir
            if (Directory.GetFiles(Path.Combine(@Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\"),
                "*.exe").Length > 0)
            {
                logger.Log("Cleaning up leftover installers...", Logger.LogLevel.INFO, logTextBox);
                foreach (string exePath in Directory.GetFiles(Path.Combine(@Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\"),
                "*.exe"))
                {
                    File.Delete(exePath);
                }
            }

            if (updateFailed == true)
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
                installUpdatesButton.Enabled = false;
            }
            if (updateFailed == false)
            {
                updatesStatusLabel.ForeColor = normalGreen;
                updatesStatusLabel.Text = "Succesfully installed all updates";
                Utilities.CenterControl(updatesStatusLabel, updatesStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                updatesStatusLabel.Visible = true;
                installUpdatesButton.Enabled = true;
                CheckForUpdates();
            }

            refreshUpdatesButton.Enabled = true;           
        }
        #endregion

        #region InstallNewApps()
        public async void InstallNewApps(List<App> appList)
        {
            bool installFailed = false;
            logger.Log("New app installation started...", Logger.LogLevel.INFO, logTextBox);
            refreshAppsButton.Enabled = false;
            installAppsButton.Enabled = false;
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
                logger.Log("No applications selected to install, aborting...",
                    Logger.LogLevel.WARN, logTextBox);
            }

            // Download
            List<Task> tasks = new List<Task>();
            int currentUpdate = 0;

            foreach (App app in selectedAppList)
            {
                currentUpdate++;
                Task downloadTask = Task.Run(async () =>
                {
                    logger.Log(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentUpdate, selectedAppList.Count),
                        Logger.LogLevel.INFO, logTextBox);

                    string fileName = Path.GetFileName(app.DL);
                    app.SavePath = Path.Combine(
                        @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"Slim Software\Slim Updater\" + fileName);
                    logger.Log("Saving to: " + app.SavePath, Logger.LogLevel.INFO, logTextBox);

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
                                        app.AppItem.Status = String.Format(
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
                                logger.Log("An error occurred when attempting to download " +
                                    "the installer." + e.Message, Logger.LogLevel.ERROR, logTextBox);
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
                                return;
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
            currentUpdate = 0;
            foreach (App app in selectedAppList)
            {
                currentUpdate++;
                launchInstaller:
                if (File.Exists(app.SavePath))
                {
                    logger.Log(string.Format("Installing {0} ({1} of {2}) ...", app.Name,
                        currentUpdate, selectedAppList.Count), Logger.LogLevel.INFO, logTextBox);
                    using (var p = new Process())
                    {
                        p.StartInfo.FileName = app.SavePath;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Arguments = app.InstallSwitch;
                        try
                        {
                            p.Start();
                        }
                        catch (Exception)
                        {
                            var result = MessageBox.Show(
                                "Lauching the installer failed. \nWould you like to try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            logger.Log("Launching the installer failed. Asking user for retry.",
                                Logger.LogLevel.INFO, logTextBox);
                            if (result == DialogResult.Yes)
                            {
                                logger.Log("User chose yes.", Logger.LogLevel.INFO, logTextBox);
                                goto launchInstaller;
                            }
                            else
                            {
                                logger.Log("User chose no. Skipping this app.", Logger.LogLevel.INFO, logTextBox);
                                continue;
                            }
                        }
                        app.AppItem.Status = "Installing...";
                        app.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                        p.WaitForExit();
                        if (p.ExitCode == 0)
                        {
                            logger.Log("Installer exited with exit code 0.",
                                Logger.LogLevel.INFO, logTextBox);
                            File.Delete(app.SavePath);
                            app.AppItem.Status = "Install complete";
                            app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                            app.AppItem.Progress = 100;                            
                        }
                        if (p.ExitCode != 0)
                        {
                            logger.Log(string.Format("Installation failed. Installer exited with " +
                                "exit code {0}.", p.ExitCode), Logger.LogLevel.ERROR, logTextBox);
                            app.AppItem.Status = String.Format(
                                "Install failed. Exit code: {0}", p.ExitCode);
                            app.AppItem.Progress = 0;
                            app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                        }
                    }
                }
            }

            // Cleanup any leftover exe's in appdata dir
            if (Directory.GetFiles(Path.Combine(@Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\"),
                "*.exe").Length > 0)
            {
                logger.Log("Cleaning up leftover installers...", Logger.LogLevel.INFO, logTextBox);
                foreach (string exePath in Directory.GetFiles(Path.Combine(@Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\"),
                "*.exe"))
                {
                    File.Delete(exePath);
                }
            }

            if (installFailed == true)
            {
                // Only show the failed apps
                List<AppItem> failedApps = new List<AppItem>();
                foreach (Control control in getNewAppsContentPanel.Controls)
                {
                    if (control is AppItem appItem)
                    {
                        if (appItem.Status != "Install complete")
                        {
                            failedApps.Add(appItem);
                        }
                    }
                }
                getNewAppsContentPanel.Controls.Clear();
                Utilities.AddAppItems(failedApps, getNewAppsContentPanel);
                newAppsStatusLabel.ForeColor = Color.Red;
                newAppsStatusLabel.Text = "Some applications failed to install.";
                Utilities.CenterControl(newAppsStatusLabel, newAppsStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                newAppsStatusLabel.Visible = true;
            }
            if (installFailed == false)
            {
                newAppsStatusLabel.ForeColor = normalGreen;
                newAppsStatusLabel.ResetText();
                Utilities.CenterControl(newAppsStatusLabel, newAppsStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                newAppsStatusLabel.Visible = true;
                installAppsButton.Enabled = false;
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
                logger.Log("Run Once Portable App install started...", 
                    Logger.LogLevel.INFO, logTextBox);
            }
            else
            {
                logger.Log("Portable App installation started...",
                    Logger.LogLevel.INFO, logTextBox);
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
                    logger.Log("No Portable Apps selected to install, aborting...",
                        Logger.LogLevel.WARN, logTextBox);
                }
            }
            else
            {
                selectedAppList = new List<PortableApp>(portableAppList);
                portableAppList = null;

                if (selectedAppList.Count == 0)
                {
                    MessageBox.Show("You have not selected any Portable Apps.");
                    logger.Log("No Portable Apps selected to install, aborting...",
                        Logger.LogLevel.WARN, logTextBox);
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
                    logger.Log(string.Format("Downloading {0} ({1} of {2}) ...",
                        app.Name, currentApp, selectedAppList.Count), 
                        Logger.LogLevel.INFO, logTextBox);

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            app.AppItem.ShowLink2 = false;
                        }));
                    }

                    Directory.CreateDirectory(Path.Combine(settings.PortableAppDir, app.Name));
                    string fileName = @Path.GetFileName(app.DL);
                    app.SavePath = @Path.Combine(settings.PortableAppDir, app.Name, fileName);
                    logger.Log("Saving to: " + app.SavePath, Logger.LogLevel.INFO, logTextBox);

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
                                        app.AppItem.Status = String.Format(
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
                                logger.Log("An error occurred when attempting to download " +
                                    "the Portable App." + e.Message, Logger.LogLevel.ERROR, logTextBox);
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
                                return;
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
            currentApp = 0;
            string sevenZipPath = null;
            #if DEBUG
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles), "7-Zip")))
                {
                    sevenZipPath = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
                }
                else
                {
                    sevenZipPath = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
                }
            #else
                    if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles), "7-Zip")))
                    {
                        sevenZipPath = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
                    }
                    else
                    {
                        sevenZipPath = Path.Combine(Environment.GetFolderPath(
                                Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
                    }
            #endif

            // Prefer 7-Zip in Program Files if installed
            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles), "7-Zip")))
            {
                sevenZipPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.exe");
            }
            else
            {
                sevenZipPath = Path.Combine(Application.StartupPath, "7z.exe");
            }
            logger.Log("7-Zip path: " + sevenZipPath, Logger.LogLevel.INFO, logTextBox);

            foreach (PortableApp app in selectedAppList)
            {
                if (File.Exists(app.SavePath))
                {
                    if (runOnce == true && app.ExtractMode == "single")
                    {
                        logger.Log("Launching " + app.Name, Logger.LogLevel.INFO, logTextBox);
                        app.AppItem.Status = "Running";
                        using (var ro = new Process())
                        {
                            ro.StartInfo.FileName = Path.Combine(
                                settings.PortableAppDir, app.Name, app.Launch);
                            // TODO: Add support for optional arguments and use shell execute here
                            ro.Start();
                        }
                        await Task.Run(() =>
                        {
                        // TODO: Better watching system if process has exited
                        Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                            while (processes == null | processes.Length != 0)
                            {
                                Thread.Sleep(1000);
                                processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                            }
                        });
                        logger.Log("All processes exited. Cleaning up...", Logger.LogLevel.INFO, logTextBox);

                        // Cleanup
                        Directory.Delete(Path.Combine(settings.PortableAppDir, app.Name), true);
                    }

                    if (app.ExtractMode == "folder")
                    {
                        using (var p = new Process())
                        {
                            p.StartInfo.FileName = sevenZipPath;
                            p.StartInfo.Arguments = "e \"" + app.SavePath + "\" -o\""
                                + Path.Combine(settings.PortableAppDir, app.Name) + "\" -aoa";
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            try
                            {
                                p.Start();
                            }
                            catch (Exception e)
                            {
                                app.AppItem.Status = "Extracting failed: " + e.InnerException;
                                logger.Log("Extracting failed" + e.Message,
                                    Logger.LogLevel.ERROR, logTextBox);
                                return;
                            }

                            app.AppItem.Status = "Extracting...";
                            app.AppItem.ProgressBarStyle = ProgressBarStyle.Marquee;
                            logger.Log(string.Format("Extracting {0} ({1} of {2}) ...",
                                app.Name, currentApp, selectedAppList.Count),
                                Logger.LogLevel.INFO, logTextBox);
                            p.WaitForExit();
                            if (p.ExitCode == 0)
                            {
                                logger.Log("Extract succesful.", Logger.LogLevel.INFO, logTextBox);
                                File.Delete(app.SavePath);
                                app.AppItem.Status = "Install complete";
                                app.AppItem.Progress = 100;
                                app.AppItem.ProgressBarStyle = ProgressBarStyle.Continuous;
                                await Task.Delay(1000);

                                if (runOnce == true)
                                {
                                    logger.Log("Launching " + app.Name, Logger.LogLevel.INFO, logTextBox);
                                    app.AppItem.Status = "Running";
                                    using (var ro = new Process())
                                    {
                                        ro.StartInfo.FileName = Path.Combine(
                                            settings.PortableAppDir, app.Name, app.Launch);
                                        // TODO: Add support for optional arguments and use shell execute here
                                        ro.Start();
                                    }
                                    await Task.Run(() =>
                                    {
                                    // TODO: Better watching system if process has exited
                                    Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                        while (processes == null | processes.Length != 0)
                                        {
                                            Thread.Sleep(1000);
                                            processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(app.Launch));
                                        }
                                    });
                                    logger.Log("All processes exited. Cleaning up...",
                                        Logger.LogLevel.INFO, logTextBox);
                                    Directory.Delete(Path.Combine(settings.PortableAppDir, app.Name), true);
                                }
                            }
                            if (p.ExitCode != 0)
                            {
                                app.AppItem.Status = String.Format(
                                    "Extract failed. Exit code: {0}", p.ExitCode);
                                app.AppItem.Progress = 0;
                                logger.Log("Extract failed. Exit code: " + p.ExitCode,
                                    Logger.LogLevel.ERROR, logTextBox);
                            }
                        }
                    }
                }
            }

            if (installFailed == true)
            {
                // Only show the failed apps
                List<AppItem> failedApps = new List<AppItem>();
                foreach (Control control in getPortableContentPanel.Controls)
                {
                    if (control is AppItem appItem)
                    {
                        if (appItem.Status != "Install complete")
                        {
                            failedApps.Add(appItem);
                        }
                    }
                }
                getPortableContentPanel.Controls.Clear();
                Utilities.AddAppItems(failedApps, getPortableContentPanel);
                portableStatusLabel.ForeColor = Color.Red;
                portableStatusLabel.Text = "Some Portable Apps failed to install.";
                Utilities.CenterControl(portableStatusLabel, portableStatusLabel.Parent, 
                    Utilities.CenterMode.Horizontal);
                portableStatusLabel.Visible = true;
                downloadPortableButton.Enabled = false;
            }
            if (installFailed == false)
            {
                portableStatusLabel.ForeColor = normalGreen;
                portableStatusLabel.ResetText();
                Utilities.CenterControl(portableStatusLabel, 
                    portableStatusLabel.Parent, Utilities.CenterMode.Horizontal);
                portableStatusLabel.Visible = true;
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
            if (settings.DefenitionURL != null)
            {
                defenitionURL = settings.DefenitionURL;
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
            if (topBar.Size.Height != 35)
            {
                topBar.Size = new Size(topBar.Size.Width, 35);
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
                    titleButtonLeft.Text = "Home";
                    titleButtonLeft.ArrowLeft = false;
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
                    // TODO: Don't use Controls[0].Name but just Controls[0].
                    if (this.Controls[0].Name == "updatePage")
                    {
                        titleButtonLeft.Text = "Updates";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0].Name == "getNewAppsPage")
                    {
                        titleButtonLeft.Text = "Get New Applications";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0].Name == "portableAppsPage")
                    {
                        titleButtonLeft.Text = "Updates";
                        topBar.BorderStyle = BorderStyle.None;
                    }
                    if (this.Controls[0].Name == "getPortableAppsPage")
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
                        titleButtonLeft.Text = "Home";
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonRight.Visible = false;
                        updatesStatusLabel.Visible = false;
                        newAppsStatusLabel.Visible = false;
                        portableStatusLabel.Visible = false;
                    }
                    else
                    {
                        if (updaterTile.Text != "No updates available")
                        {
                            updaterTile.BackColor = normalOrange;
                        }
                        startPage.BringToFront();
                        titleButtonLeft.Text = "Home";
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonRight.ArrowRight = false;
                        updatesStatusLabel.Visible = false;
                        newAppsStatusLabel.Visible = false;
                        portableStatusLabel.Visible = false;
                    }
                }
                if (aboutButton.Visible == false)
                {
                    aboutButton.Visible = true;
                }
                if (logButton.Visible == false)
                {
                    logButton.Visible = true;
                }
            }
        }

        private void TitleButtonRight_Click(object sender, EventArgs e)
        {
            getPortableAppsPage.BringToFront();
            titleButtonLeft.Text = "Portable Apps";
            titleButtonLeft.ArrowLeft = true;
            titleButtonRight.Visible = false;
            topBar.BorderStyle = BorderStyle.None;
            CheckForPortableApps();
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            logPage.BringToFront();
            titleButtonLeft.Text = "Log";
            titleButtonLeft.ArrowLeft = true;
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
            topBar.Size = new Size(topBar.Size.Width, 35);
            aboutPage.BringToFront();
            titleButtonLeft.Text = "About";
            titleButtonLeft.ArrowLeft = true;
            aboutButton.Hide();
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
                updatePage.BringToFront();
                bool updatesAvailable = CheckForUpdates();
                if (updatesAvailable == false)
                {
                    // No updates are available and therefore the details view is active
                    titleButtonLeft.Text = "Details";
                }
                else
                {
                    titleButtonLeft.Text = "Updates";
                }
                AddUpdatesToContentPanel();
                titleButtonLeft.ArrowLeft = true;                
                topBar.BorderStyle = BorderStyle.None;
            }

            if (installUpdatesButton.Enabled == false)
            {
                installUpdatesButton.Enabled = true;
            }
        }

        private void GetNewAppsTile_Click(object sender, EventArgs e)
        {
            getNewAppsPage.BringToFront();
            titleButtonLeft.Text = "Get New Applications";
            titleButtonLeft.ArrowLeft = true;
            topBar.BorderStyle = BorderStyle.None;
            CheckForNewApps();

            if (installAppsButton.Enabled == false)
            {
                installAppsButton.Enabled = true;
            }
        }

        private void PortableAppsTile_Click(object sender, EventArgs e)
        {
            if (settings.PortableAppDir != null)
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
                setPortableAppFolderPage.BringToFront();
                locationBox2.Text = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments), "Portable Apps");
                titleButtonLeft.Text = "Portable Apps";
                titleButtonLeft.ArrowLeft = true;
                topBar.BorderStyle = BorderStyle.None;
            }
        }

        private void SettingsTile_Click(object sender, EventArgs e)
        {
            settings.Load();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run") ??
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key.GetValue("Slim Updater") != null)
                {
                    autoStartCheckBox.Checked = true;
                }
            }
            if (settings.MinimizeToTray == true)
            {
                minimizeToTrayCheckBox.Checked = true;
            }
            if (settings.PortableAppDir != null)
            {
                locationBox1.Text = settings.PortableAppDir;
            }
            if (settings.DefenitionURL != null)
            {
                customURLTextBox.Text = settings.DefenitionURL;
                officialDefenRadioBtn.Checked = false;
                customDefenRadioBtn.Checked = true;
            }                
            settingsPage.BringToFront();
            titleButtonLeft.Text = "Settings";
            titleButtonLeft.ArrowLeft = true;
            topBar.BorderStyle = BorderStyle.None;           
        }

        private void OfflineRetryLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            offlineLabel.Visible = false;
            offlineRetryLink.Visible = false;
            ReadDefenitions();
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
            refreshUpdatesButton.Enabled = false;
            ReadDefenitions();
            CheckForUpdates();
            AddUpdatesToContentPanel();
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
                    settings.PortableAppDir = locationBox2.Text;
                    settings.Save();
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

        private void SelectAllPortableCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAllPortableCheckBox1.Checked == true)
            {
                foreach (Control ctl in installedPortableContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = true;
                        selectAllPortableCheckBox1.Text = "Unselect All";
                    }
                }
            }
            else
            {
                foreach (Control ctl in installedPortableContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = false;
                        selectAllPortableCheckBox1.Text = "Select All";
                    }
                }
            }
        }

        private void RefreshPortableButton_Click(object sender, EventArgs e)
        {
            CheckForInstalledPortableApps();
        }
        #endregion

        #region getPortableAppsPage Mouse Events
        private void SelectAllPortableCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAllPortableCheckBox2.Checked == true)
            {
                foreach (Control ctl in getPortableContentPanel.Controls)
                {
                    if (ctl is AppItem)
                    {
                        (ctl as AppItem).Checked = true;
                        selectAllPortableCheckBox2.Text = "Unselect All";
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
                        selectAllPortableCheckBox2.Text = "Select All";
                    }
                }
            }
        }

        private void DownloadPortableButton_Click(object sender, EventArgs e)
        {
            InstallPortableApps(portableAppList, true);
        }

        private void RefreshPortableButton2_Click(object sender, EventArgs e)
        {
            CheckForPortableApps();
        }
    #endregion

        #region settingsPage Events
        private void LocationBox1_TextChanged(object sender, EventArgs e)
        {
            if (saveButton.Enabled == false)
            {
                saveButton.Enabled = true;
            }
        }

        private void BrowseButton1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    locationBox1.Text = fbd.SelectedPath;
                }
            }

            if (saveButton.Enabled == false)
            {
                saveButton.Enabled = true;
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
            // Test if the folder is accessible without admin rights
            try
            {
                File.Create(Path.Combine(locationBox1.Text, "Slim Updater Tempfile")).Close();
                // Enable ok button and hide error label if the folder is writeable
                if (setPortableAppFolderButton.Enabled == false)
                {
                    saveButton.Enabled = true;
                    paFolderNotWriteableLabel1.Visible = false;
                    File.Delete(Path.Combine(locationBox1.Text, "Slim Updater Tempfile"));
                }
            }
            catch (Exception)
            {
                saveButton.Enabled = false;
                paFolderLocationLabel.ResetText();
                paFolderNotWriteableLabel1.Visible = true;
            }

            if (!Directory.Exists(locationBox1.Text) && locationBox1.Text != "")
            {
                Directory.CreateDirectory(locationBox1.Text);
            }

            settings.PortableAppDir = locationBox1.Text;
            if (customDefenRadioBtn.Checked == true && customURLTextBox.Text != null)
            {
                settings.DefenitionURL = customURLTextBox.Text;
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
            if (customDefenRadioBtn.Checked == true && customURLTextBox == null)
            {
                MessageBox.Show("You must specify a custom Defenition URL or use the official Defentions");
            }
            settings.Save();
        }
        #endregion

        #region Tray Icon Click Events
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
                CheckForUpdates();
            }
        }

        private void OpenTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            ReadDefenitions();
            if (trayIcon.Icon != Properties.Resources.Slim_UpdaterIcon_Grey)
            {
                CheckForUpdates();
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.Start("https://www.slimsoft.tk/slimupdater/update.xml");
            }
            this.Show();
        }

        private void SettingsTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Visible == false | this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Show();
            }
            SettingsTile_Click(sender, e);
        }

        private void ExitTrayIconMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }

    #region App Class
    public class App
    {
        public string Name { get; set; }
        public AppItem AppItem { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string DL { get; set; }
        public string SavePath { get; set; }
        public string InstallSwitch { get; set; }

        public App(string name, string latestVersion, string localVersion, string arch,
            string type, string installSwitch, string dl, AppItem appItem = null, 
            string savePath = null)
        {
            Name = name;
            AppItem = AppItem;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DL = dl;
            SavePath = savePath;
        }
    }
    #endregion

    #region Portable App Class
    public class PortableApp
    {
        public string Name { get; set; }
        public AppItem AppItem { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }

        public PortableApp(string name, string latestVersion, string localVersion, string arch,
            string launch, string dl, string extractMode, AppItem appItem = null,
            string savePath = null)
        {
            Name = name;
            AppItem = appItem;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Launch = launch;
            DL = dl;
            ExtractMode = extractMode;
            SavePath = savePath;
        }
    }
    #endregion

    #region Settings Class
    public class Settings
    {
        public bool MinimizeToTray { get; set; }        
        public string DefenitionURL { get; set; }
        public string PortableAppDir { get; set; }       

        public void Load()
        {
            // Load XML File
            if (File.Exists(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), 
                    @"Slim Software\Slim Updater\Settings.xml")))
            {
                XDocument settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), 
                    @"Slim Software\Slim Updater\Settings.xml"));

                // Get content from XML nodes
                string defenitionURL = settingXML.Root.Element("DefenitionURL").Value;
                string portableAppDir = settingXML.Root.Element("PortableAppDir").Value;
                if (settingXML.Root.Element("MinimizeToTray").Value != null)
                {
                    MinimizeToTray = XmlConvert.ToBoolean(
                        settingXML.Root.Element("MinimizeToTray").Value);
                }
                if (defenitionURL != string.Empty)
                {
                    DefenitionURL = defenitionURL;
                }
                if (portableAppDir != string.Empty)
                {
                    PortableAppDir = portableAppDir;
                }

                // Unload XML File
                settingXML = null;
            }
            else
            {
                CreateXMLFile();
            }
        }
        
        public void CreateXMLFile()
        {
            // Check if folder exists
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater"));
            }

            XDocument doc =
            new XDocument(new XElement("Settings", new XElement("DefenitionURL", String.Empty),
                new XElement("PortableAppDir"), String.Empty, new XElement("MinimizeToTray", "true")));
            doc.Save(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));
            // Unload XML
            doc = null;      
        }

        public void Save()
        {
            // Check if XML File exists
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")))
            {
                CreateXMLFile();
            }

            // Load XML File
            XElement settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")).Element("Settings");

            // Save values
            if (DefenitionURL != null)
            {
                settingXML.Descendants("DefenitionURL").Remove();
                XElement defenitionURL = new XElement("DefenitionURL");
                defenitionURL.Value = DefenitionURL;
                settingXML.Add(defenitionURL);
            }
            if (PortableAppDir != null)
            {
                settingXML.Descendants("PortableAppDir").Remove();
                XElement portableAppDir = new XElement("PortableAppDir");
                portableAppDir.Value = PortableAppDir;
                settingXML.Add(portableAppDir);
            }
            if (MinimizeToTray != XmlConvert.ToBoolean(settingXML.Element("MinimizeToTray").Value))
            {
                settingXML.Descendants("MinimizeToTray").Remove();
                XElement minimizeToTray = new XElement("MinimizeToTray");
                minimizeToTray.Value = MinimizeToTray.ToString();
                settingXML.Add(minimizeToTray);
            }

            // Save XML File
            settingXML.Save(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));

            // Unload XML File
            settingXML = null;
        }
    }
    #endregion
}