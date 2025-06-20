using Newtonsoft.Json;

namespace MicroservicesUser.Models.DTO
{
    public class APIResponseMetaData
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("errors")]
        public List<string>? Errors { get; set; }

    }
}
