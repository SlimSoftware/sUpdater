using Microsoft.Management.Deployment;
using sUpdater.Helpers;
using sUpdater.Models.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Task<List<WinGetApp>> GetInstalledApps()
        {
            return Task.Run(async () =>
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

                foreach (var matchResult in packagesResult.Matches.ToArray())
                {
                    var catalogPackage = matchResult.CatalogPackage;
                    var packageMetadata = catalogPackage.DefaultInstallVersion?.GetCatalogPackageMetadata();

                    apps.Add(new WinGetApp
                    {
                        Id = catalogPackage.Id,
                        Name = catalogPackage.Name,
                        LocalVersion = catalogPackage.InstalledVersion?.Version,
                        LatestVersion = catalogPackage.DefaultInstallVersion?.Version,
                        Icon = IconHelper.GetIconFromPackageId(catalogPackage.InstalledVersion?.Id),
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
                var remoteCatalogRef = PackageManager.GetPredefinedPackageCatalog(PredefinedPackageCatalog.OpenWindowsCatalog);

                var connectResult = await remoteCatalogRef.ConnectAsync();
                if (connectResult.Status != ConnectResultStatus.Ok) return [];
                if (cancellationToken.IsCancellationRequested) return [];

                var filter = PackageManagerFactory.CreatePackageMatchFilter();
                filter.Field = PackageMatchField.Name;
                filter.Option = PackageFieldMatchOption.ContainsCaseInsensitive;
                filter.Value = searchQuery;

                var findPackagesOptions = PackageManagerFactory.CreateFindPackagesOptions();
                findPackagesOptions.Filters.Add(filter);

                var result = await connectResult.PackageCatalog.FindPackagesAsync(findPackagesOptions);
                if (cancellationToken.IsCancellationRequested) return [];

                var apps = await ConvertPackagesToApplications(result);
                if (cancellationToken.IsCancellationRequested) return [];

                return apps;
            });
        }
    }
}
