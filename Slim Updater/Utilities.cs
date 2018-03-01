using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SlimUpdater
{
    public static class Utilities
    {
        public static bool IsUpToDate(string latestVersion, string localVersion)
        {
            if (string.Compare(latestVersion, localVersion) == -1)
            {
                // localVersion is higher than latestVersion
                return true;
            }
            if (string.Compare(latestVersion, localVersion) == 0)
            {
                // localVersion is equal to latestVersion
                return true;
            }
            else
            {
                // latestVersion is higer than localVersion
                return false;
            }
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

        public static void CenterControl(Control control, Control parent)
        {
            control.Left = (parent.Width - control.Width) / 2;
            control.Top = (parent.Height - control.Height) / 2;
        }
    }
}
