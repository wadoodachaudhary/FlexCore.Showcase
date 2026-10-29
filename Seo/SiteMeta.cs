using System.Reflection;

namespace FlexCore.Showcase.Seo;

/// <summary>
/// One page as search engines, link previews and AI assistants see it (2026-10-29). <see cref="Path"/> is the canonical
/// base-relative path ("" for home). <see cref="Heading"/> is the page's visible h1 and breadcrumb text; <see cref="Title"/> is the
/// document title (the &lt;PageTitle&gt;, the search result and the link preview), which names the framework and the control the
/// way people search for them. <see cref="Aliases"/> are the other @page routes that render the same page, whose canonical link
/// points here; <see cref="Section"/> groups the page in the sitemap and in llms.txt; <see cref="Controls"/> names the library
/// types the page demonstrates and <see cref="Keywords"/> the words people search for the subject under (the other vendors'
/// spellings: "datagrid", "data table"), both for the Markdown twin and llms.txt. <see cref="Index"/> false keeps the page out
/// of the sitemap and adds a noindex tag.
/// </summary>
public sealed record PageMeta(
    string Path,
    string Heading,
    string Title,
    string Description,
    string Section,
    string[]? Aliases = null,
    string[]? Controls = null,
    string[]? Keywords = null,
    bool Index = true)
{
    /// <summary>Address of the page's Markdown twin: /demo/grid.md, /index.md for home.</summary>
    public string MarkdownPath => (Path.Length == 0 ? "index" : Path) + ".md";

    /// <summary>The title without the site name, for structured data and link text.</summary>
    public string Headline => Title.EndsWith(" — " + SiteMeta.SiteName, StringComparison.Ordinal) ? Title[..^(3 + SiteMeta.SiteName.Length)] : Title;
}

/// <summary>
/// The words and addresses the site publishes about itself: canonical origin and host rules, and the page registry in
/// SiteMeta.Pages.cs. SiteHead reads it for the head tags, SeoEndpoints for robots.txt, sitemap.xml and llms.txt. A page's
/// &lt;PageTitle&gt; is kept equal to its <see cref="PageMeta.Title"/> here; scripts/check-seo.py checks that.
/// </summary>
public static partial class SiteMeta
{
    public const string SiteName = "FlexCore";
    public const string Origin = "https://flexcoreui.com";
    public const string CanonicalHost = "flexcoreui.com";

    /// <summary>Hosts that 301 to <see cref="Origin"/>: the www name and production's App Service default name.</summary>
    public static readonly string[] RedirectHosts = { "www.flexcoreui.com", "flexcore.azurewebsites.net" };

    /// <summary>The QA copy of the site: never redirected, served with noindex and a robots.txt that disallows everything.</summary>
    public const string StagingHost = "flexcore-showcase.azurewebsites.net";

    /// <summary>1200 x 630 link-preview image (wwwroot/og-image.png). Absolute, because preview fetchers do not resolve relative URLs.</summary>
    public static string OgImage => Origin + "/og-image.png";

    public static bool IsStaging(string host) => host.Equals(StagingHost, StringComparison.OrdinalIgnoreCase);

    /// <summary>Base-relative path without query, fragment or slashes: "demo/grid", "" for home.</summary>
    public static string Normalize(string path) => path.Split('?')[0].Split('#')[0].Trim('/');

    public static string Url(string path) => path.Length == 0 ? Origin + "/" : $"{Origin}/{path}";

    /// <summary>The registry entry for a path or one of its aliases, or null for pages that are not registered.</summary>
    public static PageMeta? Find(string path)
    {
        var p = Normalize(path);
        return Pages.FirstOrDefault(m => m.Path.Equals(p, StringComparison.OrdinalIgnoreCase)
            || m.Aliases?.Any(a => a.Equals(p, StringComparison.OrdinalIgnoreCase)) == true);
    }

    public static IEnumerable<PageMeta> Indexable => Pages.Where(p => p.Index);

    /// <summary>Section names in registry order; the sitemap and llms.txt list pages in this order.</summary>
    public static IEnumerable<string> Sections => Pages.Select(p => p.Section).Distinct();

    /// <summary>The page a section's breadcrumb links to (its overview), or null for sections without one.</summary>
    public static PageMeta? Landing(string section) =>
        SectionLandings.TryGetValue(section, out var path) ? Pages.FirstOrDefault(p => p.Path == path) : null;

    /// <summary>The library version the demos run, from the FlexCore assembly ("0.2.49"); the version on NuGet can trail it.</summary>
    public static string LibraryVersion { get; } =
        typeof(Fx.ControlKit.ButtonControl).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion.Split('+')[0] ?? "";
}
