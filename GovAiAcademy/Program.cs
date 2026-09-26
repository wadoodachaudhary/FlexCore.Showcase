using GovAiAcademy.Components;
using GovAiAcademy.Security;
using GovAiAcademy.Services;
using Fx.ControlKit.Notifications;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var authSection = builder.Configuration.GetSection(AcademyOptions.SectionName);
var academyOptions = new AcademyOptions();
authSection.Bind(academyOptions);
if (!Enum.TryParse<AcademyAuthMode>(authSection["Mode"], ignoreCase: true, out var mode))
    mode = AcademyAuthMode.DevelopmentMock;
academyOptions.Mode = mode;
builder.Services.AddSingleton(academyOptions);

// Development personas only. Entra ID wiring is documented in README and is not
// activated here, so this repo never needs a client secret to build or run.
// When a tenant is ready:
//   dotnet add package Microsoft.Identity.Web
//   builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//       .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("Authentication:Entra"));
//   builder.Services.AddAuthorization();
//   app.UseAuthentication();
//   app.UseAuthorization();
// Map Entra app roles to Learner, Instructor, and Administrator. Do not keep personas.

builder.Services.AddSingleton<AcademyStore>();
builder.Services.AddScoped<AcademyAuthState>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<Fx.ControlKit.ZoomService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // App Service terminates TLS. Restrict the app so only the platform front end can reach it.
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAcademySecurityHeaders();
app.UseAntiforgery();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .AllowAnonymous();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
