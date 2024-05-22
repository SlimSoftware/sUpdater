using Microsoft.Win32;
using sUpdater.Models;
using sUpdater.Models.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace sUpdater.Controllers
{
    public static class PortableAppController
    {
        /// <summary>
        /// All portable apps that are available on the server
        /// </summary>
        public static List<PortableApp> PortableApps { get; private set; } = new List<PortableApp>();

        /// <summary>
        /// Populates the Portable Apps list with the portable apps from the App Server 
        /// and checks if they are installed on the user's system
        /// </summary>
        public static async Task CheckForPortableApps()
        {
            PortableApps.Clear();
            var portableAppDTOs = await Utilities.CallAPI<PortableAppDTO[]>("portable-apps");

            foreach (PortableAppDTO portableAppDTO in portableAppDTOs)
            {
                ArchiveDTO archiveDTO;
                ArchiveDTO x64Archive = Array.Find(portableAppDTO.Archives, a => a.Arch == Arch.x64);

                if (Environment.Is64BitOperatingSystem && x64Archive != null)
                {
                    archiveDTO = x64Archive;
                }
                else 
                {
                    ArchiveDTO x86Archive = Array.Find(portableAppDTO.Archives, d => d.Arch == Arch.x86);
                    archiveDTO = x86Archive ?? Array.Find(portableAppDTO.Archives, d => d.Arch == Arch.Any);
                }

                PortableApp portableApp = new PortableApp(portableAppDTO, archiveDTO);
                PortableApps.Add(portableApp);
            }
        }

        public static async Task CheckForInstalledPortableApps()
        {
            if (PortableApps.Count == 0) await CheckForPortableApps();

            string[] installedAppPaths = null;
            if (Utilities.Settings.PortableAppDir != null)
            {
                if (!Directory.Exists(Utilities.Settings.PortableAppDir))
                {
                    Directory.CreateDirectory(Utilities.Settings.PortableAppDir);
                }

                installedAppPaths = Directory.GetDirectories(Utilities.Settings.PortableAppDir);
            }

            foreach (PortableApp portableApp in PortableApps)
            {
                string installedAppDirPath = installedAppPaths.FirstOrDefault((p) => Path.GetFileName(p).Equals(portableApp.Name));

                if (installedAppDirPath != null)
                {
                    portableApp.Installed = true;

                    string launchPath = Path.Combine(installedAppDirPath, portableApp.Archive.LaunchFile);
                    Utilities.PopulatePortableAppIcon(portableApp, launchPath);

                    portableApp.LinkText = "Run";
                    portableApp.LinkClickCommand = new LinkClickCommand(new Action(async () =>
                    {
                        portableApp.LinkText = "";
                        portableApp.Status = "Running...";
                        await portableApp.Run();

                        portableApp.Status = "";
                        portableApp.LinkText = "Run";
                    }));
                }
            }
        }

        /// <summary>
        /// Returns a list of not installed portable apps
        /// </summary>
        public static async Task<List<PortableApp>> GetNotInstalledPortableApps()
        {
            if (PortableApps.Count == 0) await CheckForPortableApps();

            return await Task.Run(() => PortableApps.FindAll(app => app.Installed == false));
        }


        /// <summary>
        /// Returns a list of  installed portable apps
        /// </summary>
        public static async Task<List<PortableApp>> GetInstalledPortableApps()
        {
            if (PortableApps.Count == 0) await CheckForPortableApps();

            return await Task.Run(() => PortableApps.FindAll(app => app.Installed));
        }
    }
}
