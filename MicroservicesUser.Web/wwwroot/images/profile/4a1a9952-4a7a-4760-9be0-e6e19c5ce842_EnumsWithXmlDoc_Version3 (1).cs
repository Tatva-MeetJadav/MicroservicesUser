using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Indicates the deliverability status of an email address.
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum DeliverabilityEnum
{
    /// <summary>
    /// The email address is highly deliverable.
    /// </summary>
    [EnumMember(Value = "high")]
    High = 1,

    /// <summary>
    /// The email address has medium deliverability.
    /// </summary>
    [EnumMember(Value = "medium")]
    Medium = 2,

    /// <summary>
    /// The email address has low deliverability.
    /// </summary>
    [EnumMember(Value = "low")]
    Low = 3
}

/// <summary>
/// Represents the velocity (activity level) of a domain.
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum DomainVelocityEnum
{
    /// <summary>
    /// The domain has high velocity.
    /// </summary>
    [EnumMember(Value = "high")]
    High = 1,

    /// <summary>
    /// The domain has medium velocity.
    /// </summary>
    [EnumMember(Value = "medium")]
    Medium = 2,

    /// <summary>
    /// The domain has low velocity.
    /// </summary>
    [EnumMember(Value = "low")]
    Low = 3,

    /// <summary>
    /// The domain has no activity.
    /// </summary>
    [EnumMember(Value = "none")]
    None = 4,

    /// <summary>
    /// Indicates a higher plan is required to view this information.
    /// </summary>
    [EnumMember(Value = "Enterprise Mini or higher required.")]
    Upgrade = 5
}

/// <summary>
/// Represents the trust level of a domain.
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum DomainTrustEnum
{
    /// <summary>
    /// The domain is trusted.
    /// </summary>
    [EnumMember(Value = "trusted")]
    Trusted = 1,

    /// <summary>
    /// The domain has a positive trust rating.
    /// </summary>
    [EnumMember(Value = "positive")]
    Positive = 2,

    /// <summary>
    /// The domain has a neutral trust rating.
    /// </summary>
    [EnumMember(Value = "neutral")]
    Neutral = 3,

    /// <summary>
    /// The domain is suspicious.
    /// </summary>
    [EnumMember(Value = "suspicious")]
    Suspicious = 4,

    /// <summary>
    /// The domain is malicious.
    /// </summary>
    [EnumMember(Value = "malicious")]
    Malicious = 5,

    /// <summary>
    /// The domain has not been rated.
    /// </summary>
    [EnumMember(Value = "notRated")]
    NotRated = 6,

    /// <summary>
    /// Indicates a higher plan is required to view this information.
    /// </summary>
    [EnumMember(Value = "Upgraded plan required.")]
    Upgrade = 7
}

/// <summary>
/// Represents the user activity level associated with an email address.
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum UserActivityEnum
{
    /// <summary>
    /// High user activity.
    /// </summary>
    [EnumMember(Value = "high")]
    High = 1,

    /// <summary>
    /// Medium user activity.
    /// </summary>
    [EnumMember(Value = "medium")]
    Medium = 2,

    /// <summary>
    /// Low user activity.
    /// </summary>
    [EnumMember(Value = "low")]
    Low = 3,

    /// <summary>
    /// No user activity.
    /// </summary>
    [EnumMember(Value = "none")]
    None = 4,

    /// <summary>
    /// Indicates a higher plan is required to view this information.
    /// </summary>
    [EnumMember(Value = "Enterprise L4+ required.")]
    Upgrade = 5
}

/// <summary>
/// Represents the spam trap score for an email address.
/// </summary>
[JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum SpamTrapScoreEnum
{
    /// <summary>
    /// High risk of being a spam trap.
    /// </summary>
    [EnumMember(Value = "high")]
    High = 1,

    /// <summary>
    /// Medium risk of being a spam trap.
    /// </summary>
    [EnumMember(Value = "medium")]
    Medium = 2,

    /// <summary>
    /// Low risk of being a spam trap.
    /// </summary>
    [EnumMember(Value = "low")]
    Low = 3,

    /// <summary>
    /// No risk of being a spam trap.
    /// </summary>
    [EnumMember(Value = "none")]
    None = 4
}