using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace MicroservicesUser.Models.ViewModels.EmailVerification
{
    public class EmailVerificationResponseVM
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("timedOut")]
        public bool TimedOut { get; set; }

        [JsonProperty("disposable")]
        public bool Disposable { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("deliverability")]
        public DeliverabilityEnum Deliverability { get; set; }

        [JsonProperty("smtpScore")]
        public int SmtpScore { get; set; }

        [JsonProperty("overallScore")]
        public int OverallScore { get; set; }

        [JsonProperty("catchAll")]
        public bool CatchAll { get; set; }

        [JsonProperty("generic")]
        public bool Generic { get; set; }

        [JsonProperty("common")]
        public bool Common { get; set; }

        [JsonProperty("dnsValid")]
        public bool DnsValid { get; set; }

        [JsonProperty("honeypot")]
        public bool Honeypot { get; set; }

        [JsonProperty("frequentComplainer")]
        public bool FrequentComplainer { get; set; }

        [JsonProperty("suspect")]
        public bool Suspect { get; set; }

        [JsonProperty("recentAbuse")]
        public bool RecentAbuse { get; set; }

        [JsonProperty("fraudScore")]
        public int FraudScore { get; set; }

        [JsonProperty("leaked")]
        public bool Leaked { get; set; }

        [JsonProperty("suggestedDomain")]
        public string? SuggestedDomain { get; set; }

        [JsonProperty("domainVelocity")]
        public DomainVelocityEnum? DomainVelocity { get; set; }

        [JsonProperty("domainTrust")]
        public DomainTrustEnum? DomainTrust { get; set; }

        [JsonProperty("userActivity")]
        public UserActivityEnum? UserActivity { get; set; }

        [JsonProperty("associatedNames")]
        public AssociatedNamesDTO? AssociatedNames { get; set; }

        [JsonProperty("associatedPhoneNumbers")]
        public AssociatedPhoneNumbersDTO? AssociatedPhoneNumbers { get; set; }

        [JsonProperty("firstSeen")]
        public TimeInfoDTO? FirstSeen { get; set; }

        [JsonProperty("domainAge")]
        public TimeInfoDTO? DomainAge { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("spamTrapScore")]
        public SpamTrapScoreEnum? SpamTrapScore { get; set; }

        [JsonProperty("riskyTld")]
        public bool RiskyTld { get; set; }

        [JsonProperty("spfRecord")]
        public bool SpfRecord { get; set; }

        [JsonProperty("dmarcRecord")]
        public bool DmarcRecord { get; set; }

        [JsonProperty("sanitizedEmail")]
        public string? SanitizedEmail { get; set; }

        [JsonProperty("mxRecords")]
        public List<string>? MxRecords { get; set; }

        [JsonProperty("requestId")]
        public string? RequestId { get; set; }

        [JsonProperty("aRecords")]
        public List<string>? ARecords { get; set; }
    }

    public class AssociatedNamesDTO
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("names")]
        public List<string>? Names { get; set; }
    }

    public class AssociatedPhoneNumbersDTO
    {
        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("phoneNumbers")]
        public List<string>? PhoneNumbers { get; set; }
    }

    public class TimeInfoDTO
    {
        [JsonProperty("human")]
        public string? Human { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("iso")]
        public string? Iso { get; set; }
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DeliverabilityEnum
    {
        [EnumMember(Value = "high")]
        High = 1,

        [EnumMember(Value = "medium")]
        Medium = 2,

        [EnumMember(Value = "low")]
        Low = 3
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DomainVelocityEnum
    {
        [EnumMember(Value = "high")]
        High = 1,

        [EnumMember(Value = "medium")]
        Medium = 2,

        [EnumMember(Value = "low")]
        Low = 3,

        [EnumMember(Value = "none")]
        None = 4,

        [EnumMember(Value = "Enterprise Mini or higher required.")]
        Upgrade = 5
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DomainTrustEnum
    {
        [EnumMember(Value = "trusted")]
        Trusted = 1,

        [EnumMember(Value = "positive")]
        Positive = 2,

        [EnumMember(Value = "neutral")]
        Neutral = 3,

        [EnumMember(Value = "suspicious")]
        Suspicious = 4,

        [EnumMember(Value = "malicious")]
        Malicious = 5,

        [EnumMember(Value = "notRated")]
        NotRated = 6,

        [EnumMember(Value = "Upgraded plan required.")]
        Upgrade = 7
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum UserActivityEnum
    {
        [EnumMember(Value = "high")]
        High = 1,

        [EnumMember(Value = "medium")]
        Medium = 2,

        [EnumMember(Value = "low")]
        Low = 3,

        [EnumMember(Value = "none")]
        None = 4,

        [EnumMember(Value = "Enterprise L4+ required.")]
        Upgrade = 5
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum SpamTrapScoreEnum
    {
        [EnumMember(Value = "high")]
        High = 1,

        [EnumMember(Value = "medium")]
        Medium = 2,

        [EnumMember(Value = "low")]
        Low = 3,

        [EnumMember(Value = "none")]
        None = 4
    }
}
