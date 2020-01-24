using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PortableAppsPage : Page
    {
        private List<PortableApp> portableApps = new List<PortableApp>();

        public PortableAppsPage()
        {
            InitializeComponent();
            portableApps = GetPortableApps();
            portableAppsListView.ItemsSource = portableApps;
        }

        /// <summary>
        /// Fills the portableApps list with all Portable Apps from the definitions
        /// </summary>
        public static List<PortableApp> GetPortableApps()
        {
            List<PortableApp> apps = new List<PortableApp>();

            // Load XML File
            XElement definitions = XElement.Load("https://www.slimsoft.tk/slimupdater/defenitions.xml");

            foreach (XElement portableAppElement in definitions.Descendants("portable"))
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

                apps.Add(new PortableApp(nameAttribute.Value, versionElement.Value,
                    localVersion, archElement.Value, launchElement.Value, dlElement.Value,
                    extractModeElement.Value));             
            }

            return apps;
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in portableAppsListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!portableAppsListView.SelectedItems.Contains(app))
                    {
                        portableAppsListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                portableAppsListView.SelectedItems.Clear();    
            }
        }

        private void GetAppsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (portableAppsListView.SelectedItems.Count == portableApps.Count)
            {
                if (selectAllCheckBox.IsChecked == false)
                {
                    selectAllCheckBox.IsChecked = true;
                }
            }
            else
            {
                if (selectAllCheckBox.IsChecked == true)
                {
                    selectAllCheckBox.IsChecked = false;
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GetPortableApps();
            portableAppsListView.Items.Refresh();
        }

        private async void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
