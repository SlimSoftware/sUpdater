using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Slim_Updater
{
    public partial class MainWindow : Form
    {
        public List<App> appList = new List<App>();
        public List<App> updateList;
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);

        public MainWindow()
        {
            InitializeComponent();
            ReadDefenitions();
            CheckforUpdates();
        }

        public void ReadDefenitions()
        {
            // Load XML File
            XElement defenitions = XElement.Load("http://www.slimsoft.tk/SlimUpdater/Defenitions.xml");
            //(Path.Combine(Environment.GetFolderPath(
            //Environment.SpecialFolder.ApplicationData), @"Local\SlimSoftware\Slim Updater\Defenitions.txt"))))

            foreach (XElement appElement in defenitions.Elements("app"))
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
                string localVersion = Registry.GetValue(regkeyElement.Value, regvalueElement.Value, false).ToString();
                if (localVersion == null)
                {
                    localVersion = "-";
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
                if (descriptionElement == null && changelogElement != null)
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

        public void CheckforUpdates()
        {
            updateList = new List<App>(appList);
            int previousY = 0;
            int previousHeight = 0;
            foreach (App app in updateList.ToArray())
            {
                AppItem appItem = new AppItem();
                Separator separator = new Separator();
                appItem.Click += (sender, e) =>
                {
                    ShowDetails(app.Changelog);
                };

                // Remove up to date apps from the updateList
                if (float.Parse(app.LatestVersion.ToString()) <= float.Parse(app.LocalVersion.ToString()))
                {
                    updateList.Remove(app);
                }
                else
                {
                    // Add app to updateList
                    appItem.Name = app.Name;
                    appItem.Version = app.LatestVersion;
                    if (updateContentPanel.Controls.Count == 0)
                    {
                        updateContentPanel.Controls.Add(separator);
                        separator = new Separator();
                        separator.Location = new Point(0, 45);
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
            }

            // Change updaterTile on the startpage accordingly
            if (updateList.Count != 0)
            {
                updaterTile.BackColor = normalOrange;
                updaterTile.Text = String.Format("{0} updates available", updateList.Count);
            }
        }

        public async void InstallUpdates()
        {
            refreshButton.Enabled = false;
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
            foreach (App app in selectedUpdateList)
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
                            await wc.DownloadFileTaskAsync(new Uri(app.DL), app.SavePath);
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
            foreach (App app in selectedUpdateList)
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
                        await Task.Delay(1500);
                        CheckforUpdates();
                    }
                    if (p.ExitCode != 0)
                    {
                        (updateContentPanel.Controls[index] as AppItem).Status = String.Format(
                            "Install failed. Exit code: {0}", p.ExitCode);
                        (updateContentPanel.Controls[index] as AppItem).Progress = 0;
                        refreshButton.Enabled = true;
                        // TODO: Add retry system here (add failed updates to list and install only those if clicked
                        // on installUpdatesButton renamed to Retry Failed
                    }
                    enableButtons:
                    if (refreshButton.Enabled == false | installUpdatesButton.Enabled == false)
                    {
                        refreshButton.Enabled = true;
                        installUpdatesButton.Enabled = true;
                    }
                }
            }
        }
 
        #region StartPage/TopBar Mouse Events
        private void updaterTile_Click(object sender, System.EventArgs e)
        {
            updatePage.BringToFront();
            titleButton.Text = "Updates";
            titleButton.Arrow = true;
            topBar.BorderStyle = BorderStyle.None;
        }

        private void titleButton_Click(object sender, System.EventArgs e)
        {
            if (topBar.Size.Height != 35)
            {
                topBar.Size = new Size(topBar.Size.Width, 35);
            }
            
            if (topBar.BorderStyle == BorderStyle.None)
            {
                topBar.BorderStyle = BorderStyle.FixedSingle;
            }

            if (titleButton.Text == "Details")
            {
                detailsPage.SendToBack();
                if (this.Controls[0].Name == "updatePage")
                {
                    titleButton.Text = "Updates";
                    topBar.BorderStyle = BorderStyle.None;
                }
            }
            else
            {
                if (titleButton.Arrow == true)
                {
                    startPage.BringToFront();
                    titleButton.Text = "Home";
                    titleButton.Arrow = false;
                }
                if (aboutLabel.Visible == false)
                {
                    aboutLabel.Visible = true;
                }
            }
        }

        private void aboutLabel_MouseEnter(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = Color.White;
            aboutLabel.BackColor = normalGreen;
        }

        private void aboutLabel_MouseLeave(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = normalGreen;
            aboutLabel.BackColor = Color.White;
        }

        private void aboutLabel_Click(object sender, EventArgs e)
        {
            topBar.Size = new Size(topBar.Size.Width, 35);
            aboutPage.BringToFront();
            titleButton.Text = "About";
            titleButton.Arrow = true;
            aboutLabel.Hide();
        }

        private void siteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.slimsoft.tk");
        }
        #endregion

        private void selectAllUpdatesCheckBox_Click(object sender, EventArgs e)
        {
            if (selectAllUpdatesCheckBox.Checked == true)
            {
                foreach (Control c in updateContentPanel.Controls)
                {
                    if (c is AppItem)
                    {
                        (c as AppItem).Checked = true;
                        selectAllUpdatesCheckBox.Text = "Unselect All";
                    }
                }
            }

            else
            {
                foreach (Control c in updateContentPanel.Controls)
                {
                    if (c is AppItem)
                    {
                        (c as AppItem).Checked = false;
                        selectAllUpdatesCheckBox.Text = "Select All";
                    }
                }
            }
        }

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
                }
                detailsPage.BringToFront();
                titleButton.Text = "Details";
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
                    }
                    detailsPage.BringToFront();
                    titleButton.Text = "Details";
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReadDefenitions();
            CheckforUpdates();
        }

        private void installUpdatesButton_Click(object sender, EventArgs e)
        {
            InstallUpdates();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (this.Controls[0] == updatePage)
            {
                topBar.BorderStyle = BorderStyle.None;
            }
        }
    }

    public class App
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Changelog { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string InstallSwitch { get; set; }
        public string DL { get; set; }
        public string SavePath { get; set; }
        public string Description { get; set; }

        public App(string name, string latestVersion, string localVersion,
            string arch, string type, string installSwitch, string dl,
            string changelog = null, string description = null, string savePath = null)
        {
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Changelog = changelog;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DL = dl;
            SavePath = savePath;
            Description = description;
        }
    }
}
