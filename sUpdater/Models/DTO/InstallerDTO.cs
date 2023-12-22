namespace sUpdater.Models.DTO
{
    public class InstallerDTO
    {
        public int DetectInfoId { get; private set; }
        public string DownloadLink { get; private set; }
        public string DownloadLinkParsed { get; private set; }
        public string LaunchArgs { get; private set; }
    }
}
