using Hardcodet.Wpf.TaskbarNotification;
using sUpdater.Controllers;
using sUpdater.Models;
using System;
using System.Reflection;
using System.Windows;

namespace sUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        async void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && args[1] == "/tray")
            {
                Utilities.MinimizeToTray(mainWindow);
                mainWindow.Visibility = Visibility.Hidden;
            }
            else
            {
                mainWindow.Show();
            }

            string version = Utilities.GetFriendlyVersion(Assembly.GetEntryAssembly().GetName().Version);
            Log.Append($"sUpdater v{version} started on {Utilities.GetFriendlyOSName()}", Log.LogLevel.INFO);

            Utilities.LoadSettings();
            Utilities.InitHttpClient();

            if (Utilities.Settings.AppServerURL != null)
            {
                Log.Append($"Using custom App Server: {Utilities.Settings.AppServerURL}",
                    Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("Using official App Server", Log.LogLevel.INFO);
            }

            await mainWindow.CheckForAppUpdates();
            await AppController.CheckForUpdates();
        }
    }
}
