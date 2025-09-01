using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{
    public class PortableAppDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("arch")]
        public Arch Arch { get; set; }

        [JsonPropertyName("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonPropertyName("release_notes_url")]
        public string ReleaseNotesUrl { get; set; }

        [JsonPropertyName("archives")]
        public ArchiveDTO[] Archives { get; set; }
    }
}