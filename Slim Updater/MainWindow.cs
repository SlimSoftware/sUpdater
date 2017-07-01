using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Net;
using System.IO;

namespace Slim_Updater
{
    public partial class MainWindow : Form
    {
        public List<App> appList = new List<App>();
        public List<App> updateList;
        int downloadCount;
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

                // Remove first newline from app description and changelog if present
                if (changelogElement != null)
                {
                    if (changelogElement.Value.StartsWith("\n"))
                    {
                        changelogElement.Value = changelogElement.Value.TrimStart("\n".ToCharArray());
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
                if (changelogElement == null | descriptionElement == null)
                {
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

                    else
                    {
                        appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value, 
                            localVersion, archElement.Value, typeElement.Value, switchElement.Value, 
                            dlElement.Value, null, null));
                    }
                }
                else
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
                        updateContentPanel.Controls.Add(appItem);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height; // spacer underneath first
                    }
                    else
                    {
                        appItem.Location = new Point(0, (previousY + previousHeight));
                        updateContentPanel.Controls.Add(appItem);
                        previousY = appItem.Location.Y;
                        previousHeight = appItem.Height; // spacer below second, third...
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

        public void InstallUpdates()
        {
            int maxDownloads = 3; // TODO: Add user setting here
            foreach (AppItem update in updateContentPanel.Controls)
            {
                if (update.Checked == false)
                {
                    continue;
                }

                // Download
                for (downloadCount = 1; downloadCount <= maxDownloads; downloadCount++)
                {
                    foreach (App app in updateList)
                    {
                        if (app.Name.Equals(update.Name) == false)
                        {
                            continue;
                        }

                        string fileName = Path.GetFileName(app.DL);
                        string savePath = Path.Combine(
                            @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SlimSoftware\Slim Updater\" + fileName);
                        using (var wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += (s, e) =>
                            {
                                update.Progress = e.ProgressPercentage;
                                float mbDownloaded = (e.BytesReceived / 1024f) / 1024f;
                                float mbTotal = (e.TotalBytesToReceive / 1024f) / 1024f;
                                update.Status = string.Format("Downloading... ({0} MB of {1} MB",
                                    mbDownloaded, mbTotal);
                            };
                            wc.DownloadFileCompleted += (s, e) =>
                            {
                                downloadCount--;
                            };
                            wc.DownloadFileAsync(new Uri(app.DL), savePath);
                        }
                        break;
                    }
                }
            }

                // Install
                {
                    
                }
            }

        #region StartPage/TopBar Mouse Events
        private void updaterTile_Click(object sender, System.EventArgs e)
        {
            updatePage.BringToFront();
            titleButton.Text = "Updates";
            titleButton.Arrow = true;
            topBar.Size = new Size(topBar.Size.Width, 55);
        }

        private void titleButton_Click(object sender, System.EventArgs e)
        {
            topBar.Size = new Size(topBar.Size.Width, 35);
            if (titleButton.Text == "Details")
            {
                detailsPage.SendToBack();
                if (this.Controls[0].Name == "updatePage")
                {
                    titleButton.Text = "Updates";
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
            System.Diagnostics.Process.Start("http://www.slimsoft.tk");
        }
        #endregion

        private void selectAllUpdatesCheckBox_Click(object sender, EventArgs e)
        {
            if (selectAllUpdatesCheckBox.Checked == true)
            {
                foreach (AppItem update in updateContentPanel.Controls)
                {
                    update.Checked = true;
                    selectAllUpdatesCheckBox.Text = "Unselect All";
                }
            }

            else
            {
                foreach (AppItem update in updateContentPanel.Controls)
                {
                    update.Checked = false;
                    selectAllUpdatesCheckBox.Text = "Select All";
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

        public bool IsFirstAppItem
        {
            get
            {
                if (updateContentPanel.Controls.Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
