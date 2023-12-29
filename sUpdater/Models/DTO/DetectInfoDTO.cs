using System.Text.Json.Serialization;

namespace sUpdater.Models.DTO
{
    public class DetectInfoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("app_id")]
        public int AppId { get; set; }

        [JsonPropertyName("arch")]
        public Arch Arch { get; set; }

        [JsonPropertyName("reg_key")]
        public string RegKey { get; set; }

        [JsonPropertyName("reg_value")]
        public string RegValue { get; set; }

        [JsonPropertyName("exe_path")]
        public string ExePath { get; set; }
    }
}
