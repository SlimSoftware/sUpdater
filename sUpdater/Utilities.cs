﻿using Microsoft.Win32;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Application = sUpdater.Models.Application;

namespace sUpdater
{
    public static class Utilities
    {
        public static Settings Settings { get; set; }

        /// <summary>
        /// Whether the last API request was succesful or not
        /// </summary>
        public static bool ConnectedToServer = true;

        public static HttpClient HttpClient { get; private set; }

        private static readonly string settingsXmlDir = Debugger.IsAttached ?
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) :
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Slim Software\sUpdater");

        private static readonly string settingsXmlPath = Path.Combine(settingsXmlDir, "settings.xml");

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
            string osName = "";

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
                    if (Environment.OSVersion.Version.Build >= 22000)
                        osName = "Windows 11";
                    else
                        osName = "Windows 10";
                    break;
            }

            if (osName != "")
            {
                return osName;
            }
            else
            {
                return "unknown Windows version";
            }
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
            if (mainWindow != null)
            {
                mainWindow.Hide();
                mainWindow.ShowInTaskbar = false;
            }
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

        public static void InitHttpClient()
        {
            if (HttpClient != null)
            {
                HttpClient.Dispose();
            }
            HttpClient = new HttpClient();

            string appServerURL = Settings.AppServerURL ?? "https://www.slimsoftware.dev/supdater/api/v2/";
            if (!appServerURL.EndsWith("/"))
            {
                appServerURL += "/";
            }
            HttpClient.BaseAddress = new Uri(appServerURL);

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<T> CallAPI<T>(string url)
        {
            try
            {
                Log.Append($"GET {HttpClient.BaseAddress}{url}", Log.LogLevel.INFO);
                var response = await HttpClient.GetFromJsonAsync<T>(url);

                if (ConnectedToServer == false) ConnectedToServer = true;
                return response;
            }
            catch (Exception ex)
            {
                Log.Append($"GET {HttpClient.BaseAddress}{url} ({ex.Message})", Log.LogLevel.ERROR);
                ConnectedToServer = false;
                return default;
            }
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
            var dialog = new OpenFolderDialog();
            dialog.InitialDirectory = defaultPath;
            var isOk = dialog.ShowDialog();

            if (isOk == true) return dialog.FolderName;
            return null;
        }

        public static void LoadSettings()
        {
            // Load XML File
            if (File.Exists(settingsXmlPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(settingsXmlPath, FileMode.Open))
                {
                    Settings = (Settings)serializer.Deserialize(fs);
                }
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

        public static string GetAppServerURL()
        {
            return Settings.AppServerURL ?? "https://www.slimsoftware.dev/supdater/api";
        }

        public static string RemoveLeadingNewLinesAndTabs(string text)
        {
            string newText = text;

            if (text.StartsWith("\n"))
            {
                newText = newText.TrimStart("\n".ToCharArray());
            }
            if (text.Contains("\t"))
            {
                newText = newText.Replace("\t", string.Empty);
            }

            return newText;
        }

        /// <summary>
        /// Replaces all variables in the given exePath string and returns the resulting string
        /// </summary>
        public static string ParseExePath(string exePath)
        {
            if (exePath.Contains("%pf32%"))
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86));
                }
                else
                {
                    exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFiles));
                }
            }
            else if (exePath.Contains("%pf64%"))
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    // We cannot use SpecialFolder.ProgramFiles here, because we are running as a 32-bit process
                    // SpecialFolder.ProgramFiles would return the 32-bit ProgramFiles here
                    exePath = exePath.Replace("%pf64%", Environment.GetEnvironmentVariable("ProgramW6432"));
                }
            }

            // Replace system environment vars
            exePath = Environment.ExpandEnvironmentVariables(exePath);

            return exePath;
        }

        public static BitmapSource GetIconFromFile(string filePath)
        {
            using (var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filePath))
            {
                return Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle, Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public static void PopulatePortableAppIcon(PortableApp portableApp, string exePath)
        {
            if (portableApp.Icon == null && File.Exists(exePath))
            {
                portableApp.Icon = GetIconFromFile(exePath);
            }
        }
        public static Application GetApplicationFromControl(object sender)
        {
            var control = sender as Control;
            return (Application)control.DataContext;
        }

        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object child in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (child is T typedChild)
                    {
                        yield return typedChild;
                    }

                    if (child is DependencyObject depChild)
                    {
                        foreach (T childOfChild in FindLogicalChildren<T>(depChild))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static void OpenWebLink(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}

