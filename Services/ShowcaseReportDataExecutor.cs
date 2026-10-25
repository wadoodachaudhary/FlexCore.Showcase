using System.Data;
using System.Text.RegularExpressions;
using FlexCore.Showcase.Data;
using Fx.ControlKit.Reports;
using Microsoft.Extensions.Logging;

namespace FlexCore.Showcase.Services;

/// <summary>
/// Showcase stand-in for <see cref="IReportDataExecutor"/>.
/// Instead of hitting a real database, this executor inspects the column
/// aliases in the SELECT list and synthesizes rows from
/// <see cref="TravelData"/>. That lets the showcase exercise the full XML →
/// SQL → HTML report pipeline (parameters, grouping, sums, page-break logic)
/// with zero database wiring.
///
/// <para>Adding more reports: dispatch on a fingerprint of the SQL (a table
/// name, a unique formula alias, etc.) inside <see cref="Execute"/> and emit a
/// builder per fingerprint.</para>
/// </summary>
public class ShowcaseReportDataExecutor : IReportDataExecutor
{
    private readonly ILogger<ShowcaseReportDataExecutor> _logger;

    public ShowcaseReportDataExecutor(ILogger<ShowcaseReportDataExecutor> logger)
    {
        _logger = logger;
    }

    public DataTable Execute(string sql, IDictionary<string, object>? parameters)
    {
        _logger.LogInformation("Showcase report SQL:\n{Sql}\nParams: {Params}",
            sql, parameters == null ? "(none)" : string.Join(", ", parameters.Select(p => $"@{p.Key}={p.Value}")));

        var aliases = ExtractSelectAliases(sql);

        // Dispatch by a table-name fingerprint. New showcase reports register
        // their own builder here.
        if (sql.Contains("PackageCostLines", StringComparison.OrdinalIgnoreCase))
            return BuildPackageCostTable(aliases, parameters);

        // Unknown report — return an empty table with the requested columns so
        // the renderer doesn't NRE on a missing field.
        return BuildEmptyTable(aliases);
    }

