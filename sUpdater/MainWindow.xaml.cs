using AutoUpdaterDotNET;
using Hardcodet.Wpf.TaskbarNotification;
using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Application = sUpdater.Models.Application;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TaskbarIcon TaskbarIcon { get; private set; }
        private UpdateInfoEventArgs updateInfo;

        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "/tray")
            {
                Utilities.MinimizeToTray(this);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string version = Utilities.GetFriendlyVersion(Assembly.GetEntryAssembly().GetName().Version);
            Log.Append($"sUpdater v{version} started on {Utilities.GetFriendlyOSName()}", Log.LogLevel.INFO);

            TaskbarIcon = (TaskbarIcon)FindResource("TrayIcon");
            TaskbarIcon.ContextMenu = (ContextMenu)FindResource("trayMenu");
            TaskbarIcon.TrayLeftMouseDown += TaskbarIcon_TrayLeftMouseDown;

            Utilities.LoadSettings();
            if (Utilities.Settings.AppServerURL != null)
            {
                Log.Append($"Using custom App Server: {Utilities.Settings.AppServerURL}",
                    Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("Using official App Server", Log.LogLevel.INFO);
            }
 
            Utilities.InitHttpClient();

            await AppController.CheckForUpdates();
            UpdateTaskbarIcon();

            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.Start("https://www.slimsoft.tk/supdater/update.xml");
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {           
                    if (ShowInTaskbar)
                    {
                        AppUpdatePage appUpdatePage = new AppUpdatePage(args);
                        frame.Navigate(appUpdatePage);
                    }

                    updateInfo = args;
                    TaskbarIcon.Icon = Properties.Resources.sUpdater_Orange;
                    TaskbarIcon.ToolTipText = $"sUpdater\nApplication update available";
                }
            }
        }

        public void UpdateTaskbarIcon()
        {
            if (Utilities.ConnectedToServer)
            {
                int updateCount = AppController.GetUpdateCount();
                if (updateCount > 0)
                {
                    string notifiedUpdates = "";
                    TaskbarIcon.Icon = Properties.Resources.sUpdater_Orange;

                    foreach (int appId in AppController.UpdateIds)
                    {
                        if (appId != AppController.UpdateIds.Last())
                        {
                            notifiedUpdates += $"{appId},";
                        }
                        else
                        {
                            // Do not add a comma after the last app name
                            notifiedUpdates += appId;
                        }
                    }

                    if (notifiedUpdates != Utilities.Settings.NotifiedUpdates && ShowInTaskbar == false)
                    {
                        if (updateCount > 1)
                        {
                            TaskbarIcon.ShowBalloonTip($"{updateCount} updates available",
                                "Click for details", BalloonIcon.Info);
                        }
                        else
                        {
                            TaskbarIcon.ShowBalloonTip($"1 update available",
                                "Click for details", BalloonIcon.Info);
                        }

                        TaskbarIcon.TrayBalloonTipClicked += (s, e) =>
                        {
                            frame.Navigate(new UpdaterPage());
                            Utilities.ShowFromTray(this);
                        };
                    }

                    if (updateCount > 1)
                    {
                        TaskbarIcon.ToolTipText = $"sUpdater\n{updateCount} updates available";
                    }
                    else
                    {
                        TaskbarIcon.ToolTipText = "sUpdater\n1 update available";
                    }

                    Utilities.Settings.NotifiedUpdates = notifiedUpdates;
                    Utilities.SaveSettings();
                }
                else
                {
                    TaskbarIcon.Icon = Properties.Resources.sUpdater;
                    TaskbarIcon.ToolTipText = $"sUpdater\nNo updates available";
                }
            }
            else
            {
                TaskbarIcon.Icon = Properties.Resources.sUpdater_Grey;
                TaskbarIcon.ToolTipText = $"sUpdater\nCannot connect to the server.\nPlease check your internet connection.";
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindResource("menu") as ContextMenu;
            menu.PlacementTarget = sender as Button;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            menu.IsOpen = true;
        }

        private void LogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogPage logPage = new LogPage();
            frame.Navigate(logPage);
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutPage aboutPage = new AboutPage();
            frame.Navigate(aboutPage);
        }

        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Utilities.ShowFromTray(this);
            if (updateInfo != null)
            {
                AppUpdatePage appUpdatePage = new AppUpdatePage(updateInfo);
                frame.Navigate(appUpdatePage);
            }
        }

        private async void TrayCheckUpdatesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await AppController.CheckForUpdates();
            UpdateTaskbarIcon();
            
            if (frame.Content is StartPage)
            {
                StartPage startPage = (StartPage)frame.Content;
                startPage.UpdateGUI();
            }
        }

        private void TrayOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TaskbarIcon_TrayLeftMouseDown(sender, e);
        }

        private void TraySettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new SettingsPage());
            Utilities.ShowFromTray(this);
        }

        private void TrayCloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Utilities.Settings.MinimizeToTray)
            {
                e.Cancel = true;
                Utilities.MinimizeToTray(this);
            }
        }
    }
}
