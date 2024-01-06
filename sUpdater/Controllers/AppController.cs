﻿using Microsoft.Win32;
using sUpdater.Models;
using sUpdater.Models.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

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
                string exePath = null;
                ImageSource icon = null;

                if (detectInfoDTO.RegKey != null)
                {
                    localVersion = GetLocalVersionFromRegistry(detectInfoDTO);
                }
                if (detectInfoDTO.ExePath != null)
                {  
                    exePath = Utilities.ParseExePath(detectInfoDTO.ExePath);

                    if (File.Exists(exePath))
                    {
                        // Prefer getting version from registry because those are usually more
                        // prettily formatted than the file version number
                        if (localVersion == null)
                        {
                            localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                        }

                        icon = Utilities.GetIconFromFile(exePath);
                    }
                }

                if (localVersion != null)
                {
                    if (InstalledApps.Find(a => appDTO.Name == a.Name) == null)
                    {
                        InstallerDTO installerDTO = Array.Find(appDTO.Installers, i => i.DetectInfoId == detectInfoDTO.Id);
                        Application application = new Application(appDTO, new Installer(installerDTO));
                        application.LocalVersion = localVersion;
                        application.Icon = icon;

                        InstalledApps.Add(application);
                    }
                }
            }
        }

        private static string GetLocalVersionFromRegistry(DetectInfoDTO detectInfo)
        {
            var registryHive = detectInfo.RegKey.StartsWith("HKEY_LOCAL_MACHINE") 
                ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
            var registryView = detectInfo.Arch == Arch.x64 
                ? RegistryView.Registry64 : RegistryView.Registry32;
            var baseKey = RegistryKey.OpenBaseKey(registryHive, registryView);       

            string keyPath = detectInfo.RegKey;
            keyPath = keyPath.Replace("HKEY_LOCAL_MACHINE\\", "");
            keyPath = keyPath.Replace("HKEY_CURRENT_USER\\", "");

            var key = baseKey.OpenSubKey(keyPath, false);
            if (key == null)
            {
                return null;
            }

            var regValue = key.GetValue(detectInfo.RegValue, null);
            if (regValue == null && Environment.Is64BitOperatingSystem)
            {
                // In case we are on 64-bit we can check once more under the 64-bit registry
                baseKey = RegistryKey.OpenBaseKey(registryHive, RegistryView.Registry64);
                key = baseKey.OpenSubKey(keyPath, false);
                regValue = key.GetValue(detectInfo.RegValue, null);
            }

            return regValue?.ToString();
        }

        /// <summary>
        /// Checks for applications that have an update available and stores them in the Updates property
        /// </summary>
        public static async Task CheckForUpdates()
        {
            Log.Append("Checking for updates...", Log.LogLevel.INFO);

            //try
            //{
                await CheckForInstalledApps();
            //}
            //catch (Exception ex)
            //{
            //    Log.Append($"Error while checking for installed apps: {ex.Message}", Log.LogLevel.ERROR);
            //}

            Updates.Clear();

            foreach (Application app in InstalledApps)
            {
                if (!app.NoUpdate && Utilities.UpdateAvailable(app.LatestVersion, app.LocalVersion))
                {
                    Application updateApp = app.Clone();
                    updateApp.Name += $" {app.LatestVersion}";
                    updateApp.DisplayedVersion = $"Installed: {app.LocalVersion}";

                    Updates.Add(updateApp);
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
