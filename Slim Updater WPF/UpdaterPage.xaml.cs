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
        private List<Application> UpdateList;

        public UpdaterPage(List<Application> updateList)
        {
            InitializeComponent();
            UpdateList = updateList;
            updateListView.ItemsSource = updateList;
            updateListView.SelectAll();
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
            if (updateListView.SelectedItems.Count == UpdateList.Count)
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
