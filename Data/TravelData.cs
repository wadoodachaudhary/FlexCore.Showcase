namespace FlexCore.Showcase.Data;

/// <summary>
/// FlexCore Travel Agency, the imagined tour operator behind the home page's quote desk and the Report writer page's package cost sheet.
/// Package, supplier and service names are invented; no real airline, hotel or company is named.
/// </summary>
public static class TravelData
{
    /// <summary>A packaged trip the operator sells: where it goes, how long it is and which city its suppliers are named after.</summary>
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

    /// <summary>Departure seasons; the cost sheet prices a package per season.</summary>
    public static readonly string[] Seasons = { "Spring", "Summer", "Autumn", "Winter" };

    /// <summary>Every destination the operator sells, with its region and country. The rate card and the supplier list cover all of them.</summary>
    public static readonly (string City, string Region, string Country)[] Destinations =
    {
        ("Lisbon", "Southern Europe", "Portugal"), ("Amalfi", "Southern Europe", "Italy"), ("Santorini", "Southern Europe", "Greece"),
        ("Kyoto", "East Asia", "Japan"), ("Hoi An", "East Asia", "Vietnam"),
        ("Cape Town", "Southern Africa", "South Africa"), ("Marrakech", "North Africa", "Morocco"),
        ("Reykjavik", "Northern Europe", "Iceland"), ("Vancouver", "North America", "Canada"), ("Banff", "North America", "Canada"),
        ("Queenstown", "Oceania", "New Zealand"), ("Patagonia", "South America", "Argentina"),
    };

    /// <summary>The four cost categories on every cost sheet and rate card.</summary>
    public static readonly string[] Categories = { "Transport", "Accommodation", "Experiences", "Services" };
}
