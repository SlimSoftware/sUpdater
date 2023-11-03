using Microsoft.Win32;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sUpdater.Controllers
{
    public static class AppController
    {
        public static int UpdateCount { get; private set; }

        /// <summary>
        /// Contains all installed apps
        /// </summary>
        public static List<Application> InstalledApps { get; private set; } = new List<Application>();

        /// <summary>
        /// All apps that are available on the server
        /// </summary>
        public static List<Application> Apps = new List<Application>();

        /// <summary>
        /// Checks which apps are installed and adds them to the InstalledApps list
        /// </summary>
        private static async Task CheckForInstalledApps()
        {
            Apps = await Utilities.CallAPI<List<Application>>("apps");

            foreach (Application app in Apps)
            {
                foreach (DetectInfo detectInfo in app.DetectInfo)
                {
                    // Check whether this app run on the system
                    if (!Environment.Is64BitOperatingSystem && detectInfo.Arch == Arch.x64)
                    {
                        // This app cannot run on the system, so skip it
                        continue;
                    }

                    // Get local version if installed
                    string localVersion = null;
                    if (detectInfo?.RegKey != null)
                    {
                        var regValue = Registry.GetValue(detectInfo?.RegKey, detectInfo?.RegValue, null);
                        if (regValue != null)
                        {
                            localVersion = regValue.ToString();
                        }
                    }
                    else if (detectInfo?.ExePath != null)
                    {
                        string exePath = detectInfo?.ExePath;
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
                        detectInfo.IsInstalled = true;

                        if (!InstalledApps.Contains(app))
                        {
                            InstalledApps.Add(app);
                        }
                    }
                }
            }
        }

        public static async Task<List<Application>> GetUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);
            List<Application> updates = new List<Application>();
            try
            {
                await CheckForInstalledApps();
            }
            catch (Exception ex)
            {
                Log.Append($"Error while checking for updates: {ex.Message}", Log.LogLevel.ERROR);
            }

            foreach (Application app in InstalledApps)
            {
                if (Utilities.UpdateAvailable(app.LatestVersion, app.LocalVersion) && !app.NoUpdate)
                {
                    updates.Add(app);
                }
            }

            if (updates.Count > 0)
            {
                Log.Append($"{updates.Count} updates available", Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("1 update available", Log.LogLevel.INFO);
            }

            return updates;
        }

        /// <summary>
        /// Returns a list of not installed applications
        /// </summary>
        public static async Task<List<Application>> GetNotInstalledApps()
        {
            return await Task.Run(() =>
            {
                return InstalledApps.FindAll(app => app.LatestVersion != app.LocalVersion);
            });
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
