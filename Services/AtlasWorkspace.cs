using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Fx.ControlKit.Reports;

namespace FlexCore.Showcase.Services;

public sealed class AtlasReport
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = "";
    public string Category { get; set; } = "Uploaded";
    public string Description { get; set; } = "";
    public string Accent { get; set; } = "#087f73";
    public string Metric { get; set; } = "";
    public string SecondaryMetric { get; set; } = "";
    public string Disclosure { get; set; } = "External report; data not included";
    public string File { get; set; } = "";
    public string Preview { get; set; } = "";
    public bool IsSample { get; set; }
    public string? SourcePath { get; set; }
    public string? XmlPath { get; set; }
    public List<string> Log { get; } = [];
    public bool IsRpt => Path.GetExtension(SourcePath ?? File).Equals(".rpt", StringComparison.OrdinalIgnoreCase);
}

/// <summary>Page-owned report copies. Never writes into the bundled sample pack.</summary>
public sealed class AtlasWorkspace(IWebHostEnvironment environment) : IAsyncDisposable
{
    public const long MaxBytes = 32 * 1024 * 1024;
    private readonly string _root = Path.Combine(Path.GetTempPath(), "FlexCore-Atlas", Guid.NewGuid().ToString("N"));
    private readonly SemaphoreSlim _gate = new(1);
    private bool _disposed;
    private int _uploads;
    private int _conversions;
    public string SampleRoot => Path.Combine(environment.WebRootPath, "reports", "ai-atlas");

    public List<AtlasReport> Catalog()
    {
        var reports = JsonSerializer.Deserialize<List<AtlasReport>>(System.IO.File.ReadAllText(Path.Combine(SampleRoot, "catalog.json"))) ?? [];
        foreach (var report in reports) { report.IsSample = true; report.SourcePath = ResolveSample(report.File); }
        return reports;
    }

    public string ResolveSample(string relative)
    {
        var path = Path.GetFullPath(relative, SampleRoot);
        if (!path.StartsWith(SampleRoot + Path.DirectorySeparatorChar, StringComparison.Ordinal)) throw new InvalidDataException("Report is outside the sample pack.");
        for (FileSystemInfo entry = new FileInfo(path); entry.FullName != SampleRoot; entry = Directory.GetParent(entry.FullName)!)
            if ((entry.Attributes & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Symbolic links are not supported.");
        return path;
    }

    public async Task<AtlasReport> UploadAsync(string name, Stream input, CancellationToken token)
    {
        var fileName = Path.GetFileName(name.Replace('\\','/'));
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (extension is not (".rpt" or ".xml") || fileName.Length > 160) throw new InvalidDataException("Choose a .rpt or .xml report with a filename under 160 characters.");
        await _gate.WaitAsync(token);
        string? directory = null;
        try
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_uploads >= 12) throw new InvalidDataException("This workspace accepts 12 uploads. Reopen the page for a new workspace.");
            directory = Path.Combine(_root,Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
            var destination = Path.Combine(directory,fileName);
            await using(var output = new FileStream(destination,FileMode.CreateNew,FileAccess.Write))
            {
                var buffer=new byte[81920]; long total=0; int count;
                while((count=await input.ReadAsync(buffer,token))>0) { total+=count; if(total>MaxBytes) throw new InvalidDataException("Report exceeds 32 MB."); await output.WriteAsync(buffer.AsMemory(0,count),token); }
                if(total==0) throw new InvalidDataException("The uploaded file is empty.");
            }
            if(extension==".xml") ValidateUploadedXml(await System.IO.File.ReadAllTextAsync(destination,token));
            _uploads++;
            var report=new AtlasReport {Title=Path.GetFileNameWithoutExtension(fileName),SourcePath=destination,XmlPath=extension==".xml"?destination:null,File=fileName,Metric=extension[1..].ToUpperInvariant(),SecondaryMetric="Private workspace",Description="Imported report",Disclosure="External report. Layout only until mapped to AI Atlas tables."};
            report.Log.Add("Uploaded " + fileName); return report;
        }
        catch { if(directory is not null && Directory.Exists(directory)) Directory.Delete(directory,true); throw; }
        finally { _gate.Release(); }
    }

    public static void ValidateUploadedXml(string xml)
    {
        using var reader = XmlReader.Create(new StringReader(xml),new XmlReaderSettings { DtdProcessing=DtdProcessing.Prohibit,XmlResolver=null,MaxCharactersInDocument=MaxBytes });
        var document=XDocument.Load(reader);
        if(document.Descendants().Attributes("SubreportXmlPath").Any(a=>!string.IsNullOrWhiteSpace(a.Value))
            || document.Descendants("SubreportReference").Attributes("FileName").Any(a=>!string.IsNullOrWhiteSpace(a.Value)))
            throw new InvalidDataException("Uploaded XML must embed its subreports; external file paths are not accepted.");
        ReportDesignerXmlSerializer.FromXml(xml);
    }

    public async Task<string> OpenAsync(AtlasReport report, bool reconvert, Action<string>? progress, CancellationToken token)
    {
        await _gate.WaitAsync(token);
        try
        {
            ObjectDisposedException.ThrowIf(_disposed,this);
            if(report.XmlPath is not null && !reconvert) return report.XmlPath;
            if(++_conversions > 32) throw new InvalidDataException("This workspace has reached 32 conversions or working copies. Reopen the page for a new workspace.");
            var directory=Path.Combine(_root,report.Id,Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory);
            var output=Path.Combine(directory,Path.ChangeExtension(Path.GetFileName(report.SourcePath!),".xml"));
            if(report.IsRpt)
            {
                var result=await CrystalRptToXml.ConvertAsync(report.SourcePath!,output,new(ExtractSubreports:true,Progress:progress),token);
                token.ThrowIfCancellationRequested(); report.XmlPath=result.ReportXmlPath;
                progress?.Invoke($"Created XML and {result.SubreportXmlPaths.Count} subreport definition(s).");
            }
            else { System.IO.File.Copy(report.SourcePath!,output); report.XmlPath=output; }
            return report.XmlPath;
        }
        finally { _gate.Release(); }
    }

    public async Task<ReportDesignerSaveResult> SaveAsync(ReportDesignerSaveRequest request, CancellationToken token)
    {
        await _gate.WaitAsync(token);
        try
        {
            ObjectDisposedException.ThrowIf(_disposed,this);
            var destination=request.SavedPath;
            if(destination is null || !Path.GetFullPath(destination).StartsWith(_root+Path.DirectorySeparatorChar,StringComparison.Ordinal))
                destination=Path.Combine(_root,"designs",request.DocumentId.ToString("N")+".xml");
            return await ReportDesignerFileStore.SaveAsync(request,destination,cancellationToken:token);
        }
        finally { _gate.Release(); }
    }

    public async ValueTask DisposeAsync()
    {
        await _gate.WaitAsync();
        try { if(_disposed) return; _disposed=true; if(Directory.Exists(_root)) Directory.Delete(_root,true); }
        finally { _gate.Release(); }
    }
}
