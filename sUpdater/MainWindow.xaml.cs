using Hardcodet.Wpf.TaskbarNotification;
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
        public TaskbarIcon TaskbarIcon { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            Log.Append("Slim Updater v" + Utilities.GetFriendlyVersion() + " " + 
                "started on " + Utilities.GetFriendlyOSName(), Log.LogLevel.INFO);

            TaskbarIcon = (TaskbarIcon)FindResource("TrayIcon");
            Settings.Load();
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
    }
}
