using Newtonsoft.Json;

namespace MicroservicesUser.Models.DTO
{
    public class ProxyAndVpnDetectionResponseDTO
    {

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("proxy")]
        public bool Proxy { get; set; }

        [JsonProperty("ISP")]
        public string? ISP { get; set; }

        [JsonProperty("organization")]
        public string? Organization { get; set; }

        [JsonProperty("ASN")]
        public int ASN { get; set; }

        [JsonProperty("host")]
        public string? Host { get; set; }

        [JsonProperty("countryCode")]
        public string? CountryCode { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }

        [JsonProperty("isCrawler")]
        public bool IsCrawler { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("zipCode")]
        public string? ZipCode { get; set; }

        [JsonProperty("timezone")]
        public string? Timezone { get; set; }

        [JsonProperty("vpn")]
        public bool Vpn { get; set; }

        [JsonProperty("tor")]
        public bool Tor { get; set; }

        [JsonProperty("activeVpn")]
        public bool ActiveVpn { get; set; }

        [JsonProperty("activeTor")]
        public bool ActiveTor { get; set; }

        [JsonProperty("recentAbuse")]
        public bool RecentAbuse { get; set; }

        [JsonProperty("botStatus")]
        public bool BotStatus { get; set; }

        [JsonProperty("mobile")]
        public bool Mobile { get; set; }

        [JsonProperty("fraudScore")]
        public int FraudScore { get; set; }

    }
}
