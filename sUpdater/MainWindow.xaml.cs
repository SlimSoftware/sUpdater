using Hardcodet.Wpf.TaskbarNotification;
using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        private AppUpdateInfo appUpdateInfo;

        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "/tray")
            {
                Utilities.MinimizeToTray(this);
            }

            AppController.CheckForUpdatesCompleted += AppController_CheckForUpdatesCompleted;
        }

        private void AppController_CheckForUpdatesCompleted(object sender, EventArgs e)
        {
            UpdateTaskbarIcon();
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

            await CheckForAppUpdates();
 
            Utilities.InitHttpClient();
            await AppController.CheckForUpdates();
        }

        private async Task CheckForAppUpdates()
        {
            appUpdateInfo = await AppUpdateController.GetAppUpdateInfo();

            if (appUpdateInfo.UpdateAvailable)
            {
                TaskbarIcon.Icon = Properties.Resources.sUpdater_Orange;
                TaskbarIcon.ToolTipText = $"sUpdater update available";

                Label updateAvailableHeaderLink = (Label)frame.Template.FindName("updateAvailableHeaderLink", frame);
                updateAvailableHeaderLink.Visibility = Visibility.Visible;

                if (ShowInTaskbar)
                {
                    AppUpdatePage appUpdatePage = new AppUpdatePage(appUpdateInfo);
                    frame.Navigate(appUpdatePage);
                }
           }
        }

        public void UpdateTaskbarIcon()
        {
            if (Utilities.ConnectedToServer)
            {
                int updateCount = AppController.Updates.Count;
                if (updateCount > 0)
                {
                    string notifiedUpdates = "";
                    TaskbarIcon.Icon = Properties.Resources.sUpdater_Orange;

                    foreach (Application app in AppController.Updates)
                    {
                        if (app != AppController.Updates.Last())
                        {
                            notifiedUpdates += $"{app.Id},";
                        }
                        else
                        {
                            notifiedUpdates += app.Id;
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
            if (appUpdateInfo != null)
            {
                AppUpdatePage appUpdatePage = new AppUpdatePage(appUpdateInfo);
                frame.Navigate(appUpdatePage);
            }
        }

        private async void TrayCheckUpdatesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await AppController.CheckForUpdates();
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

        private void UpdateAvailableHeaderLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(frame.Content is AppUpdatePage))
            {
                AppUpdatePage appUpdatePage = new AppUpdatePage(appUpdateInfo);
                frame.Navigate(appUpdatePage);
            }
        }
    }
}
