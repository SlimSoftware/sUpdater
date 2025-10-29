using sUpdater.Commands;
using System.Threading.Tasks;
using System.Windows.Media;

namespace sUpdater.Models.Apps
{
    public interface IApplication
    {
        string Name { get; set; }
        ImageSource Icon { get; set; }
        string LatestVersion { get; set; }
        string LocalVersion { get; set; }
        bool Installed { get; set; }
        string DisplayedVersion { get; set; }
        bool Checkbox { get; set; }
        string SavePath { get; set; }
        int Progress { get; set; }
        string Status { get; set; }
        bool IsWaiting { get; set; }
        LinkClickCommand LinkClickCommand { get; set; }
        string LinkText { get; set; }

        Task<bool> Download();
        Task<bool> Install();

        IApplication Clone();
    }
}
