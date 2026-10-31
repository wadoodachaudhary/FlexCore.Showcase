using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace FlexCore.Showcase.Components.Shared;

/// <summary>
/// Source of the home page's app window for its Razor and C# tabs (2026-10-29), and of the site-tabs rules in app.css for the Tabs &amp;
/// accordion page's CSS tab, and of ChartPalette.cs for the chart pages' C# tabs. FlexCore.Showcase.csproj embeds the Getaways*.razor
/// pages, ChartPalette.cs, Home.razor and wwwroot/app.css, so every tab
/// shows the code exactly as it is built:
/// <see cref="Markup"/> and <see cref="Code"/> split a page into its markup (after its opening maintainer comment) and the body of
/// its @code block; <see cref="Region"/> collects the parts of a file between "code-tab:name" and "/code-tab" marker lines.
/// </summary>
public static class AppSource
{
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static string Markup(string file) => Cache.GetOrAdd("markup:" + file, static key =>
    {
        var text = Read(key[7..]);
        var codeAt = text.LastIndexOf("\n@code", StringComparison.Ordinal);
        if (codeAt >= 0) text = text[..codeAt];
        var commentEnd = text.IndexOf("*@", StringComparison.Ordinal);   // the opening comment is for maintainers
        if (commentEnd >= 0) text = text[(commentEnd + 2)..];
        return text.Trim();
    });

    public static string Code(string file) => Cache.GetOrAdd("code:" + file, static key =>
    {
        var text = Read(key[5..]);
        var codeAt = text.LastIndexOf("\n@code", StringComparison.Ordinal);
        if (codeAt < 0) return "";
        var body = text[(text.IndexOf('{', codeAt) + 1)..text.LastIndexOf('}')];
        return Dedent(body.Split('\n'));
    });

    public static string Region(string file, string name) => Cache.GetOrAdd($"region:{name}:{file}", key =>
    {
        var start = new Regex(@"code-tab:" + Regex.Escape(name) + @"(\s|\*|$)");
        var blocks = new List<string>();
        List<string>? block = null;
        foreach (var line in Read(file).Split('\n'))
        {
            if (block is null) { if (start.IsMatch(line)) block = []; }
            else if (line.Contains("/code-tab", StringComparison.Ordinal)) { blocks.Add(Dedent(block)); block = null; }
            else block.Add(line);
        }
        return string.Join("\n\n", blocks);
    });

    private static string Read(string file)
    {
        using var stream = typeof(AppSource).Assembly.GetManifestResourceStream($"FlexCore.Showcase.Source.{file}");
        return stream is null ? "" : new StreamReader(stream).ReadToEnd().Replace("\r\n", "\n");
    }

    // Drops the common leading indent and the blank lines at either end
    private static string Dedent(IEnumerable<string> lines)
    {
        var list = lines.Select(l => l.TrimEnd()).ToList();
        while (list.Count > 0 && list[0].Length == 0) list.RemoveAt(0);
        while (list.Count > 0 && list[^1].Length == 0) list.RemoveAt(list.Count - 1);
        var indent = list.Where(l => l.Length > 0).Select(l => l.Length - l.TrimStart().Length).DefaultIfEmpty(0).Min();
        return string.Join("\n", list.Select(l => l.Length >= indent ? l[indent..] : l.TrimStart()));
    }
}
