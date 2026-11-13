using System.Data;
using System.Text;
using FlexCore.Showcase.Services;
using Fx.ControlKit;
using Fx.ControlKit.Reports;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace FlexCore.Showcase.Components.Pages.Demos.Reports;

public partial class CrystalReportsBench
{
    private readonly List<TabStripItem> _tabs = [new("Library","bi bi-collection"),new("Viewer","bi bi-file-earmark-text"),new("Designer","bi bi-pencil-square"),new("Data","bi bi-database")];
    private readonly List<Choice> _scenarios = [new("Balanced","Balanced"),new("Cautious","Cautious"),new("Accelerated","Accelerated")];
    private readonly List<Choice> _dataModes = [new("layout","Layout only"),new("sqlite","AI Atlas SQLite")];
    private readonly List<YearChoice> _years = Enumerable.Range(2022,9).Select(y=>new YearChoice(y,y.ToString())).ToList();
    private readonly List<Choice> _tables = AtlasDatabase.Tables.Select(t=>new Choice(t,t)).ToList();
    private readonly CancellationTokenSource _lifetime = new();
    private CancellationTokenSource? _operation;
    private AtlasWorkspace _workspace = default!;
    private List<AtlasReport> _reports = [];
    private IReadOnlyList<ReportDesignerDataTable> _schema = [];
    private AtlasReport? _selected;
    private ReportWriterControl? _writer;
    private ReportDesignerControl? _designer;
    private ReportDesignerDocument? _document;
    private string? _designerPath;
    private string _designerKey = Guid.NewGuid().ToString("N");
    private IJSObjectReference? _browser;
    private string _status = "Ready";
    private string _search = "";
    private string _scenario = "Balanced";
    private string _dataMode = "sqlite";
    private string _table = "Milestones";
    private List<string> _columns = [];
    private List<Dictionary<string,object>> _data = [];
    private int _year=2026, _tab;
    private bool _busy, _failed, _disposed, _interactive;
    private string SampleDefinitions => Path.Combine(_workspace.SampleRoot,"definitions");
    private IEnumerable<AtlasReport> FilteredReports => _reports.Where(r=>string.IsNullOrWhiteSpace(_search) || (r.Title+" "+r.Category).Contains(_search,StringComparison.OrdinalIgnoreCase));
    private static string PreviewUrl(AtlasReport report) => "reports/ai-atlas/"+report.Preview;
    private Dictionary<string,object> Parameters() => new() { ["Year"]=_year,["Scenario"]=_scenario };

