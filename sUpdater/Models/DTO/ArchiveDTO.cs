using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{ 
    public class ArchiveDTO
    {
        public string DownloadLink { get; set; }

        [JsonPropertyName("download_link")] // download_link_parsed
        public string DownloadLinkParsed { get; set; }

        [JsonPropertyName("arch")]
        public Arch Arch { get; set; }

        [JsonPropertyName("extract_mode")]
        public ExtractMode ExtractMode { get; set; }

        [JsonPropertyName("launch_file")]
        public string LaunchFile { get; set; }
    }
}
