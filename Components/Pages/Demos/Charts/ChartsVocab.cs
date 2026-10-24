// 2026-10-24: the chart documentation rows live in the ChartsVocab.*.cs partials next to this file (parameters, model classes,
// chart types, CSS); the Charts page shows them all, other chart pages show a subset through TypesNamed.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos.Charts;

public static partial class ChartsVocab
{
    /// <summary>The ChartType rows for the named members, in enum order, for pages that show a few types.</summary>
    public static ApiRow[] TypesNamed(params string[] names) => Types
        .Where(r => names.Contains(r.Name))
        .Select(r => r with { Description = r.Description.Replace(" Not in the gallery.", "") })  // the note is about the Charts page
        .ToArray();
}
