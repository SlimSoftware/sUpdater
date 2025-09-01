using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{
    public class ApplicationDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("noupdate")]
        public bool NoUpdate { get; set; }

        [JsonPropertyName("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonPropertyName("release_notes_url")]
        public string ReleaseNotesUrl { get; set; }

        [JsonPropertyName("detectinfo")]
        public DetectInfoDTO[] DetectInfo { get; set; }

        [JsonPropertyName("installers")]
        public InstallerDTO[] Installers { get; set; }
    }
}