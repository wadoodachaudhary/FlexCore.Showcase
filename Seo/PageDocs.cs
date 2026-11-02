using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using FlexCore.Showcase.Components.Shared;
using Microsoft.AspNetCore.Components;

namespace FlexCore.Showcase.Seo;

/// <summary>
/// The site's documentation as Markdown (2026-10-29): one twin per page at /&lt;path&gt;.md and all of them in /llms-full.txt, for
/// AI assistants and developers who want the text without the app. Per registered page: its description and keywords, the
/// documentation tables (the pages' ApiRow arrays, read by reflection so they are the same rows the Documentation sections
/// show) and the code examples (the pages' Code constants, the same text as the Razor, C# and CSS tabs, with the @using lines
/// CodeUsings adds there). Built once per process; the content only changes with a deployment.
/// </summary>
public static partial class PageDocs
{
    // Declared first: static initialisers run in order, and Twins reads it.
    /// <summary>Routed page types by base-relative path, including alias routes.</summary>
    public static IReadOnlyDictionary<string, Type> PageTypes { get; } = typeof(PageDocs).Assembly.GetTypes()
        .SelectMany(t => t.GetCustomAttributes<RouteAttribute>().Select(r => (Path: SiteMeta.Normalize(r.Template), Type: t)))
        .GroupBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
        .ToDictionary(g => g.Key, g => g.First().Type, StringComparer.OrdinalIgnoreCase);

    private static readonly Lazy<string> Full = new(Build);
    private static readonly Dictionary<string, Lazy<string>> Twins = SiteMeta.Indexable
        .Where(p => PageTypes.ContainsKey(p.Path))
        .ToDictionary(p => p.Path, p => new Lazy<string>(() => Markdown(p)), StringComparer.OrdinalIgnoreCase);

    public static string LlmsFull => Full.Value;

    /// <summary>The Markdown twin of one page, or null when the page has none (unregistered, not indexed).</summary>
    public static string? Twin(string path) => Twins.TryGetValue(SiteMeta.Normalize(path), out var twin) ? twin.Value : null;

    /// <summary>Pages that have a Markdown twin, in registry order.</summary>
    public static IEnumerable<PageMeta> WithTwin => SiteMeta.Indexable.Where(p => Twins.ContainsKey(p.Path));

    private static string Build()
    {
        var sb = new StringBuilder();
        sb.Append("# ").Append(SiteMeta.SiteName).Append(" documentation\n\n> ").Append(SiteMeta.Summary).Append("\n\n");
        foreach (var paragraph in SiteMeta.Facts) sb.Append(paragraph).Append("\n\n");
        sb.Append("This file is generated from the pages of ").Append(SiteMeta.Origin).Append(": each section is one page, with its documentation tables and the code of its examples. Library version ")
          .Append(SiteMeta.LibraryVersion).Append(". The short index is ").Append(SiteMeta.Origin).Append("/llms.txt.\n\n");
        foreach (var page in WithTwin) sb.Append("\n---\n\n").Append(Twin(page.Path));
        return sb.ToString();
    }

    private static string Markdown(PageMeta page)
    {
        var sb = new StringBuilder();
        var type = PageTypes[page.Path];
        sb.Append("# ").Append(page.Headline).Append("\n\n");
        sb.Append("URL: ").Append(SiteMeta.Url(page.Path)).Append("  \nSection: ").Append(page.Section);
        if (page.Controls is { Length: > 0 }) sb.Append("  \nControls: ").Append(string.Join(", ", page.Controls));
        if (page.Keywords is { Length: > 0 }) sb.Append("  \nKeywords: ").Append(string.Join(", ", page.Keywords));
        sb.Append("\n\n");
        if (page.Description.Length > 0) sb.Append(page.Description).Append("\n\n");

        var tables = Tables(page, type).ToList();
        if (tables.Count > 0)
        {
            sb.Append("## Documentation\n\n");
            foreach (var (name, rows) in tables) AppendTable(sb, name, rows);
        }

        var examples = Examples(type).ToList();
        if (examples.Count > 0)
        {
            sb.Append("## Examples\n\n");
            foreach (var (name, razor, csharp, css) in examples)
            {
                sb.Append("### ").Append(Words(name)).Append("\n\n");
                if (razor is not null) Fence(sb, "razor", CodeUsings.Header(razor, csharp, razorStyle: true) + razor);
                if (csharp is not null) Fence(sb, "csharp", (razor is null ? CodeUsings.Header(null, csharp, razorStyle: false) : "") + csharp);
                if (css is not null) Fence(sb, "css", css);
            }
        }
        return sb.ToString();
    }

