using System;
using System.Collections.Generic;
using System.Linq;
using sUpdater.Controllers;
using sUpdater.Models;
using sUpdater.Models.Apps;
using sUpdater.Models.Settings;

namespace sUpdater.Helpers;

public static class UpdateHelper
{
    private static readonly Dictionary<AppType, Type> _appTypes = new()
    {
        { AppType.sUpdater, typeof(SUpdaterApp) },
        { AppType.WinGet, typeof(WinGetApp) }
    };
    
    public static bool IsIgnored(IApplication app)
    {
        var appType = app is SUpdaterApp ? AppType.sUpdater : AppType.WinGet;
                    
        var ignoredUpdate = Utilities.Settings.IgnoredUpdates
            .FirstOrDefault(i => i.Id == app.Id && i.Type == appType);

        if (ignoredUpdate == null) return false;
        
        if (ignoredUpdate.Version == null)
        {
            Log.Append($"Skipping {app.Name} - all updates ignored", Log.LogLevel.INFO);
            return true;
        }

        if (ignoredUpdate.Version == app.LatestVersion)
        {
            Log.Append($"Skipping {app.Name} version {app.LatestVersion} - ignored", Log.LogLevel.INFO);
            return true;
        }

        return false;
    }
    
    public static IApplication GetAppFromIgnoredUpdate(IgnoredUpdate ignoredUpdate)
    {
        var appType = _appTypes[ignoredUpdate.Type];
        return AppController.Apps.FirstOrDefault(x => x.Id == ignoredUpdate.Id && x.GetType() == appType);
    }
}