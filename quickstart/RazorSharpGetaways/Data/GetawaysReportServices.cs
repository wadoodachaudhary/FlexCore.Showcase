using System.Data;
using Fx.ControlKit.Reports;

namespace RazorSharpGetaways.Data;

/// <summary>
/// The report writer's data source. It receives the SQL the Crystal XML loader built and the parameter values, and here answers
/// from memory through <see cref="PackageCostSheet"/>. A real application runs the SQL against its database in this one method,
/// with Dapper, EF Core or a plain command, and returns the rows as a DataTable.
/// </summary>
public sealed class GetawaysReportData : IReportDataExecutor
{
    public DataTable Execute(string sql, IDictionary<string, object>? parameters) =>
        sql.Contains("PackageCostLines", StringComparison.OrdinalIgnoreCase)
            ? PackageCostSheet.Build(sql, parameters)
            : PackageCostSheet.Empty(sql);
}

/// <summary>
/// Values for parameters a report's SQL names but the caller did not pass (ReportOptions.SessionAutoInjectParameters lists them).
/// The cost sheet names none, so this is the worked example; a real application reads them from the signed-in user.
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
/// The XML report pipeline needs no Crystal runtime; this stub stands in for <see cref="IReportExporter"/>, which the writer only
/// calls for native .rpt files, because the control resolves the service whether or not it is used.
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
