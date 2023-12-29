using Microsoft.Win32;
using sUpdater.Models;
using sUpdater.Models.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace sUpdater.Controllers
{
    public static class AppController
    {
        /// <summary>
        /// Contains all installed apps
        /// </summary>
        public static List<Application> InstalledApps { get; private set; } = new List<Application>();

        /// <summary>
        /// All apps that are available on the server
        /// </summary>
        public static List<Application> Apps { get; private set; } = new List<Application>();

        /// <summary>
        /// All apps that have an update available to install
        /// </summary>
        public static List<Application> Updates { get; private set; } = new List<Application>();

        /// <summary>
        /// Checks which apps are installed and adds them to the InstalledApps list
        /// </summary>
        private static async Task CheckForInstalledApps()
        {
            InstalledApps.Clear();
            var appDTOs = await Utilities.CallAPI<ApplicationDTO[]>("apps");

            foreach (ApplicationDTO appDTO in appDTOs)
            {
                DetectInfoDTO detectInfoDTO;
                DetectInfoDTO x64DetectInfo = Array.Find(appDTO.DetectInfo, d => d.Arch == Arch.x64);

                if (Environment.Is64BitOperatingSystem && x64DetectInfo != null)
                {
                    detectInfoDTO = x64DetectInfo;
                }
                else 
                {
                    DetectInfoDTO x86BitDetectInfo = Array.Find(appDTO.DetectInfo, d => d.Arch == Arch.x86);
                    detectInfoDTO = x86BitDetectInfo ?? Array.Find(appDTO.DetectInfo, d => d.Arch == Arch.Any);
                }

                // Get local version if installed
                string localVersion = null;
                if (detectInfoDTO?.RegKey != null)
                {
                    var regValue = Registry.GetValue(detectInfoDTO?.RegKey, detectInfoDTO?.RegValue, null);
                    if (regValue != null)
                    {
                        localVersion = regValue.ToString();
                    }
                }
                else if (detectInfoDTO?.ExePath != null)
                {
                    string exePath = detectInfoDTO?.ExePath;
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
                    if (InstalledApps.Find(a => appDTO.Name == a.Name) == null)
                    {
                        InstallerDTO installerDTO = Array.Find(appDTO.Installers, i => i.DetectInfoId == detectInfoDTO.Id);
                        Application application = new Application(appDTO, new DetectInfo(detectInfoDTO), new Installer(installerDTO));

                        InstalledApps.Add(application);
                    }
                }
            }
        }

        /// <summary>
        /// Checks for applications that have an update available and stores them in the Updates property
        /// </summary>
        public static async Task CheckForUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);

            try
            {
                await CheckForInstalledApps();
            }
            catch (Exception ex)
            {
                Log.Append($"Error while checking for installed apps: {ex.Message}", Log.LogLevel.ERROR);
            }

            Updates.Clear();

            foreach (Application app in InstalledApps)
            {
                if (Utilities.UpdateAvailable(app.LatestVersion, app.LocalVersion) && !app.NoUpdate)
                {
                    Updates.Add(app);
                }
            }

            if (Updates.Count > 0)
            {
                Log.Append($"{Updates.Count} updates available", Log.LogLevel.INFO);
            }
            else
            {
                Log.Append("1 update available", Log.LogLevel.INFO);
            }
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
    }
}
