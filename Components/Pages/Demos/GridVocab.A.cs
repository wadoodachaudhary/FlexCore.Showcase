// 2026-10-24: GridControl parameter rows, part A (GridControl.razor.cs lines 240-592), for the Grid overview Documentation.
// Checked against the read sites, not just the declarations; "never read" rows say so.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos;

public static partial class GridVocab
{
    // Data binding
    public static readonly ApiRow[] A_Data =
    {
        new("DataSource", "The rows to show, enumerated for display, filtering, sorting and export. Cannot be set together with <code>ItemsProvider</code>.", "IEnumerable&lt;TValue&gt;?", "null"),
        new("ChildContent", "Holds the column definitions. Rendered hidden inside a cascading <code>IGridOwner</code> so each <code>GridColumn</code> registers itself with the grid. See <a href=\"demo/grid/column-swap\">Column sets</a>.", "RenderFragment?", "null"),
    };

    // Size and layout
    public static readonly ApiRow[] A_Layout =
    {
        new("Height", "CSS height of the grid. A bounded height makes the body the scroll container and is required by row windowing and <code>ItemsProvider</code>; without one a local source larger than <code>AutoPageRowThreshold</code> falls back to paging.", "string?", "null"),
        new("Width", "CSS width of the grid host; blank leaves the width to the container.", "string?", "null"),
        new("WidthMode", "<code>FillAvailable</code> stretches the table to the container and overflows only when the columns need more; <code>FitColumns</code> keeps every column at its own width, so the table may end short of the container or scroll horizontally.", "GridWidthMode", "GridWidthMode.FillAvailable"),
        new("EnableViewportSafeSizing", "Wraps a pixel, viewport or calc <code>Height</code> or <code>Width</code> in <code>min()</code> with the matching viewport-safe limit when it could exceed it; percentages pass through unchanged.", "bool", "true"),
        new("ViewportSafeMaxWidth", "Upper bound applied to <code>Width</code> by viewport-safe sizing; blank applies none.", "string", "\"\""),
        new("ViewportSafeMaxHeight", "Upper bound applied to <code>Height</code> by viewport-safe sizing; blank applies none.", "string", "\"\""),
        new("ExtendVerticalScrollbarIntoHeader", "Adds the <code>fx-grid-vscroll-header-gutter</code> class, a header-height scrollbar gutter so the vertical scrollbar appears to start at the header rather than at the first data row.", "bool", "false"),
        new("ShowHeader", "False skips the <code>thead</code> entirely, including header bands and the filter row, and reports zero header rows to assistive technology.", "bool", "true"),
    };

    // Context menu
    public static readonly ApiRow[] A_ContextMenu =
    {
        new("OnContextMenu", "Raised from the grid host's <code>contextmenu</code> event; attaching a handler also suppresses the browser menu.", "EventCallback&lt;MouseEventArgs&gt;"),
        new("PreventDefaultContextMenu", "Suppresses the browser context menu on the host even with no <code>OnContextMenu</code> handler and no cell context menu.", "bool", "false"),
        new("StopContextMenuPropagation", "Stops the <code>contextmenu</code> event from bubbling past the grid host.", "bool", "false"),
    };

