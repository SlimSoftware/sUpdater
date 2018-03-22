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
        public string PortableAppDir { get; set; }

        public void Load()
        {
            // Load XML File
            if (File.Exists(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                    @"Slim Software\Slim Updater\Settings.xml")))
            {
                XDocument settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                    @"Slim Software\Slim Updater\Settings.xml"));

                // Get content from XML nodes
                string defenitionURL = settingXML.Root.Element("DefenitionURL").Value;
                string portableAppDir = settingXML.Root.Element("PortableAppDir").Value;
                if (settingXML.Root.Element("MinimizeToTray").Value != null)
                {
                    MinimizeToTray = XmlConvert.ToBoolean(
                        settingXML.Root.Element("MinimizeToTray").Value);
                }
                if (defenitionURL != string.Empty)
                {
                    DefenitionURL = defenitionURL;
                }
                if (portableAppDir != string.Empty)
                {
                    PortableAppDir = portableAppDir;
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
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater"));
            }

            XDocument doc =
            new XDocument(new XElement("Settings", new XElement("DefenitionURL", String.Empty),
                new XElement("PortableAppDir"), String.Empty, new XElement("MinimizeToTray", "true")));
            doc.Save(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));
            // Unload XML
            doc = null;
        }

        public void Save()
        {
            // Check if XML File exists
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")))
            {
                CreateXMLFile();
            }

            // Load XML File
            XElement settingXML = XDocument.Load(Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml")).Element("Settings");

            // Save values
            if (DefenitionURL != null)
            {
                settingXML.Descendants("DefenitionURL").Remove();
                XElement defenitionURL = new XElement("DefenitionURL");
                defenitionURL.Value = DefenitionURL;
                settingXML.Add(defenitionURL);
            }
            if (PortableAppDir != null)
            {
                settingXML.Descendants("PortableAppDir").Remove();
                XElement portableAppDir = new XElement("PortableAppDir");
                portableAppDir.Value = PortableAppDir;
                settingXML.Add(portableAppDir);
            }
            if (MinimizeToTray != XmlConvert.ToBoolean(settingXML.Element("MinimizeToTray").Value))
            {
                settingXML.Descendants("MinimizeToTray").Remove();
                XElement minimizeToTray = new XElement("MinimizeToTray");
                minimizeToTray.Value = MinimizeToTray.ToString();
                settingXML.Add(minimizeToTray);
            }

            // Save XML File
            settingXML.Save(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), @"Slim Software\Slim Updater\Settings.xml"));

            // Unload XML File
            settingXML = null;
        }
    }
}
