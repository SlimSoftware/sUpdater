using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace Slim_Updater
{
    public partial class MainWindow : Form
    {
        public List<App> applist = new List<App>();

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
                
                // Add app to applist
                applist.Add(new App(nameAttribute.Value.ToString(), versionElement.Value, localVersion, 
                    archElement.Value, typeElement.Value, switchElement.Value, dlElement.Value));
            }
        }

        public void CheckforUpdates()
        {
            foreach (App app in applist.ToArray())
            {
                // Remove updated apps from the applist
                if (float.Parse(app.LatestVersion.ToString()) <= float.Parse(app.LocalVersion.ToString()))
                {
                    applist.Remove(app);
                }

                // Add the contents of the applist to the listview
                updatesListView.SetObjects(applist);
                updatesListView.BuildList(shouldPreserveState: false);
            }
        }

        #region StartPage/TopBar Mouse Events

        #region Mouse Enter/Leave Events
        // Create hover and normal color
        Color hoverGreen = Color.FromArgb(0, 206, 0);
        Color normalGreen = Color.FromArgb(0, 186, 0);

        //private void updaterButton_MouseEnter(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = hoverGreen;
        //}

        //private void updaterButton_MouseLeave(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = normalGreen;
        //}

        //private void updaterIcon_MouseEnter(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = hoverGreen;
        //}

        //private void updaterIcon_MouseLeave(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = normalGreen;
        //}

        //private void updaterLabel_MouseEnter(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = hoverGreen;
        //}

        //private void updaterLabel_MouseLeave(object sender, System.EventArgs e)
        //{
        //    updaterButton.BackColor = normalGreen;
        //}

        private void getNewAppsButton_MouseEnter(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = hoverGreen;
        }

        private void getNewAppsButton_MouseLeave(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = normalGreen;
        }

        private void getNewAppsIcon_MouseEnter(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = hoverGreen;
        }

        private void getNewAppsIcon_MouseLeave(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = normalGreen;
        }

        private void getNewAppsLabel_MouseEnter(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = hoverGreen;
        }

        private void getNewAppsLabel_MouseLeave(object sender, System.EventArgs e)
        {
            getNewAppsButton.BackColor = normalGreen;
        }

        private void portableAppsButton_MouseEnter(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = hoverGreen;
        }

        private void portableAppsButton_MouseLeave(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = normalGreen;
        }

        private void portableAppsIcon_MouseEnter(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = hoverGreen;
        }

        private void portableAppsIcon_MouseLeave(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = normalGreen;
        }

        private void portableAppsLabel_MouseEnter(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = hoverGreen;
        }

        private void portableAppsLabel_MouseLeave(object sender, System.EventArgs e)
        {
            portableAppsButton.BackColor = normalGreen;
        }

        private void settingsButton_MouseEnter(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = hoverGreen;
        }

        private void settingsButton_MouseLeave(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = normalGreen;
        }

        private void settingsIcon_MouseEnter(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = hoverGreen;
        }

        private void settingsIcon_MouseLeave(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = normalGreen;
        }

        private void settingsLabel_MouseEnter(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = hoverGreen;
        }

        private void settingsLabel_MouseLeave(object sender, System.EventArgs e)
        {
            settingsButton.BackColor = normalGreen;
        }
        #endregion

        #region Click Events
        private void updaterButton_Click(object sender, System.EventArgs e)
        {
            updatePage.BringToFront();
            topBar.BringToFront();
            titleButton.Text = "Updates";
            titleButton.Arrow = true;
        }

        private void updaterIcon_Click(object sender, System.EventArgs e)
        {
            updatePage.BringToFront();
            topBar.BringToFront();
            titleButton.Text = "Updates";
            titleButton.Arrow = true;
        }

        private void updaterLabel_Click(object sender, System.EventArgs e)
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
