namespace KCSCommunity.Access.Security.ApiSignature;

public class ApiSignatureSettings
{
    public const string SectionName = "ApiSignature";
    public bool Enabled { get; set; }
    public int TimeWindowSeconds { get; set; }
    public ApiKeySecretPair[] ApiKeys { get; set; } = Array.Empty<ApiKeySecretPair>();
}

public class ApiKeySecretPair
{
    public string Key { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}