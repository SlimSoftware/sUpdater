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
            foreach (App app in updateList.ToArray())
            {
                // Remove up to date apps from the updateList
                if (float.Parse(app.LatestVersion.ToString()) <= float.Parse(app.LocalVersion.ToString()))
                {
                    updateList.Remove(app);
                }
            }

            // Add the contents of the updateList to the listview
            updatesListView.SetObjects(updateList);
            updatesListView.BuildList(shouldPreserveState: false);

            // Change updaterTile on the startpage accordingly
            if (updateList.Count != 0)
            {
                updaterTile.BackColor = normalOrange;
                updaterTile.Text = String.Format("{0} updates available", updateList.Count);
            }
        }

        #region StartPage/TopBar Mouse Events
        private void updaterTile_Click(object sender, System.EventArgs e)
        {
            updatePage.BringToFront();
            topBar.BringToFront();
            titleButton.Text = "Updates";
            titleButton.Arrow = true;
        }

        private void titleButton_Click(object sender, System.EventArgs e)
        {
            if (titleButton.Arrow == true)
            {
                startPage.BringToFront();
                topBar.BringToFront();
                titleButton.Text = "Home";
                titleButton.Arrow = false;
            }
        }
        #endregion
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
