using System.Text.RegularExpressions;

namespace FlexCore.Showcase.Components.Shared;

/// <summary>
/// The @using lines a code tab needs in a new Blazor project (2026-10-27): every Fx.ControlKit namespace whose public types the
/// Razor or C# tab names, read from the FlexCore and FlexCore.Documents assemblies, plus the few System namespaces the examples
/// use beyond a new project's implicit usings and the template's _Imports.razor. Names are taken from tags, generic arguments,
/// @-expressions, Type.Member values and C# code; plain text and plain attribute values are skipped so prose does not count.
/// </summary>
internal static partial class CodeUsings
{
    private static readonly Lazy<Dictionary<string, string[]>> Types = new(() =>
        new[] { typeof(Fx.ControlKit.ButtonControl).Assembly, typeof(Fx.ControlKit.Pdf.PdfViewerControl).Assembly }
            .Distinct()
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => !t.IsNested && t.Namespace?.StartsWith("Fx.ControlKit", StringComparison.Ordinal) == true)
            .GroupBy(t => t.Name.Split('`')[0])
            .ToDictionary(g => g.Key, g => g.Select(t => t.Namespace!).Distinct().ToArray()));

    // System namespaces the examples use that a new project does not import already.
    private static readonly (Regex Pattern, string Namespace)[] Framework =
    [
        (new(@"\[(Required|Range|MinLength|MaxLength|StringLength|Display|EmailAddress|Phone|RegularExpression|Compare|Url)\b"), "System.ComponentModel.DataAnnotations"),
        (new(@"\b(CultureInfo|NumberStyles|DateTimeStyles)\b"), "System.Globalization"),
        (new(@"\bStopwatch\b"), "System.Diagnostics"),
        (new(@"\bStringBuilder\b"), "System.Text"),
        (new(@"\bRegex\b"), "System.Text.RegularExpressions"),
        (new(@"\bJsonSerializer\b"), "System.Text.Json"),
        (new(@"\bExpression<"), "System.Linq.Expressions"),
        (new(@"\bObservableCollection<"), "System.Collections.ObjectModel"),
    ];

    /// <summary>The using lines for one example, Razor style (@using X) or C# style (using X;), or "" when it needs none.</summary>
    public static string Header(string? razor, string? csharp, bool razorStyle)
    {
        var code = Strip(razor ?? "", markup: true) + "\n" + Strip(csharp ?? "", markup: false);
        var needed = new SortedSet<string>(StringComparer.Ordinal);
        foreach (Match m in Identifier().Matches(code))
        {
            if (!Types.Value.TryGetValue(m.Value, out var namespaces)) continue;
            needed.Add(namespaces.Length == 1 ? namespaces[0]
                : m.Value == "TreeNode" && code.AsSpan(m.Index + m.Length).TrimStart().StartsWith("<") ? "Fx.ControlKit.Grid"
                : "Fx.ControlKit");
        }
        foreach (var (pattern, ns) in Framework)
            if (pattern.IsMatch(code)) needed.Add(ns);

        var existing = UsingLine().Matches(razor + "\n" + csharp).Select(m => m.Groups[1].Value).ToHashSet();
        var lines = needed.Where(ns => !existing.Contains(ns))
            .OrderBy(ns => ns.StartsWith("System", StringComparison.Ordinal) ? 0 : 1).ThenBy(ns => ns, StringComparer.Ordinal)
            .Select(ns => razorStyle ? $"@using {ns}" : $"using {ns};")
            .ToList();
        return lines.Count == 0 ? "" : string.Join("\n", lines) + "\n\n";
    }

    // Comments, string literals and, in markup, text between tags and plain attribute values (keeping their Type.Member parts).
    private static string Strip(string code, bool markup)
    {
        if (!markup) return StringLiteral().Replace(CSharpComment().Replace(code, " "), "\"\"");
        code = MarkupComment().Replace(code, " ");
        code = TextNode().Replace(code, "><");
        return PlainAttribute().Replace(code, m =>
            m.Groups[1].Value is "TValue" or "TItem" or "TKey" ? m.Value : string.Join(" ", TypeMember().Matches(m.Groups[2].Value).Select(t => t.Value)));
    }

    [GeneratedRegex(@"(?<![.\w])[A-Z][A-Za-z0-9]*\b")] private static partial Regex Identifier();   // not after a dot: members are not types
    [GeneratedRegex(@"^\s*@?using\s+([\w.]+)\s*;?\s*$", RegexOptions.Multiline)] private static partial Regex UsingLine();
    [GeneratedRegex(@"/\*[\s\S]*?\*/|//[^\n]*")] private static partial Regex CSharpComment();
    [GeneratedRegex(@"@\*[\s\S]*?\*@|<!--[\s\S]*?-->")] private static partial Regex MarkupComment();
    [GeneratedRegex(@"@?\$?""(?:[^""\\]|\\.|"""")*""")] private static partial Regex StringLiteral();
    [GeneratedRegex(@">[^<@]*<")] private static partial Regex TextNode();
    [GeneratedRegex(@"\b([A-Za-z][\w-]*)=""(?!@)([^""]*)""")] private static partial Regex PlainAttribute();
    [GeneratedRegex(@"\b[A-Z]\w*\.[A-Z]\w*")] private static partial Regex TypeMember();
}
