using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RazorSharpGetaways.Data;

/// <summary>
/// The rows behind the Package Cost Sheet report (wwwroot/reports/Package Cost Sheet.xml): the service lines the agency pays its
/// suppliers for one package in one departure season, grouped by cost group and supplier, built in memory from
/// <see cref="TripPackages"/> and the same for the same choices every time. The report's own parameters are Package and Season.
/// When the Invoices page runs the sheet for a booked trip it also passes Pax, Nights, Quote and Request: the lines are then costed
/// for that party and scaled so the sheet comes to <see cref="CostShareOfQuote"/> of the price quoted to the client.
/// </summary>
public static class PackageCostSheet
{
    /// <summary>What the suppliers' costs come to as a share of the price quoted to the client.</summary>
    public const decimal CostShareOfQuote = 0.82m;

    /// <summary>Answers the report's SQL: a table with the columns the SELECT list asks for and one row per service line.</summary>
    public static DataTable Build(string sql, IDictionary<string, object>? parameters)
    {
        var table = EmptyTable(SelectAliases(sql));
        var package = TripPackages.ByName(Lookup(parameters, "Package")) ?? TripPackages.All[0];
        var season = Lookup(parameters, "Season") ?? TripPackages.Seasons[0];
        int pax = int.TryParse(Lookup(parameters, "Pax"), out var p) && p > 0 ? p : 2;
        int nights = int.TryParse(Lookup(parameters, "Nights"), out var n) && n > 0 ? n : package.Nights;
        decimal? quote = decimal.TryParse(Lookup(parameters, "Quote"), NumberStyles.Number, CultureInfo.InvariantCulture, out var q) && q > 0 ? q : null;
        var request = Lookup(parameters, "Request");

        // The same package and season always print the same sheet
        var rnd = new Random(HashSeed(package.Name, season));
        var city = package.City;
        int rooms = (pax + 1) / 2;   // two to a room

        // Prices move with the season and with how far the package is from home
        decimal seasonFactor = season switch { "Summer" => 1.25m, "Autumn" => 1.05m, "Winter" => 0.85m, _ => 1.0m };
        decimal flightFactor = package.Region switch
        {
            "East Asia" => 1.6m, "Southern Africa" => 1.8m, "North Africa" => 0.9m, "Northern Europe" => 1.1m,
            "North America" => 1.4m, "Oceania" => 1.9m, "South America" => 1.5m, _ => 1.0m
        };

        // Cost groups, each with its suppliers; each supplier with its services: code letter, description, quantity, unit, rate range, note
        var costGroups = new (string Code, string Desc, (string Supplier, (string Code, string Desc, int Qty, string Unit, int Low, int High, string Note)[] Services)[] Suppliers)[]
        {
            ("A", "Transport", new[]
            {
                ("Meridian Airways", new[]
                {
                    ("F", "Return flights, economy",            pax, "pax",  380, 620, "Non-refundable"),
                    ("F", "Checked baggage, 23 kg",             pax, "pax",   40,  60, ""),
                    ("F", "Seat selection, outbound and return", pax, "pax",   15,  30, ""),
                }),
                ($"{city} Airport Transfers", new[]
                {
                    ("T", "Airport to hotel, private car",      1, "trip",   45,  90, ""),
                    ("T", "Hotel to airport, private car",      1, "trip",   45,  90, ""),
                }),
                ($"{city} Rail & Coach", new[]
                {
                    ("T", "Day-trip coach with driver",         1, "day",   120, 260, "Net rate"),
                    ("T", "Regional rail pass, 3 days",       pax, "pax",    90, 180, ""),
                }),
            }),
            ("B", "Accommodation", new[]
            {
                ($"Hotel {city} Central", new[]
                {
                    ("H", "Double room with breakfast",    nights * rooms, rooms > 1 ? "room-night" : "night", 110, 260, "Commissionable 10%"),
                    ("H", "City tax",                      nights * pax,   "pax-night",   2,   5, "Paid locally"),
                }),
                ($"{city} Boutique Stays", new[]
                {
                    ("H", "Room upgrade, view side",       nights * rooms, rooms > 1 ? "room-night" : "night",  40,  90, ""),
                    ("H", "Late checkout on departure day",     1, "stay",   30,  60, ""),
                }),
            }),
            ("C", "Experiences", new[]
            {
                ($"{city} Walks", new[]
                {
                    ("E", "Old town walking tour",          pax, "pax",   25,  45, ""),
                    ("E", "Evening food tour",              pax, "pax",   65, 110, ""),
                }),
                ($"{city} Day Tours", new[]
                {
                    ("E", "Full-day excursion with lunch",  pax, "pax",   80, 160, ""),
                    ("E", "Entrance tickets",               pax, "pax",   18,  40, "Pre-booked"),
                }),
                ($"Taste of {city}", new[]
                {
                    ("E", "Cooking class with market visit", pax, "pax",   70, 120, ""),
                }),
            }),
            ("D", "Services", new[]
            {
                ("Guardian Travel Cover", new[]
                {
                    ("S", "Travel insurance, comprehensive", pax, "pax",   38,  75, ""),
                }),
                ($"{city} Guide Co-op", new[]
                {
                    ("S", "Private guide, full day",          2, "day",   180, 320, "Net rate"),
                }),
                ("Razor Sharp Getaways, head office", new[]
                {
                    ("S", "Booking fee",                      1, "booking", 45,  60, ""),
                    ("S", "Documents and delivery",           1, "booking", 12,  25, ""),
                }),
            }),
        };

        // Draw every rate first, so a quote can scale them all by one factor
        var lines = new List<(string Group, string GroupDesc, string Supplier, string Service, string ServiceDesc, int Qty, string Unit, decimal Rate, string Note)>();
        int lineNo = 1;
        foreach (var group in costGroups)
            foreach (var (supplier, services) in group.Suppliers)
                foreach (var svc in services)
                {
                    var factor = svc.Code == "F" ? flightFactor * seasonFactor : seasonFactor;
                    // Rates in cents times the factors, rounded back to two places, so every rate prints with two decimals
                    var rate = Math.Round(rnd.Next(svc.Low * 100, svc.High * 100 + 1) / 100m * factor, 2);
                    lines.Add((group.Code, group.Desc, supplier, $"{svc.Code}{lineNo:D3}", svc.Desc, svc.Qty, svc.Unit, rate, svc.Note));
                    lineNo++;
                }

        if (quote is decimal target)
        {
            var total = lines.Sum(l => l.Qty * l.Rate);
            if (total > 0)
            {
                var scale = target * CostShareOfQuote / total;
                lines = lines.Select(l => l with { Rate = Math.Round(l.Rate * scale, 2) }).ToList();
            }
        }

        var packageDesc = request is null ? $"{package.Name}, {season}" : $"{package.Name}, {season}, request {request}";
        foreach (var l in lines)
        {
            var row = table.NewRow();
            SetIfPresent(row, "PackageCostLines_Region",        package.Region);
            SetIfPresent(row, "PackageCostLines_Package",       package.Name);
            SetIfPresent(row, "PackageCostLines_Season",        season);
            SetIfPresent(row, "PackageCostLines_CostGroup",     l.Group);
            SetIfPresent(row, "PackageCostLines_CostGroupDesc", l.GroupDesc);
            SetIfPresent(row, "PackageCostLines_Supplier",      l.Supplier);
            SetIfPresent(row, "PackageCostLines_Service",       l.Service);
            SetIfPresent(row, "PackageCostLines_ServiceDesc",   l.ServiceDesc);
            SetIfPresent(row, "PackageCostLines_Qty",           (decimal)l.Qty);
            SetIfPresent(row, "PackageCostLines_Unit",          l.Unit);
            SetIfPresent(row, "PackageCostLines_Rate",          l.Rate);
            SetIfPresent(row, "PackageCostLines_Notes",         l.Note);
            // Formula columns: what the Crystal formulas would give, so the groups and sums agree with the design, except PackageDesc,
            // which adds the request key when the sheet is run for a booking
            SetIfPresent(row, "Formula_Amount",      l.Qty * l.Rate);
            SetIfPresent(row, "Formula_PackageDesc", packageDesc);
            SetIfPresent(row, "Formula_GroupDesc",   $"{l.Group} {l.GroupDesc}");
            table.Rows.Add(row);
        }
        return table;
    }

