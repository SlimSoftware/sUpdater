using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{
    public class InstallerDTO
    {
        [JsonPropertyName("detectinfo_id")]
        public int DetectInfoId { get; set; }

        [JsonPropertyName("download_link")]
        public string DownloadLink { get; set; }

        [JsonPropertyName("download_link_parsed")]
        public string DownloadLinkParsed { get; set; }

        [JsonPropertyName("launch_args")]
        public string LaunchArgs { get; set; }
    }
}
