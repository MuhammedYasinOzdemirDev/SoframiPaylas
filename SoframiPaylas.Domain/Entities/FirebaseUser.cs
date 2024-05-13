using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace SoframiPaylas.Domain.Entities;
public class FirebaseUser
{
    [JsonProperty("localId")]
    public string localId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("emailVerified")]
    public bool EmailVerified { get; set; }

    [JsonProperty("passwordUpdatedAt")]
    public long PasswordUpdatedAt { get; set; }

    [JsonProperty("providerUserInfo")]
    public List<ProviderUserInfo> ProviderUserInfo { get; set; }

    [JsonProperty("validSince")]
    public string ValidSince { get; set; }

    [JsonProperty("disabled")]
    public bool Disabled { get; set; }

    [JsonProperty("lastLoginAt")]
    public string LastLoginAt { get; set; }

    [JsonProperty("createdAt")]
    public long CreatedAt { get; set; }

    [JsonProperty("customAttributes")]
    public string CustomAttributes { get; set; }

    [JsonProperty("lastRefreshAt")]
    public DateTime LastRefreshAt { get; set; }
}

public class ProviderUserInfo
{
    [JsonProperty("providerId")]
    public string ProviderId { get; set; }

    [JsonProperty("displayName")]
    public string ProviderDisplayName { get; set; }

    [JsonProperty("federatedId")]
    public string FederatedId { get; set; }

    [JsonProperty("email")]
    public string ProviderEmail { get; set; }

    [JsonProperty("rawId")]
    public string RawId { get; set; }
}
