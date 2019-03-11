using System;

namespace SlimUpdater
{
    public static class Utilities
    {
        public static bool IsUpToDate(string latestVersion, string localVersion)
        {
            if (string.Compare(latestVersion, localVersion) == -1)
            {
                // localVersion is higher than latestVersion
                return true;
            }
            if (string.Compare(latestVersion, localVersion) == 0)
            {
                // localVersion is equal to latestVersion
                return true;
            }
            else
            {
                // latestVersion is higer than localVersion
                return false;
            }
        }

        public enum CenterMode
        {
            Horizontal,
            Vertical,
            Both,
        }

        public static string GetFriendlyOSName()
        {
            string osName = "Unknown Windows version";

            switch (Environment.OSVersion.Version.Major)
            {
                case 6:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            osName = "Windows Vista";
                            break;
                        case 1:
                            osName = "Windows 7";
                            break;
                        case 2:
                            osName = "Windows 8";
                            break;
                        case 3:
                            osName = "Windows 8.1";
                            break;
                    }
                    break;
                case 10:
                    osName = "Windows 10";
                    break;
            }
            return osName;
        }

        public static void MinimizeToTray(MainWindow mainWindow)
        {
            mainWindow.Hide();
            mainWindow.ShowInTaskbar = false;
            // Hide Slim Updater in Alt+Tab menu
            //mainWindow.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            //mainWindow.WindowState = FormWindowState.Minimized;
        }

        public static void ShowFromTray(MainWindow mainWindow)
        {
            mainWindow.Show();
            mainWindow.ShowInTaskbar = true;
            // Show Slim Updater in Alt+Tab menu
            //mainWindow.FormBorderStyle = FormBorderStyle.FixedSingle;
            //mainWindow.WindowState = FormWindowState.Normal;
        }
    }
}
