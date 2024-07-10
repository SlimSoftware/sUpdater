using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{
    public class InstallerDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("app_id")]
        public int AppId { get; set; }

        [JsonPropertyName("detectinfo_id")]
        public int DetectInfoId { get; set; }

        [JsonPropertyName("download_link")]
        public string DownloadLink { get; set; }

        [JsonPropertyName("download_link_raw")]
        public string DownloadLinkRaw { get; set; }

        [JsonPropertyName("launch_args")]
        public string LaunchArgs { get; set; }
    }
}
