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
builder.Services.AddSingleton<BenchDataStore>();

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
