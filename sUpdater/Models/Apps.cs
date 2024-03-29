﻿using System.Collections.Generic;

namespace sUpdater.Models
{
    /// <summary>
    /// Holds the ObservableCollections with information about apps and updates.
    /// This class is used to share these lists between pages.
    /// </summary>
    public static class Apps
    {
        /// <summary>
        /// Contains all apps, except for portable apps
        /// </summary>
        public static List<Application> Regular;

        /// <summary>
        /// Contains all updates that are currently available
        /// </summary>
        public static List<Application> Updates;
    }
}
