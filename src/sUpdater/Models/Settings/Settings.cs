using System;
using System.Collections.Generic;
using System.IO;

namespace sUpdater.Models.Settings
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; } = true;
        public string AppServerURL { get; set; }

        public string DataDir { get; set; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\sUpdater");
        public string PortableAppDir { get; set; } = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), "Portable Apps");

        public string NotifiedUpdates { get; set; }

        public List<IgnoredUpdate> IgnoredUpdates { get; set; }
    }
}
