using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SlimUpdater
{
    public class Settings
    {
        public bool MinimizeToTray { get; set; }
        public string DefenitionURL { get; set; }
        public string DataDir { get; set; }
        public string PortableAppDir { get; set; }
        public string NotifiedUpdates { get; set; }
        private string XmlPath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml");
        private string XmlDir = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater");

        public void Load()
        {
            // Load XML File
            if (File.Exists(XmlPath))
            {
                XDocument settingXML = XDocument.Load(XmlPath);

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
                if (dataDir != string.Empty)
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

        public void CreateXMLFile()
        {
            // Check if folder exists
            if (!Directory.Exists(XmlDir))
            {
                Directory.CreateDirectory(XmlDir);
            }

            XDocument doc =
            new XDocument(new XElement("Settings", new XElement("DefenitionURL", String.Empty),
                new XElement("DataDir", string.Empty), new XElement("PortableAppDir"), String.Empty, 
                new XElement("MinimizeToTray", "true"), new XElement("NotifiedUpdates"), string.Empty));
            doc.Save(XmlPath);
            // Unload XML
            doc = null;
        }

        public void Save()
        {
            // Check if XML File exists
            if (!File.Exists(XmlPath))
            {
                CreateXMLFile();
            }

            // Load XML File
            XElement settingXML = XDocument.Load(XmlPath)
                .Element("Settings");

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
                {
                    XElement notifiedUpdates = new XElement("NotifiedUpdates");
                    notifiedUpdates.Value = NotifiedUpdates;
                    settingXML.Add(notifiedUpdates);
                }
            }

            // Save XML File
            settingXML.Save(XmlPath);

            // Unload XML File
            settingXML = null;
        }
    }
}
