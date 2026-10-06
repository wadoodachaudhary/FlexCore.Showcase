namespace FlexCore.Showcase.Components.Shared;

/// <summary>One menu entry. <see cref="Href"/> null makes it a sub-heading row.</summary>
public sealed record NavItem(string Text, string? Href);

/// <summary>One top-bar group. <see cref="Href"/> is where clicking the group label goes;
/// <see cref="Items"/> empty makes it a flat link.</summary>
public sealed record NavGroup(string Label, string Icon, string? Href, string AllLabel, NavItem[] Items);

/// <summary>
/// The single source of truth for site navigation (2026-10-06). The menubar, phone drawer,
/// footer and the Grid overview's link list all read this, so they cannot drift apart.
/// Rules: labels say what the page shows; no internal HomeFront names; a page appears here
/// only if it exists. Phase 2 pages (Forms &amp; pickers, PDF viewer, Gantt, AI prompt,
/// /performance, /migration) are added when they render.
/// </summary>
public static class SiteNav
{
    public static readonly NavGroup[] Groups =
    {
        new("Components", "bi-collection", "components", "All components", new NavItem[]
        {
            new("Inputs & feedback", null),
            new("Buttons", "demo/buttons"),
            new("Dialogs", "demo/dialog"),
            new("Toasts", "demo/notifications"),
            new("UI elements", "demo/new-components"),
            new("Control tour", "demo/counterparts"),          // 7-tab sampler; split in phase 2
            new("Layout & navigation", null),
            new("Page layouts", "demo/layout"),
            new("Tree view", "demo/tree/treeview-explorer"),
        }),
        new("Data Grid", "bi-grid-3x3-gap-fill", "demo/grid", "Grid overview", new NavItem[]
        {
            new("Scenarios", null),
            new("Four scenarios", "demo/grid/scenarios"),
            new("Batch editing", "demo/grid/batch-editing"),
            new("Features", null),
            new("Frozen columns", "demo/grid/advanced"),
            new("Export & templates", "demo/grid/enterprise-suite"),
            new("Wide grids", "demo/grid/column-virtualization"),
            new("Column sets", "demo/grid/column-swap"),
            new("Column reorder", "demo/grid/reordering"),
            new("Typing modes", "demo/grid/typing-modes"),
            new("Unsaved-row lock", "demo/grid/unsaved-row-lock"),
            new("Dynamic columns", "demo/grid/dynamic-columns"),
            new("Data", null),
            new("Grouping & totals", "demo/grid/grouping"),
            new("On-demand loading", "demo/grid/provider-enterprise"),
            new("Master-detail", "demo/grid/master-detail"),
            new("Pivot tables", "demo/pivot-formulas"),
            new("Tree grid", "demo/grid/tree-grid"),
        }),
        new("Charts", "bi-bar-chart-line", "demo/charts", "Charts overview", new NavItem[]
        {
            new("Financial & gauges", "demo/advanced-charts"),
            new("Sunburst", "demo/charts/sunburst"),
        }),
        new("Reports", "bi-file-earmark-spreadsheet", "demo/reports/model-costs", "Report writer", new NavItem[]
        {
            new("PDF viewer (in Control tour)", "demo/counterparts"),   // own page in phase 2
        }),
        // Only pages that measure something and say how. Tree grid load joins when it times the real load.
        new("Performance", "bi-speedometer2", null, "", new NavItem[]
        {
            new("100k rows (scenarios)", "demo/grid/scenarios"),
            new("50k rows", "demo/grid/row-selection-50k"),
            new("Scroll vs paging", "demo/grid/virtualization-bench"),
        }),
    };

    /// <summary>Footer shows the groups that have a landing page, in bar order.</summary>
    public static IEnumerable<NavGroup> FooterGroups => Groups.Where(g => g.Href is not null);

    /// <summary>Items of one group, headings excluded. Used by overview pages for their link lists.</summary>
    public static IEnumerable<NavItem> LinksOf(string groupLabel) =>
        Groups.FirstOrDefault(g => g.Label == groupLabel)?.Items.Where(i => i.Href is not null) ?? Enumerable.Empty<NavItem>();

    /// <summary>True when <paramref name="path"/> (base-relative, no query or fragment) is the group's landing page or one of its links.</summary>
    public static bool IsActive(NavGroup g, string path)
    {
        path = path.Split('?')[0].Split('#')[0].TrimEnd('/');
        if (g.Href is not null && path == g.Href.Split('#')[0]) return true;
        return g.Items.Any(i => i.Href is not null && path == i.Href.Split('#')[0]);
    }
}
