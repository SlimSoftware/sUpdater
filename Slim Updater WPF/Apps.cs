using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SlimUpdater
{
    /// <summary>
    /// Holds the lists with information about apps and updates.
    /// The class is used to share these lists between pages.
    /// </summary>
    public static class Apps
    {
        /// <summary>
        /// A List that contains all apps, except for portable apps
        /// </summary>
        public static ObservableCollection<Application> Regular;

        /// <summary>
        /// A List that contains all updates that are currently available
        /// </summary>
        public static ObservableCollection<Application> Updates;

        /// <summary>
        /// A temporary List used for the details mode of the UpdatePage
        /// when no updates are available
        /// </summary>
        public static ObservableCollection<Application> Details;

        /// <summary>
        /// A List that contains all Portable Apps
        /// </summary>
        public static ObservableCollection<Application> Portable;
    }
}
