using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace Slim_Updater
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadDefenitions();
            CheckforUpdates();
        }

        public List<App> appList = new List<App>();
        Color normalGreen = Color.FromArgb(0, 186, 0);
        Color normalOrange = Color.FromArgb(254, 124, 35);

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
                XElement archElement = appElement.Element("arch");
                XElement typeElement = appElement.Element("type");
                XElement switchElement = appElement.Element("switch");
                XElement dlElement = appElement.Element("dl");
                XElement regkeyElement = appElement.Element("regkey");
                XElement regvalueElement = appElement.Element("regvalue");

                // Get local version if installed
                string localVersion = Registry.GetValue(regkeyElement.Value, regvalueElement.Value, false).ToString();
                if (localVersion == null)
                {
                    localVersion = "-";
                }
                
                // Add app to appList
                appList.Add(new App(nameAttribute.Value.ToString(), versionElement.Value, localVersion, 
                    archElement.Value, typeElement.Value, switchElement.Value, dlElement.Value));
            }
        }

        public void CheckforUpdates()
        {
            List<App> updateList = new List<App>(appList);
            AppItem appItem = new AppItem();
            appItem.Click += (sender, e) => 
            {
                if (appItem.Height == 45)
                {
                    appItem.Expand();
                }
                else
                {
                    if (appItem.Height == 200)
                    {
                        appItem.Shrink();
                    }
                }
            };
            foreach (App app in updateList.ToArray())
            {
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
                    updateContentPanel.Controls.Add(appItem);
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
    }


    public class App
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LocalVersion { get; set; }
        public string Arch { get; set; }
        public string Type { get; set; }
        public string InstallSwitch { get; set; }
        public string DL { get; set; }

        public App(string name, string latestVersion, string localVersion, string arch, string type,
            string installSwitch, string dl)
        {
            Name = name;
            LatestVersion = latestVersion;
            LocalVersion = localVersion;
            Arch = arch;
            Type = type;
            InstallSwitch = installSwitch;
            DL = dl;
        }
    }
}
