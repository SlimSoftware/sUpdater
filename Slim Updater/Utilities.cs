using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SlimUpdater
{
    public static class Utilities
    {
        public static bool UpdateAvailable(string latestVersion, string localVersion)
        {
            string[] latestVersionSplit = latestVersion.Split('.');
            string[] localVersionSplit = localVersion.Split('.');

            for (int i = 0; i < Math.Max(latestVersionSplit.Length, localVersionSplit.Length); i++)
            {
                int v1; 
                int v2;
                bool v1IsNumber = int.TryParse(i < latestVersionSplit.Length ? latestVersionSplit[i] : "0", out v1);
                bool v2IsNumber = int.TryParse(i < localVersionSplit.Length ? localVersionSplit[i] : "0", out v2);

                if (v1IsNumber && v2IsNumber)
                {
                    if (v1.CompareTo(v2) > 0)
                    {
                        return true;
                    }
                    else if (v1.CompareTo(v2) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    string v1String = i < latestVersionSplit.Length ? latestVersionSplit[i] : "0";
                    string v2String = i < localVersionSplit.Length ? localVersionSplit[i] : "0";
                    if (v1String.CompareTo(v2String) > 0)
                    {
                        return true;
                    }
                    else if (v2String.CompareTo(v1String) < 0)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static void AddAppItems(List<AppItem> appItemList, Panel panel)
        {
            foreach (AppItem appItem in appItemList)
            {
                Separator separator = new Separator();
                int previousY = 0;
                int previousHeight = 0;

                if (panel.Controls.Count == 0)
                {
                    panel.Controls.Add(separator);
                    separator = new Separator()
                    {
                        Location = new Point(0, 45)
                    };
                    panel.Controls.Add(separator);
                    panel.Controls.Add(appItem);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
                else
                {
                    appItem.Location = new Point(0, (previousY + previousHeight));
                    separator.Location = new Point(0, (appItem.Location.Y + 45));
                    panel.Controls.Add(appItem);
                    panel.Controls.Add(separator);
                    previousY = appItem.Location.Y;
                    previousHeight = appItem.Height;
                }
            }
        }

        public static void AddAppItem(AppItem appItem, Panel panel)
        {
            Separator separator = new Separator();
            int previousY = 0;
            int previousHeight = 0;

            if (panel.Controls.Count == 0)
            {
                panel.Controls.Add(separator);
                separator = new Separator()
                {
                    Location = new Point(0, 45)
                };
                panel.Controls.Add(appItem);
                panel.Controls.Add(separator);
                previousY = appItem.Location.Y;
                previousHeight = appItem.Height;
            }
            else
            {
                // Get last AppItem's location
                previousY = panel.Controls[panel.Controls.Count - 1].Location.Y;
                previousHeight = panel.Controls[panel.Controls.Count - 1].Size.Height;
                appItem.Location = new Point(0, (previousY + previousHeight));
                separator.Location = new Point(0, (appItem.Location.Y + 45));
                panel.Controls.Add(appItem);
                panel.Controls.Add(separator);
            }
        }

        public enum CenterMode
        {
            Horizontal,
            Vertical,
            Both,
        }

        public static void CenterControl(Control control, Control parent, CenterMode centerMode)
        {
            if (centerMode == CenterMode.Horizontal)
            {
                control.Left = (parent.Width - control.Width) / 2;
            }

            if (centerMode == CenterMode.Vertical)
            {
                control.Top = (parent.Height - control.Height) / 2;
            }

            if (centerMode == CenterMode.Both)
            {
                control.Left = (parent.Width - control.Width) / 2;
                control.Top = (parent.Height - control.Height) / 2;
            }
        }

        /// <summary>
        /// Reduces the width of controls so that a horziontal scrollbar 
        /// doesn't appear in a container.
        /// </summary>
        /// <param name="controls">The controls to reduce the width for.</param>
        public static void FixScrollbars(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                int newWidth = control.Size.Width - SystemInformation.VerticalScrollBarWidth;
                control.Size = new Size(newWidth, control.Size.Height);
            }
        }

        public static string GetFriendlyOSName()
        {
            string osName = "Unknown Windows version";

            switch (Environment.OSVersion.Version.Major)
            {
                case 6:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            osName = "Windows Vista";
                            break;
                        case 1:
                            osName = "Windows 7";
                            break;
                        case 2:
                            osName = "Windows 8";
                            break;
                        case 3:
                            osName = "Windows 8.1";
                            break;
                    }
                    break;
                case 10:
                    osName = "Windows 10";
                    break;
            }
            return osName;
        }

        public static void MinimizeToTray(MainWindow mainWindow)
        {
            mainWindow.Hide();
            mainWindow.ShowInTaskbar = false;
            // Hide Slim Updater in Alt+Tab menu
            mainWindow.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }

        public static void ShowFromTray(MainWindow mainWindow)
        {
            // Show Slim Updater in Alt+Tab menu
            mainWindow.FormBorderStyle = FormBorderStyle.FixedSingle;            
            mainWindow.Show();
            mainWindow.ShowInTaskbar = true;
            // Make sure that the window is focused
            mainWindow.Activate();
        }
    }
}