    protected override void OnInitialized()
    {
        _workspace=new(Env); RunContext.Enabled=true;
        try { _reports=_workspace.Catalog(); _selected=_reports.FirstOrDefault(); _schema=Database.Schema(); TableChanged(_table); _status=$"{_reports.Count} reports ready. Sample originals remain unchanged when edited."; }
        catch(Exception error) { Failure(error); }
    }
    protected override void OnAfterRender(bool firstRender)
    {
        if(firstRender) { _interactive=true; StateHasChanged(); }
    }
    private Task SearchChanged(string? value) { _search=value??""; return Task.CompletedTask; }
    private Task YearChanged(int value) { _year=value; _status="Year changed. Run report to refresh."; return Task.CompletedTask; }
    private Task ScenarioChanged(string? value) { _scenario=value??"Balanced"; _status="Scenario changed. Run report to refresh."; return Task.CompletedTask; }
    private Task DataModeChanged(string? value) { _dataMode=value??"layout"; RunContext.LayoutOnly=_dataMode=="layout"; _status="Data source changed. Run report to refresh."; return Task.CompletedTask; }
    private void TableChanged(string? value)
    {
        _table=value??"Milestones";
        var table=Database.Browse(_table); _columns=table.Columns.Cast<DataColumn>().Select(c=>c.ColumnName).ToList();
        _data=table.Rows.Cast<DataRow>().Select(r=>_columns.ToDictionary(c=>c,c=>r[c])).ToList();
    }
    private async Task<bool> CanReplaceAsync() => _designer is null || await _designer.CanReplaceDocumentAsync();
    private async Task SelectAsync(AtlasReport report)
    {
        if(_busy || _selected==report || !await CanReplaceAsync()) return;
        Select(report);
    }
    private void Select(AtlasReport report)
    {
        _selected=report; _designerPath=null; _designer=null; _document=null; _writer?.Clear(); _tab=0;
        _dataMode=report.IsSample?"sqlite":"layout"; RunContext.LayoutOnly=_dataMode=="layout";
        _failed=false; _status=report.IsRpt && report.XmlPath is null?"RPT ready for native C# conversion.":"Selected "+report.Title+".";
    }
    private async Task BrowsePickedAsync(string? path)
    {
        var report=_reports.FirstOrDefault(r=>r.IsSample && r.SourcePath==path);
        if(report is not null) await SelectAsync(report);
    }
    private async Task ChangeTabAsync(int value)
    {
        if(_busy) return;
        if(value==1) { if(_writer?.ReportLoaded==true) _tab=1; else await RunAsync(); }
        else if(value==2) await EditAsync();
        else _tab=value;
    }
    private async Task WorkAsync(Func<CancellationToken,Task> action)
    {
        if(_busy || _disposed) return;
        _busy=true; _failed=false; _operation=CancellationTokenSource.CreateLinkedTokenSource(_lifetime.Token);
        await InvokeAsync(StateHasChanged);
        try { await action(_operation.Token); }
        catch(OperationCanceledException) { _status="Operation cancelled."; }
        catch(Exception error) { Failure(error); }
        finally { _busy=false; _operation.Dispose(); _operation=null; if(!_disposed) await InvokeAsync(StateHasChanged); }
    }
    private void Failure(Exception error) { _failed=true; _status=error.Message; Logger.LogError(error,"AI Atlas report operation failed"); }
    private void Cancel() => _operation?.Cancel();
    private Task UploadAsync(InputFileChangeEventArgs args) => WorkAsync(async token=>
    {
        if(!await CanReplaceAsync()) return;
        if(args.FileCount>6) throw new InvalidDataException("Upload up to six reports at a time.");
        foreach(var file in args.GetMultipleFiles(6))
        {
            _status="Uploading "+file.Name; await InvokeAsync(StateHasChanged);
            await using var stream=file.OpenReadStream(AtlasWorkspace.MaxBytes,token);
            var report=await _workspace.UploadAsync(file.Name,stream,token); _reports.Add(report); Select(report);
        }
    });
    private Task NewAsync() => WorkAsync(async token=>
    {
        if(!await CanReplaceAsync()) return;
        var doc=ReportDesignerDocument.CreateBlank("Untitled report");
        await using var stream=new MemoryStream(Encoding.UTF8.GetBytes(ReportDesignerXmlSerializer.ToXml(doc)));
        var report=await _workspace.UploadAsync("Untitled report.xml",stream,token); _reports.Add(report); Select(report);
        await OpenEditorAsync(token);
    });
    private Task ConvertAsync() => WorkAsync(async token=>
    {
        if(_selected is null || !_selected.IsRpt || !await CanReplaceAsync()) return;
        _tab=0; _status="Converting "+_selected.Title+"..."; _selected.Log.Clear();
        await OpenAsync(true,token); _designerPath=null; _designer=null; _document=null; _writer?.Clear();
        _status="Converted to XML with the native C# converter.";
    });
    private async Task<string> OpenAsync(bool force,CancellationToken token)
    {
        var report=_selected??throw new InvalidOperationException("Select a report.");
        var progress=new List<string>();
        var path=await _workspace.OpenAsync(report,force,message=> { lock(progress) progress.Add(message); },token);
        lock(progress) report.Log.AddRange(progress);
        return path;
    }
    private Task EditAsync() => WorkAsync(OpenEditorAsync);
    private async Task OpenEditorAsync(CancellationToken token)
    {
        if(_selected is null) return;
        if(_designerPath is null) { _designerPath=await OpenAsync(false,token); _designerKey=Guid.NewGuid().ToString("N"); }
        _tab=2; _status="Editing a private working copy. Download XML to keep the result.";
    }
    private Task RunAsync() => WorkAsync(async token=>
    {
        if(_writer is null || _selected is null) return;
        _status="Running "+_selected.Title+"..."; RunContext.Enabled=true; RunContext.LayoutOnly=_dataMode=="layout";
        var path=await OpenAsync(false,token); _tab=1; await InvokeAsync(StateHasChanged);
        if(_document is not null) await _writer.ShowDesignerReport(_document,Parameters());
        else await _writer.ShowXmlReport(path,Parameters(),positioned:true);
        token.ThrowIfCancellationRequested();
        if(_writer.ErrorMessage is not null) throw new InvalidDataException(_writer.ErrorMessage);
        _status=$"{_writer.PageCount} page(s) rendered. "+(_dataMode=="layout"?"Layout only; no detail data.":"Read-only SQLite dataset.");
    });
    private Task DocumentChanged(ReportDesignerDocument document) { _document=document; return Task.CompletedTask; }
    private Task PreviewAsync(ReportDesignerDocument document) { _document=document; return RunAsync(); }
    private async Task<ReportDesignerSaveResult> SaveAsync(ReportDesignerSaveRequest request)
    {
        var result=await _workspace.SaveAsync(request,_lifetime.Token);
        _status=result.Succeeded?"Working copy saved. Download XML to keep it beyond this session.":"Save failed: "+result.Error;
        if(result.Succeeded && _selected is not null) _selected.XmlPath=result.Path;
        return result;
    }
    private Task DownloadAsync() => WorkAsync(async token=>
    {
        var path=await OpenAsync(false,token);
        if(_designer is not null && !await _designer.SaveAsync()) return;
        path=_selected!.XmlPath??path;
        // FileStore embeds external subreports when saving a converted package.
        var xml=await File.ReadAllTextAsync(path,token);
        var request=new ReportDesignerSaveRequest(Guid.NewGuid(),xml,Path.GetFileName(path),null,null,path);
        var exported=await _workspace.SaveAsync(request,token);
        if(!exported.Succeeded) throw new InvalidDataException(exported.Error);
        var bytes=await File.ReadAllBytesAsync(exported.Path!,token);
        _browser??=await JS.InvokeAsync<IJSObjectReference>("import","./_content/FlexCore/browser-capabilities.js");
        await _browser.InvokeVoidAsync("invoke","download",null,new {bytes,type="application/xml",name=Path.GetFileName(path)});
        _status="Downloaded report XML, including embedded subreports.";
    });
    public async ValueTask DisposeAsync()
    {
        if(_disposed) return; _disposed=true; _lifetime.Cancel(); RunContext.Enabled=false;
        await _workspace.DisposeAsync();
        if(_browser is not null) { try { await _browser.DisposeAsync(); } catch(JSDisconnectedException) { } }
        _lifetime.Dispose();
    }
    private sealed record Choice(string Value,string Label);
    private sealed record YearChoice(int Value,string Label);
}
