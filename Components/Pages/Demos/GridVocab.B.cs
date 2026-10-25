// 2026-10-24: GridControl parameter rows, part B (GridControl.razor.cs from line 593, the partial files) and the AggregateRow and
// AggregateColumn tables, for the Grid overview Documentation. Checked against the read sites; "never read" rows say so.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos;

public static partial class GridVocab
{
    // Toolbar
    public static readonly ApiRow[] B_Toolbar =
    {
        new("Toolbar", "Names of the built-in toolbar buttons, rendered in order. <code>add</code>, <code>edit</code>, <code>delete</code>, <code>csv</code>, <code>excel</code> and <code>pdf</code> run their built-in action; every click also reaches <code>OnToolbarItemClick</code>.", "List&lt;string&gt;?", "null"),
        new("ShowGridToolbar", "Forces the toolbar on or off. <code>null</code> shows it when <code>Toolbar</code> has items, <code>ShowSearchBar</code> is on or a <code>GridToolBarTemplate</code> is set.", "bool?", "null"),
        new("OnToolbarItemClick", "Fires with the item name after the built-in action for that toolbar button has run.", "EventCallback&lt;string&gt;"),
        new("GridToolBarTemplate", "Extra content appended inside the toolbar after the buttons and the search box; setting it makes the toolbar render.", "RenderFragment?", "null"),
    };

    // Advanced view and options rail
    public static readonly ApiRow[] B_OptionsRail =
    {
        new("ShowAdvancedViewToggleButton", "Adds a toggle in the group strip that hides the expression-filter button and the options rail until the user switches to the advanced view. The toggle only renders when one of those tools is configured.", "bool", "false"),
        new("DefaultAdvancedView", "Initial state of the advanced view when <code>ShowAdvancedViewToggleButton</code> is on; read once, after that the toggle owns it.", "bool", "false"),
        new("ShowExpressionFilterButton", "Shows the expression-filter button in the group strip; needs <code>AllowFiltering</code>.", "bool", "false"),
        new("ShowGridOptionsRail", "Renders the side rail; it also needs columns and at least one of the rail buttons below.", "bool", "false"),
        new("ShowColumnOptionsButton", "Column options button on the rail, which opens the column panel.", "bool", "false"),
        new("ShowFilterPanelButton", "Declared, but never read; column filtering lives in each column header popup.", "bool", "false"),
        new("AllowPivoting", "Lets the grid switch into the embedded pivot view; turning it off while in pivot mode returns the grid to its rows.", "bool", "false"),
        new("ShowPivotPanelButton", "Pivot button on the rail; also needs <code>AllowPivoting</code>.", "bool", "false"),
        new("ShowGridThemeToggle", "Theme button on the rail, which opens the Theme panel where the user picks one of the <code>GridTheme</code> schemes.", "bool", "false"),
        new("Theme", "Initial colour scheme; read once, after which the Theme panel owns the active scheme.", "GridTheme", "GridTheme.Default"),
        new("ShowGridBackButton", "Back button on the rail that leaves the pivot view and closes the open panel.", "bool", "false"),
    };

