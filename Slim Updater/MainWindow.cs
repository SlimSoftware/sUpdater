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
