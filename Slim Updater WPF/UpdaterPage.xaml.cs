using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        public UpdaterPage()
        {
            InitializeComponent();
            updateListView.ItemsSource = Apps.Updates;
            updateListView.SelectAll();

            //// Add all apps to updatecontentPanel for details view 
            //if (detailsView)
            //{
            //    foreach (App a in AppList)
            //    {
            //        Application app = a;
            //        app.Checkbox = false;

            //        if (app.LocalVersion != null)
            //        {
            //            app.Name = app.Name + " " + app.LatestVersion;
            //            if (app.Type == "noupdate")
            //            {
            //                app.Version = "Installed: " + app.LocalVersion + " (Using own updater)";
            //            }
            //            else
            //            {
            //                app.Version = "Installed: " + app.LocalVersion;
            //            }
            //        }
            //        else
            //        {
            //            app.Name = app.Name + " " + app.LatestVersion;
            //            app.Version = "Not Found";
            //        }
            //        UpdateList.Add(app);
            //    }

            // Hide select all checkbox and bottom buttons for details view
            //selectAllCheckBox.Visible = false;
            //installUpdatesButton.Visible = false;
            //refreshUpdatesButton.Visible = false;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (Application app in updateListView.SelectedItems)
            {

            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in updateListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!updateListView.SelectedItems.Contains(app))
                    {
                        updateListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                updateListView.SelectedItems.Clear();    
            }
        }

        private void ListViewItem_Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Get app associated with the listview item
            Application app = ((ListViewItem)sender).Content as Application;

            if (updateListView.SelectedItems.Contains(app))
            {
                updateListView.SelectedItems.Remove(app);
            }
            else
            {
                updateListView.SelectedItems.Add(app);
            }
            
        }

        private void UpdateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (updateListView.SelectedItems.Count == Apps.Updates.Count)
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
