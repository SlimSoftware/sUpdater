using System;
using System.Reflection;

namespace sUpdater
{
    public static class Utilities
    {
        public static bool UpdateAvailable(string latestVersion, string localVersion)
        {
            if (float.Parse(latestVersion) > float.Parse(localVersion))
                return true;
            else
                return false;
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

        public static string GetFriendlyVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            if (version.Minor == 0 && version.Build == 0 && version.Revision == 0)
            {
                return string.Format("{0}.{1}", version.Major.ToString(), 0);
            }
            else if (version.Minor != 0 && version.Build == 0 && version.Revision == 0)
            {
                return string.Format("{0}.{1}", version.Major.ToString(), version.Minor.ToString());
            }
            else if (version.Minor != 0 && version.Build != 0 && version.Revision == 0)
            {
                return string.Format("{0}.{1}.{2}", version.Major.ToString(), version.Minor.ToString(),
                    version.Build.ToString());
            }
            else if (version.Minor == 0 && version.Build != 0 && version.Revision == 0)
            {
                return string.Format("{0}.{1}.{2}", version.Major.ToString(), 
                    version.Build.ToString(), 0);
            }
            //else if (version.Minor == 0 && version.Build == 0 && version.Revision != 0)
            //{
            //    return string.Format("{0}.{1}.{2}", version.Major.ToString(), 0,
            //        version.Build.ToString(), version.Revision);
            //}
            //else if (version.Minor == 0 && version.Build != 0 && version.Revision != 0)
            //{
            //    return string.Format("{0}.{1}.{2}.{", version.Major.ToString(), 0,
            //        version.Build.ToString(), version.Revision);
            //}
            else
            {
                return version.ToString();
            }
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
