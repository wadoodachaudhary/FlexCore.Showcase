namespace FlexCore.Showcase.Components.Shared;

/// <summary>Column set an <see cref="ApiTable"/> renders.</summary>
public enum ApiKind
{
    Parameters,   // Parameter | Type | Default | Description
    Properties,   // Property | Type | Default | Description (model classes such as SplitButtonItem)
    Events,       // Event | Payload | Description
    Methods,      // Method | Returns | Description
    CssClasses,   // Class | Effect
    CssVariables, // Variable | Fallback | Purpose
    Keyboard      // Key | Action
}

/// <summary>One vocabulary row. <paramref name="Description"/> may hold inline HTML such as &lt;code&gt;.</summary>
public sealed record ApiRow(string Name, string Description, string? Type = null, string? Default = null);
