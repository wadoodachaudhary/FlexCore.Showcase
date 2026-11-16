using System.Data;
using Fx.ControlKit.Reports;
using RazorSharpGetaways.Data;

namespace FlexCore.Showcase.Services;

/// <summary>
/// The site's <see cref="IReportDataExecutor"/>. Every report is answered in memory by the Quick Start's
/// <see cref="GetawaysReportData"/> (the Package Cost Sheet), except runs of the Crystal Reports bench: while the bench is open
/// (<see cref="AtlasRunContext.Enabled"/>) a report reads the bounded, read-only AI Atlas SQLite database, and in Layout-only
/// mode it gets an empty table with the report's fields, so an uploaded report shows its layout without data.
/// </summary>
public sealed class ShowcaseReportDataExecutor(AtlasDatabase atlas, AtlasRunContext context) : IReportDefinitionDataExecutor
{
    private readonly GetawaysReportData _getaways = new();

    public DataTable Execute(ReportDefinition definition, IDictionary<string, object>? parameters)
    {
        if (context.Enabled && context.LayoutOnly)
        {
            var table = new DataTable();
            foreach (var name in (definition.PositionedLayout?.Bindings.Values ?? definition.Columns.Select(c => c.Field))
                .Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase))
                table.Columns.Add(name, typeof(object));
            return table;
        }
        return Execute(definition.Sql, parameters);
    }

    public DataTable Execute(string sql, IDictionary<string, object>? parameters)
    {
        if (!context.Enabled)
            return _getaways.Execute(sql, parameters);
        if (context.LayoutOnly)
            throw new InvalidOperationException("Switch to AI Atlas SQLite to execute a data query.");
        return atlas.Execute(sql, parameters);
    }
}
