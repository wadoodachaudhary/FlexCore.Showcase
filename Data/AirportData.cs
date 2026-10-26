using System.Globalization;

namespace FlexCore.Showcase.Data;

/// <summary>One airport with scheduled passenger service, from the OurAirports snapshot (Data/OurAirports, public domain).</summary>
public sealed record Airport(
    string Code, string Icao, string Name, string Type, string City, string CountryCode, string Country,
    string Continent, string Region, double Latitude, double Longitude, int? ElevationFt, string Wikipedia,
    int Runways, int? LongestRunwayFt)
{
    /// <summary>Code and city, e.g. "LIS · Lisbon": the text a closed lookup shows.</summary>
    public string Label => $"{Code} · {City}";
}

/// <summary>One runway of an <see cref="Airport"/>; <see cref="Surface"/> is a plain group, <see cref="SurfaceCode"/> the source's code.</summary>
public sealed record Runway(
    string AirportCode, string Name, int? LengthFt, int? WidthFt, string Surface, string SurfaceCode, bool Lighted, bool Closed);

/// <summary>
/// The airport examples' data (2026-10-26): every airport with an IATA code and scheduled service in the OurAirports dump, with its
/// runways, built by Data/OurAirports/build-snapshot.py and embedded in the assembly. Read-only and immutable, so one copy is shared
/// by every visitor; a page that edits rows copies them first.
/// </summary>
public static class AirportData
{
    public const string Source = "OurAirports";
    public const string SourceUrl = "https://ourairports.com/data/";
    public const string SnapshotDate = "26 October 2026";

    public static readonly IReadOnlyDictionary<string, string> Continents = new Dictionary<string, string>
    {
        ["AF"] = "Africa", ["AN"] = "Antarctica", ["AS"] = "Asia", ["EU"] = "Europe",
        ["NA"] = "North America", ["OC"] = "Oceania", ["SA"] = "South America",
    };

    private static readonly Lazy<(IReadOnlyList<Airport> Airports, IReadOnlyList<Runway> Runways)> Loaded = new(Load);

    public static IReadOnlyList<Airport> Airports => Loaded.Value.Airports;
    public static IReadOnlyList<Runway> Runways => Loaded.Value.Runways;

    public static IEnumerable<Runway> RunwaysOf(string airportCode) =>
        Runways.Where(r => r.AirportCode == airportCode);

    private static (IReadOnlyList<Airport>, IReadOnlyList<Runway>) Load()
    {
        var runways = Rows("runways.tsv").Select(c => new Runway(
            c[0], c[1], Int(c[2]), Int(c[3]), c[4], c[5], c[6] == "1", c[7] == "1")).ToList();
        var open = runways.Where(r => !r.Closed).ToLookup(r => r.AirportCode);
        var airports = Rows("airports.tsv").Select(c => new Airport(
            c[0], c[1], c[2], c[3], c[4], c[5], c[6], Continents.GetValueOrDefault(c[7], c[7]), c[8],
            double.Parse(c[9], CultureInfo.InvariantCulture), double.Parse(c[10], CultureInfo.InvariantCulture), Int(c[11]), c[12],
            open[c[0]].Count(), open[c[0]].Max(r => r.LengthFt))).ToList();
        return (airports, runways);
    }

    // Tab-separated, one header row after the "#" comment line; values never contain tabs (the build script strips them).
    private static IEnumerable<string[]> Rows(string file)
    {
        using var stream = typeof(AirportData).Assembly.GetManifestResourceStream($"FlexCore.Showcase.Data.OurAirports.{file}")
            ?? throw new InvalidOperationException($"Embedded airport data {file} is missing.");
        using var reader = new StreamReader(stream);
        var header = true;
        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith('#')) continue;
            if (header) { header = false; continue; }
            yield return line.Split('\t');
        }
    }

    private static int? Int(string value) =>
        int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null;
}
