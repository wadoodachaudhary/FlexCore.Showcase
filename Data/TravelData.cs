namespace FlexCore.Showcase.Data;

/// <summary>
/// Razor Sharp Getaways, the imagined tour operator: packages, destinations and seasons for the demo pages and the
/// grid data in DemoData and BenchDataStore. Package, supplier and service names are invented.
/// </summary>
public static class TravelData
{
    /// <summary>A packaged trip: region, nights and the city its suppliers are named after. Text inputs and the Items grid read them;
    /// the Cards carousel keeps its own copy of the six; the full twelve (one per destination) are RazorSharpGetaways.Data.TripPackages in the Quick Start
    /// project, used by the cost sheet report and the home window.</summary>
    public record Package(string Name, string Region, string City, int Nights);

    public static readonly Package[] Packages =
    {
        new("Lisbon Explorer",        "Southern Europe", "Lisbon",    7),
        new("Amalfi Coast Drive",     "Southern Europe", "Amalfi",    6),
        new("Kyoto Classic",          "East Asia",       "Kyoto",     8),
        new("Hoi An Lantern Trail",   "East Asia",       "Hoi An",    9),
        new("Cape Winelands",         "Southern Africa", "Cape Town", 10),
        new("Marrakech to the Dunes", "North Africa",    "Marrakech", 6),
    };

    /// <summary>Departure seasons.</summary>
    public static readonly string[] Seasons = { "Spring", "Summer", "Fall", "Winter" };

    /// <summary>Every destination, with region and country; the rate card covers all of them.</summary>
    public static readonly (string City, string Region, string Country)[] Destinations =
    {
        ("Lisbon", "Southern Europe", "Portugal"), ("Amalfi", "Southern Europe", "Italy"), ("Santorini", "Southern Europe", "Greece"),
        ("Kyoto", "East Asia", "Japan"), ("Hoi An", "East Asia", "Vietnam"),
        ("Cape Town", "Southern Africa", "South Africa"), ("Marrakech", "North Africa", "Morocco"),
        ("Reykjavik", "Northern Europe", "Iceland"), ("Vancouver", "North America", "Canada"), ("Banff", "North America", "Canada"),
        ("Queenstown", "Oceania", "New Zealand"), ("Patagonia", "South America", "Argentina"),
    };

}
