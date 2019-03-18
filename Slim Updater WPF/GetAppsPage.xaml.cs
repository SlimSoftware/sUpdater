using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GetAppsPage : Page
    {
        private List<Application> notInstalledApps = new List<Application>();

        public GetAppsPage()
        {
            InitializeComponent();
            GetNotInstalledApps();
            getAppsListView.ItemsSource = notInstalledApps;
        }

        /// <summary>
        /// Fills the notInstalledApps list with apps that are not installed
        /// </summary>
        /// <returns></returns>
        public void GetNotInstalledApps()
        {
            foreach (Application app in Apps.Regular)
            {
                // If the LocalVersion is null, then the app is not installed
                if (app.LocalVersion == null)
                {
                    // Ensure the app will have a checkbox
                    if (app.Checkbox == false)
                    {
                        app.Checkbox = true;
                    }
                    
                    notInstalledApps.Add(app);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Application app in getAppsListView.SelectedItems)
            {

            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in getAppsListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!getAppsListView.SelectedItems.Contains(app))
                    {
                        getAppsListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                getAppsListView.SelectedItems.Clear();    
            }
        }

        private void ListViewItem_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get app associated with the listview item
            Application app = ((ListViewItem)sender).Content as Application;

            if (app.Checkbox == true)
            {
                if (getAppsListView.SelectedItems.Contains(app))
                {
                    getAppsListView.SelectedItems.Remove(app);
                }
                else
                {
                    getAppsListView.SelectedItems.Add(app);
                }
            }
        }

        private void GetAppsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (getAppsListView.SelectedItems.Count == notInstalledApps.Count)
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
    }
}
