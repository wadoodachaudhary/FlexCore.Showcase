using Fx.ControlKit.Reports;

namespace FlexCore.Showcase.Services;

/// <summary>
/// No-op implementation of <see cref="IReportExporter"/>. The Crystal exporter
/// is optional in real apps (only used to render <c>.rpt</c> binaries via SAP's
/// Crystal Reports Engine), but Blazor's <c>@inject</c> on a nullable type still
/// requires the service to be in DI. Register this stub so the XML pipeline can
/// resolve the control without dragging in SAP DLLs.
/// </summary>
public class NoopReportExporter : IReportExporter
{
    public bool IsLoaded => false;
    public void LoadReport(string rptFilePath)            { /* showcase: no .rpt support */ }
    public void SetParameters(Dictionary<string, string> p) { /* showcase: no-op */ }
    public byte[] ExportToPdf()   => Array.Empty<byte>();
    public byte[] ExportToExcel() => Array.Empty<byte>();
    public byte[] ExportToWord()  => Array.Empty<byte>();
    public byte[] ExportToRtf()   => Array.Empty<byte>();
    public byte[] ExportToCsv()   => Array.Empty<byte>();
}
