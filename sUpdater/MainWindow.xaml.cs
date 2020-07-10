using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TaskbarIcon TaskbarIcon { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "/tray")
            {
                Utilities.MinimizeToTray(this);
            }

            Log.Append("Slim Updater v" + Utilities.GetFriendlyVersion() + " " +
                "started on " + Utilities.GetFriendlyOSName(), Log.LogLevel.INFO);

            TaskbarIcon = (TaskbarIcon)FindResource("TrayIcon");
            TaskbarIcon.ContextMenu = (ContextMenu)FindResource("trayMenu");
            TaskbarIcon.TrayLeftMouseDown += TaskbarIcon_TrayLeftMouseDown;

            Settings.Load();

            StartPage.ReadDefenitions();
            StartPage.CheckForUpdates();
            UpdateTaskbarIcon();
        }

        public void UpdateTaskbarIcon()
        {
            if (Apps.Updates.Count > 0)
            {
                string notifiedUpdates = "";
                TaskbarIcon.Icon = Properties.Resources.sUpdater_Orange;

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

                if (notifiedUpdates != Settings.NotifiedUpdates && ShowInTaskbar == false)
                {
                    if (Apps.Updates.Count > 1)
                    {
                        TaskbarIcon.ShowBalloonTip($"{Apps.Updates.Count} updates available",
                            "Click for details", BalloonIcon.Info);                 
                    }
                    else
                    {
                        TaskbarIcon.ShowBalloonTip($"An update for {Apps.Updates[0].Name.Split(' ')[0]} is available",
                            "Click for details", BalloonIcon.Info);                     
                    }

                    TaskbarIcon.TrayBalloonTipClicked += (s, e) =>
                    {
                        frame.Navigate(new UpdaterPage());
                        Utilities.ShowFromTray(this);
                    };
                }

                if (Apps.Updates.Count > 1)
                {
                    TaskbarIcon.ToolTipText = $"sUpdater\n{Apps.Updates.Count} updates available";
                }
                else
                {
                    TaskbarIcon.ToolTipText = "sUpdater\n1 update available";
                }

                Settings.NotifiedUpdates = notifiedUpdates;
                Settings.Save();
            }
            else
            {
                TaskbarIcon.Icon = Properties.Resources.sUpdater;
                TaskbarIcon.ToolTipText = $"sUpdater\nNo updates available";
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
        }

        private void TrayCheckUpdatesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StartPage.ReadDefenitions();
            StartPage.CheckForUpdates();
            UpdateTaskbarIcon();
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
            if (Settings.MinimizeToTray)
            {
                e.Cancel = true;
                Utilities.MinimizeToTray(this);
            }
        }
    }
}
