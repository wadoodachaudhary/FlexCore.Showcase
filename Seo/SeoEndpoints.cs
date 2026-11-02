using System.Net;
using System.Text;

namespace FlexCore.Showcase.Seo;

/// <summary>
/// The crawler-facing plumbing (2026-10-29): one host for the site, and robots.txt, sitemap.xml and llms.txt generated from
/// <see cref="SiteMeta"/> so they cannot drift from the pages. Wired in Program.cs.
/// </summary>
public static class SeoEndpoints
{
    /// <summary>
    /// 301s GET/HEAD requests on the www name and production's azurewebsites name to https://flexcoreui.com, so search engines
    /// see one copy of each page. Runs before HTTPS redirection so http://www... is one hop. Leaves the SignalR endpoint and App
    /// Service's own probes alone: the platform pings the default host name, does not follow redirects, and must get a 200.
    /// The staging host is never redirected; every response from it carries X-Robots-Tag: noindex instead (a robots.txt block
    /// would hide the noindex, and a blocked URL can still be listed by address).
    /// </summary>
    public static IApplicationBuilder UseCanonicalHost(this IApplicationBuilder app) => app.Use(async (context, next) =>
    {
        var request = context.Request;
        if ((HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method))
            && SiteMeta.RedirectHosts.Contains(request.Host.Host, StringComparer.OrdinalIgnoreCase)
            && !request.Path.StartsWithSegments("/_blazor")
            && !IsPlatformProbe(request))
        {
            context.Response.Redirect(SiteMeta.Origin + request.Path + request.QueryString, permanent: true);
            return;
        }
        if (SiteMeta.IsStaging(request.Host.Host))
            context.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        await next(context);
    });

    // Always On pings with this user agent; Health check sends x-ms-auth-internal-token; /robots933456.txt is the warm-up probe.
    private static bool IsPlatformProbe(HttpRequest request) =>
        request.Headers.UserAgent.ToString().Contains("AlwaysOn", StringComparison.OrdinalIgnoreCase)
        || request.Headers.ContainsKey("x-ms-auth-internal-token")
        || request.Path.StartsWithSegments("/robots933456.txt");

    public static void MapSeoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/robots.txt", (HttpContext http) => Text(http, Robots(http.Request.Host.Host), "text/plain"));
        endpoints.MapGet("/sitemap.xml", (HttpContext http) => Text(http, Sitemap(), "application/xml"));
        endpoints.MapGet("/llms.txt", (HttpContext http) => Text(http, LlmsTxt(), "text/plain"));
        endpoints.MapGet("/llms-full.txt", (HttpContext http) => Text(http, PageDocs.LlmsFull, "text/plain"));
        // One Markdown twin per page (/demo/grid.md), the form llms.txt links to; explicit routes, so nothing else is caught.
        foreach (var page in PageDocs.WithTwin)
            endpoints.MapGet("/" + page.MarkdownPath, (HttpContext http) => Text(http, PageDocs.Twin(page.Path)!, "text/markdown"));
    }

    private static IResult Text(HttpContext http, string body, string mediaType)
    {
        http.Response.Headers.CacheControl = "public, max-age=3600";
        return Results.Text(body, mediaType + "; charset=utf-8");
    }

    /// <summary>
    /// Everything may be crawled except the SignalR endpoint and the error pages; search engines and AI assistants are welcome,
    /// so there are no per-bot groups (a named group would replace the * rules for that bot, not add to them). Staging gets the
    /// same rules without the sitemap; its noindex header does the rest.
    /// </summary>
    private static string Robots(string host) =>
        "# Search engines and AI assistants are welcome to crawl and cite this site.\nUser-agent: *\nContent-Signal: search=yes, ai-input=yes, ai-train=yes\nDisallow: /_blazor\nDisallow: /Error\nDisallow: /not-found\n"
        + (SiteMeta.IsStaging(host) ? "" : $"\nSitemap: {SiteMeta.Origin}/sitemap.xml\n");

    /// <summary>Canonical pages only (no aliases, no error pages), with the page file's last commit date when the build knew it.</summary>
    private static string Sitemap()
    {
        var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
        foreach (var page in SiteMeta.Indexable)
        {
            sb.Append("  <url><loc>").Append(WebUtility.HtmlEncode(SiteMeta.Url(page.Path))).Append("</loc>");
            if (PageDates.For(page.Path) is { } date) sb.Append("<lastmod>").Append(date.ToString("yyyy-MM-dd")).Append("</lastmod>");
            sb.Append("</url>\n");
        }
        return sb.Append("</urlset>\n").ToString();
    }

    /// <summary>
    /// llms.txt per llmstxt.org: an H1, a one-paragraph blockquote, plain paragraphs of facts, then H2 sections of
    /// "- [title](url): description" lines, one section per registry section. Links go to the Markdown twins when a page has
    /// one, as the spec asks, and to the page itself otherwise.
    /// </summary>
    private static string LlmsTxt()
    {
        var sb = new StringBuilder();
        sb.Append("# ").Append(SiteMeta.SiteName).Append("\n\n> ").Append(SiteMeta.Summary).Append("\n\n");
        foreach (var paragraph in SiteMeta.Facts) sb.Append(paragraph).Append("\n\n");
        foreach (var section in SiteMeta.Sections)
        {
            sb.Append("## ").Append(section).Append("\n\n");
            foreach (var page in SiteMeta.Indexable.Where(p => p.Section == section))
            {
                var link = PageDocs.Twin(page.Path) is null ? SiteMeta.Url(page.Path) : SiteMeta.Url(page.MarkdownPath);
                sb.Append("- [").Append(page.Headline).Append("](").Append(link).Append("): ").Append(page.Description);
                if (page.Controls is { Length: > 0 }) sb.Append(" Controls: ").Append(string.Join(", ", page.Controls)).Append('.');
                sb.Append('\n');
            }
            sb.Append('\n');
        }
        sb.Append("## Optional\n\n");
        sb.Append("- [Full documentation as one file](").Append(SiteMeta.Origin).Append("/llms-full.txt): every page's documentation tables and example code in one Markdown file; each page is also at its own address with .md appended.\n");
        foreach (var (title, url, note) in SiteMeta.ExternalLinks) sb.Append("- [").Append(title).Append("](").Append(url).Append("): ").Append(note).Append('\n');
        return sb.ToString();
    }
}
