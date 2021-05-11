using System;
using System.Windows;
using sUpdater.Models;
using System.IO;
using System.Xml.Serialization;
using DialogResult = System.Windows.Forms.DialogResult;
using FolderBrowser = System.Windows.Forms.FolderBrowserDialog;

namespace sUpdater
{
    public static class Utilities
    {
        public static Settings Settings { get; set; }

        private static string settingsXmlDir = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\sUpdater");
        private static string settingsXmlPath = Path.Combine(settingsXmlDir, "settings.xml");

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
                return $"{version.Major}.0";
            }
            else if (version.Minor != 0 && version.Build == 0 && version.Revision == 0)
            {
                return $"{version.Major}.{version.Minor}";
            }
            else if (version.Minor != 0 && version.Build != 0 && version.Revision == 0)
            {
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
            else if (version.Minor == 0 && version.Build != 0 && version.Revision == 0)
            {
                return $"{version.Major}.0.{version.Build}";
            }
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
            mainWindow.ShowInTaskbar = true;
           
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }

            mainWindow.Show();
            mainWindow.Activate();
        }

        public static MainWindow GetMainWindow()
        {
            return (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        /// <summary>
        /// Opens a folder browser dialog and returns the selected path
        /// </summary>
        /// <param name="defaultPath">The path to the folder that the dialog should show when opened</param>
        /// <returns>The path selected by the user or null if the dialog has been cancelled</returns>
        public static string BrowseForFolder(string defaultPath)
        {
            using (FolderBrowser fbd = new FolderBrowser())
            {
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    return fbd.SelectedPath;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void LoadSettings()
        {
            // Load XML File
            if (File.Exists(settingsXmlPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                FileStream fs = new FileStream(settingsXmlPath, FileMode.Open);
                Settings = (Settings)serializer.Deserialize(fs);
            }
            else
            {
                CreateSettingsXMLFile();
                LoadSettings();
            }
        }

        private static void CreateSettingsXMLFile()
        {
            if (!Directory.Exists(settingsXmlDir))
            {
                Directory.CreateDirectory(settingsXmlDir);
            }

            Settings = new Settings();
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter(settingsXmlPath);
            serializer.Serialize(writer, Settings);
            writer.Close();
        }

        public static void SaveSettings()
        {
            if (!File.Exists(settingsXmlPath))
            {
                CreateSettingsXMLFile();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter(settingsXmlPath);
            serializer.Serialize(writer, Settings);
            writer.Close();
        }
    }
}
