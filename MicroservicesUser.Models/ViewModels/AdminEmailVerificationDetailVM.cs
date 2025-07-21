using Newtonsoft.Json;

namespace MicroservicesUser.Models.ViewModels
{
    public class AdminEmailVerificationDetailVM
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
        public string? Deliverability { get; set; }

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
        public int DomainVelocity { get; set; }

        [JsonProperty("domainTrust")]
        public int DomainTrust { get; set; }

        [JsonProperty("userActivity")]
        public int UserActivity { get; set; }

        [JsonProperty("firstSeen")]
        public TimeInfoVM? FirstSeen { get; set; }

        [JsonProperty("domainAge")]
        public TimeInfoVM? DomainAge { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("spamTrapScore")]
        public string? SpamTrapScore { get; set; }

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

        [JsonProperty("aRecords")]
        public List<string>? ARecords { get; set; }
    }

    public class TimeInfoVM
    {
        [JsonProperty("human")]
        public string? Human { get; set; }

        [JsonProperty("iso")]
        public DateTime Iso { get; set; }
    }
}
