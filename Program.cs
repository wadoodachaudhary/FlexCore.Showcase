using FlexCore.Showcase.Components;
using FlexCore.Showcase.Services;
using Fx.ControlKit.Notifications;
using Fx.ControlKit.Reports;

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
builder.Services.AddSingleton<BenchDataStore>();
// Sign-up / question form (2026-10-13): JSON-lines file store; "Signups:Path" overrides the location. Email delivery not wired yet (SC-274).
builder.Services.Configure<FlexCore.Showcase.Services.Signups.SignupOptions>(builder.Configuration.GetSection("Signups"));
builder.Services.AddSingleton<FlexCore.Showcase.Services.Signups.ISignupStore, FlexCore.Showcase.Services.Signups.FileSignupStore>();

// Report pipeline — stub services that synthesize demo data instead of hitting a DB.
builder.Services.AddSingleton(new ReportOptions
{
    SchemaPrefix = "",                                       // showcase XML doesn't use a schema
    SessionAutoInjectParameters = { "DivisionID", "UserId" }, // auto-fill from session ctx
});
builder.Services.AddScoped<CrystalXmlReportLoader>();
builder.Services.AddScoped<IReportSessionContext, ShowcaseReportSessionContext>();
builder.Services.AddScoped<IReportDataExecutor, ShowcaseReportDataExecutor>();
// IReportExporter is logically optional (only needed for .rpt binaries) but
// Blazor's @inject can't see C# nullability — register a no-op stub.
builder.Services.AddScoped<IReportExporter, NoopReportExporter>();
builder.Services.AddScoped<IReportViewerSettings, InMemoryReportViewerSettings>();
builder.Services.AddScoped<IReportPickListProvider, EmptyReportPickListProvider>();

var app = builder.Build();

// Pre-load benchmark store
var benchData = app.Services.GetRequiredService<BenchDataStore>();
benchData.Load();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
