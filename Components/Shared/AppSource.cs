using System.Collections.Concurrent;

namespace FlexCore.Showcase.Components.Shared;

/// <summary>
/// Markup of the home page's app-window pages for its Razor tab (2026-10-29). FlexCore.Showcase.csproj embeds the Getaways*.razor
/// files; <see cref="Markup"/> returns one from the end of its opening comment up to its @code block, so the tab always shows the
/// page exactly as it is built.
/// </summary>
public static class AppSource
{
    private static readonly ConcurrentDictionary<string, string> Cache = new();

    public static string Markup(string file) => Cache.GetOrAdd(file, static f =>
    {
        using var stream = typeof(AppSource).Assembly.GetManifestResourceStream($"FlexCore.Showcase.Source.{f}");
        if (stream is null) return "";
        var text = new StreamReader(stream).ReadToEnd().Replace("\r\n", "\n");
        var codeAt = text.LastIndexOf("\n@code", StringComparison.Ordinal);
        if (codeAt >= 0) text = text[..codeAt];
        var commentEnd = text.IndexOf("*@", StringComparison.Ordinal);   // the opening comment is for maintainers
        if (commentEnd >= 0) text = text[(commentEnd + 2)..];
        return text.Trim();
    });
}
