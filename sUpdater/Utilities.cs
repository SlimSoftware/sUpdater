using System;
using System.Reflection;

namespace sUpdater
{
    public static class Utilities
    {
        public static bool UpdateAvailable(string latestVersion, string localVersion)
        {
            string[] latestVersionSplit = latestVersion.Split('.');
            string[] localVersionSplit = localVersion.Split('.');

            for (int i = 0; i < Math.Max(latestVersionSplit.Length, localVersionSplit.Length); i++)
            {
                int v1;
                int v2;
                bool v1IsNumber = int.TryParse(i < latestVersionSplit.Length ? latestVersionSplit[i] : "0", out v1);
                bool v2IsNumber = int.TryParse(i < localVersionSplit.Length ? localVersionSplit[i] : "0", out v2);

                if (v1IsNumber && v2IsNumber)
                {
                    if (v1.CompareTo(v2) > 0)
                    {
                        return true;
                    }
                    else if (v1.CompareTo(v2) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    string v1String = i < latestVersionSplit.Length ? latestVersionSplit[i] : "0";
                    string v2String = i < localVersionSplit.Length ? localVersionSplit[i] : "0";
                    if (v1String.CompareTo(v2String) > 0)
                    {
                        return true;
                    }
                    else if (v2String.CompareTo(v1String) < 0)
                    {
                        return false;
                    }
                }
            }

            return false;
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

        public static string GetFriendlyVersion(Version version)
        {
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
        }

        public static void ShowFromTray(MainWindow mainWindow)
        {
            mainWindow.Show();
            mainWindow.ShowInTaskbar = true;
        }

        public static MainWindow GetMainWindow()
        {
            return (MainWindow)System.Windows.Application.Current.MainWindow;
        }
    }
}
