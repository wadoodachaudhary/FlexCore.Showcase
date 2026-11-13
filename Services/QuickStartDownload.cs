namespace FlexCore.Showcase.Services;

/// <summary>
/// Quick Start download (2026-11-04): quickstart/RazorSharpGetaways, zipped and embedded by FlexCore.Showcase.csproj every build,
/// so it always matches the home page's app window.
/// </summary>
public static class QuickStartDownload
{
    public const string Path = "/downloads/razor-sharp-getaways.zip";
    public const string FileName = "razor-sharp-getaways.zip";
    private const string Resource = "FlexCore.Showcase.Downloads.razor-sharp-getaways.zip";

    public static IEndpointRouteBuilder MapQuickStartDownload(this IEndpointRouteBuilder app)
    {
        app.MapMethods(Path, ["GET", "HEAD"], () =>
        {
            var stream = typeof(QuickStartDownload).Assembly.GetManifestResourceStream(Resource);
            return stream is null ? Results.NotFound() : Results.File(stream, "application/zip", FileName);
        });
        return app;
    }
}
