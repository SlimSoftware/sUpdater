using Microsoft.Management.Deployment;
using Microsoft.Win32;
using sUpdater.Helpers;
using sUpdater.Models.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Threading;
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

        private static PackageCatalog _appsCatalog;
        private static readonly SemaphoreSlim _catalogLock = new(1, 1);

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

        public static async Task<PackageCatalog> GetAppCatalog()
        {
            if (_appsCatalog != null) return _appsCatalog;

            await _catalogLock.WaitAsync();
            try
            {
                if (_appsCatalog != null) return _appsCatalog;

                var remoteCatalogRef = PackageManager.GetPredefinedPackageCatalog(PredefinedPackageCatalog.OpenWindowsCatalog);

                var connectResult = await remoteCatalogRef.ConnectAsync();

                if (connectResult.Status != ConnectResultStatus.Ok) return null;

                NetworkChange.NetworkAvailabilityChanged += (_, e) =>
                {
                    if (e.IsAvailable) _appsCatalog = null;
                };

                SystemEvents.PowerModeChanged += (_, e) =>
                {
                    if (e.Mode == PowerModes.Resume) _appsCatalog = null;
                };

                _appsCatalog = connectResult.PackageCatalog;
                return _appsCatalog;
            }
            finally
            {
                _catalogLock.Release();
            }
        }

        public static Task<List<WinGetApp>> GetInstalledApps()
        {
            return Task.Run(async () =>
            {
                CreateCompositePackageCatalogOptions createCompositePackageCatalogOptions = PackageManagerFactory.CreateCreateCompositePackageCatalogOptions();
                var catalogs = PackageManager.GetPackageCatalogs().ToList();
                foreach (var catalogRef in catalogs)
                {
                    createCompositePackageCatalogOptions.Catalogs.Add(catalogRef);
                }

                createCompositePackageCatalogOptions.CompositeSearchBehavior = CompositeSearchBehavior.LocalCatalogs;
                PackageCatalogReference installedSearchCatalogRef = PackageManager.CreateCompositePackageCatalog(createCompositePackageCatalogOptions);

                var connectResult = await installedSearchCatalogRef.ConnectAsync();
                if (connectResult.Status != ConnectResultStatus.Ok) return [];

                var findPackagesOptions = PackageManagerFactory.CreateFindPackagesOptions();

                var operation = connectResult.PackageCatalog.FindPackagesAsync(findPackagesOptions);

                var findPackagesResult = await connectResult.PackageCatalog.FindPackagesAsync(findPackagesOptions);
                var apps = await ConvertPackagesToApplications(findPackagesResult);

                return apps;
            });
        }

        private static async Task<List<WinGetApp>> ConvertPackagesToApplications(FindPackagesResult packagesResult)
        {
            return await Task.Run(() =>
            {
                var apps = new List<WinGetApp>();
                var matches = packagesResult.Matches.ToList();

                foreach (var matchResult in matches)
                {
                    var catalogPackage = matchResult.CatalogPackage;
                    var packageMetadata = catalogPackage.DefaultInstallVersion?.GetCatalogPackageMetadata();

                    apps.Add(new WinGetApp
                    {
                        Id = catalogPackage.Id,
                        Name = catalogPackage.Name,
                        LocalVersion = catalogPackage.InstalledVersion?.Version,
                        LatestVersion = catalogPackage.DefaultInstallVersion?.Version,
                        Installed = true,
                        ReleaseNotesUrl = packageMetadata?.ReleaseNotesUrl,
                        WebsiteUrl = packageMetadata?.PackageUrl,
                        CatalogPackage = catalogPackage,
                    });
                }

                return apps;
            });
        }

        public static Task<List<WinGetApp>> GetNonInstalledAppsBySearch(string searchQuery, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                var catalog = await GetAppCatalog();
                if (catalog == null) return [];

                var filter = PackageManagerFactory.CreatePackageMatchFilter();
                filter.Field = PackageMatchField.Name;
                filter.Option = PackageFieldMatchOption.ContainsCaseInsensitive;
                filter.Value = searchQuery;

                var findPackagesOptions = PackageManagerFactory.CreateFindPackagesOptions();
                findPackagesOptions.Filters.Add(filter);

                var result = await catalog.FindPackagesAsync(findPackagesOptions);
                if (cancellationToken.IsCancellationRequested) return [];

                var apps = await ConvertPackagesToApplications(result);
                if (cancellationToken.IsCancellationRequested) return [];

                apps.Sort((a, b) => a.Name.CompareTo(b.Name));
                return apps;
            });
        }
    }
}
