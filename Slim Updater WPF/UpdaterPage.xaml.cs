using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace SlimUpdater
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UpdaterPage : Page
    {
        List<Application> appList;

        public UpdaterPage()
        {
            InitializeComponent();
            ReadDefenitions();
            updateListView.ItemsSource = appList;
            updateListView.SelectAll();
        }

        #region ReadDefenitions()
        public void ReadDefenitions()
        {
            //logger.Log("Loading definitions file", Logger.LogLevel.INFO, logTextBox);
            appList = new List<Application>();
            XDocument defenitions = new XDocument();

            // Load XML File
            //try
            //{
            //    if (settings.DefenitionURL != null)
            //    {
            //        logger.Log("Using custom definition file from " + settings.DefenitionURL,
            //            Logger.LogLevel.INFO, logTextBox);
            //        defenitions = XDocument.Load(settings.DefenitionURL);
            //    }
            //    else
            //    {
                    //logger.Log("Using official definitions", Logger.LogLevel.INFO, logTextBox);
                    defenitions = XDocument.Load("defenitions.xml");
            //    }
            //}
            //catch (Exception e)
            //{
            //    logger.Log("Cannot check for updates: " + e.Message,
            //        Logger.LogLevel.ERROR, logTextBox);
            //    trayIcon.Icon = Properties.Resources.Slim_UpdaterIcon_Grey;
            //    trayIcon.Text = e.Message;
            //    updaterTile.BackColor = normalGrey;
            //    getNewAppsTile.BackColor = normalGrey;
            //    portableAppsTile.BackColor = normalGrey;
            //    updaterTile.Text = "Cannot check for updates";
            //    offlineLabel.Visible = true;
            //    offlineRetryLink.Visible = true;
            //    return;
            //}

            //if (updaterTile.BackColor == normalGrey)
            //{
            //    trayIcon.Icon = Properties.Resources.SlimUpdaterIcon;
            //    trayIcon.Text = "Slim Updater";
            //    updaterTile.BackColor = normalGreen;
            //    getNewAppsTile.BackColor = normalGreen;
            //    portableAppsTile.BackColor = normalGreen;
            //    offlineLabel.Visible = false;
            //    offlineRetryLink.Visible = false;
            //}

            foreach (XElement appElement in defenitions.Descendants("app"))
            {
                // Get content from XML nodes
                XAttribute nameAttribute = appElement.Attribute("name");
                XElement versionElement = appElement.Element("version");
                XElement archElement = appElement.Element("arch");
                XElement typeElement = appElement.Element("type");
                XElement switchElement = appElement.Element("switch");
                XElement dlElement = appElement.Element("dl");
                XElement regkeyElement = appElement.Element("regkey");
                XElement regvalueElement = appElement.Element("regvalue");
                XElement exePathElement = appElement.Element("exePath");

                // Get local version if installed
                string localVersion = null;
                if (regkeyElement?.Value != null)
                {
                    var regValue = Registry.GetValue(regkeyElement.Value,
                        regvalueElement.Value, null);
                    if (regValue != null)
                    {
                        localVersion = regValue.ToString();
                    }
                }
                if (exePathElement?.Value != null)
                {
                    string exePath = exePathElement.Value;
                    if (exePath.Contains("%pf32%"))
                    {
                        exePath = exePath.Replace("%pf32%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFilesX86));
                    }
                    if (exePath.Contains("%pf64%"))
                    {
                        exePath = exePath.Replace("%pf64%", Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
                    }

                    if (File.Exists(exePath))
                    {
                        localVersion = FileVersionInfo.GetVersionInfo(exePath).FileVersion;
                    }
                }

                // Add app to appList
                appList.Add(new Application(nameAttribute.Value.ToString(), versionElement.Value,
                    localVersion, archElement.Value, typeElement.Value, switchElement.Value,
                    dlElement.Value, null));
            }
        }
        #endregion

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (Application app in updateListView.SelectedItems)
            {

            }
        }

        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                // Select all unselected apps
                foreach (Application app in updateListView.Items)
                {
                    // Check if the app is not selected, if so check it
                    if (!updateListView.SelectedItems.Contains(app))
                    {
                        updateListView.SelectedItems.Add(app);
                    }
                }
            }
            else
            {
                // Unselect all selected apps                
                updateListView.SelectedItems.Clear();    
            }
        }
    }
}