    // Grouping and totals
    public static readonly ApiRow[] B_Grouping =
    {
        new("ShowExpandAllButton", "Declared, but never read.", "bool", "true"),
        new("ShowCollapseAllButton", "Declared, but never read.", "bool", "true"),
        new("GroupColumns", "Fields grouped on load; a new list replaces the live group descriptors on the next parameter set. See <a href=\"demo/grid/grouping\">Grouping &amp; totals</a>.", "List&lt;string&gt;?", "null"),
        new("GroupHeaderTextSelector", "Called with the field, the raw key and the group's items to produce the group header text; a blank result falls back to the key. Collapse state still keys on the raw value.", "Func&lt;string, string, IReadOnlyList&lt;TValue&gt;, string&gt;?", "null"),
        new("FocusGroupHeaderOnContextMenu", "Makes a right-click on a group header the current group before the context-menu event bubbles, so a host handler can read <code>GetCurrentGroup()</code>.", "bool", "false"),
        new("AggregateRows", "Aggregate definitions computed per group and for the grid footer; the tables below describe <code>AggregateRow</code> and <code>AggregateColumn</code>. With an <code>ItemsProvider</code> the fields and types travel as query aggregates instead. See <a href=\"demo/grid/grouping\">Grouping &amp; totals</a>.", "List&lt;AggregateRow&gt;?", "null"),
        new("ShowGroupExpandCollapse", "While groups exist, shows the expand-all/collapse-all toggle and the clear-all-grouping button in the group strip.", "bool", "true"),
        new("HideGroupedColumns", "Removes the grouped fields from the visible columns and renders a placeholder column of the same width for each of them.", "bool", "true"),
        new("ShowGroupCount", "Appends the item count to each group header row.", "bool", "false"),
        new("DefaultGroupsCollapsed", "Starts every group collapsed; applied once on the first parameter set, after which the user's expand and collapse actions own the state. Provider groups honour it too.", "bool", "false"),
        new("GroupedColumnColor", "CSS colour for the grouped value: the header text falls back to it when <code>GroupItemTextColor</code> is empty, and a 12 percent tint of it feeds the <code>--fx-grid-group-color-soft</code> chip background.", "string", "&quot;&quot;"),
        new("GroupItemTextColor", "Text colour of the grouped value on group header rows; empty falls back to <code>GroupedColumnColor</code>.", "string", "&quot;&quot;"),
        new("GroupTotalTextColor", "Text colour of group aggregates: caption totals, header-row totals and footer totals. Empty keeps the stylesheet colours.", "string", "&quot;&quot;"),
        new("GroupExpandIconStyle", "Built-in expand/collapse glyph preset for group headers, <code>PlusMinus</code> or <code>Triangle</code>; it picks the glyph pair, the CSS class and the inline style unless the overrides below replace them.", "GroupExpandIconStyle", "GroupExpandIconStyle.PlusMinus"),
        new("CollapsedGlyph", "Glyph shown on a collapsed group instead of the preset's plus or right-pointing triangle.", "string?", "null"),
        new("ExpandedGlyph", "Glyph shown on an expanded group instead of the preset's minus or down-pointing triangle.", "string?", "null"),
        new("ExpandIconStyle", "Inline CSS applied to the glyph span instead of the preset's style.", "string?", "null"),
        new("ExpandIconTemplate", "Replaces the glyph span entirely; receives <code>true</code> when the group is expanded and must handle its own click.", "RenderFragment&lt;bool&gt;?", "null"),
        new("GroupFooterTemplate", "Declared, but never read; group footers are built from <code>AggregateRows</code>. See <a href=\"demo/grid/grouping\">Grouping &amp; totals</a>.", "RenderFragment&lt;object&gt;?", "null"),
        new("GroupFootersAlwaysVisible", "Declared, but never read. See <a href=\"demo/grid/grouping\">Grouping &amp; totals</a>.", "bool", "false"),
    };

    // Column header menu, cell menu and reorder
    public static readonly ApiRow[] B_HeaderMenu =
    {
        new("AllowColumnReorder", "Lets a header be dragged onto another header's drop zone to move the column. Drag-to-group is wired separately through the group strip. See <a href=\"demo/grid/reordering\">Column reorder</a>.", "bool", "true"),
        new("EnableHeaderContextMenu", "Opens the built-in menu on a header right-click; off, the event bubbles to the host unless <code>ShowColumnMenu</code> is on.", "bool", "true"),
        new("HeaderContextMenuShowsColumns", "Replaces the header menu's commands with a checked list of columns that toggles their visibility.", "bool", "false"),
        new("HeaderClickShowsColumns", "A left-click on a header opens that checked column list instead of sorting; needs the header menu or <code>ShowColumnMenu</code>.", "bool", "false"),
        new("OnHeaderAttachmentsRequested", "Adds an Attachments item to the header menu and fires with the column's field; without a handler the item is not rendered.", "EventCallback&lt;string&gt;"),
        new("EnableCellContextMenu", "Opens a Cut, Copy, Paste and Delete menu on a data-cell right-click, except while a row or batch edit is active. Off, the right-click reaches <code>OnContextMenu</code>.", "bool", "false"),
        new("ColumnReorderPipeColor", "CSS colour of the vertical insertion pipe shown while a header is dragged; blank falls back to the default.", "string", "&quot;#2b2b2b&quot;"),
        new("ColumnRenameMaxLength", "Longest caption the header rename box accepts; 0 removes the page limit. A registered <code>IGridColumnCaptionLimit</code> can lower it, never raise it.", "int", "255"),
        new("ShowColumnMenu", "Adds sort, filter, visibility, auto-fit and left/right locking commands to the header menu, and lets that menu open even when <code>EnableHeaderContextMenu</code> is off.", "bool", "false"),
        new("ShowColumnMenuButton", "Three-dot button on each header that opens the header menu; needs <code>EnableHeaderContextMenu</code> or <code>ShowColumnMenu</code>.", "bool", "false"),
    };

