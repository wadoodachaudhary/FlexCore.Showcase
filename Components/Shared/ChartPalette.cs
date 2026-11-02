namespace FlexCore.Showcase.Components.Shared;

// Chart colours (2026-10-31, Baaria): every chart on the site passes these as ColorPalette instead of ChartControl's built-in eight,
// so the charts wear the site's colours. The chart pages' C# tabs print the lines between the markers (FlexCore.Showcase.csproj
// embeds this file for AppSource.Region). The order was picked with a colour-blindness check: under protanopia and deuteranopia
// each colour stays clearly apart from the next (OKLab distance 16 or more), and all eight hold 3:1 against the white card.
// The logo's sky (#38bdf8) is left out because it falls to 2.1:1 on white.
// code-tab:chart-palette
/// <summary>Chart colours: the FlexCore logo's blue, green and indigo first, then five that go with them.</summary>
public static class ChartPalette
{
    public const string Blue = "#0284c7";
    public const string Green = "#059669";   // the logo's #10b981 one step darker, for contrast on white
    public const string Indigo = "#6366f1";
    public const string Amber = "#d97706";
    public const string Fuchsia = "#c026d3";
    public const string Rose = "#e11d48";
    public const string Violet = "#7c3aed";
    public const string Teal = "#0d9488";

    /// <summary>For ColorPalette: series, slices and stages take the colours in this order.</summary>
    public static readonly string[] Site = [Blue, Green, Indigo, Amber, Fuchsia, Rose, Violet, Teal];

    /// <summary>For amounts on a map: one hue, light to dark, through the logo's sky and blue; each step a visible lightness apart.</summary>
    public static readonly string[] Scale = ["#bae6fd", "#7dd3fc", "#38bdf8", Blue, "#0369a1", "#0c4a6e"];
}
// /code-tab
