using System;
using System.IO;

namespace sUpdater.Models
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; } = true;
        public string DefenitionURL { get; set; }

        public string DataDir { get; set; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\sUpdater");
        public string PortableAppDir { get; set; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), "Portable Apps");

        public string NotifiedUpdates { get; set; }
    }
}
