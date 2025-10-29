using Microsoft.Management.Deployment;
using sUpdater.Controllers;
using System;
using System.Threading.Tasks;

namespace sUpdater.Models.Apps
{
    public class WinGetApp : BaseApplication, IApplication
    {
        public CatalogPackage CatalogPackage { get; set; }

        //public async Task<bool> Download()
        //{
        //    var options = new DownloadOptions() { AcceptPackageAgreements = true };
        //    var operation = WinGetAppController.PackageManager.DownloadPackageAsync(CatalogPackage, options);



        //    var result = await operation;
        //    return result.Status == DownloadResultStatus.Ok;
        //}

        public Task<bool> Download()
        {
            // WinGet COM API does not support only downloading, this is done automatically when the install function is executed
            return Task.FromResult(true);
        }

        public async Task<bool> Install()
        {
            var options = new InstallOptions() { AcceptPackageAgreements = true };
            var operation = WinGetAppController.PackageManager.InstallPackageAsync(CatalogPackage, options);

            Progress = 1; // Make sure the progress bar is always visible

            operation.Progress = (asyncOperation, prog) =>
            {
                if (prog.State == PackageInstallProgressState.Downloading)
                {
                    Progress = (int)prog.DownloadProgress / 2;

                    double recievedSize = Math.Round(prog.BytesDownloaded / 1024d / 1024d, 1);
                    double totalSize = Math.Round(prog.BytesRequired / 1024d / 1024d, 1);

                    Status = string.Format("Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                }
                else
                {
                    Progress = ((int)prog.InstallationProgress / 2) + 50;
                    Status = $"Installing... {Math.Round(prog.InstallationProgress, 0)}%";
                }
            };

            var result = await operation;
            return result.Status == InstallResultStatus.Ok;
        }
    }
}
