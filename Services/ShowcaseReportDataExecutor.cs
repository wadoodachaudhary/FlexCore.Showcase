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
/// <see cref="DemoData"/>. That lets the showcase exercise the full XML →
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
        if (sql.Contains("AssemblyCostsAllCommunities", StringComparison.OrdinalIgnoreCase))
            return BuildModelCostsTable(aliases, parameters);

        // Unknown report — return an empty table with the requested columns so
        // the renderer doesn't NRE on a missing field.
        return BuildEmptyTable(aliases);
    }

    // ─────────────────────────────────────────────────────────────────
    // Model Costs report — synthesizes Item rows for the selected
    // Community + Model, grouped by phase-group / phase.
    // ─────────────────────────────────────────────────────────────────
    private DataTable BuildModelCostsTable(IReadOnlyList<string> aliases, IDictionary<string, object>? parameters)
    {
        var dt = BuildEmptyTable(aliases);

        // Resolve filters — fall back to first house model + a default community
        // if the caller didn't pass anything.
        string community = LookupParameter(parameters, "Community") ?? "Showcase Estates";
        string model     = LookupParameter(parameters, "Model")     ?? DemoData.HouseModels[0].Name;

        // Build a deterministic set of items: each phase-group contains 2-3 phases,
        // each phase contains 3-5 line items. Drives sums at every group level.
        var rnd = new Random(HashSeed(community, model));

        // Phase-groups (the outer of the inner two groups). Each maps to a slice
        // of the global Phases array.
        var phaseGroups = new (string GrpPhase, string GrpDesc, int From, int To)[]
        {
            ("A", "Site & Foundation",  0, 3),   // Permits, Excavation, Foundation
            ("B", "Shell",              3, 5),   // Framing, Roofing
            ("C", "Mechanical",         5, 8),   // Plumbing rough, Electrical rough, HVAC
            ("D", "Finishes",           8, 14),  // Insulation, Drywall, Cabinets, Flooring, Paint, Finishes
        };

        // Pick a reasonable item-name pool keyed to phase, so the rows look like
        // a real estimating breakdown rather than random words.
        var itemsByPhaseGrp = new Dictionary<string, string[]>
        {
            ["A"] = new[] { "Permit Filing", "Lot Clearing", "Site Excavation", "Footing Concrete", "Foundation Walls", "Waterproofing" },
            ["B"] = new[] { "Wall Framing", "Roof Trusses", "Sheathing", "Asphalt Shingles", "Ridge Vent", "Gutters" },
            ["C"] = new[] { "Rough Plumbing", "Sewer Connection", "Rough Electrical", "Panel 200A", "Furnace 80k BTU", "Ductwork", "Thermostat" },
            ["D"] = new[] { "Insulation R-19", "Drywall 1/2\"", "Cabinet Install", "Granite Counters", "Hardwood Flooring", "Tile Bath", "Interior Paint", "Exterior Trim" },
        };

        int detailCounter = 1;
        foreach (var (grpPhase, grpDesc, from, to) in phaseGroups)
        {
            for (int phaseIdx = from; phaseIdx < to && phaseIdx < DemoData.Phases.Length; phaseIdx++)
            {
                var phaseFull = DemoData.Phases[phaseIdx];
                // "030 Excavation" → code = "030", desc = "Excavation"
                var spaceIdx = phaseFull.IndexOf(' ');
                var phaseCode = spaceIdx > 0 ? phaseFull[..spaceIdx] : phaseFull;
                var phaseDesc = spaceIdx > 0 ? phaseFull[(spaceIdx + 1)..] : phaseFull;

                int rowsInPhase = rnd.Next(3, 6);
                var itemPool = itemsByPhaseGrp[grpPhase];
                for (int r = 0; r < rowsInPhase; r++)
                {
                    var itemDesc = itemPool[r % itemPool.Length];
                    var itemCode = $"I{1000 + detailCounter:D4}";
                    var vendor = DemoData.Vendors[(phaseIdx + r) % DemoData.Vendors.Length];
                    var qty = (decimal)Math.Round(rnd.NextDouble() * 90 + 5, 2);
                    var rate = (decimal)Math.Round(rnd.NextDouble() * 250 + 8, 2);
                    var uom = DemoData.UnitOfMeasure[(phaseIdx + r) % DemoData.UnitOfMeasure.Length];

                    var row = dt.NewRow();
                    SetIfPresent(row, "AssemblyCostsAllCommunities_CommunityDesc", community);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_Model",         model);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_AssemblyDesc",  $"{model} Standard");
                    SetIfPresent(row, "AssemblyCostsAllCommunities_GrpPhase",      grpPhase);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_GrpDesc",       grpDesc);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_Phase",         phaseCode);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_PhaseDesc",     phaseDesc);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_Item",          itemCode);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_ItemDesc",      itemDesc);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_OrderQty",      qty);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_TakeoffUOM",    uom);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_Rate",          rate);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_VendorDesc",    vendor);
                    SetIfPresent(row, "AssemblyCostsAllCommunities_DivisionID",    1);

                    // Formula columns — values must match the Crystal formulas so
                    // grouping & summary aggregation work consistently.
                    SetIfPresent(row, "Formula_Amount",    qty * rate);
                    SetIfPresent(row, "Formula_ModelDesc", $"{model} {model} Standard");
                    SetIfPresent(row, "Formula_GroupDesc", $"{grpPhase} {grpDesc}");
                    SetIfPresent(row, "Formula_PhaseDesc", $"{phaseCode} {phaseDesc}");

                    dt.Rows.Add(row);
                    detailCounter++;
                }
            }
        }

        _logger.LogInformation("Showcase Model Costs: produced {Rows} rows for community='{C}', model='{M}'",
            dt.Rows.Count, community, model);
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