    /// <summary>Pages whose documentation rows live in a shared class rather than a nested Vocab (the grid and chart overviews).</summary>
    private static readonly Dictionary<string, Type> SharedVocab = new(StringComparer.OrdinalIgnoreCase)
    {
        ["demo/grid"] = typeof(Components.Pages.Demos.GridVocab),
        ["demo/charts"] = typeof(Components.Pages.Demos.Charts.ChartsVocab),
    };

    /// <summary>The page's ApiRow arrays: in a nested static class named Vocab, on the page type itself, or in its shared class; declaration order.</summary>
    private static IEnumerable<(string Name, ApiRow[] Rows)> Tables(PageMeta page, Type type)
    {
        const BindingFlags Any = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        var holders = new[] { type.GetNestedType("Vocab", BindingFlags.Public | BindingFlags.NonPublic), type, SharedVocab.GetValueOrDefault(page.Path) };
        foreach (var holder in holders.Where(h => h is not null))
            foreach (var field in holder!.GetFields(Any).Where(f => f.FieldType == typeof(ApiRow[])).OrderBy(f => f.MetadataToken))
                if (field.GetValue(null) is ApiRow[] { Length: > 0 } rows) yield return (field.Name, rows);
    }

    /// <summary>The page's code constants grouped by example: Name (Razor), NameCs (C#), NameCss (CSS); declaration order.</summary>
    private static IEnumerable<(string Name, string? Razor, string? CSharp, string? Css)> Examples(Type page)
    {
        var code = page.GetNestedType("Code", BindingFlags.Public | BindingFlags.NonPublic);
        if (code is null) yield break;
        var fields = code.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(string) && (f.IsLiteral || f.IsInitOnly))
            .OrderBy(f => f.MetadataToken)
            .ToDictionary(f => f.Name, f => f.GetValue(null) as string ?? "");
        var seen = new HashSet<string>();
        foreach (var name in fields.Keys)
        {
            var baseName = name.EndsWith("Css", StringComparison.Ordinal) ? name[..^3] : name.EndsWith("Cs", StringComparison.Ordinal) ? name[..^2] : name;
            if (!seen.Add(baseName)) continue;
            fields.TryGetValue(baseName, out var razor);
            fields.TryGetValue(baseName + "Cs", out var cs);
            fields.TryGetValue(baseName + "Css", out var css);
            if (razor is null && cs is null && css is null) continue;
            yield return (baseName, razor, cs, css);
        }
    }

    private static void AppendTable(StringBuilder sb, string name, ApiRow[] rows)
    {
        var hasType = rows.Any(r => !string.IsNullOrEmpty(r.Type));
        var hasDefault = rows.Any(r => !string.IsNullOrEmpty(r.Default));
        var hasInputs = rows.Any(r => !string.IsNullOrEmpty(r.Inputs));
        sb.Append("### ").Append(Words(name)).Append("\n\n| Name |");
        if (hasType) sb.Append(" Type |");
        if (hasDefault) sb.Append(" Default |");
        if (hasInputs) sb.Append(" Inputs |");
        sb.Append(" Description |\n|---|");
        if (hasType) sb.Append("---|");
        if (hasDefault) sb.Append("---|");
        if (hasInputs) sb.Append("---|");
        sb.Append("---|\n");
        foreach (var row in rows)
        {
            sb.Append("| ").Append(Cell(row.Name)).Append(" |");
            if (hasType) sb.Append(' ').Append(Cell(row.Type)).Append(" |");
            if (hasDefault) sb.Append(' ').Append(Cell(row.Default)).Append(" |");
            if (hasInputs) sb.Append(' ').Append(Cell(row.Inputs)).Append(" |");
            sb.Append(' ').Append(Cell(row.Description)).Append(" |\n");
        }
        sb.Append('\n');
    }

    private static void Fence(StringBuilder sb, string language, string code) =>
        sb.Append("```").Append(language).Append('\n').Append(code.TrimEnd()).Append("\n```\n\n");

    /// <summary>Row text as a table cell: inline HTML such as &lt;code&gt; stripped, entities decoded, pipes and newlines made safe.</summary>
    private static string Cell(string? html) => html is null ? "" :
        WebUtility.HtmlDecode(Tag().Replace(html, "")).Replace("|", "\\|").Replace("\r", "").Replace("\n", " ").Trim();

    /// <summary>"StripItem" → "Strip item", "TextBoxEvents" → "Text box events", "A_Data" → "Data"; single words stay as written.</summary>
    private static string Words(string name)
    {
        var spaced = CamelBoundary().Replace(name.Length > 2 && name[1] == '_' ? name[2..] : name, " $1");
        return spaced.Length == 0 ? name : char.ToUpperInvariant(spaced[0]) + spaced[1..].ToLowerInvariant();
    }

    [GeneratedRegex(@"<[^>]+>")] private static partial Regex Tag();
    [GeneratedRegex(@"(?<=[a-z0-9])([A-Z])")] private static partial Regex CamelBoundary();
}
