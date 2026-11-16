using Fx.ControlKit.Dialogs;
using Fx.ControlKit.Notifications;
using Fx.ControlKit.Reports;
using RazorSharpGetaways.Components;
using RazorSharpGetaways.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FlexCore services: toasts, confirm/alert/prompt dialogs, shared text zoom. Their hosts (NotificationDisplayControl,
// DialogHostControl) are in MainLayout.razor.
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<IDialogService>(sp => sp.GetRequiredService<DialogService>());
builder.Services.AddScoped<Fx.ControlKit.ZoomService>();

// Report writer (Invoices page): options, Crystal XML loader and the five host services it resolves. Data executor, session
// context and exporter are this app's (Data/GetawaysReportServices.cs); viewer settings and pick list are library stand-ins.
// SessionAutoInjectParameters: parameters a report takes from the session instead of a prompt; the cost sheet uses none, so these
// two are an example.
builder.Services.AddSingleton(new ReportOptions
{
    SchemaPrefix = "",
    SessionAutoInjectParameters = { "AgencyId", "UserId" },
});
builder.Services.AddScoped<CrystalXmlReportLoader>();
builder.Services.AddScoped<IReportDataExecutor, GetawaysReportData>();
builder.Services.AddScoped<IReportSessionContext, GetawaysReportSession>();
builder.Services.AddScoped<IReportExporter, NoReportExporter>();
builder.Services.AddScoped<IReportViewerSettings, InMemoryReportViewerSettings>();
builder.Services.AddScoped<IReportPickListProvider, EmptyReportPickListProvider>();

// App state, one instance per visitor: trip requests and Settings choices
builder.Services.AddScoped<TripRequestStore>();
builder.Services.AddScoped<GetawaysAppSettings>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
