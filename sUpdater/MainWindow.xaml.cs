using AutoUpdaterDotNET;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
using sUpdater.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Application = sUpdater.Models.Application;
using Colors = sUpdater.Models.Colors;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        public bool ConnectedToServer { get; set; }
        private UpdateInfoEventArgs updateInfo;

        public MainWindow()
        {
            InitializeComponent();

            Wpf.Ui.Appearance.Accent.Apply(Colors.normalGreenBrush.Color);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "/tray")
            {
                Utilities.MinimizeToTray(this);
            }

            string version = Utilities.GetFriendlyVersion(Assembly.GetEntryAssembly().GetName().Version);
            Log.Append($"sUpdater v{version} started on {Utilities.GetFriendlyOSName()}", Log.LogLevel.INFO);

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

            ConnectedToServer = StartPage.ReadDefenitions();
            if (ConnectedToServer)
            {
                StartPage.CheckForUpdates();
            }
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
                    //trayIcon.Icon = Properties.Resources.sUpdater_Orange;
                    trayIcon.TooltipText = $"sUpdater\nApplication update available";

                    new ToastContentBuilder()
                        .AddText("Test bla bla")
                        .Show();
                }
            }
        }

        public void UpdateTaskbarIcon()
        {
            if (ConnectedToServer)
            {
                if (Apps.Updates.Count > 0)
                {
                    string notifiedUpdates = "";
                    //trayIcon.Icon = Properties.Resources.sUpdater_Orange;

                    foreach (Application update in Apps.Updates)
                    {
                        if (update != Apps.Updates.Last())
                        {
                            notifiedUpdates += update.Name + " ";
                        }
                        else
                        {
                            // Do not add a space after the last app name
                            notifiedUpdates += update.Name;
                        }
                    }

                    if (notifiedUpdates != Utilities.Settings.NotifiedUpdates && ShowInTaskbar == false)
                    {
                        //if (Apps.Updates.Count > 1)
                        //{
                        //    new ToastContentBuilder.($"{Apps.Updates.Count} updates available",
                        //        "Click for details", BalloonIcon.Info);
                        //}
                        //else
                        //{
                        //    trayIcon.ShowBalloonTip($"An update for {Apps.Updates[0].Name.Split(' ')[0]} is available",
                        //        "Click for details", BalloonIcon.Info);
                        //}

                        //TaskbarIcon.TrayBalloonTipClicked += (s, e) =>
                        //{
                        //    frame.Navigate(new UpdaterPage());
                        //    Utilities.ShowFromTray(this);
                        //};
                    }

                    //if (Apps.Updates.Count > 1)
                    //{
                    //    TaskbarIcon.ToolTipText = $"sUpdater\n{Apps.Updates.Count} updates available";
                    //}
                    //else
                    //{
                    //    TaskbarIcon.ToolTipText = "sUpdater\n1 update available";
                    //}

                    Utilities.Settings.NotifiedUpdates = notifiedUpdates;
                    Utilities.SaveSettings();
                }
                else
                {
                    //TaskbarIcon.Icon = Properties.Resources.sUpdater;
                    //TaskbarIcon.ToolTipText = $"sUpdater\nNo updates available";
                }
            }
            else
            {
                //TaskbarIcon.Icon = Properties.Resources.sUpdater_Grey;
                //TaskbarIcon.ToolTipText = $"sUpdater\nCannot connect to the server.\nPlease check your internet connection.";
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = FindResource("menu") as ContextMenu;
            menu.PlacementTarget = sender as System.Windows.Controls.Button;
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

        private void TrayCheckUpdatesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConnectedToServer = StartPage.ReadDefenitions();
            if (ConnectedToServer)
            {
                StartPage.CheckForUpdates();
            }
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
