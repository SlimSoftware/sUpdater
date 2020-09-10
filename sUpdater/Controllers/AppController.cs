using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace sUpdater.Controllers
{
    public static class AppController
    {
        public static async Task<Application> GetDetectInfo()
        {
            using (var response = await Utilities.HttpClient.GetAsync("apps/?f=id,arch,version,regkey,regvalue,exe_path"))
            {
                if (response.IsSuccessStatusCode)
                {
                    Application app = await response.Content.ReadAsAsync<Application>();
                    return app;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
