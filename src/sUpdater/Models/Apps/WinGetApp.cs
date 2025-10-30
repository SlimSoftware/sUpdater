using Microsoft.Management.Deployment;
using sUpdater.Controllers;
using System;
using System.Threading.Tasks;

namespace sUpdater.Models.Apps
{
    public class WinGetApp : BaseApplication, IApplication
    {
        public string Id { get; init; }
        public CatalogPackage CatalogPackage { get; set; }


        public Task<bool> Download()
        {
            // WinGet COM API does not support only downloading, this is done automatically when the install function is executed
            return Task.FromResult(true);
        }

        public async Task<bool> Install()
        {
            var options = WinGetAppController.PackageManagerFactory.CreateInstallOptions();
            options.AcceptPackageAgreements = true;
            options.PackageInstallMode = PackageInstallMode.Silent;

            var operation = WinGetAppController.PackageManager.InstallPackageAsync(CatalogPackage, options);

            operation.Progress = (asyncOperation, prog) =>
            {
                if (prog.State == PackageInstallProgressState.Downloading)
                {
                    Progress = (int)(prog.DownloadProgress / 2.0);

                    double downloadedMB = prog.BytesDownloaded / 1024d / 1024d;
                    double totalMB = prog.BytesRequired / 1024d / 1024d;
                    Status = $"Downloading... {downloadedMB:0.0} MB / {totalMB:0.0} MB";
                }
                else
                {
                    Progress = 50 + (int)(prog.InstallationProgress / 2.0);
                    Status = $"Installing... {prog.InstallationProgress:0}%";
                }
            };

            var result = await operation;
            return result.Status == InstallResultStatus.Ok;
        }
    }
}
