using sUpdater.Models;
using sUpdater.Models.DTO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace sUpdater.Controllers
{
    public static class AppUpdateController
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<AppUpdateInfo> GetAppUpdateInfo()
        {
            ApplicationDTO appDTO = await httpClient.GetFromJsonAsync<ApplicationDTO>("https://www.slimsoftware.dev/supdater/update.json");
            Application app = new Application(appDTO, null, appDTO.Installers.First());
            app.LocalVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            AppUpdateInfo appUpdateInfo = new AppUpdateInfo()
            {
                UpdateAvailable = Utilities.UpdateAvailable(app.LatestVersion, app.LocalVersion),
                App = app
            };

            if (appUpdateInfo.UpdateAvailable)
            {
                appUpdateInfo.ChangelogRawText = await httpClient.GetStringAsync(app.ReleaseNotesUrl);
            }

            return appUpdateInfo;
        }
    }
}
