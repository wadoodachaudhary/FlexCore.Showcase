namespace FlexCore.Showcase.Seo;

/// <summary>
/// The JSON-LD each page carries (2026-10-29). Home describes the library once (SoftwareSourceCode) and the site (WebSite);
/// every other registered page carries its breadcrumb trail and a TechArticle whose facts are the page's own title, description
/// and last change date. Dictionaries, because the keys start with "@". System.Text.Json's default encoder escapes "&lt;" so the
/// output is safe inside a script element.
/// </summary>
public static class StructuredData
{
    private const string Context = "https://schema.org";

    public static object For(PageMeta page, string url) => page.Path.Length == 0
        ? new object[] { Website(), Library() }
        : new object[] { Breadcrumbs(page, url), Article(page, url) };

    private static Dictionary<string, object?> Website() => new()
    {
        ["@context"] = Context,
        ["@type"] = "WebSite",
        ["name"] = SiteMeta.SiteName,
        ["url"] = SiteMeta.Origin + "/",
        ["description"] = SiteMeta.Summary,
    };

    private static Dictionary<string, object?> Library() => new()
    {
        ["@context"] = Context,
        ["@type"] = "SoftwareSourceCode",
        ["name"] = SiteMeta.SiteName,
        ["description"] = SiteMeta.Summary,
        ["url"] = SiteMeta.Origin + "/",
        ["codeRepository"] = SiteMeta.Repository,
        ["programmingLanguage"] = "C#",
        ["runtimePlatform"] = ".NET 10",
        ["sameAs"] = new[] { SiteMeta.Repository, SiteMeta.NuGet },
        ["targetProduct"] = new Dictionary<string, object?>
        {
            ["@type"] = "SoftwareApplication",
            ["name"] = SiteMeta.SiteName,
            ["applicationCategory"] = "DeveloperApplication",
            ["operatingSystem"] = "Any (ASP.NET Core Blazor, .NET 10)",
            ["softwareVersion"] = SiteMeta.LibraryVersion,
            ["downloadUrl"] = SiteMeta.NuGet,
            ["installUrl"] = SiteMeta.NuGet,
            ["softwareHelp"] = new Dictionary<string, object?> { ["@type"] = "WebPage", ["url"] = SiteMeta.Origin + "/get-started" },
            ["license"] = SiteMeta.License,
        },
        ["license"] = SiteMeta.License,
        ["version"] = SiteMeta.LibraryVersion,
        ["keywords"] = SiteMeta.Keywords,
    };
    // No offers or ratings: Google's software rich result needs a price and a rating, and neither can be stated honestly here.

    private static Dictionary<string, object?> Breadcrumbs(PageMeta page, string url)
    {
        var items = new List<Dictionary<string, object?>>
        {
            Crumb(1, "Home", SiteMeta.Origin + "/"),
        };
        // The same trail the page shows: Home > section > page, with the visible names.
        var section = SiteMeta.Landing(page.Section);
        if (section is not null && section.Path != page.Path) items.Add(Crumb(2, page.Section, SiteMeta.Url(section.Path)));
        items.Add(Crumb(items.Count + 1, page.Heading, url));
        return new()
        {
            ["@context"] = Context,
            ["@type"] = "BreadcrumbList",
            ["itemListElement"] = items,
        };
    }

    private static Dictionary<string, object?> Crumb(int position, string name, string item) => new()
    {
        ["@type"] = "ListItem",
        ["position"] = position,
        ["name"] = name,
        ["item"] = item,
    };

    private static Dictionary<string, object?> Article(PageMeta page, string url)
    {
        var article = new Dictionary<string, object?>
        {
            ["@context"] = Context,
            ["@type"] = "TechArticle",
            ["headline"] = page.Headline,
            ["description"] = page.Description,
            ["url"] = url,
            ["inLanguage"] = "en",
            ["image"] = SiteMeta.OgImage,
            ["isPartOf"] = new Dictionary<string, object?> { ["@type"] = "WebSite", ["name"] = SiteMeta.SiteName, ["url"] = SiteMeta.Origin + "/" },
            ["about"] = new Dictionary<string, object?> { ["@type"] = "SoftwareSourceCode", ["name"] = SiteMeta.SiteName, ["codeRepository"] = SiteMeta.Repository },
        };
        if (page.Keywords is { Length: > 0 }) article["keywords"] = page.Keywords;
        if (PageDates.For(page.Path) is { } date) article["dateModified"] = date.ToString("yyyy-MM-dd");
        return article;
    }
}
