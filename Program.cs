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
// Scoped per circuit (2026-10-26): pages edit these rows in place; a singleton leaked one visitor's edits to all others.
builder.Services.AddScoped<BenchDataStore>();
// Sign-up / question form (2026-10-13): JSON-lines file store; "Signups:Path" overrides its path. No email delivery yet (SC-274).
builder.Services.Configure<FlexCore.Showcase.Services.Signups.SignupOptions>(builder.Configuration.GetSection("Signups"));
builder.Services.AddSingleton<FlexCore.Showcase.Services.Signups.ISignupStore, FlexCore.Showcase.Services.Signups.FileSignupStore>();

// Home window = Quick Start project (2026-11-04): trip requests and Settings choices, one per circuit, as in the download.
builder.Services.AddScoped<TripRequestStore>();
builder.Services.AddScoped<GetawaysAppSettings>();

// Report pipeline without a database: the Quick Start's in-memory executor, session context and no-op exporter
// (quickstart/RazorSharpGetaways/Data/GetawaysReportServices.cs), shared by Report writer and the window's Invoices page.
builder.Services.AddSingleton(new ReportOptions
{
    SchemaPrefix = "",                                       // showcase XML doesn't use a schema
    SessionAutoInjectParameters = { "AgencyId", "UserId" }, // the parameters GetawaysReportSession answers; no report here names them
});
builder.Services.AddScoped<CrystalXmlReportLoader>();
builder.Services.AddScoped<IReportSessionContext, GetawaysReportSession>();
builder.Services.AddScoped<IReportDataExecutor, GetawaysReportData>();
// IReportExporter is optional (.rpt binaries only), but @inject can't see C# nullability: register a no-op stub.
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
