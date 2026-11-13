using System.Data;
using Fx.ControlKit.Reports;

namespace RazorSharpGetaways.Data;

/// <summary>
/// Report writer data source: gets the SQL the Crystal XML loader built plus the parameter values; here it answers from memory via
/// <see cref="PackageCostSheet"/>. A real app runs the SQL against its database here (Dapper, EF Core or a plain command) and
/// returns the rows as a DataTable.
/// </summary>
public sealed class GetawaysReportData : IReportDataExecutor
{
    public DataTable Execute(string sql, IDictionary<string, object>? parameters) =>
        sql.Contains("PackageCostLines", StringComparison.OrdinalIgnoreCase)
            ? PackageCostSheet.Build(sql, parameters)
            : PackageCostSheet.Empty(sql);
}

/// <summary>
/// Values for parameters a report's SQL names but the caller omits (listed in ReportOptions.SessionAutoInjectParameters). The cost
/// sheet uses none, so this is an example; a real app reads them from the signed-in user.
/// </summary>
public sealed class GetawaysReportSession : IReportSessionContext
{
    public object? Get(string parameterName) => parameterName.ToLowerInvariant() switch
    {
        "agencyid" => 1,
        "userid" => TripRequestStore.SignedInAgent,
        _ => null,
    };
}

/// <summary>
/// Stub <see cref="IReportExporter"/>: XML reports need no Crystal runtime and the writer calls it only for native .rpt files,
/// but the control resolves the service anyway.
/// </summary>
public sealed class NoReportExporter : IReportExporter
{
    public bool IsLoaded => false;
    public void LoadReport(string rptFilePath) { }
    public void SetParameters(Dictionary<string, string> parameters) { }
    public byte[] ExportToPdf() => [];
    public byte[] ExportToExcel() => [];
    public byte[] ExportToWord() => [];
    public byte[] ExportToRtf() => [];
    public byte[] ExportToCsv() => [];
}
