using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace sUpdater
{
    public static class Settings
    {
        public static bool MinimizeToTray { get; set; }
        public static string DefenitionURL { get; set; }
        public static string DataDir { get; set; }
        public static string PortableAppDir { get; set; }
        public static string NotifiedUpdates { get; set; }

        private static string xmlDir = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\sUpdater");
        private static string xmlPath = Path.Combine(xmlDir, "settings.xml");

        public static void Load()
        {
            // Load XML File
            if (File.Exists(xmlPath))
            {
                XDocument settingXML = XDocument.Load(xmlPath);

                // Get content from XML nodes
                string defenitionURL = settingXML.Root.Element("DefenitionURL")?.Value;
                string dataDir = settingXML.Root.Element("DataDir")?.Value;
                string portableAppDir = settingXML.Root.Element("PortableAppDir")?.Value;
                string notifiedUpdates = settingXML.Root.Element("NotifiedUpdates")?.Value;

                if (settingXML.Root.Element("MinimizeToTray").Value != null)
                {
                    MinimizeToTray = XmlConvert.ToBoolean(
                        settingXML.Root.Element("MinimizeToTray").Value);
                }
                if (defenitionURL != string.Empty)
                {
                    DefenitionURL = defenitionURL;
                }
                if (dataDir != string.Empty && dataDir != null)
                {
                    DataDir = dataDir;
                }
                else
                {
                    // Set to default data dir
                    DataDir = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater");
                }
                if (portableAppDir != string.Empty)
                {
                    PortableAppDir = portableAppDir;
                }
                if (notifiedUpdates != string.Empty)
                {
                    NotifiedUpdates = notifiedUpdates;
                }

                // Unload XML File
                settingXML = null;
            }
            else
            {
                CreateXMLFile();
            }
        }

        public static void CreateXMLFile()
        {
            if (!Directory.Exists(xmlDir))
            {
                Directory.CreateDirectory(xmlDir);
            }

            XDocument doc = new XDocument(new XElement("Settings", new XElement("DefenitionURL", string.Empty),
                new XElement("DataDir", string.Empty), new XElement("PortableAppDir", string.Empty),
                new XElement("MinimizeToTray", "true"), new XElement("NotifiedUpdates", string.Empty)));
            doc.Save(xmlPath);

            // Unload XML
            doc = null;
        }

        public static void Save()
        {
            if (!File.Exists(xmlPath))
            {
                CreateXMLFile();
            }

            XElement settingXML = XDocument.Load(xmlPath).Element("Settings");

            // Save values
            if (MinimizeToTray != XmlConvert.ToBoolean(settingXML.Element("MinimizeToTray")?.Value))
            {
                settingXML.Descendants("MinimizeToTray").Remove();
                XElement minimizeToTray = new XElement("MinimizeToTray");
                minimizeToTray.Value = MinimizeToTray.ToString().ToLower();
                settingXML.Add(minimizeToTray);
            }
            if (DefenitionURL != settingXML.Element("DefenitionURL")?.Value)
            {
                settingXML.Descendants("DefenitionURL").Remove();
                if (DefenitionURL != null)
                {
                    XElement defenitionURL = new XElement("DefenitionURL");
                    defenitionURL.Value = DefenitionURL;
                    settingXML.Add(defenitionURL);
                }
            }
            if (DataDir != settingXML.Element("DataDir")?.Value)
            {
                settingXML.Descendants("DataDir").Remove();
                if (DataDir != null)
                {
                    XElement dataDir = new XElement("DataDir");
                    dataDir.Value = DataDir;
                    settingXML.Add(dataDir);
                }
            }
            if (PortableAppDir != settingXML.Element("PortableAppDir")?.Value)
            {
                settingXML.Descendants("PortableAppDir").Remove();
                if (PortableAppDir != null)
                {
                    XElement portableAppDir = new XElement("PortableAppDir");
                    portableAppDir.Value = PortableAppDir;
                    settingXML.Add(portableAppDir);
                }
            }
            if (NotifiedUpdates != settingXML.Element("NotifiedUpdates")?.Value)
            {
                settingXML.Descendants("NotifiedUpdates").Remove();
                if (NotifiedUpdates != null)
                {
                    XElement notifiedUpdates = new XElement("NotifiedUpdates");
                    notifiedUpdates.Value = NotifiedUpdates;
                    settingXML.Add(notifiedUpdates);
                }
            }

            settingXML.Save(xmlPath);

            // Unload XML File
            settingXML = null;
        }
    }
}
