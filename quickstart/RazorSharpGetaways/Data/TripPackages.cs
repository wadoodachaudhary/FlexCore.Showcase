namespace RazorSharpGetaways.Data;

/// <summary>A packaged trip the agency sells: where it goes, the city its suppliers are named after and how long it is.</summary>
public sealed record TripPackage(string Name, string Region, string Destination, string City, int Nights);

/// <summary>
/// The agency's packages, one for each destination its trip requests name, and the departure seasons its cost sheets price them
/// by. The Invoices page picks a booked trip's package by its destination and its season by its departure date.
/// </summary>
public static class TripPackages
{
    public static readonly TripPackage[] All =
    [
        new("Lisbon Explorer",           "Southern Europe", "Lisbon",       "Lisbon",     7),
        new("Amalfi Coast Drive",        "Southern Europe", "Amalfi Coast", "Amalfi",     6),
        new("Kyoto Classic",             "East Asia",       "Kyoto",        "Kyoto",      8),
        new("Hoi An Lantern Trail",      "East Asia",       "Hoi An",       "Hoi An",     9),
        new("Cape Winelands",            "Southern Africa", "Cape Town",    "Cape Town", 10),
        new("Marrakech to the Dunes",    "North Africa",    "Marrakech",    "Marrakech",  6),
        new("Santorini Blue Domes",      "Southern Europe", "Santorini",    "Santorini",  7),
        new("Reykjavik Northern Lights", "Northern Europe", "Reykjavik",    "Reykjavik",  5),
        new("Vancouver and the Islands", "North America",   "Vancouver",    "Vancouver",  8),
        new("Banff Mountain Week",       "North America",   "Banff",        "Banff",      7),
        new("Queenstown Adventure",      "Oceania",         "Queenstown",   "Queenstown", 9),
        new("Patagonia Trek",            "South America",   "Patagonia",    "Patagonia", 10),
    ];

    /// <summary>Departure seasons; a cost sheet prices a package per season.</summary>
    public static readonly string[] Seasons = ["Spring", "Summer", "Autumn", "Winter"];

    /// <summary>The package sold for a destination; the first package when the destination is not on the list.</summary>
    public static TripPackage ForDestination(string destination) =>
        All.FirstOrDefault(p => p.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)) ?? All[0];

    public static TripPackage? ByName(string? name) =>
        name is null ? null : All.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>The season a departure falls in: March to May is spring, June to August summer, and so on.</summary>
    public static string SeasonOf(DateTime departure) => departure.Month switch
    {
        3 or 4 or 5 => "Spring",
        6 or 7 or 8 => "Summer",
        9 or 10 or 11 => "Autumn",
        _ => "Winter"
    };
}