    /// <summary>A table with the SELECT list's columns and no rows, for SQL this app does not answer.</summary>
    public static DataTable Empty(string sql) => EmptyTable(SelectAliases(sql));

    private static DataTable EmptyTable(IEnumerable<string> aliases)
    {
        var table = new DataTable();
        foreach (var alias in aliases) table.Columns.Add(alias, typeof(object));
        return table;
    }

    private static void SetIfPresent(DataRow row, string column, object value)
    {
        if (row.Table.Columns.Contains(column)) row[column] = value;
    }

    private static string? Lookup(IDictionary<string, object>? parameters, string key) =>
        parameters?.FirstOrDefault(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)).Value?.ToString();

    /// <summary>Every "AS [alias]" or "AS alias" in the SELECT list, in order; the report's loader writes one per field and formula.</summary>
    private static List<string> SelectAliases(string sql)
    {
        var aliases = new List<string>();
        var select = Regex.Match(sql, @"SELECT\s+(.*?)\s+FROM\s", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!select.Success) return aliases;
        foreach (Match m in Regex.Matches(select.Groups[1].Value, @"AS\s+\[([^\]]+)\]", RegexOptions.IgnoreCase))
            aliases.Add(m.Groups[1].Value);
        foreach (Match m in Regex.Matches(select.Groups[1].Value, @"AS\s+([A-Za-z_][A-Za-z0-9_]*)", RegexOptions.IgnoreCase))
            if (!aliases.Contains(m.Groups[1].Value, StringComparer.OrdinalIgnoreCase)) aliases.Add(m.Groups[1].Value);
        return aliases.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static int HashSeed(params string[] parts)
    {
        unchecked
        {
            int h = 17;
            foreach (var part in parts)
                foreach (var c in part) h = h * 31 + c;
            return h;
        }
    }
}
