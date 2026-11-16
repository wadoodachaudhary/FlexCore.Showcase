namespace FlexCore.Showcase.Components.Shared;

/// <summary>Column set an <see cref="ApiTable"/> renders.</summary>
public enum ApiKind
{
    Parameters,   // Parameter | Type | Default | Description
    Properties,   // Property | Type | Default | Description (model classes such as SplitButtonItem)
    Events,       // Event | Payload | Description
    Methods,      // Method | Inputs | Returns | Description
    CssClasses,   // Class | Effect
    CssVariables, // Variable | Fallback | Purpose
    Keyboard,     // Key | Action
    Values        // Value | Description (enum members, 2026-10-20; were Properties rows with empty Type and a dash Default)
}

/// <summary>One documentation row. <paramref name="Description"/> may hold inline HTML such as &lt;code&gt;.
/// <see cref="ApiKind.Methods"/> rows show <c>Name()</c> with parameters in an Inputs column, from <paramref name="Inputs"/> or
/// split out of a <paramref name="Name"/> written as <c>Name(type a, type b)</c>.</summary>
public sealed record ApiRow(string Name, string Description, string? Type = null, string? Default = null, string? Inputs = null);