    // ─────────────────────────────────────────────────────────────────
    // Package Cost Sheet — synthesizes the service lines a tour operator
    // pays its suppliers for one package in one departure season, grouped
    // by cost group and supplier.
    // ─────────────────────────────────────────────────────────────────
    private DataTable BuildPackageCostTable(IReadOnlyList<string> aliases, IDictionary<string, object>? parameters)
    {
        var dt = BuildEmptyTable(aliases);

        // Resolve filters — fall back to the first package and season if the caller didn't pass anything.
        string packageName = LookupParameter(parameters, "Package") ?? TravelData.Packages[0].Name;
        string season      = LookupParameter(parameters, "Season")  ?? TravelData.Seasons[0];
        var package = TravelData.Packages.FirstOrDefault(p => p.Name.Equals(packageName, StringComparison.OrdinalIgnoreCase))
                      ?? TravelData.Packages[0];

        // Deterministic per package and season, so the same choice always prints the same sheet.
        var rnd = new Random(HashSeed(package.Name, season));
        var city = package.City;
        int pax = 2;
        int nights = package.Nights;

        // Prices move with the season and with how far the package is from home.
        decimal seasonFactor = season switch { "Summer" => 1.25m, "Autumn" => 1.05m, "Winter" => 0.85m, _ => 1.0m };
        decimal flightFactor = package.Region switch { "East Asia" => 1.6m, "Southern Africa" => 1.8m, "North Africa" => 0.9m, _ => 1.0m };

        // Cost groups, each with its suppliers; each supplier with its services:
        // (code suffix, description, quantity, unit, low rate, high rate, note)
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
                    ("H", "Double room with breakfast",    nights, "night", 110, 260, "Commissionable 10%"),
                    ("H", "City tax",                      nights, "night",   2,   5, "Paid locally"),
                }),
                ($"{city} Boutique Stays", new[]
                {
                    ("H", "Room upgrade, view side",       nights, "night",  40,  90, ""),
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
                ("Showcase Travel, head office", new[]
                {
                    ("S", "Booking fee",                      1, "booking", 45,  60, ""),
                    ("S", "Documents and delivery",           1, "booking", 12,  25, ""),
                }),
            }),
        };

        int lineNo = 1;
        foreach (var group in costGroups)
        {
            foreach (var (supplier, services) in group.Suppliers)
            {
                foreach (var svc in services)
                {
                    var factor = svc.Code == "F" ? flightFactor * seasonFactor : seasonFactor;
                    // Rates in cents times the factors, rounded back to two places, so every rate prints with two decimals.
                    var rate = Math.Round(rnd.Next(svc.Low * 100, svc.High * 100 + 1) / 100m * factor, 2);
                    var serviceCode = $"{svc.Code}{lineNo:D3}";

                    var row = dt.NewRow();
                    SetIfPresent(row, "PackageCostLines_Region",        package.Region);
                    SetIfPresent(row, "PackageCostLines_Package",       package.Name);
                    SetIfPresent(row, "PackageCostLines_Season",        season);
                    SetIfPresent(row, "PackageCostLines_CostGroup",     group.Code);
                    SetIfPresent(row, "PackageCostLines_CostGroupDesc", group.Desc);
                    SetIfPresent(row, "PackageCostLines_Supplier",      supplier);
                    SetIfPresent(row, "PackageCostLines_Service",       serviceCode);
                    SetIfPresent(row, "PackageCostLines_ServiceDesc",   svc.Desc);
                    SetIfPresent(row, "PackageCostLines_Qty",           (decimal)svc.Qty);
                    SetIfPresent(row, "PackageCostLines_Unit",          svc.Unit);
                    SetIfPresent(row, "PackageCostLines_Rate",          rate);
                    SetIfPresent(row, "PackageCostLines_Notes",         svc.Note);

                    // Formula columns — values must match the Crystal formulas so
                    // grouping & summary aggregation work consistently.
                    SetIfPresent(row, "Formula_Amount",      svc.Qty * rate);
                    SetIfPresent(row, "Formula_PackageDesc", $"{package.Name}, {season}");
                    SetIfPresent(row, "Formula_GroupDesc",   $"{group.Code} {group.Desc}");

                    dt.Rows.Add(row);
                    lineNo++;
                }
            }
        }

        _logger.LogInformation("Showcase Package Cost Sheet: produced {Rows} rows for package='{P}', season='{S}'",
            dt.Rows.Count, package.Name, season);
        return dt;
    }

    // ─────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────

    private static DataTable BuildEmptyTable(IReadOnlyList<string> aliases)
    {
        var dt = new DataTable();
        foreach (var a in aliases)
            dt.Columns.Add(a, typeof(object));
        return dt;
    }

    private static void SetIfPresent(DataRow row, string column, object value)
    {
        if (row.Table.Columns.Contains(column))
            row[column] = value;
    }

    private static string? LookupParameter(IDictionary<string, object>? parameters, string key)
    {
        if (parameters == null) return null;
        foreach (var kvp in parameters)
        {
            if (string.Equals(kvp.Key, key, StringComparison.OrdinalIgnoreCase))
                return kvp.Value?.ToString();
        }
        return null;
    }

    /// <summary>
    /// Pulls every <c>AS [alias]</c> (or <c>AS alias</c>) from the SELECT clause
    /// up to FROM. Robust to either bracketed or bare aliases; preserves order.
    /// </summary>
    private static List<string> ExtractSelectAliases(string sql)
    {
        var aliases = new List<string>();
        // Find the top-level SELECT…FROM slice
        var selectMatch = Regex.Match(sql, @"SELECT\s+(.*?)\s+FROM\s", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!selectMatch.Success) return aliases;
        var selectList = selectMatch.Groups[1].Value;

        // Bracketed alias:  ... AS [alias]
        foreach (Match m in Regex.Matches(selectList, @"AS\s+\[([^\]]+)\]", RegexOptions.IgnoreCase))
            aliases.Add(m.Groups[1].Value);
        // Bare alias:       ... AS alias  (only if not already captured)
        foreach (Match m in Regex.Matches(selectList, @"AS\s+([A-Za-z_][A-Za-z0-9_]*)", RegexOptions.IgnoreCase))
        {
            var a = m.Groups[1].Value;
            if (!aliases.Contains(a, StringComparer.OrdinalIgnoreCase))
                aliases.Add(a);
        }
        return aliases.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static int HashSeed(params string[] parts)
    {
        unchecked
        {
            int h = 17;
            foreach (var p in parts)
                foreach (var c in p) h = h * 31 + c;
            return h;
        }
    }
}
