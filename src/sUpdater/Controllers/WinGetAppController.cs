using Microsoft.Management.Deployment;
using sUpdater.Helpers;
using sUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using WindowsPackageManager.Interop;

namespace sUpdater.Controllers
{
    public class WinGetAppController
    {
        public static WindowsPackageManagerFactory PackageManagerFactory { get => GetPackageManagerFactory(); }
        private static WindowsPackageManagerFactory _packageManagerFactory;

        public static PackageManager PackageManager { get => GetPackageManager(); }
        private static PackageManager _packageManager;

        private static WindowsPackageManagerFactory GetPackageManagerFactory()
        {
            if (_packageManagerFactory != null) return _packageManagerFactory;

            // If the user is an administrator, use the elevated factory. Otherwhise COM will crash
            bool isAdministrator = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

            _packageManagerFactory = isAdministrator ? new WindowsPackageManagerElevatedFactory() : new WindowsPackageManagerStandardFactory();
            return _packageManagerFactory;
        }

        private static PackageManager GetPackageManager()
        {
            if (_packageManager != null) return _packageManager;

            _packageManager = PackageManagerFactory.CreatePackageManager();
            return _packageManager;
        }

        public async static Task<List<Application>> GetInstalledApps()
        {
            CreateCompositePackageCatalogOptions createCompositePackageCatalogOptions = PackageManagerFactory.CreateCreateCompositePackageCatalogOptions();
            foreach (var catalogRef in PackageManager.GetPackageCatalogs().ToArray())
            {
                createCompositePackageCatalogOptions.Catalogs.Add(catalogRef);
            }

            createCompositePackageCatalogOptions.CompositeSearchBehavior = CompositeSearchBehavior.LocalCatalogs;
            PackageCatalogReference installedSearchCatalogRef = PackageManager.CreateCompositePackageCatalog(createCompositePackageCatalogOptions);

            var connectResult = await installedSearchCatalogRef.ConnectAsync();
            if (connectResult.Status != ConnectResultStatus.Ok) return [];

            var findPackagesOptions = PackageManagerFactory.CreateFindPackagesOptions();

            var findPackagesResult = await connectResult.PackageCatalog.FindPackagesAsync(findPackagesOptions);
            var apps = await ConvertPackagesToApplications(findPackagesResult);

            return apps;
        }

        private static async Task<List<Application>> ConvertPackagesToApplications(FindPackagesResult packagesResult)
        {
            return await Task.Run(() =>
            {
                var apps = new List<Application>();

                foreach (var matchResult in packagesResult.Matches.ToArray())
                {
                    // if (matchResult.CatalogPackage.InstalledVersion != null)
                    //{
                    apps.Add(new Application
                    {
                        Name = matchResult.CatalogPackage.Name,
                        LocalVersion = matchResult.CatalogPackage.InstalledVersion.Version,
                        LatestVersion = matchResult.CatalogPackage.DefaultInstallVersion?.Version,
                        Icon = IconHelper.GetIconFromPackageId(matchResult.CatalogPackage.InstalledVersion.Id),
                        Installed = true
                    });
                    // }
                }

                return apps;
            });
        }
    }
}