    // Print and export
    public static readonly ApiRow[] B_Print =
    {
        new("ShowPrintOptionsDialog", "The header menu's Print command opens the print-options dialog seeded from the defaults below; off, it exports a PDF with those defaults at once. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "bool", "true"),
        new("DefaultPrintOrientation", "Page orientation the print options start from. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "GridPdfOrientation", "GridPdfOrientation.Portrait"),
        new("DefaultPrintPageSize", "Page size (<code>Letter</code>, <code>Legal</code> or <code>A4</code>) the print options start from. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "GridPdfPageSize", "GridPdfPageSize.Letter"),
        new("DefaultPrintColumnLayout", "How columns meet the page width: <code>WrapText</code>, <code>ClipText</code> or <code>FitColumnsToPage</code>. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "GridPdfColumnLayout", "GridPdfColumnLayout.WrapText"),
        new("DefaultPrintGridLines", "Whether the printed table draws grid lines. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "bool", "true"),
        new("DefaultPrintZoomMode", "<code>FitToPage</code> or <code>Percent</code>; the latter uses <code>DefaultPrintZoomPercent</code>. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "GridPdfZoomMode", "GridPdfZoomMode.FitToPage"),
        new("DefaultPrintZoomPercent", "Zoom for the <code>Percent</code> mode, clamped to 25 to 200. A change to any print default re-seeds the options unless the dialog is open. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "int", "100"),
        new("IncludeTotalsInExport", "Appends the <code>AggregateRows</code> marked <code>ShowInFooter</code> as total rows at the end of every export table. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "bool", "true"),
        new("DelimitedExportEncoding", "Text encoding of CSV and TSV files; the other formats ignore it. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "GridDelimitedTextEncoding", "GridDelimitedTextEncoding.Utf8NoBom"),
        new("ExportAllProviderItems", "With an <code>ItemsProvider</code>, exports page through the whole current query instead of the loaded window. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "bool", "false"),
        new("ProviderExportPageSize", "Rows requested per page during a complete provider export, clamped to 1 to 100000. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "int", "1_000"),
        new("ProviderExportProgressChanged", "Raised after each received export page and once more on completion or cancellation. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "EventCallback&lt;GridProviderExportProgress&gt;"),
    };

    // Choose Columns, layout persistence and state
    public static readonly ApiRow[] B_ColumnChooser =
    {
        new("AvailableColumns", "Schema for the Choose Columns dialog and the column panel, so columns not currently rendered can be listed unchecked. With no <code>GridColumn</code> children and auto-generation off it also supplies transient columns. Null falls back to the rendered columns. See <a href=\"demo/grid/dynamic-columns\">Dynamic columns</a>.", "IEnumerable&lt;ChooseColumnDescriptor&gt;?", "null"),
        new("DefaultColumns", "Layout applied by the dialog's Restore Default Layout button: listed columns take its visibility and order, the others keep theirs. Null restores the snapshot taken when the dialog opened.", "IEnumerable&lt;ChooseColumnDescriptor&gt;?", "null"),
        new("OnColumnsChosen", "Fires on OK in the Choose Columns dialog with the chosen visibility and order; a subscribed host applies the layout itself and the grid only persists it. Unsubscribed, the grid applies it.", "EventCallback&lt;ChooseColumnsResult&gt;"),
        new("OnLayoutChanged", "Fires with a <code>GridSettings</code> snapshot after the user reorders, resizes, groups, hides or renames a column.", "EventCallback&lt;GridSettings&gt;"),
        new("PersistenceKey", "Key under which an <code>IGridSettingsStore</code> registered in DI loads the saved layout once columns exist and saves it after each user change; empty, or no store, disables persistence.", "string?", "null"),
        new("ColumnChooserTemplate", "Replaces the body of the Choose Columns dialog; receives a <code>GridColumnChooserContext</code>.", "RenderFragment&lt;GridColumnChooserContext&gt;?", "null"),
        new("OnStateChanged", "Fires with a detached <code>GridState</code> snapshot and the kind of change after user-driven state changes; suppressed while a state is being applied.", "EventCallback&lt;GridStateChangedEventArgs&gt;"),
    };

    // Chart view
    public static readonly ApiRow[] B_Chart =
    {
        new("ShowAsChart", "Renders each row as a mini bar chart through <code>ChartControl</code> instead of the table; needs <code>ChartValueFields</code>. Row windowing and column virtualization are off in this view.", "bool", "false"),
        new("ChartValueFields", "Properties whose numeric values become the bars of each row.", "IList&lt;string&gt;?", "null"),
        new("ChartLabelField", "Property used as each row's label; null uses the first visible column's field.", "string?", "null"),
        new("ChartValueLabels", "Axis labels for the value fields, passed to the chart.", "IList&lt;string&gt;?", "null"),
        new("ChartBarColor", "Bar colour passed to the chart.", "string", "&quot;#2563eb&quot;"),
        new("ChartShowValues", "Prints each bar's value above it.", "bool", "false"),
    };

    // Settings objects and direct row events
    public static readonly ApiRow[] B_Settings =
    {
        new("FilterSettingsRef", "Plain <code>FilterSettings</code> object read for the filter type, case sensitivity, search-as-you-type, the immediate-mode delay and the filter-row operators.", "FilterSettings?", "null"),
        new("PageSettingsRef", "Plain <code>PageSettings</code> object whose <code>PageSize</code>, <code>PageSizes</code> and <code>PageCount</code> take precedence over the grid's own paging parameters.", "PageSettings?", "null"),
        new("EditSettingsRef", "Plain <code>EditSettings</code> object that turns editing on: mode, adding, deleting, confirmation and new-row position are all read from it. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "EditSettings?", "null"),
        new("SelectionSettingsRef", "Plain <code>SelectionSettings</code> object read for the selection mode, the checkbox column and checkbox-only selection.", "SelectionSettings?", "null"),
        new("EventsRef", "Plain <code>GridControlEvents</code> bundle; each grid event is raised through it when the matching callback has a delegate.", "GridControlEvents&lt;TValue&gt;?", "null"),
        new("RowResizing", "Fires while a row is being resized, after the <code>EventsRef</code> counterpart; set <code>Cancel</code> to keep the old height or change <code>NewHeight</code>.", "EventCallback&lt;RowResizeEventArgs&lt;TValue&gt;&gt;"),
        new("RowResized", "Fires when the resize ends, with the final height.", "EventCallback&lt;RowResizeEventArgs&lt;TValue&gt;&gt;"),
        new("RowReordering", "Fires before a dragged row is moved; set <code>Cancel</code> to abandon the drop.", "EventCallback&lt;RowReorderEventArgs&lt;TValue&gt;&gt;"),
        new("RowReordered", "Fires after the row has moved and been selected, with the resolved <code>NewIndex</code>.", "EventCallback&lt;RowReorderEventArgs&lt;TValue&gt;&gt;"),
    };

    // Adding, cloning and validating rows
    public static readonly ApiRow[] B_Editing =
    {
        new("NewItemFactory", "Creates the item for a new row; null uses the parameterless constructor and throws when there is none. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "Func&lt;TValue&gt;?", "null"),
        new("EnsureTrailingNewRow", "Keeps one blank new row at the end of an <code>IList</code> data source while <code>EditSettings.AllowAdding</code> is on.", "bool", "false"),
        new("IsTrailingNewRow", "Tells the grid whether its tracked trailing row is still blank; null, or a predicate that throws, counts as blank.", "Func&lt;TValue, bool&gt;?", "null"),
        new("BlankTrailingNewRowText", "Shows the tracked trailing row with empty cells while it is blank, whatever defaults the factory gave it; needs <code>IsTrailingNewRow</code>.", "bool", "false"),
        new("AddNewRowOnLastCellExit", "Moving forward from the last navigable cell appends a new row and starts editing its first editable cell; needs <code>EditSettings.AllowAdding</code> and an <code>IList</code> data source.", "bool", "false"),
        new("CanAddNewRowOnLastCellExit", "Guard called with the current row before that append; returning false leaves focus at the grid edge.", "Func&lt;TValue, bool&gt;?", "null"),
        new("CloneFactory", "Copies a row before editing; null creates a new item and copies the values across, entry by entry for dictionary-shaped rows and property by property otherwise.", "Func&lt;TValue, TValue&gt;?", "null"),
        new("ValidationSettings", "Opts row and batch editing into <code>EditContext</code> validation: data annotations, a custom validator and validation on each field change. Null performs no model validation.", "GridValidationSettings&lt;TValue&gt;?", "null"),
    };

    // Row windowing and scrolling
    public static readonly ApiRow[] B_Scrolling =
    {
        new("WindowOverscanRows", "Rows rendered above and below the visible slice, and the ceiling for the cell budget. See <a href=\"demo/grid/row-selection-50k\">100k rows</a>.", "int", "60"),
        new("WindowOverscanCellBudget", "Per-side overscan budget in cells: the effective row overscan is the budget divided by the visible column count, clamped between 15 (or <code>WindowOverscanRows</code> when lower) and <code>WindowOverscanRows</code>. 0 disables the budget. See <a href=\"demo/grid/row-selection-50k\">100k rows</a>.", "int", "900"),
        new("DeferredScrollOverscanRows", "Rows buffered around the destination when a thumb release commits with <code>ScrollTrack</code> off; never below the refresh guard nor above the effective overscan.", "int", "20"),
        new("ScrollTrack", "True tracks the vertical thumb live. False renders a browser-owned proxy thumb that previews locally and commits the destination window in one server render on release.", "bool", "true"),
        new("WheelScrollScale", "Multiplier on vertical wheel and trackpad travel while the proxy thumb owns scrolling, clamped to 0.1 to 2.", "double", "1d"),
        new("EnableScrollBoundarySlowdown", "Slows wheel movement inside the prefetch zone near the rendered edge; only in force together with <code>EnableScrollBoundaryGuard</code>.", "bool", "false"),
        new("EnableAdaptiveWheelScrollPacing", "Learns a per-grid wheel rate from the browser's acknowledgement time; needs the guard and the slowdown.", "bool", "false"),
        new("EnableScrollBoundaryGuard", "Pauses wheel momentum on the last rendered row until the next window arrives, so a spacer is never shown; only while <code>ScrollTrack</code> is false.", "bool", "false"),
        new("EnableScrollBoundaryTelemetry", "Turns on browser-side wheel-window telemetry for the bench pages; only while <code>ScrollTrack</code> is false.", "bool", "false"),
        new("DiagnosticWindowScrollDelayMs", "Bench-only delay, clamped to 0 to 5000 ms, added to each row-window callback of this grid.", "int", "0"),
        new("MinimumRowsForWindowing", "Collection data sources with this many rows or fewer skip row windowing; 0 windows every grid, and sources without a count keep windowing.", "int", "0"),
        new("EnableGroupedRowWindowing", "Windows grouped grids over a flattened header, item and footer stream; off, a grouped grid renders every row.", "bool", "true"),
        new("PerformanceMode", "<code>ServerOptimized</code> and <code>ClientPreviewAndServerOptimized</code> keep the server-side row lookup and render caches; <code>ClientPreview</code> and the combined mode move the cursor in the browser before the server render when <code>ShowActiveCell</code> is on.", "GridPerformanceMode", "GridPerformanceMode.ClientPreviewAndServerOptimized"),
    };

    // Filtering and sorting
    public static readonly ApiRow[] B_Filtering =
    {
        new("FilterMode", "<code>Simple</code>, <code>SimpleWithMenu</code> and <code>Excel</code> add a filter row under the header and count it in the ARIA row count; the grid reads no other value.", "GridFilterMode", "GridFilterMode.None"),
        new("ShowMultiColumnSortingIndex", "Shows the sort priority badge next to the sort arrow when several columns are sorted.", "bool", "false"),
    };

    // Master-detail
    public static readonly ApiRow[] B_MasterDetail =
    {
        new("DetailTemplate", "Renders a detail row under each expanded item and enables the expand column and the <code>ExpandRow</code> and <code>CollapseRow</code> API. Not allowed with a flat <code>ItemsProvider</code>. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "RenderFragment&lt;TValue&gt;?", "null"),
        new("ExpandMode", "<code>Single</code> collapses the other rows when one expands; <code>Multiple</code> allows several and the expand-all button. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "GridExpandMode", "GridExpandMode.Multiple"),
        new("ShowExpandColumn", "Shows the expand column when a <code>DetailTemplate</code> is set. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "bool", "true"),
        new("ShowExpandAll", "Puts an expand-all/collapse-all button in the expand column's header; only in <code>Multiple</code> mode. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "bool", "false"),
        new("RowExpand", "Fires with the item after it has been expanded. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "EventCallback&lt;TValue&gt;"),
        new("RowCollapse", "Fires with the item after it has been collapsed. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "EventCallback&lt;TValue&gt;"),
    };

    // Column generation, header bands and column virtualization
    public static readonly ApiRow[] B_Columns =
    {
        new("AutoGenerateColumns", "Builds the columns from the public readable properties of <code>TValue</code> and exposes them through <code>Columns</code>. See <a href=\"demo/grid/dynamic-columns\">Dynamic columns</a>.", "bool", "false"),
        new("ColumnHeaderBands", "One-level parent header bands over the listed fields; an entry here wins over a column's <code>HeaderBand</code>. Bands disable column virtualization. See <a href=\"demo/grid/dynamic-columns\">Dynamic columns</a>.", "IReadOnlyList&lt;GridColumnHeaderBand&gt;?", "null"),
        new("EnableColumnVirtualization", "Renders only the horizontally visible non-frozen columns plus the overscan; frozen columns stay mounted. Falls back to the full column set for pivot, chart, grouped, inline-editing, banded, templated, stacked and adaptive layouts. See <a href=\"demo/grid/column-virtualization\">Wide grids</a>.", "bool", "false"),
        new("ColumnVirtualizationOverscan", "Extra non-frozen columns kept on each side of the viewport. See <a href=\"demo/grid/column-virtualization\">Wide grids</a>.", "int", "2"),
        new("ColumnVirtualizationDefaultWidth", "Pixel width assumed for columns with no pixel <code>Width</code> and no runtime resize; 0 makes such a column force the full render. See <a href=\"demo/grid/column-virtualization\">Wide grids</a>.", "double", "0"),
        new("ShowColumnVirtualizationFallback", "Shows a status line explaining why virtualization was requested but the grid fell back to the full render. See <a href=\"demo/grid/column-virtualization\">Wide grids</a>.", "bool", "true"),
    };

    // On-demand loading
    public static readonly ApiRow[] B_Provider =
    {
        new("ItemsProvider", "Range provider that replaces <code>DataSource</code>; needs a bounded <code>Height</code> and, without provider grouping, fixed-height rows. Filtering, sorting and grouping become request descriptors. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "GridItemsProvider&lt;TValue&gt;?", "null"),
        new("ItemsProviderContextKey", "Opaque value sent with every request; a change starts a new query generation from the first row.", "object?", "null"),
        new("ItemKeySelector", "Stable key for rendering, selection state and grid state when provider calls return new instances of the same row. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "Func&lt;TValue, object?&gt;?", "null"),
        new("ItemsProviderCacheSize", "Completed ranges kept per grid, clamped to 1 to 8.", "int", "4"),
        new("EnableProviderFilterValueRequests", "Lets the Excel-style checklist ask the provider for distinct values with the <code>FilterValues</code> purpose; off, a partial checklist selection cannot be applied. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "bool", "false"),
        new("ProviderFilterValueRequestSize", "Distinct values requested for one checklist, clamped to 1 to 100000.", "int", "1_000"),
        new("EnableProviderGrouping", "Provider-executed lazy grouping when <code>AllowGrouping</code> and group descriptors are active; groups and their rows load on expansion. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "bool", "false"),
        new("ProviderGroupPageSize", "Groups requested per lazy group page, clamped to 1 to 10000. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "int", "100"),
        new("ProviderGroupItemPageSize", "Leaf rows requested per lazy group page, clamped to 1 to 100000. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "int", "250"),
        new("ProviderGroupsInitiallyCollapsed", "Newly received provider groups start collapsed; off, <code>DefaultGroupsCollapsed</code> decides. See <a href=\"demo/grid/provider-enterprise\">On-demand loading</a>.", "bool", "true"),
    };

    // Presentation, paging and accessibility
    public static readonly ApiRow[] B_Presentation =
    {
        new("GridLines", "Which cell borders the stylesheet draws: <code>Both</code>, <code>Horizontal</code>, <code>Vertical</code> or <code>None</code>; <code>Default</code> means <code>Both</code>.", "GridLines", "GridLines.Both"),
        new("Density", "<code>Compact</code> adds the <code>fx-grid-density-compact</code> class for tighter rows.", "GridDensity", "GridDensity.Default"),
        new("PagerPosition", "Where the pager renders while paging is active: bottom, top or both.", "GridPagerPosition", "GridPagerPosition.Bottom"),
        new("PagerTemplate", "Replaces the pager contents; receives a <code>GridPagerContext</code> with the page numbers and navigation callbacks.", "RenderFragment&lt;GridPagerContext&gt;?", "null"),
        new("RowTemplate", "Emits all data cells of a flat row itself, so built-in cell editing in that row is host-owned. Turns off row windowing and column virtualization, and is rejected with a flat <code>ItemsProvider</code>.", "RenderFragment&lt;GridRowTemplateContext&lt;TValue&gt;&gt;?", "null"),
        new("NoDataTemplate", "Content of the empty row when the current view has no records.", "RenderFragment?", "null"),
        new("DataLayoutMode", "<code>Stacked</code> adds the stacked-layout class and turns off row windowing and column virtualization; rejected with a flat <code>ItemsProvider</code>.", "GridDataLayoutMode", "GridDataLayoutMode.Columns"),
        new("AdaptiveMode", "<code>Auto</code> adds the adaptive class; any value other than <code>None</code> turns off row windowing and column virtualization and is rejected with a flat <code>ItemsProvider</code>.", "GridAdaptiveMode", "GridAdaptiveMode.None"),
        new("StackedColumnsCount", "Sets the <code>--fx-grid-stacked-columns</code> variable for the stacked layout, clamped to 1 to 6.", "int", "1"),
        new("AccessibleLabel", "The table's <code>aria-label</code>.", "string", "&quot;Data grid&quot;"),
        new("CustomKeyboardShortcuts", "Maps gestures such as <code>Ctrl+Home</code> or <code>Alt+ArrowDown</code> to a <code>GridKeyboardCommand</code>, matched case-insensitively before the built-in keys.", "IReadOnlyDictionary&lt;string, GridKeyboardCommand&gt;?", "null"),
    };

    // AggregateRow (Properties)
    public static readonly ApiRow[] AggregateRow =
    {
        new("Columns", "The aggregates in this row; each <code>AggregateColumn</code> yields one value per group and one for the grid footer.", "List&lt;AggregateColumn&gt;", "new()"),
        new("ShowInGroupFooter", "Renders the row as a footer under each group's items; it is also counted in the grouped-window height math.", "bool", "true"),
        new("ShowInFooter", "Renders the row in the grid footer across all data, and includes it in exports while <code>IncludeTotalsInExport</code> is on.", "bool", "false"),
        new("ShowInGroupCaption", "Appends each value inline to the group header label, formatted with <code>GroupCaptionTemplate</code>.", "bool", "false"),
        new("ShowInGroupHeader", "Adds column-aligned cells to the group header row from the first aggregate column onward, formatted with <code>GroupCaptionTemplate</code> or else <code>GroupFooterTemplate</code>.", "bool", "false"),
    };

    // AggregateColumn (Properties)
    public static readonly ApiRow[] AggregateColumn =
    {
        new("Field", "Property aggregated; with an <code>ItemsProvider</code> it is sent as a query aggregate.", "string", "&quot;&quot;"),
        new("Type", "<code>Sum</code>, <code>Average</code>, <code>Count</code>, <code>Min</code> or <code>Max</code>; field and type together key the computed value.", "AggregateType", "AggregateType.Sum"),
        new("Format", ".NET format string applied in the current culture when the value is <code>IFormattable</code>.", "string?", "null"),
        new("FooterTemplate", "Text with a <code>{value}</code> placeholder used for the grid footer and as the fallback for the other templates; Grand Total, Sub Total and Total prefixes are stripped from the result.", "string", "&quot;{value}&quot;"),
        new("GroupFooterTemplate", "Template for group footers and, failing a caption template, for header-row cells; null falls back to <code>FooterTemplate</code>.", "string?", "null"),
        new("GroupCaptionTemplate", "Template for the inline caption values and the header-row cells; null falls back to <code>FooterTemplate</code> for captions and to <code>GroupFooterTemplate</code> for header-row cells.", "string?", "null"),
    };
}
