﻿using Microsoft.Win32;
using sUpdater.Controllers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            UpdateGUI();            
        }

        /// <summary>
        /// Updates to GUI according to whether there are updates or there is a connection to the server
        /// </summary>
        private void UpdateGUI()
        {
            if (Utilities.ConnectedToServer)
            {
                if (updaterTile.Background == Colors.normalGreyBrush)
                {
                    // Update state when connection is available again
                    getAppsTile.Background = Colors.normalGreenBrush;
                    portableAppsTile.Background = Colors.normalGreenBrush;
                    offlineNoticePanel.Visibility = Visibility.Hidden;
                }

                int updateCount = AppController.GetUpdateCount();
                if (AppController.GetUpdateCount() > 0)
                {
                    updaterTile.Background = Colors.normalOrangeBrush;
                    updaterTile.Title = $"{updateCount} updates available";
                }
                else
                {
                    updaterTile.Background = Colors.normalGreenBrush;
                    updaterTile.Title = "No updates available";
                    Log.Append("No updates available", Log.LogLevel.INFO);
                }
            }
            else
            {
                updaterTile.Background = Colors.normalGreyBrush;
                updaterTile.MouseLeftButtonDown -= UpdaterTile_MouseLeftButtonDown;
                updaterTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                getAppsTile.Background = Colors.normalGreyBrush;
                getAppsTile.MouseLeftButtonDown -= GetAppsTile_MouseLeftButtonDown;
                getAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                portableAppsTile.Background = Colors.normalGreyBrush;
                portableAppsTile.MouseLeftButtonDown -= PortableAppsTile_MouseLeftButtonDown;
                getAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                offlineNoticePanel.Visibility = Visibility.Visible;
            }
        }

        private void TileClickedWithNoConnection(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("This functionality is not available when there is no connection to the server.", 
                "sUpdater", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void UpdaterTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UpdaterPage updaterPage = new UpdaterPage();
            NavigationService.Navigate(updaterPage);
        }


        private void GetAppsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GetAppsPage getAppsPage = new GetAppsPage();
            NavigationService.Navigate(getAppsPage);
        }

        private void SettingsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage();
            NavigationService.Navigate(settingsPage);
        }

        private void PortableAppsTile_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InstalledPortableAppsPage portableAppPage = new InstalledPortableAppsPage();
            NavigationService.Navigate(portableAppPage);
        }

        private async void OfflineRetryLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            await AppController.CheckForUpdates();
            UpdateGUI();
        }
    }
}