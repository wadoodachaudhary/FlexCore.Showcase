using FlexCore.Showcase.Components;
using FlexCore.Showcase.Seo;
using FlexCore.Showcase.Services;
using Fx.ControlKit.Notifications;
using Fx.ControlKit.Reports;
using RazorSharpGetaways.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FlexCore services
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<Fx.ControlKit.ZoomService>();
// Dialog service (confirm / alert / prompt); its host is <DialogHostControl /> in MainLayout. Added 2026-10-12 for /demo/dialog.
builder.Services.AddScoped<Fx.ControlKit.Dialogs.DialogService>();
builder.Services.AddScoped<Fx.ControlKit.Dialogs.IDialogService>(sp => sp.GetRequiredService<Fx.ControlKit.Dialogs.DialogService>());
// Scoped, one store per circuit (2026-10-26): the grid and form pages edit these rows in place, so a shared singleton let one
// visitor's edits show up for every other visitor until the app restarted.
builder.Services.AddScoped<BenchDataStore>();
// Sign-up / question form (2026-10-13): JSON-lines file store; "Signups:Path" overrides the location. Email delivery not wired yet (SC-274).
builder.Services.Configure<FlexCore.Showcase.Services.Signups.SignupOptions>(builder.Configuration.GetSection("Signups"));
builder.Services.AddSingleton<FlexCore.Showcase.Services.Signups.ISignupStore, FlexCore.Showcase.Services.Signups.FileSignupStore>();

// The home page's app window is the Quick Start project (quickstart/RazorSharpGetaways, 2026-11-04): its trip requests and its
// Settings page's choices, one instance per circuit, as in the download.
builder.Services.AddScoped<TripRequestStore>();
builder.Services.AddScoped<GetawaysAppSettings>();

// Report pipeline: the Quick Start project's in-memory data executor, session context and no-op exporter stand in for a database
// (quickstart/RazorSharpGetaways/Data/GetawaysReportServices.cs); the Report writer page and the window's Invoices page share them.
builder.Services.AddSingleton(new ReportOptions
{
    SchemaPrefix = "",                                       // showcase XML doesn't use a schema
    SessionAutoInjectParameters = { "AgencyId", "UserId" }, // the parameters GetawaysReportSession answers; no report here names them
});
builder.Services.AddScoped<CrystalXmlReportLoader>();
builder.Services.AddScoped<IReportSessionContext, GetawaysReportSession>();
builder.Services.AddScoped<IReportDataExecutor, GetawaysReportData>();
// IReportExporter is logically optional (only needed for .rpt binaries) but
// Blazor's @inject can't see C# nullability — register a no-op stub.
builder.Services.AddScoped<IReportExporter, NoReportExporter>();
builder.Services.AddScoped<IReportViewerSettings, InMemoryReportViewerSettings>();
builder.Services.AddScoped<IReportPickListProvider, EmptyReportPickListProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// One host for search engines (2026-10-29): www and the azurewebsites name 301 to flexcoreui.com. Before HTTPS redirection so
// http://www... is a single hop. Seo/SeoEndpoints.cs; staging and platform probes are left alone.
app.UseCanonicalHost();
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapSeoEndpoints();   // robots.txt, sitemap.xml, llms.txt from Seo/SiteMeta.Pages.cs
app.MapQuickStartDownload();   // /downloads/razor-sharp-getaways.zip, the Quick Start project zipped at build (Services/QuickStartDownload.cs)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
