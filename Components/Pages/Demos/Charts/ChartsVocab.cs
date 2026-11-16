// 2026-10-24: chart documentation rows are in the ChartsVocab.*.cs partials (parameters, models, chart types, CSS); the Charts
// page shows all, other chart pages a subset via TypesNamed.
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
