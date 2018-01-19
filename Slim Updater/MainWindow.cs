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

namespace Slim_Updater
{
    public partial class MainWindow : Form
    {
        public List<App> appList = new List<App>();
        public List<App> updateList;
        public List<App> notInstalledApps = new List<App>();
        public List<PortableApp> portableAppList = new List<PortableApp>();
        public List<App> failedInstallList = new List<App>();
        public List<PortableApp> failedPortableList = new List<PortableApp>();
        public Settings settings = new Settings();
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);

        public MainWindow()
        {
            InitializeComponent();
            settings.Load();
            ReadDefenitions();
            CheckForUpdates();
        }

        #region ReadDefenitions()
        public void ReadDefenitions()
        {
            // Load XML File
            XDocument defenitions = XDocument.Load("http://www.slimsoft.tk/slimupdater/defenitions.xml");
            foreach (XElement appElement in defenitions.Descendants("app"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = appElement.Attribute("name");
                XElement versionElement = appElement.Element("version");
                XElement descriptionElement = appElement.Element("description");
                XElement archElement = appElement.Element("arch");
                XElement typeElement = appElement.Element("type");
                XElement switchElement = appElement.Element("switch");
                XElement dlElement = appElement.Element("dl");
                XElement regkeyElement = appElement.Element("regkey");
                XElement regvalueElement = appElement.Element("regvalue");
                XElement changelogElement = appElement.Element("changelog");

                // Get local version if installed
                string localVersion = null;
                object regValue = Registry.GetValue(regkeyElement.Value, regvalueElement.Value, null);
                if (regValue != null)
                {
                    localVersion = regValue.ToString();
                }

                // Remove first newline and/or tabs from app description and changelog if present
                if (changelogElement != null)
                {
                    if (changelogElement.Value.StartsWith("\n"))
                    {
                        changelogElement.Value = changelogElement.Value.TrimStart("\n".ToCharArray());
                    }

                    if (changelogElement.Value.Contains("\t"))
                    {
                        changelogElement.Value = changelogElement.Value.Replace("\t", String.Empty);
                    }
                }

                if (descriptionElement != null)
                {
                    if (descriptionElement.Value.StartsWith("\n"))
                    {
                        descriptionElement.Value = descriptionElement.Value.TrimStart("\n".ToCharArray());
                    }
                }

                // Add app to appList
                if (changelogElement != null && descriptionElement == null)
                {
                    appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                        dlElement.Value, changelogElement.Value, null));
                }

                if (changelogElement == null && descriptionElement != null)
                {
                    appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                        dlElement.Value, null, descriptionElement.Value));
                }

                if (changelogElement == null && descriptionElement == null)
                {
                    appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                        dlElement.Value, null, null));
                }

                if (changelogElement != null && descriptionElement != null)
                {
                    appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value, localVersion,
                        changelogElement.Value, archElement.Value, typeElement.Value, switchElement.Value,
                        dlElement.Value, descriptionElement.Value));
                }
            }
        }
        #endregion

        #region Check Methods

        #region CheckForUpdates()
        public bool CheckForUpdates()
        {
            updateContentPanel.Controls.Clear();

            updateList = new List<App>(appList);
            int previousY = 0;
            int previousHeight = 0;
            foreach (App app in updateList.ToArray())
            {
                AppItem appItem = new AppItem();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(app.Changelog);
                };
                Separator separator = new Separator();

                // Remove not installed apps from the updateList
                if (app.LocalVersion == null)
                {
                    updateList.Remove(app);
                    continue;
                }
                else
                {
                    // Remove up to date apps from the updateList
                    if (float.Parse(app.LatestVersion.ToString()) <= 
                        float.Parse(app.LocalVersion.ToString()))
                    {
                        updateList.Remove(app);
                        continue;
                    }
                }
                // Add app to the content panel
                appItem.Name = app.Name + " " + app.LatestVersion;
                appItem.Version = "Installed: " + app.LocalVersion;

                if (updateContentPanel.Controls.Count == 0)
                {
                    updateContentPanel.Controls.Add(separator);
                    separator = new Separator()
                    {
                        Location = new Point(0, 45)
                    };
                    updateContentPanel.Controls.Add(separator);
                    updateContentPanel.Controls.Add(appItem);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
                else
                {
                    appItem.Location = new Point(0, (previousY + previousHeight));
                    separator.Location = new Point(0, (appItem.Location.Y + 45));
                    updateContentPanel.Controls.Add(appItem);
                    updateContentPanel.Controls.Add(separator);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
            }

            // Change updaterTile on the startpage accordingly
            if (updateList.Count != 0)
            {
                updaterTile.BackColor = normalOrange;
                updaterTile.Text = String.Format("{0} updates available", updateList.Count);
                if (updateContentPanel.VerticalScroll.Visible == true)
                {
                    FixScrollbars(updateContentPanel.Controls);
                }
                return true;
            }
            else
            {
                updaterTile.BackColor = normalGreen;
                updaterTile.Text = "No updates available";

                // Add all apps to updatecontentPanel for details view
                if (this.Controls[0] == updatePage) // Only if page is actually visible
                {
                    foreach (App app in appList)
                    {
                        AppItem appItem = new AppItem();
                        Separator separator = new Separator();

                        if (app.LocalVersion != null)
                        {
                            appItem.Name = app.Name + " " + app.LatestVersion;
                            appItem.Version = "Installed: " + app.LocalVersion;
                        }
                        else
                        {
                            appItem.Name = app.Name + " " + app.LatestVersion;
                            appItem.Version = "Not Found";
                        }

                        if (updateContentPanel.Controls.Count == 0)
                        {
                            updateContentPanel.Controls.Add(separator);
                            separator = new Separator()
                            {
                                Location = new Point(0, 45)
                            };
                            updateContentPanel.Controls.Add(separator);
                            updateContentPanel.Controls.Add(appItem);
                            previousY = appItem.Location.Y;
                            previousHeight = appItem.Height;
                        }
                        else
                        {
                            appItem.Location = new Point(0, (previousY + previousHeight));
                            separator.Location = new Point(0, (appItem.Location.Y + 45));
                            updateContentPanel.Controls.Add(appItem);
                            updateContentPanel.Controls.Add(separator);
                            previousY = appItem.Location.Y;
                            previousHeight = appItem.Height;
                        }
                    }

                    if (updateContentPanel.VerticalScroll.Visible == true)
                    {
                        FixScrollbars(updateContentPanel.Controls);
                    }

                    // Hide select all checkbox and bottom buttons for details view
                    updateContentPanel.Location = new Point(0, 0);
                    updateContentPanel.Size = new Size(updateContentPanel.Size.Width, 425);
                    selectAllUpdatesCheckBox.Visible = false;
                    installUpdatesButton.Visible = false;
                    refreshUpdatesButton.Visible = false;
                }
                return false;
            }
        }
        #endregion

        #region CheckForNonInstalledApps()
        public void CheckForNonInstalledApps()
        {
            getNewAppsContentPanel.Controls.Clear();

            int previousY = 0;
            int previousHeight = 0;
            foreach (App app in appList.ToArray())
            {
                AppItem appItem = new AppItem();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(app.Description);
                };
                appItem.Checked = false;
                Separator separator = new Separator();

                // Make sure installed and recenter apps are not included
                if (app.LocalVersion == null || app.LocalVersion != null &&
                    !(float.Parse(app.LatestVersion.ToString()) >= 
                    float.Parse(app.LocalVersion.ToString())))
                {
                    notInstalledApps.Add(app);
                    // Add app to the content panel
                    appItem.Name = app.Name;
                    appItem.Version = app.LatestVersion;
                    if (getNewAppsContentPanel.Controls.Count == 0)
                    {
                        getNewAppsContentPanel.Controls.Add(separator);
                        separator = new Separator()
                        {
                            Location = new Point(0, 45)
                        };
                        getNewAppsContentPanel.Controls.Add(separator);
                        getNewAppsContentPanel.Controls.Add(appItem);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height;
                    }
                    else
                    {
                        appItem.Location = new Point(0, (previousY + previousHeight));
                        separator.Location = new Point(0, (appItem.Location.Y + 45));
                        getNewAppsContentPanel.Controls.Add(appItem);
                        getNewAppsContentPanel.Controls.Add(separator);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height;
                    }
                }
            }

            if (getNewAppsContentPanel.VerticalScroll.Visible == true)
            {
                FixScrollbars(getNewAppsContentPanel.Controls);
            }
        }
        #endregion

        #region CheckForPortableApps()
        private void CheckForPortableApps()
        {
            portableAppList.Clear();
            getPortableContentPanel.Controls.Clear();

            // Load XML File
            XElement defenitions = XElement.Load("http://www.slimsoft.tk/slimupdater/defenitions.xml");

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

                // Remove first newline and/or tabs from app description and changelog if present
                if (changelogElement != null)
                {
                    if (changelogElement.Value.StartsWith("\n"))
                    {
                        changelogElement.Value = changelogElement.Value.TrimStart("\n".ToCharArray());
                    }

                    if (changelogElement.Value.Contains("\t"))
                    {
                        changelogElement.Value = changelogElement.Value.Replace("\t", String.Empty);
                    }
                }

                if (descriptionElement != null)
                {
                    if (descriptionElement.Value.StartsWith("\n"))
                    {
                        descriptionElement.Value = descriptionElement.Value.TrimStart("\n".ToCharArray());
                    }
                }

                // Add app to portableAppList
                if (changelogElement == null && descriptionElement != null)
                {
                    portableAppList.Add(new PortableApp(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                        extractModeElement.Value, changelogElement.Value, null));
                }

                if (changelogElement != null && descriptionElement == null)
                {
                    portableAppList.Add(new PortableApp(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                        extractModeElement.Value, null, descriptionElement.Value));
                }

                if (changelogElement == null && descriptionElement == null)
                {
                    portableAppList.Add(new PortableApp(nameAttribute.Value.ToString(), versionElement.Value,
                        localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                        extractModeElement.Value, null, null));
                }

                if (changelogElement != null && descriptionElement != null)
                {
                    portableAppList.Add(new PortableApp(nameAttribute.Value.ToString(), versionElement.Value, localVersion,
                        changelogElement.Value, archElement.Value, launchElement.Value, dlElement.Value,
                        savePathElement.Value, extractModeElement.Value, descriptionElement.Value));
                }
            }

            int previousY = 0;
            int previousHeight = 0;
            Separator separator = new Separator();

            foreach (PortableApp portableApp in portableAppList.ToArray())
            {
                AppItem appItem = new AppItem();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(portableApp.Description);
                };
                appItem.Link2Clicked += (sender, e) =>
                {
                    List<PortableApp> selectedAppList = new List<PortableApp>();
                    selectedAppList.Add(portableAppList.Find(x => x.Name == appItem.Name));
                    InstallPortableApps(selectedAppList, true);
                };
                appItem.Checked = false;

                // Make sure only not installed apps are included
                if (portableApp.LocalVersion == "-" || portableApp.LocalVersion != null
                    && !(float.Parse(portableApp.LatestVersion.ToString()) >= float.Parse(portableApp.LocalVersion.ToString())))
                {
                    // Add app to the content panel
                    appItem.Name = portableApp.Name;
                    appItem.Version = portableApp.LatestVersion;
                    appItem.Link2Text = "Run";
                    appItem.ShowLink2 = true;

                    if (getPortableContentPanel.Controls.Count == 0)
                    {
                        getPortableContentPanel.Controls.Add(separator);
                        separator = new Separator()
                        {
                            Location = new Point(0, 45)
                        };                       
                        getPortableContentPanel.Controls.Add(appItem);
                        getPortableContentPanel.Controls.Add(separator);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height;
                    }
                    else
                    {
                        appItem.Location = new Point(0, (previousY + previousHeight));
                        separator.Location = new Point(0, (appItem.Location.Y + 45));
                        getPortableContentPanel.Controls.Add(appItem);
                        getPortableContentPanel.Controls.Add(separator);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height;
                    }
                }
                portableApp.AppItem = appItem;
            }

            if (updateContentPanel.VerticalScroll.Visible == true)
            {
                FixScrollbars(updateContentPanel.Controls);
            }
        }
        #endregion

        #region CheckForInstalledPortableApps()
        private void CheckForInstalledPortableApps()
        {
            CheckForPortableApps();
            installedPortableContentPanel.Controls.Clear();

            string[] installedAppPaths = Directory.GetDirectories(settings.PortableAppDir);
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
                noticeLabel.Left = (installedPortableContentPanel.Width
                    - noticeLabel.Width) / 2;
                noticeLabel.Top = (installedPortableContentPanel.Height
                    - noticeLabel.Height - topBar.Height) / 2;
                return;
            }

            int previousY = 0;
            int previousHeight = 0;
            Separator separator = new Separator();

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
                    ShowDetails(portableApp.Description);
                };

                if (installedPortableContentPanel.Controls.Count == 0)
                {
                    installedPortableContentPanel.Controls.Add(separator);
                    separator = new Separator()
                    {
                        Location = new Point(0, 45)
                    };
                    installedPortableContentPanel.Controls.Add(separator);
                    installedPortableContentPanel.Controls.Add(appItem);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
                else
                {
                    appItem.Location = new Point(0, (previousY + previousHeight));
                    separator.Location = new Point(0, (appItem.Location.Y + 45));
                    installedPortableContentPanel.Controls.Add(appItem);
                    installedPortableContentPanel.Controls.Add(separator);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
                portableApp.AppItem = appItem;
            }

            if (updateContentPanel.VerticalScroll.Visible == true)
            {
                FixScrollbars(updateContentPanel.Controls);
            }
        }
        #endregion

        #endregion

        #region Install Methods

        #region InstallUpdates()
        public async void InstallUpdates(List<App> updateList)
        {
            refreshUpdatesButton.Enabled = false;
            installUpdatesButton.Enabled = false;

            int i = 0;
            int j = 0;
            List<int> indexList = new List<int>();
            List<App> selectedUpdateList = new List<App>(updateList);

            foreach (Control update in updateContentPanel.Controls)
            {
                i++;
                if (!(update is Separator))
                {
                    j++;
                    if ((update as AppItem).Checked == true)
                    {
                        indexList.Add(i - 1);
                    }
                    else
                    {
                        selectedUpdateList.RemoveAt(j - 1);
                    }
                }
            }
            // Download
            i = 0;
            List<Task> tasks = new List<Task>();
            foreach (App update in selectedUpdateList)
            {
                Task downloadTask = Task.Run(async () =>
                {
                    i++;
                    int index = indexList[i - 1];
                    string fileName = Path.GetFileName(update.DL);
                    update.SavePath = Path.Combine(
                        @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"SlimSoftware\Slim Updater\" + fileName);

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

                                // Get the correct AppItem to set the progress for
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        (updateContentPanel.Controls[index] as AppItem).Progress = e.ProgressPercentage / 2;
                                        (updateContentPanel.Controls[index] as AppItem).Status = String.Format(
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
                                        (updateContentPanel.Controls[index] as AppItem).Status = "Download complete";
                                    }));
                                }
                            };
                            await wc.DownloadFileTaskAsync(new Uri(update.DL), update.SavePath);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                (updateContentPanel.Controls[index] as AppItem).Progress = 50;
                            }));
                        }
                    }
                });
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Install
            i = 0;
            foreach (App update in selectedUpdateList)
            {
                i++;
                int index = indexList[i - 1];
                launchInstaller:
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
                            "Lauching the installer failed. \nWould you like to try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            goto launchInstaller;
                        }
                        if (result == DialogResult.No)
                        {
                            if (i == selectedUpdateList.Count)
                            {
                                goto enableButtons;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    (updateContentPanel.Controls[index] as AppItem).Status = "Installing...";
                    p.WaitForExit();
                    if (p.ExitCode == 0)
                    {
                        (updateContentPanel.Controls[index] as AppItem).Status = "Install complete";
                        (updateContentPanel.Controls[index] as AppItem).Progress = 100;
                    }
                    if (p.ExitCode != 0)
                    {
                        (updateContentPanel.Controls[index] as AppItem).Status = String.Format(
                            "Install failed. Exit code: {0}", p.ExitCode);
                        (updateContentPanel.Controls[index] as AppItem).Progress = 0;
                        refreshUpdatesButton.Enabled = true;
                        failedInstallList.Add(update);
                    }
                    enableButtons:
                    if (refreshUpdatesButton.Enabled == false | installUpdatesButton.Enabled == false)
                    {
                        refreshUpdatesButton.Enabled = true;
                        installUpdatesButton.Enabled = true;
                    }
                }
            }

            if (failedInstallList.Count > 0)
            {
                // Check for failed updates and ask for retry
                failedUpdateLabel.Visible = true;
                installUpdatesButton.Text = "Yes";
                refreshUpdatesButton.Text = "No";
            }
            else
            {
                // All updates installed succesfully
                await Task.Delay(1500);
                CheckForUpdates();
            }
        }
        #endregion

        #region InstallNewApps()
        public async void InstallNewApps(List<App> appList)
        {
            refreshAppsButton.Enabled = false;
            installAppsButton.Enabled = false;

            int i = 0;
            int j = 0;
            List<int> indexList = new List<int>();
            List<App> selectedAppList = new List<App>(appList);

            foreach (Control app in getNewAppsContentPanel.Controls)
            {
                i++;
                if (!(app is Separator))
                {
                    j++;
                    if ((app as AppItem).Checked == true)
                    {
                        indexList.Add(i - 1);
                    }
                    else
                    {
                        selectedAppList.RemoveAt(j - 1);
                    }
                }
            }
            // Download
            i = 0;
            List<Task> tasks = new List<Task>();
            foreach (App app in selectedAppList)
            {
                Task downloadTask = Task.Run(async () =>
                {
                    i++;
                    int index = indexList[i - 1];
                    string fileName = Path.GetFileName(app.DL);
                    app.SavePath = Path.Combine(
                        @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        @"SlimSoftware\Slim Updater\" + fileName);

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

                                // Get the correct AppItem to set the progress for
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        (getNewAppsContentPanel.Controls[index] as AppItem).Progress = e.ProgressPercentage / 2;
                                        (getNewAppsContentPanel.Controls[index] as AppItem).Status = String.Format(
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
                                        (getNewAppsContentPanel.Controls[index] as AppItem).Status = "Download complete";
                                    }));
                                }
                            };
                            await wc.DownloadFileTaskAsync(new Uri(app.DL), app.SavePath);
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                (getNewAppsContentPanel.Controls[index] as AppItem).Progress = 50;
                            }));
                        }
                    }
                });
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());

            // Install
            i = 0;
            foreach (App app in selectedAppList)
            {
                i++;
                int index = indexList[i - 1];
                launchInstaller:
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
                        if (result == DialogResult.Yes)
                        {
                            goto launchInstaller;
                        }
                        if (result == DialogResult.No)
                        {
                            if (i == selectedAppList.Count)
                            {
                                goto enableButtons;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    (getNewAppsContentPanel.Controls[index] as AppItem).Status = "Installing...";
                    p.WaitForExit();
                    if (p.ExitCode == 0)
                    {
                        (getNewAppsContentPanel.Controls[index] as AppItem).Status = "Install complete";
                        (getNewAppsContentPanel.Controls[index] as AppItem).Progress = 100;
                    }
                    if (p.ExitCode != 0)
                    {
                        (getNewAppsContentPanel.Controls[index] as AppItem).Status = String.Format(
                            "Install failed. Exit code: {0}", p.ExitCode);
                        (getNewAppsContentPanel.Controls[index] as AppItem).Progress = 0;
                        refreshAppsButton.Enabled = true;
                        failedInstallList.Add(app);
                    }
                    enableButtons:
                    if (refreshAppsButton.Enabled == false | installAppsButton.Enabled == false)
                    {
                        refreshAppsButton.Enabled = true;
                        installAppsButton.Enabled = true;
                    }
                }
            }

            if (failedInstallList.Count > 0)
            {
                // Check for failed updates and ask for retry
                failedUpdateLabel.Visible = true;
                installAppsButton.Text = "Yes";
                refreshAppsButton.Text = "No";
            }
            else
            {
                // All updates installed succesfully
                await Task.Delay(1500);
                CheckForNonInstalledApps();
            }
        }
        #endregion

        #region InstallPortableApps()
        public async void InstallPortableApps(List<PortableApp> portableAppList, bool runOnce)
        {
            refreshPortableButton.Enabled = false;
            downloadPortableButton.Enabled = false;
            List<PortableApp> selectedAppList = new List<PortableApp>(portableAppList);
            int i = 0;

            if (runOnce != true)
            {
                foreach (Control app in installedPortableContentPanel.Controls)
                {
                    if (!(app is Separator))
                    {
                        i++;
                        (app as AppItem).ShowLink2 = false;
                    }
                    else
                    {
                        // Remove non-selected apps from the list
                        selectedAppList.RemoveAt(i - 1);
                        installedPortableContentPanel.Controls.Remove(app as AppItem);
                    }
                }

                if (selectedAppList.Count == 0)
                {
                    MessageBox.Show("You have not selected any Portable Apps.");
                }
            }
            else
            {
                selectedAppList = new List<PortableApp>(portableAppList);
                portableAppList = null;
            }

            // Download
            i = 0;
            List<Task> tasks = new List<Task>();

            foreach (PortableApp app in selectedAppList)
            {
                Task downloadTask = Task.Run(async () =>
                {
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
                            await wc.DownloadFileTaskAsync(new Uri(app.DL), app.SavePath);
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
                tasks.Add(downloadTask);
            }
            await Task.WhenAll(tasks.ToArray());
            
            // Extract
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

            foreach (PortableApp app in selectedAppList)
            {
                if (app.ExtractMode == "single")
                {
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
                            return;
                        }

                        app.AppItem.Status = "Extracting...";
                        p.WaitForExit();
                        if (p.ExitCode == 0)
                        {
                            File.Delete(app.SavePath);
                            app.AppItem.Status = "Install complete";
                            app.AppItem.Progress = 100;
                            await Task.Delay(1000);

                            if (runOnce == true)
                            {
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

                                // Cleanup
                                Directory.Delete(Path.Combine(settings.PortableAppDir, app.Name), true);
                            }
                        }
                        if (p.ExitCode != 0)
                        {
                            app.AppItem.Status = String.Format(
                                "Extract failed. Exit code: {0}", p.ExitCode);
                            app.AppItem.Progress = 0;
                            failedPortableList.Add(app);
                        }
                    }
                }
            }
            refreshPortableButton.Enabled = true;
            downloadPortableButton.Enabled = true;

            if (failedPortableList != null && failedPortableList.Count > 0)
            {
                // Check for failed apps and ask for retry
                failedPortableInstallLabel2.Visible = true;
                downloadPortableButton.Text = "Yes";
                refreshPortableButton.Text = "No";
            }
            else
            {
                // All portable apps installed succesfully
                await Task.Delay(1000);
                CheckForPortableApps();
            }
        }
        #endregion

        #endregion

        #region ShowDetails()
        public void ShowDetails(string changelogText = null, string descriptionText = null)
        {
            if (descriptionText == null)
            {
                if (changelogText != null)
                {
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
                if (changelogText == null)
                {
                    if (descriptionText != null)
                    {
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
        }
        #endregion

        #region FixScrollbars()
        /// <summary>
        /// Reduces the width of controls so that a horziontal scrollbar 
        /// doesn't appear in a container.
        /// </summary>
        /// <param name="controls">The controls to reduce the width for.</param>
        private void FixScrollbars(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                int newWidth = control.Size.Width - SystemInformation.VerticalScrollBarWidth;
                control.Size = new Size(newWidth, control.Size.Height);
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
                        titleButtonRight.Visible = true;
                    }
                    else if (this.Controls[0] == installedPortableAppsPage)
                    {
                        startPage.BringToFront();
                        titleButtonLeft.Text = "Home";
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonRight.Visible = false;
                    }
                    else
                    {
                        startPage.BringToFront();
                        titleButtonLeft.Text = "Home";
                        titleButtonLeft.ArrowLeft = false;
                        titleButtonRight.ArrowRight = false;
                    }
                }
                if (aboutLabel.Visible == false)
                {
                    aboutLabel.Visible = true;
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

        private void UpdaterTile_Click(object sender, EventArgs e)
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
            titleButtonLeft.ArrowLeft = true;
            topBar.BorderStyle = BorderStyle.None;
        }

        private void GetNewAppsTile_Click(object sender, EventArgs e)
        {
            getNewAppsPage.BringToFront();
            titleButtonLeft.Text = "Get New Applications";
            titleButtonLeft.ArrowLeft = true;
            topBar.BorderStyle = BorderStyle.None;
            CheckForNonInstalledApps();
        }

        private void PortableAppsTile_Click(object sender, EventArgs e)
        {
            if (settings.PortableAppDir != null)
            {
                installedPortableAppsPage.BringToFront();
                titleButtonLeft.Text = "Portable Apps";
                titleButtonLeft.ArrowLeft = true;
                titleButtonRight.ArrowRight = true;
                titleButtonRight.Text = "Get Portable Apps";
                titleButtonRight.Visible = true;
                topBar.BorderStyle = BorderStyle.None;
                CheckForInstalledPortableApps();
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

        private void AboutLabel_MouseEnter(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = Color.White;
            aboutLabel.BackColor = normalGreen;
        }

        private void AboutLabel_MouseLeave(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = normalGreen;
            aboutLabel.BackColor = Color.White;
        }

        private void AboutLabel_Click(object sender, EventArgs e)
        {
            topBar.Size = new Size(topBar.Size.Width, 35);
            aboutPage.BringToFront();
            titleButtonLeft.Text = "About";
            titleButtonLeft.ArrowLeft = true;
            aboutLabel.Hide();
        }

        private void SiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.slimsoft.tk");
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
            refreshUpdatesButton.Enabled = true;
        }

        private void InstallUpdatesButton_Click(object sender, EventArgs e)
        {
            if (installUpdatesButton.Text == "Yes")
            {
                InstallUpdates(failedInstallList);
                failedUpdateLabel.Visible = false;
                installUpdatesButton.Text = "Install Selected";
                refreshUpdatesButton.Text = "Refresh";
            }
            else
            {
                InstallUpdates(updateList);
            }
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
            if (installAppsButton.Text == "Yes")
            {
                InstallNewApps(appList);
                failedAppInstallLabel.Visible = false;
                installAppsButton.Text = "Install Selected";
                refreshAppsButton.Text = "Refresh";
            }
            else
            {
                InstallNewApps(appList);
            }
        }

        private void RefreshAppsButton_Click(object sender, EventArgs e)
        {
            refreshAppsButton.Enabled = false;
            ReadDefenitions();
            CheckForNonInstalledApps();
            refreshAppsButton.Enabled = true;
        }
        #endregion

        #region installedPortableAppsPage Mouse Events
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
            if (downloadPortableButton.Text == "Yes")
            {
                InstallPortableApps(failedPortableList, true);
                failedPortableInstallLabel2.Visible = false;
                downloadPortableButton.Text = "Download Selected";
                refreshPortableButton.Text = "Refresh";
            }
            else
            {
                InstallPortableApps(portableAppList, true);
            }
        }

        private void RefreshPortableButton2_Click(object sender, EventArgs e)
        {
            CheckForPortableApps();
        }

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

                CheckForPortableApps();
                installedPortableAppsPage.BringToFront();
                titleButtonLeft.Text = "Portable Apps";
                titleButtonLeft.ArrowLeft = true;
                topBar.BorderStyle = BorderStyle.None;             
            }
        }
    #endregion

        #region Settings Page Events
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
            // Check if selected Portable App folder exists
            if (locationBox1.Text == null)
            {
                MessageBox.Show("You must specify a Portable Apps Folder to continue");
            }
            else
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

                    if (!Directory.Exists(locationBox1.Text))
                    {
                        Directory.CreateDirectory(locationBox1.Text);
                    }

                    settings.PortableAppDir = locationBox1.Text;
                    if (customDefenRadioBtn.Checked == true && customURLTextBox.Text != null)
                    {
                        settings.DefenitionURL = customURLTextBox.Text;
                    }
                    if (customDefenRadioBtn.Checked == true && customURLTextBox == null)
                    {
                        MessageBox.Show("You must specify a custom Defenition URL or use the official Defentions");
                    }
                    settings.Save();
                }
                catch (Exception)
                {
                    saveButton.Enabled = false;
                    paFolderNotWriteableLabel1.Visible = true;  
                }
            }
        }
        #endregion

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (this.Controls[0] == updatePage | this.Controls[0] == getNewAppsPage |
                    this.Controls[0] == installedPortableAppsPage | 
                    this.Controls[0] == getPortableAppsPage)
            {
                topBar.BorderStyle = BorderStyle.None;
            }
        }
    }

    #region App Class
    public class App
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string DL { get; set; }
        public string SavePath { get; set; }
        public string InstallSwitch { get; set; }

        public App(string name, string latestVersion, string localVersion,
            string arch, string type, string installSwitch, string dl,
            string changelog = null, string description = null, string savePath = null)
        {
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Changelog = changelog;
            Description = description;
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
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string Arch { get; set; }
        public string DL { get; set; }
        public string ExtractMode { get; set; }
        public string SavePath { get; set; }
        public string Launch { get; set; }

        public PortableApp(string name, string latestVersion, string localVersion,
            string arch, string launch, string dl, string extractMode,
            string changelog = null, string description = null, string savePath = null, AppItem appItem = null)
        {
            Name = name;
            AppItem = appItem;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Changelog = changelog;
            Description = description;
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
        public string DefenitionURL { get; set; }
        public string PortableAppDir { get; set; }

        public void Load()
        {
            // Load XML File
            if (File.Exists(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")))
            {
                XDocument settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));

                // Get content from XML nodes
                string defenitionURL = settingXML.Root.Element("DefenitionURL").Value;
                string portableAppDir = settingXML.Root.Element("PortableAppDir").Value; 
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
        }
        
        public void Save()
        {
            // Check if XML File exists
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")))
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
                    new XElement("PortableAppDir"), String.Empty));
                doc.Save(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));
            }

            // Load XML File
            XDocument settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));

            // Save values
            if (DefenitionURL != null)
            {
                settingXML.Descendants("DefenitionURL").Remove();
                XElement defenitionURL = new XElement("DefenitionURL");
                defenitionURL.Value = DefenitionURL;
                settingXML.Element("Settings").Add(defenitionURL);
            }
            if (PortableAppDir != null)
            {
                settingXML.Descendants("PortableAppDir").Remove();
                XElement portableAppDir = new XElement("PortableAppDir");
                portableAppDir.Value = PortableAppDir;
                settingXML.Element("Settings").Add(portableAppDir);
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