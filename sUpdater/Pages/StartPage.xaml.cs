﻿using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using Colors = sUpdater.Models.Colors;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private bool noConnectionEventHandlersAttached = false;
        public StartPage()
        {
            InitializeComponent();
            UpdateGUI();

            AppController.CheckForUpdatesCompleted += AppController_CheckForUpdatesCompleted;
        }

        private void AppController_CheckForUpdatesCompleted(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        /// <summary>
        /// Updates the GUI according to whether there are updates or there is a connection to the server
        /// </summary>
        public void UpdateGUI()
        {
            if (Utilities.ConnectedToServer)
            {
                if (noConnectionEventHandlersAttached)
                {
                    // Update state when connection is available again, updaterTile will be done later below
                    getAppsTile.Background = Colors.normalGreenBrush;
                    portableAppsTile.Background = Colors.normalGreenBrush;
                    offlineNoticePanel.Visibility = Visibility.Hidden;

                    updaterTile.MouseLeftButtonDown += UpdaterTile_MouseLeftButtonDown;
                    updaterTile.MouseLeftButtonDown -= TileClickedWithNoConnection;
                    getAppsTile.MouseLeftButtonDown += GetAppsTile_MouseLeftButtonDown;
                    getAppsTile.MouseLeftButtonDown -= TileClickedWithNoConnection;
                    portableAppsTile.MouseLeftButtonDown += PortableAppsTile_MouseLeftButtonDown;
                    portableAppsTile.MouseLeftButtonDown -= TileClickedWithNoConnection;

                    noConnectionEventHandlersAttached = false;
                }

                if (!AppController.CheckingForUpdates)
                {
                    int updateCount = AppController.Updates.Count;
                    if (updateCount > 0)
                    {
                        updaterTile.Background = Colors.normalOrangeBrush;
                        updaterTile.Title = updateCount > 1 ? $"{updateCount} updates available" : "1 update available";
                    }
                    else
                    {
                        updaterTile.Background = Colors.normalGreenBrush;
                        updaterTile.Title = "No updates available";
                        Log.Append("No updates available", Log.LogLevel.INFO);
                    }

                    updaterTile.MouseLeftButtonDown += UpdaterTile_MouseLeftButtonDown;
                }
                else
                {
                    updaterTile.Background = Colors.normalGreyBrush;
                    updaterTile.MouseLeftButtonDown -= UpdaterTile_MouseLeftButtonDown;
                    updaterTile.Title = "Checking for updates...";
                }
            }
            else
            {
                offlineNoticePanel.Visibility = Visibility.Visible;
                updaterTile.Background = Colors.normalGreyBrush;
                getAppsTile.Background = Colors.normalGreyBrush;
                portableAppsTile.Background = Colors.normalGreyBrush;
                updaterTile.Title = "Could not check for updates";

                if (!noConnectionEventHandlersAttached)
                {
                    updaterTile.MouseLeftButtonDown -= UpdaterTile_MouseLeftButtonDown;
                    updaterTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                    getAppsTile.MouseLeftButtonDown -= GetAppsTile_MouseLeftButtonDown;
                    getAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;
                    portableAppsTile.MouseLeftButtonDown -= PortableAppsTile_MouseLeftButtonDown;
                    portableAppsTile.MouseLeftButtonDown += TileClickedWithNoConnection;

                    noConnectionEventHandlersAttached = true;
                }
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
        }
    }
}
