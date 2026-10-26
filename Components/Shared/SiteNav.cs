namespace FlexCore.Showcase.Components.Shared;

/// <summary>One menu entry. <see cref="Href"/> null makes it a sub-heading row.</summary>
public sealed record NavItem(string Text, string? Href);

/// <summary>One top-bar group. <see cref="Href"/> is where clicking the group label goes;
/// <see cref="Items"/> empty makes it a flat link.</summary>
public sealed record NavGroup(string Label, string Icon, string? Href, string AllLabel, NavItem[] Items);

/// <summary>
/// The single source of truth for site navigation (2026-10-06). The menubar, phone drawer,
/// footer and the Grid overview's link list all read this, so they cannot drift apart.
/// Rules: labels say what the page shows; no internal application names; a page appears here
/// only if it exists. Phase 2 pages (AI prompt,
/// /performance, /migration) are added when they render.
/// </summary>
public static class SiteNav
{
    public static readonly NavGroup[] Groups =
    {
        new("Components", "bi-collection", "components", "All components", AtoZ(new NavItem[]
        {
            new("Inputs & feedback", null),
            new("Buttons", "demo/buttons"),
            new("Split button", "demo/split-button"),
            new("Toggle buttons", "demo/toggle-buttons"),
            new("Floating action button", "demo/fab"),
            new("Dialogs", "demo/dialog"),
            new("Toasts", "demo/notifications"),
            new("Sliders", "demo/sliders"),
            new("Security code", "demo/security-code"),
            new("Date & time", "demo/date-time"),
            new("Checks & switches", "demo/checks"),
            new("Forms", "demo/forms"),                        // own page since 2026-10-19; was a Control tour tab
            new("Pickers & selectors", "demo/pickers"),        // own page since 2026-10-19; was the tour's Selection tab
            new("Text inputs", "demo/text-inputs"),            // 2026-10-25, SC-347
            new("Lists & dropdowns", "demo/lists-dropdowns"),  // 2026-10-25, SC-347
            new("Rich text editor", "demo/rich-text-editor"),  // 2026-10-25, SC-347
            new("AI prompt", "demo/ai-prompt"),                // 2026-10-25: replaced the Control tour (demo/counterparts forwards here)
            new("Layout & navigation", null),
            new("Page layouts", "demo/layout"),
            new("Page navigation", "demo/page-navigation"),               // PageControl: keyboard navigation graph (2026-10-20)
            new("Dock manager", "demo/dock-manager"),           // own page since 2026-10-19; was the tour's Docking tab
            new("Tabs & accordion", "demo/tabs-accordion"),
            new("Tree view", "demo/tree/treeview-explorer"),
            new("Display", null),
            new("Cards", "demo/cards"),
            new("UI elements", "demo/new-components"),
            new("Barcodes & QR", "demo/barcodes"),
            new("Scheduler", "demo/scheduler"),                // 2026-10-25, SC-347
            new("Diagram", "demo/diagram"),                    // 2026-10-25, SC-347
        })),
        new("Data Grid", "bi-grid-3x3-gap-fill", "demo/grid", "Grid overview", AtoZ(new NavItem[]
        {
            new("Scenarios", null),                                       // the four use cases of the retired Four scenarios page (2026-10-23)
            new("Live updates", "demo/grid/live-updates"),
            new("Org chart", "demo/grid/tree-grid#org"),
            new("Warehouse detail", "demo/grid/master-detail#warehouses"),
            new("100k rows", "demo/grid/row-selection-50k"),              // also under Performance; the top bar underlines Data Grid, the first group that lists it
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
            new("Pivot tables", "demo/pivot"),
            new("Tree grid", "demo/grid/tree-grid"),
        })),
        new("Charts", "bi-bar-chart-line", "demo/charts", "Charts overview", AtoZ(new NavItem[]
        {
            new("Financial & gauges", "demo/advanced-charts"),
            new("Sunburst", "demo/charts/sunburst"),
            new("Map story", "demo/charts/map-story"),
            new("Gantt chart", "demo/gantt"),                    // own page since 2026-10-19; was a Control tour tab
            new("Gauges & stock chart", "demo/gauges"),          // own page since 2026-10-19; was the tour's Charts tab
        })),
        new("Reports", "bi-file-earmark-spreadsheet", "demo/reports/package-costs", "Report writer", AtoZ(new NavItem[]
        {
            new("PDF viewer", "demo/pdf-viewer"),                  // own page since 2026-10-19; was a Control tour tab
        })),
        // Only pages that measure something and say how. Tree grid load joins when it times the real load.
        new("Performance", "bi-speedometer2", null, "", AtoZ(new NavItem[]
        {
            new("100k rows", "demo/grid/row-selection-50k"),
            new("Scroll vs paging", "demo/grid/virtualization-bench"),
            new("Live updates", "demo/grid/live-updates"),                // also under Data Grid; measures server render per tick
        })),
    };

    /// <summary>
    /// Menu order (Baaria, 2026-10-26): the group's overview link (<see cref="NavGroup.AllLabel"/>) stays first, then the
    /// sub-headings A to Z, and the items under each heading A to Z. Items listed before the first heading come first, A to Z.
    /// The arrays above may be written in any order.
    /// </summary>
    private static NavItem[] AtoZ(NavItem[] items)
    {
        var sections = new List<(NavItem? Heading, List<NavItem> Items)> { (null, new()) };
        foreach (var item in items)
        {
            if (item.Href is null) sections.Add((item, new()));
            else sections[^1].Items.Add(item);
        }
        var byText = StringComparer.OrdinalIgnoreCase;
        return sections.Take(1)
            .Concat(sections.Skip(1).OrderBy(sec => sec.Heading!.Text, byText))
            .SelectMany(sec => (sec.Heading is null ? Array.Empty<NavItem>() : new[] { sec.Heading })
                .Concat(sec.Items.OrderBy(i => i.Text, byText)))
            .ToArray();
    }

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
