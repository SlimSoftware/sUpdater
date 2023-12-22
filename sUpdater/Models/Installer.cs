using sUpdater.Models.DTO;

namespace sUpdater.Models
{
    public class Installer
    {
        public string DownloadLink { get; private set; }
        public string DownloadLinkParsed { get; private set; }
        public string LaunchArgs { get; private set; }

        public Installer(InstallerDTO installerDTO)
        {
            DownloadLink = installerDTO.DownloadLink;
            DownloadLinkParsed = installerDTO.DownloadLinkParsed;
            LaunchArgs = installerDTO.LaunchArgs;
        }
    }
}
