using System;
using System.Linq;

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
    }
}
