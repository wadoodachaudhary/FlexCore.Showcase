using Fx.ControlKit.Dialogs;
using Fx.ControlKit.Notifications;
using Fx.ControlKit.Reports;
using RazorSharpGetaways.Components;
using RazorSharpGetaways.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// FlexCore services: toasts, confirm/alert/prompt dialogs and the shared text zoom. MainLayout.razor mounts the two hosts
// (NotificationDisplayControl, DialogHostControl) that draw them.
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<IDialogService>(sp => sp.GetRequiredService<DialogService>());
builder.Services.AddScoped<Fx.ControlKit.ZoomService>();

// The report writer (Invoices page): its options, the Crystal XML loader and the five host services it resolves. The data
// executor and the session context are this app's (Data/GetawaysReportServices.cs); the other three are the library's stand-ins.
// SessionAutoInjectParameters names the parameters a report may take from the session instead of a prompt; the cost sheet has
// none, so these two are the worked example for a report of your own.
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

// The app's own state, one instance per visitor: the trip requests and the Settings page's choices
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
