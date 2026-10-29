using System.Globalization;

namespace FlexCore.Showcase.Seo;

/// <summary>
/// Last commit date of each routed page's .razor file, read from the git log the build embeds (the PageDates target in the
/// csproj). It gives the sitemap an honest lastmod: the day the page itself last changed. A build without git embeds nothing
/// and the sitemap carries no dates, which search engines treat as "unknown" rather than as wrong.
/// </summary>
public static class PageDates
{
    private static readonly Lazy<Dictionary<string, DateTimeOffset>> ByPath = new(Load);

    public static DateTimeOffset? For(string path) =>
        ByPath.Value.TryGetValue(SiteMeta.Normalize(path), out var date) ? date : null;

    private static Dictionary<string, DateTimeOffset> Load()
    {
        // "git log --format=%cI --name-only": a date line, then the files of that commit, newest commit first.
        var byFile = new Dictionary<string, DateTimeOffset>(StringComparer.Ordinal);
        using (var stream = typeof(PageDates).Assembly.GetManifestResourceStream("FlexCore.Showcase.Seo.pagedates.txt"))
        {
            if (stream is not null)
            {
                using var reader = new StreamReader(stream);
                DateTimeOffset? current = null;
                while (reader.ReadLine() is { } line)
                {
                    if (line.Length == 0) continue;
                    if (DateTimeOffset.TryParse(line, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) { current = d; continue; }
                    if (current is not null) byFile.TryAdd(line.Trim(), current.Value);   // first sighting is the latest commit
                }
            }
        }

        // Page types to files: the generated namespace mirrors the folder (FlexCore.Showcase.Components.Pages.Demos.GridDemo).
        var byPath = new Dictionary<string, DateTimeOffset>(StringComparer.OrdinalIgnoreCase);
        foreach (var (path, type) in PageDocs.PageTypes)
        {
            var file = type.FullName!.Replace("FlexCore.Showcase.", "", StringComparison.Ordinal).Replace('.', '/') + ".razor";
            if (byFile.TryGetValue(file, out var date)) byPath[path] = date;
        }
        return byPath;
    }
}
