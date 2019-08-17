using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            // Add all apps to Apps.Details for details view when there no updates are available
            // and the details list has not already been filled
            if (Apps.Updates.Count == 0 && (Apps.Details == null || Apps.Details?.Count == 0))
            {
                Apps.Details = new ObservableCollection<Application>();

                foreach (Application a in Apps.Regular)
                {
                    // Create a copy of the app so that the app in the list does not get modified
                    Application app = a.Clone();
                    app.Checkbox = false;

                    if (app.LocalVersion != null)
                    {
                        app.Name = app.Name + " " + app.LatestVersion;
                        if (app.Type == "noupdate")
                        {
                            app.DisplayedVersion = "Installed: " + app.LocalVersion + " (Using own updater)";
                        }
                        else
                        {
                            app.DisplayedVersion = "Installed: " + app.LocalVersion;
                        }
                    }
                    else
                    {
                        app.Name = app.Name + " " + app.LatestVersion;
                        app.DisplayedVersion = "Not Found";
                    }

                    Apps.Details.Add(app);
                }

                updateListView.ItemsSource = Apps.Details;
                updateListView.SelectionChanged += (sender, e) =>
                {
                    updateListView.SelectedItems.Clear();
                };
                Title = "Details";

                // Hide select all checkbox and bottom buttons for details view
                selectAllCheckBox.Visibility = Visibility.Hidden;
                installButton.Visibility = Visibility.Hidden;
                refreshButton.Visibility = Visibility.Hidden;

                // Set height of the other rows to 0 so that the row of the listview 
                // takes up all the available space
                selectAllRow.Height = new GridLength(0);
                buttonsRow.Height = new GridLength(0);
                listViewRow.Height = new GridLength(1, GridUnitType.Star);
            }
            else if (Apps.Details?.Count != 0)
            {
                updateListView.ItemsSource = Apps.Details;
                Title = "Details";

                // Hide select all checkbox and bottom buttons for details view
                selectAllCheckBox.Visibility = Visibility.Hidden;
                installButton.Visibility = Visibility.Hidden;
                refreshButton.Visibility = Visibility.Hidden;

                // Set height of the other rows to 0 so that the row of the listview 
                // takes up all the available space
                selectAllRow.Height = new GridLength(0);
                buttonsRow.Height = new GridLength(0);
                listViewRow.Height = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                // Clear detail list if updates are available and the details mode
                // has been shown before to free memory
                Apps.Details.Clear();        
            }
        }

        /// <summary>
        /// Sets up some visual tweaks for the details mode
        /// </summary>
        private void SetDetailsMode()
        {
            updateListView.ItemsSource = Apps.Details;
            Title = "Details";

            // Hide select all checkbox and bottom buttons for details view
            selectAllCheckBox.Visibility = Visibility.Hidden;
            installButton.Visibility = Visibility.Hidden;
            refreshButton.Visibility = Visibility.Hidden;

            // Set height of the other rows to 0 so that the row of the listview 
            // takes up all the available space
            selectAllRow.Height = new GridLength(0);
            buttonsRow.Height = new GridLength(0);
            listViewRow.Height = new GridLength(1, GridUnitType.Star);
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

            if (app.Checkbox == true)
            {
                if (updateListView.SelectedItems.Contains(app))
                {
                    updateListView.SelectedItems.Remove(app);
                }
                else
                {
                    updateListView.SelectedItems.Add(app);
                }
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
