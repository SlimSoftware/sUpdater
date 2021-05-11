using Microsoft.Win32;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace sUpdater.Controllers
{
    public static class AppController
    {
        /// <summary>
        /// Temporarily cached information to detect all apps that are available on the server
        /// </summary>
        private static List<DetectInfo> detectInfo = new List<DetectInfo>();

        /// <summary>
        /// Contains all installed apps, with its id as key and the installed version as value
        /// </summary>
        private static Dictionary<int, string> installedApps = new Dictionary<int, string>();

        /// <summary>
        /// Contains the ids of the updates that are available
        /// </summary>
        public static List<int> UpdateIds { get; set; } = new List<int>();

        /// <summary>
        /// Checks which apps are installed and adds their id and installed version to the installedApps dictionary
        /// </summary>
        private static async Task CheckInstalledApps()
        {
            detectInfo = await Utilities.CallAPI<List<DetectInfo>>("detectinfo");

            foreach (DetectInfo info in detectInfo)
            {
                // Check whether this app run on the system
                if (!Environment.Is64BitOperatingSystem && info.Arch == Models.Arch.x64)
                {
                    // This app cannot run on the system, so skip it
                    continue;
                }

                // Get local version if installed
                string localVersion = null;
                if (info?.RegKey != null)
                {
                    var regValue = Registry.GetValue(info?.RegKey, info?.RegValue, null);
                    if (regValue != null)
                    {
                        localVersion = regValue.ToString();
                    }
                }
                else if (info?.ExePath != null)
                {
                    string exePath = info?.ExePath;
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
                            exePath = exePath.Replace("%pf64%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
                        }
                        else
                        {
                            // Do not add the app to the list because it is a 64 bit app
                            // on a 32 bit system
                            continue;
                        }
                    }

                    if (File.Exists(exePath))
                    {
                        localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                    }
                }

                if (localVersion != null)
                {
                    installedApps.Add(info.Id, localVersion);
                }
            }
        }

        public static async Task CheckForUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);
            UpdateIds = new List<int>();
            try
            {
                await CheckInstalledApps();
            }
            catch (Exception ex)
            {
                Log.Append($"Error while checking for updates: {ex.Message}", Log.LogLevel.ERROR);
            }

            string installedAppIdsString = string.Join(",", installedApps.Keys);
            List<Application> latestAppVersions = await Utilities.CallAPI<List<Application>>($"appversion?ids={installedAppIdsString}");

            foreach (var kvp in installedApps)
            {
                Application app = latestAppVersions.Find(x => x.Id == kvp.Key);
                string localVersion = kvp.Value;

                if (!app.NoUpdate)
                {
                    if (Utilities.UpdateAvailable(app.LatestVersion, localVersion))
                    {
                        UpdateIds.Add(kvp.Key);
                    }
                }
            }

            if (UpdateIds.Count > 0)
            {
                Log.Append($"{UpdateIds.Count} updates available", Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("1 update available", Log.LogLevel.INFO);
            }

            // Cleanup
            detectInfo = null;
        }

        public static int GetUpdateCount()
        {
            return UpdateIds.Count;
        }

        /// <summary>
        /// Returns a list of updates containing info such as name, local and latest version
        /// </summary>
        public static async Task<List<Application>> GetUpdateInfo()
        {
            string ids = string.Join(",", UpdateIds);

            if (ids != "")
            {
                List<Application> updateInfo = await Utilities.CallAPI<List<Application>>($"apps?ids={ids}");
                return updateInfo;
            }
            else
            {
                return new List<Application>();
            }
        }

        /// <summary>
        /// Returns a list of not installed applications containing info such as name, local and latest version
        /// </summary>
        public static async Task<List<Application>> GetNotInstalledAppInfo()
        {
            string ids = string.Join(",", installedApps.Select(x => x.Key));
            List<Application> apps = await Utilities.CallAPI<List<Application>>($"app?nids={ids}");

            // Append the installed versions
            for (int i = 0; i < apps.Count; i++)
            {
                apps[i].LocalVersion = installedApps.FirstOrDefault(x => x.Key == apps[i].Id).Value;
            }

            return apps;
        }

        /// <summary>
        /// Returns a list of all apps containing info such as name, local and latest version
        /// </summary>
        public static async Task<List<Application>> GetAppInfo()
        {
            List<Application> appInfo = await Utilities.CallAPI<List<Application>>("app");

            // Append the installed versions
            for (int i = 0; i < appInfo.Count; i++)
            {
                appInfo[i].LocalVersion = installedApps.FirstOrDefault(x => x.Key == appInfo[i].Id).Value;
            }

            return appInfo;
        }

        public static async Task<Changelog> GetChangelog(int appId)
        {
            return await Utilities.CallAPI<Changelog>($"changelog?id={appId}");
        }

        public static async Task<Description> GetDescription(int appId)
        {
            return await Utilities.CallAPI<Description>($"description?id={appId}");
        }
    }
}
