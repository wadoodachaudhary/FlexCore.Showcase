namespace RazorSharpGetaways.Components.Getaways;

/// <summary>A page of the app: its key, menu text and icon, the title in the top bar, and its route.</summary>
public sealed record GetawaysPage(string Key, string Nav, string Icon, string Title, string Route);

/// <summary>
/// What both hosts of the app window share: its name, its pages and the profile menu. Razor Sharp Getaways routes each page;
/// the FlexCore site's home page switches between them inside its demo frame.
/// </summary>
public static class GetawaysPages
{
    public const string AppName = "Razor Sharp Getaways";
    public const string AppVersion = "1.0";

    public static readonly GetawaysPage[] All =
    [
        new("dashboard", "Dashboard", "bi bi-speedometer2", "Dashboard", ""),
        new("requests", "Trip Requests", "bi bi-table", "Razor Sharp Trip Requests", "requests"),
        new("itinerary", "Itinerary", "bi bi-calendar-heart", "Trip Itinerary", "itinerary"),
        new("invoices", "Invoices", "bi bi-file-earmark-spreadsheet", "Invoices", "invoices"),
        new("settings", "Settings", "bi bi-gear", "Settings", "settings"),
    ];

    public static GetawaysPage Find(string key) => All.FirstOrDefault(p => p.Key == key) ?? All[0];

    public static int IndexOf(string key) => Math.Max(0, Array.FindIndex(All, p => p.Key == key));

    /// <summary>The profile menu's entries; each answers with a toast in this app.</summary>
    public static readonly (string Text, string Icon)[] ProfileMenu =
    [
        ("My profile", "bi bi-person-circle"), ("Account settings", "bi bi-person-gear"),
        ("Notifications", "bi bi-bell"), ("Sign out", "bi bi-box-arrow-right"),
    ];

    /// <summary>The FlexCore version the app runs on, read from the assembly for the menu's footer.</summary>
    public static string FlexCoreVersion { get; } =
        typeof(Fx.ControlKit.ButtonControl).Assembly.GetName().Version is { } v ? $"{v.Major}.{v.Minor}.{v.Build}" : "";
}
