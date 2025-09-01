using sUpdater.Models.DTO;

namespace sUpdater.Models
{
    public enum ExtractMode { Single, Folder };

    public class Archive
    {
        public string DownloadLink { get; set; }
        public string DownloadLinkParsed { get; set; }
        public Arch Arch { get; set; }
        public ExtractMode ExtractMode { get; set; }
        public string LaunchFile { get; set; }

        public Archive(ArchiveDTO archiveDTO)
        {
            DownloadLink = archiveDTO.DownloadLink;
            DownloadLinkParsed = archiveDTO.DownloadLinkParsed;
            Arch = archiveDTO.Arch;
            ExtractMode = archiveDTO.ExtractMode;
            LaunchFile = archiveDTO.LaunchFile;
        }
    }
}
