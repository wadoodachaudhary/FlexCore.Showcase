namespace GovAiAcademy.Services;

public enum AcademyAuthMode
{
    DevelopmentMock,
    EntraId
}

/// <summary>
/// Auth mode is configuration, not a secret. Client secrets stay in App Service settings or Key Vault.
/// </summary>
public sealed class AcademyOptions
{
    public const string SectionName = "Authentication";

    public AcademyAuthMode Mode { get; set; } = AcademyAuthMode.DevelopmentMock;

    public EntraOptions Entra { get; set; } = new();

    public bool PersonaSignInAllowed => Mode == AcademyAuthMode.DevelopmentMock;
}

public sealed class EntraOptions
{
    /// <summary>
    /// National-cloud authority. Azure Government tenants use https://login.microsoftonline.us/.
    /// Commercial tenants use https://login.microsoftonline.com/. Confirm with the tenant admin.
    /// </summary>
    public string Instance { get; set; } = "https://login.microsoftonline.us/";

    public string TenantId { get; set; } = "";

    public string ClientId { get; set; } = "";

    public string CallbackPath { get; set; } = "/signin-oidc";

    public string SignedOutCallbackPath { get; set; } = "/signout-callback-oidc";

    public bool HasRegistration =>
        !string.IsNullOrWhiteSpace(TenantId) && !string.IsNullOrWhiteSpace(ClientId);
}
