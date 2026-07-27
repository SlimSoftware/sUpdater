using Microsoft.Management.Deployment;
using sUpdater.Controllers;
using sUpdater.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace sUpdater.Models.Apps
{
    public class WinGetApp : BaseApplication, IApplication
    {
        public string Id { get; init; }
        public CatalogPackage CatalogPackage { get; set; }

        private bool _iconLoaded = false;

        public async Task LoadIconAsync()
        {
            if (_iconLoaded || Icon != null) return;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Icon = IconHelper.GetIconFromPackageId(CatalogPackage.InstalledVersion?.Id);
                _iconLoaded = true;
            });
        }

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
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (prog.State == PackageInstallProgressState.Downloading)
                    {
                        Progress = (int)Math.Round(prog.DownloadProgress * 100, 0);

                        double recievedSize = Math.Round(prog.BytesDownloaded / 1024d / 1024d, 1);
                        double totalSize = Math.Round(prog.BytesRequired / 1024d / 1024d, 1);

                        if (Progress > 0)
                        {
                            Status = string.Format("Downloading... {0:0.0} MB/{1:0.0} MB", recievedSize, totalSize);
                            IsWaiting = false;
                        }
                        else
                        {
                            Status = "Downloading...";
                            IsWaiting = true;
                        }
                    }
                    else
                    {
                        switch (prog.State)
                        {
                            case PackageInstallProgressState.Queued:
                                Status = "Waiting for install";
                                break;
                            case PackageInstallProgressState.Installing:
                                Status = "Installing...";
                                break;
                            case PackageInstallProgressState.PostInstall:
                                Status = "Finishing install...";
                                break;
                            case PackageInstallProgressState.Finished:
                                Status = "Installed";
                                IsWaiting = false;
                                return;
                            default:
                                break;
                        }

                        IsWaiting = true;
                    }
                });
            };

            var result = await operation;

            return result.Status == InstallResultStatus.Ok;
        }
    }
}