    // Rows
    public static readonly ApiRow[] A_Rows =
    {
        new("RowHeight", "Fixed row height in px; 0 lets rows size to content, and row windowing then measures them. Never resolves below <code>MinRowHeight</code>.", "int", "0"),
        new("AllowRowResizing", "Shows a drag handle on the last data cell so users can resize rows; a resized height overrides <code>RowHeight</code>. Incompatible with <code>ItemsProvider</code> and turns off fixed-pitch row windowing.", "bool", "false"),
        new("AllowRowReorder", "Shows a row-header drag handle for moving rows to a new position; hidden while the grid is grouped.", "bool", "false"),
        new("RowReorderPredicate", "Per-row gate for the reorder handle; null shows it on every row.", "Func&lt;TValue, bool&gt;?", "null"),
        new("ShowRowSelectorHandle", "Shows a narrow row-selector handle column, grid chrome rather than a data column; hidden while the grid is grouped.", "bool", "false"),
        new("RowSelectorHandleShape", "Look of the handle: <code>HalfButton</code>, <code>Button</code>, <code>CheckBox</code> or <code>Flat</code>. It sets the handle's size, border, radius and background, and <code>Flat</code> adds <code>fx-row-selector-cell-flat</code> to the cell.", "GridRowSelectorHandleShape", "GridRowSelectorHandleShape.HalfButton"),
        new("RowSelectorHandleWidth", "Width of the handle column in px, clamped to 12-40.", "int", "18"),
        new("RowSelectorHandlePredicate", "Per-row gate for the selector handle; null shows it on every row, and a predicate that throws hides it.", "Func&lt;TValue, bool&gt;?", "null"),
        new("RowSelectorHandleEmphasisPredicate", "Marks rows whose selector handle draws emphasised; null emphasises none.", "Func&lt;TValue, bool&gt;?", "null"),
        new("MinRowHeight", "Floor in px for every resolved row height: <code>RowHeight</code>, <code>RowHeightSelector</code> results and user resizes.", "double", "16"),
        new("RowHeightSelector", "Per-row height in px, called with the item and its resolved row index; null or a non-positive result falls back to <code>RowHeight</code>. Incompatible with <code>ItemsProvider</code>.", "Func&lt;TValue, int, double?&gt;?", "null"),
        new("RowCssClassSelector", "Extra CSS class for a data row, called with the item and row index.", "Func&lt;TValue, int, string?&gt;?", "null"),
        new("RowExportStyleSelector", "Row style applied when exporting to Excel or HTML, called with the item and row index. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "Func&lt;TValue, int, GridExportCellStyle?&gt;?", "null"),
        new("CellExportStyleSelector", "Cell style applied when exporting, called with the item, field and row index. See <a href=\"demo/grid/enterprise-suite\">Export &amp; templates</a>.", "Func&lt;TValue, string, int, GridExportCellStyle?&gt;?", "null"),
        new("RowTooltipSelector", "Text for the data row's <code>title</code> attribute, shown wherever the pointer rests on the row; a cell with its own title keeps it. Null or empty adds none.", "Func&lt;TValue, int, string?&gt;?", "null"),
    };

    // Editing
    public static readonly ApiRow[] A_Editing =
    {
        new("CellEditablePredicate", "Per-cell edit gate, (item, field), checked on top of the column's <code>AllowEditing</code> at every edit start, mass-edit target and editable render cue; blocked cells are skipped silently. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "Func&lt;TValue, string, bool&gt;?", "null"),
        new("BatchEditBehavior", "<code>MultiRow</code>: a click opens the editor and typed values can fan out to every selected row. <code>SingleCell</code>: click selects, double-click edits, and typing commits to that one cell. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "GridBatchEditBehavior", "GridBatchEditBehavior.MultiRow"),
        new("EditOnSingleClick", "Opens an editable batch cell on a plain click instead of click-to-select, type-to-replace. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "bool", "false"),
        new("TextEditorTypingBehavior", "Transport for the generated text cell editors: <code>ServerBacked</code> or <code>ClientBuffered</code>. See <a href=\"demo/grid/typing-modes\">Typing modes</a>.", "TextBoxTypingBehavior", "TextBoxTypingBehavior.ServerBacked"),
        new("UseClientBufferedTextEditing", "Overrides <code>TextEditorTypingBehavior</code>: true forces client-buffered editors, false server-backed, null follows the mode. See <a href=\"demo/grid/typing-modes\">Typing modes</a>.", "bool?", "null"),
        new("AllowSingleCellColumnMassEdit", "In <code>SingleCell</code> mode, lets Ctrl/Cmd and Shift select several cells of one column and commits the typed value to all of them. See <a href=\"demo/grid/batch-editing\">Batch editing</a>.", "bool", "false"),
        new("AllowCellFormulas", "Evaluates text cell values that start with <code>=</code> as row-scoped formulas when building display values, and bypasses the display-value cache. Column <code>Formula</code> values are evaluated regardless. See <a href=\"demo/grid/dynamic-columns\">Dynamic columns</a>.", "bool", "false"),
    };

    // Sorting
    public static readonly ApiRow[] A_Sorting =
    {
        new("AllowSorting", "Master switch for header-click sorting, the sort items in the header menu and the Sort dialog; each column's own <code>AllowSorting</code> must also be true.", "bool", "false"),
        new("AllowMultiSorting", "Allows several sort levels through the Sort dialog; header clicks still sort one column. Requires <code>AllowSorting</code>.", "bool", "true"),
        new("ShowMultiSortMenu", "Shows the Custom Sort entry in the header right-click menu when both sorting switches are on; false hides it while keeping multi-column sorting available.", "bool", "true"),
    };

    // Filtering
    public static readonly ApiRow[] A_Filtering =
    {
        new("AllowFiltering", "Master switch for the header filter icon, the filter-bar inputs, the header menu's filter items and the expression-filter button; the column's own <code>AllowFiltering</code> must also be true.", "bool", "false"),
        new("EnableAdvancedFilterPopup", "Adds a second condition (Condition 1, AND/OR, Condition 2) to the column filter popup, honoured by local and provider filtering.", "bool", "false"),
        new("EnableBlankRowFilter", "Adds (Blanks) and (Non-Blanks) quick filters to the column filter popup.", "bool", "false"),
        new("ClearFiltersOnDataSourceChange", "Clears all transient filter state when a different <code>DataSource</code> instance arrives, so one context's filter cannot hide another context's rows.", "bool", "true"),
        new("ShowHeaderFilterIcon", "Shows the clickable filter icon in each filterable header; turn it off when the grid already renders filter-bar inputs.", "bool", "true"),
        new("HeaderFilterIcon", "Glyph or text for the header filter icon; blank renders the built-in glyph.", "string", "string.Empty"),
        new("ShowSearchBar", "Adds a search box to the built-in toolbar, which then renders, keeping rows whose text in any visible column contains the typed value; applied as you type or on commit.", "bool", "false"),
    };

    // Paging
    public static readonly ApiRow[] A_Paging =
    {
        new("PageSize", "Rows per page; <code>PageSettingsRef.PageSize</code> takes precedence when a settings component is present. See <a href=\"demo/grid/virtualization-bench\">Scroll vs paging</a>.", "int", "50"),
        new("PageSizes", "Choices in the pager's page-size list; <code>PageSettingsRef.PageSizes</code> wins, and an empty array falls back to the built-in set. See <a href=\"demo/grid/virtualization-bench\">Scroll vs paging</a>.", "int[]", "[25, 50, 100, 200]"),
        new("PageButtonCount", "Number of page buttons the pager shows; <code>PageSettingsRef.PageCount</code> wins, and a non-positive value falls back to 5.", "int", "5"),
        new("AutoPageRowThreshold", "With <code>AllowPaging</code> off and no <code>Height</code>, a local source with more rows than this switches to paging on its own; 0 or less disables the switch.", "int", "10000"),
        new("AllowPaging", "Turns the pager on. Off, paging still engages for an unbounded-height grid above <code>AutoPageRowThreshold</code>. See <a href=\"demo/grid/virtualization-bench\">Scroll vs paging</a>.", "bool", "false"),
    };

    // Selection
    public static readonly ApiRow[] A_Selection =
    {
        new("CellSelectablePredicate", "In cell selection mode, an (item, field) gate shared by pointer, range and keyboard selection; cells it rejects are pruned from the selection and skipped when the grid looks for its first and last cell. Null leaves read-only cells selectable.", "Func&lt;TValue, string, bool&gt;?", "null"),
        new("AutoSelectFirstRow", "Selects and focuses the first visible row when a selectable row grid first receives rows and after a sort reorders them; ignored for checkbox-only and cell selection. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "bool", "false"),
        new("InitialSelectedRowIndex", "Displayed-row index to select before the first visible render, for pickers that already know their value; re-applied when the index or the data source changes. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "int?", "null"),
        new("FocusInitialSelection", "Gives the initial selection keyboard focus once its browser reveal has completed.", "bool", "false"),
        new("NotifyInitialSelectionChanged", "Raises the normal selection-changed callback for the seeded initial selection; pickers usually leave it false because nothing changed.", "bool", "false"),
        new("InitialSelectionReady", "Fires after the seeded row has rendered and its browser reveal has completed; a first-paint readiness gate for picker hosts.", "EventCallback"),
        new("AllowSelection", "False makes the grid cursor-only: clicks and keys move the active cell and editing still works, but no row or cell is ever selected and no selection event fires. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "bool", "true"),
        new("HighlightSelectedRows", "Applies <code>fx-selected</code> and the <code>data-fx-row-highlight</code> flag to selected rows; false keeps the selection but draws nothing. See <a href=\"demo/grid/master-detail\">Master-detail</a>.", "bool", "true"),
        new("ShowActiveCell", "False hides the cell cursor (the host gets <code>data-fx-active-cell</code> of <code>hidden</code>) and turns off the browser-side navigation preview; the active cell still exists as keyboard origin and scroll anchor.", "bool", "true"),
        new("AllowRowDragSelection", "Lets a primary-button press on a row start spreadsheet-style drag selection in multi-select row grids; click selection is unaffected.", "bool", "true"),
        new("CommitSelectedRowOnEnter", "Plain Enter on the focused grid with one selected row, no open editor and no open popup invokes <code>OnRecordDoubleClick</code> for that row, so a picker accepts with Enter.", "bool", "false"),
        new("ShowSelectionInfoBar", "Shows the rows-selected banner with its Clear Selection button above the header whenever more than one row, or any cell, is selected.", "bool", "false"),
        new("TypeAheadTargetField", "Field that receives multi-row type-ahead when no selection gesture has captured an editable target column.", "string?", "null"),
        new("TypeAheadFallbackField", "Alias for <code>TypeAheadTargetField</code>; used only when that one is blank.", "string?", "null"),
        new("ShowRowSelectionEditColumnCue", "In multi-row selection, highlights the column that bulk type-ahead will write to across the selected rows, and lets such a column be captured as the target.", "bool", "false"),
    };

    // Keyboard and focus
    public static readonly ApiRow[] A_Keyboard =
    {
        new("TabNavigationMode", "Tab and Shift+Tab inside a cell: <code>WrapRows</code> walks the cells and wraps between rows, <code>WrapRowsUntilEdge</code> stops at the grid's edges, <code>PageControl</code> hands both keys to an enclosing PageControl graph. See <a href=\"demo/page-navigation\">Page navigation</a>.", "GridTabNavigationMode", "GridTabNavigationMode.WrapRows"),
        new("HostTabIndex", "<code>tabindex</code> of the grid host element; null renders 0.", "int?", "null"),
        new("SeedActiveCellOnHostFocus", "When the host gains focus with no active cell, makes the first visible cell current so arrows, Tab and type-to-edit work at once; entry through page navigation seeds regardless.", "bool", "false"),
        new("ExitGridOnTerminalTab", "Tab on the last navigable cell and Shift+Tab on the first move focus to the adjacent control outside the grid instead of wrapping.", "bool", "false"),
        new("AutoFocusFirstCell", "Makes the first cell current and focuses the grid on open, retried for up to three renders while rows arrive; stops once a cell is current.", "bool", "false"),
        new("EnterAdvancesToNextCell", "Enter in an open editor commits and moves to the next editable cell, wrapping rows, like Tab; Shift+Enter commits in place.", "bool", "false"),
        new("EnableTypeSearch", "Read-only type search: printable keys typed into the focused grid find the first row whose text in the active column starts with the buffer, and the matched prefix is highlighted in that column.", "bool", "false"),
        new("TypeSearchDelaySeconds", "Seconds of inactivity before the type-search buffer resets; values below 1 count as 1.", "int", "3"),
    };

    // Columns
    public static readonly ApiRow[] A_Columns =
    {
        new("AllowGrouping", "Enables grouping: the group drop area, header drag-to-group, the header menu's group items and grouped row rendering. See <a href=\"demo/grid/grouping\">Grouping &amp; totals</a>.", "bool", "false"),
        new("AllowResizing", "Shows column resize grips and enables drag resizing; the column's own <code>AllowResizing</code> must also be true.", "bool", "false"),
        new("AllowColumnAutoFit", "Best fit: double-clicking a resize grip sizes that column to its content, and the header menu offers Best Fit and Best Fit All Columns. The grip needs <code>AllowResizing</code>.", "bool", "true"),
        new("AutoFitSampleSize", "How many rows of the current view a best fit samples; values below 1 count as 1.", "int", "50"),
        new("AutoFitEveryRow", "When the sample also covers every row in view, a best fit measures the display text of every row, including rows outside the rendered window, instead of only the sampled rows.", "bool", "false"),
        new("AutoFitMinWidth", "Floor in px for a best-fit width; a column's own <code>MinWidth</code> wins.", "double", "40"),
        new("AutoFitMaxWidth", "Ceiling in px for a best-fit width; 0 uses the grid's own ceiling, and a column's own <code>MaxWidth</code> wins.", "double", "0"),
        new("AutoFitWidthScale", "Multiplier applied to the measured content width before the bounds are applied; values below 1 count as 1.", "double", "1"),
        new("AutoFitUseDomMeasurement", "Measures rendered text widths through the DOM; false, or an unavailable measurement, falls back to the character-count estimate.", "bool", "true"),
    };

    // Appearance and footer
    public static readonly ApiRow[] A_Appearance =
    {
        new("SelectedRowBackground", "Sets <code>--fx-grid-selected-row-bg</code> on the host; blank leaves the theme's value.", "string", "string.Empty"),
        new("SelectedRowForeground", "Sets <code>--fx-grid-selected-row-color</code> on the host; blank leaves the theme's value.", "string", "string.Empty"),
        new("SelectedRowHoverBackground", "Sets <code>--fx-grid-selected-row-hover-bg</code> on the host; blank falls back to the selected-row background.", "string", "string.Empty"),
        new("RowHoverBackground", "Sets <code>--fx-grid-row-hover-bg</code>, the hover background of unselected rows; blank leaves the theme's value.", "string", "string.Empty"),
        new("ShowRowCountInFooter", "Shows the filtered row count: a footer line when not paging, or inside the pager when paging.", "bool", "false"),
        new("EnableHover", "Adds <code>fx-hover</code> to data rows so they highlight under the pointer.", "bool", "true"),
        new("EnableAltRow", "Adds <code>fx-alt-row</code> to every second data row.", "bool", "true"),
    };
}
