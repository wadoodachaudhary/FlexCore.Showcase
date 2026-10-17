# Showcase issue tracker

Started 2026-10-06 from a full read of every page (40 routes). One row per finding. Edit the Status column in place; keep IDs stable.

Status values: `pending approval` (not yet decided) · `reviewed` (decided, not started) · `in progress` · `done`.

Rules in force: labels and copy must be true as shipped (claims rule); Showcase chrome uses FlexCore controls where one exists; retired pages keep their file with the route removed.


## 1. Phase 1 · nav, routes, titles (applied 2026-10-06)

| ID | Item | Status | Note |
|---|---|---|---|
| SC-001 | Shared nav model `Components/Shared/SiteNav.cs`; menubar, phone drawer, footer, Grid overview list and NotFound read it | done | replaces the array that lived in MainLayout |
| SC-002 | Top bar: Components ▾, Data Grid ▾, Charts ▾, Reports ▾, Performance ▾; Quick Start, GitHub | done | "VB6 Migration" and "Benchmarks" removed |
| SC-003 | Second `@page` routes: /demo/grid/scenarios, batch-editing, typing-modes, unsaved-row-lock, dynamic-columns, grouping, master-detail, tree-grid | done | old URLs still work |
| SC-004 | Retired `/demo/grid/blazor-optimizer` (file kept, route now on Scroll vs paging) | done | telemetry table was literals; buttons bound to nothing |
| SC-005 | Retired `/demo/grid/radzen-comparison` (file kept, route now on Grid overview) | done | no competitor grid rendered; figures unsourced |
| SC-006 | `/demo/workspace/page-control-bench` out of the nav, still routed and in the catalog | done | tabs are plain buttons; rebuild on TabControl in phase 2 |
| SC-007 | Catalog: eight rows relabelled from internal names; optimizer row removed; targets moved to new routes | done |  |
| SC-008 | PageTitle = heading = menu label on 29 pages (`{Label} — FlexCore Showcase`) | done | copy otherwise unchanged |
| SC-009 | `Components/Layout/NavMenu.razor` left in place with a retired note | done | unreferenced since 6 Sep; delete when convenient |

## 2. Item D · copy fixes (pending approval)

Each is a one-file edit; none changes behaviour except the double toast.

| ID | Page | Fix | Status | Detail |
|---|---|---|---|---|
| SC-010 | `/` | Version badge | done | `Home.razor` lines 12 and 34 say v0.2.31; library is 0.2.48. Replace with `typeof(Fx.ControlKit.ButtonControl).Assembly.GetName().Version` so it cannot drift. |
| SC-011 | `/ and /get-started` | Remove `downloads/FlexCore.zip` links | done | `Home.razor:624`, `GetStarted.razor:17`. No `wwwroot/downloads` folder exists; both links 404. Point to the NuGet package or the GitHub release instead. |
| SC-012 | `/get-started` | "Once published to NuGet" | done | `GetStarted.razor:10`. FlexCore 0.2.47 is on nuget.org (3.7K downloads). Replace with `dotnet add package FlexCore`. |
| SC-013 | `/demo/notifications` | Double toast | done | Page renders its own `NotificationDisplayControl` (3 references); `MainLayout` already renders one. Remove the page copy. |
| SC-014 | `/demo/grid/virtualization-bench` | Literal timings | pending approval | Copy states 1.2 ms, 60 FPS and near-zero memory; nothing on the page measures them. Remove or measure with a Stopwatch and show the method. |
| SC-015 | `/demo/grid/row-selection-50k` | "50 columns / 2.5M cells" | pending approval | Grid defines 12 columns. Change the copy or add the columns. |
| SC-016 | `/demo/charts` | "30+ chart types" | pending approval | Gallery renders 24. State 24 or render the rest. |
| SC-017 | `/demo/advanced-charts` | "newly ported" | pending approval | Time-relative wording; remove. Section 6 feeds a RadialGauge series to the linear gauge; fix the sample. |
| SC-018 | `/demo/grid/advanced` | Dead buttons and claim | pending approval | Edit/Del buttons have no handlers; intro claims auto-sizing that is not configured. Remove both. |
| SC-019 | `/demo/grid/enterprise-suite` | Dead Show Filter Row button | pending approval | Bound to no parameter; remove. Selected count does not update; wire RowSelected. |
| SC-020 | `/demo/grid/items-provider-db` | "SQLite" / "Database" wording | pending approval | No database is wired; provider serves in-memory rows. Say so until the merge into On-demand loading. |
| SC-021 | `/demo/grid/provider-enterprise` | State the setup | pending approval | Add one sentence: 12,000 in-memory rows, 90 ms simulated latency. |
| SC-022 | `/demo/grid/dynamic-columns` | Fabricated figures | pending approval | "Staged baseline" numbers are typed in; delete. |
| SC-023 | `/demo/grid/column-swap` | Unshown claims | pending approval | Batch-edit survival and scroll-offset claims are not demonstrable with the current rows; remove or add EditSettings and enough rows. |
| SC-024 | `/demo/grid/items (Grouping & totals)` | "sums" | pending approval | Group captions show Average and Count, not sums. |
| SC-025 | `/demo/buttons` | Family links | done | 2026-10-12: Related section now lists Control tour (Toolbar), Dialogs (default button), home page (Ribbon); FabMenu/Chip mentions dropped. |
| SC-026 | `/demo/counterparts` | PDF "Save to host" | pending approval | Writes a status line only; nothing is saved. Download the bytes or remove the button. |
| SC-027 | `/demo/tree/treeview-explorer` | Checkbox claim | pending approval | Copy promises checkboxes; `ShowCheckboxes` is not set. |

## 3. Phase 2 · split, merge, new pages (pending approval)

| ID | Item | Status | From |
|---|---|---|---|
| SC-028 | New `/demo/forms` Forms & pickers | pending approval | Control tour Forms + Pickers tabs; Switch/Slider/Rating/SecurityCode/TimeSpanPicker from UI elements |
| SC-029 | New `/demo/pdf-viewer` | pending approval | Control tour PDF tab; Reports ▾ row then points here |
| SC-030 | New `/demo/gantt` | pending approval | Control tour Gantt tab |
| SC-031 | New `/demo/ai-prompt` | pending approval | Control tour AI tab |
| SC-032 | Page layouts absorbs Dock manager, AppBar, GridLayout | pending approval | Control tour Docking tab |
| SC-033 | Financial & gauges absorbs the four gauge controls and StockChart | pending approval | Control tour Charts tab |
| SC-034 | Retire `/demo/counterparts` once empty (file kept, route forwards to Forms & pickers); repoint its 39 catalog rows | pending approval |  |
| SC-035 | Merge `/demo/grid/assembly` and `/demo/grid/vendor-prices` into Grouping & totals; keep files, routes forward | pending approval | identical configuration on different models |
| SC-036 | Merge `/demo/grid/items-provider-db` into On-demand loading; keep file | pending approval |  |
| SC-037 | Fold Column reorder into Grid overview; keep file | pending approval |  |
| SC-038 | Rebuild the vendor workspace on TabControl/PageControl, then list it under Layout as Tabbed workspace | pending approval | `/demo/workspace/page-control-bench` |
| SC-039 | Tree grid: time the full load (delay + render) and state the method; then add to Performance ▾ | pending approval |  |
| SC-040 | New `/performance`: per linked demo, what is timed, how, on what hardware | pending approval | Performance ▾ first row |
| SC-041 | New `/migration`: classic theme toggle on live controls, classic tree lines, unsaved-row lock, Crystal XML report, VB6/WinForms → FlexCore mapping table | pending approval | bar item added only when it renders |
| SC-042 | Home: popular cards + All components link instead of the full directory; `/components` gets an H1 | pending approval |  |
| SC-043 | Delete `Components/Layout/NavMenu.razor` and `.css` | pending approval | unreferenced since 2026-09-06 |
| SC-044 | Feature-page template (tabled proposal): FeatureDemoLayout, DemoExample, ApiTable | in progress | Components landed and Buttons converted 2026-10-12 (section 5); other pages pending approval |
| SC-045 | Library CSS additions for buttons (tabled): outline set, flat/text, icon-only, block, spinner | pending approval | FlexCore PR, owner-gated |

## 4. Findings by page (inventory of 2026-10-06)

Raw reader findings, lightly deduplicated against sections 1–3. Items already fixed in Phase 1 are marked done.

| ID | Page | Finding | Status |
|---|---|---|---|
| SC-046 | `(none — shared component embedded on / and /components)` | 57 of 97 entries route to two catch-all pages (39 to demo/counterparts, 18 to demo/new-components), so most named components land on a page listing many controls rather than the one clicked. | pending approval |
| SC-047 | `(none — shared component embedded on / and /components)` | HomeFront-internal labels: 'Master-Detail PM Entry', 'Picklist Churn', 'Veto Selection & Dirty' (fdbgrid-veto), 'SQLite ItemsProvider', 'Dual-List Transfer' / 'MultiSelect Dual List' (userperms-lists), 'Server Optimizer'. | pending approval |
| SC-048 | `(none — shared component embedded on / and /components)` | Competitor name used as a FlexCore label: 'AG Grid (4-in-1 Suite)'; popular card 'Data Grid' points at demo/grid/ag-grid-showcase while the directory 'Data Grid' points at demo/grid. | pending approval |
| SC-049 | `(none — shared component embedded on / and /components)` | '70+ ... UI controls' is not measured; the list has 97 rows but many are repeats of the same route. | pending approval |
| SC-050 | `(none — shared component embedded on / and /components)` | Misrouted or duplicate rows: 'Pager Control', 'Sparkline Charts' and 'DropDownGrid Control' all go to demo/grid; 'Stock & Financial Chart' and 'Financial Navigator' both go to counterparts; 'Tab Control' goes to page-control-bench (no TabControl there); 'Report Parameter Dialog' goes to model-costs (no dialog); 'Tree Grid' goes to a benchmark page. | pending approval |
| SC-051 | `(none — shared component embedded on / and /components)` | 'COVID-19 Sunburst' is named by dataset rather than control. | pending approval |
| SC-052 | `(none — shared component embedded on / and /components)` | All internal hrefs resolve to existing @page routes and the demo/buttons#split/#toggle/#pill/#toolbar anchors exist; no broken links found here. | pending approval |
| SC-053 | `/` | Version badge says 'v0.2.31' twice; FlexCore.csproj is 0.2.48. | pending approval |
| SC-054 | `/` | 'Download Package (.ZIP)' links to downloads/FlexCore.zip; wwwroot/downloads does not exist, so the link 404s. | pending approval |
| SC-055 | `/` | Unmeasured numbers in copy: 'sub-millisecond 60 FPS virtualization', '1,000 Rows Live Virtualized', 'zero DOM lag', '100K+ Rows Smooth', '$1,200+ / dev / year'. The GridControl on the page sets no virtualization parameter and nothing on the page measures frames or latency. | pending approval |
| SC-056 | `/` | Comparison row 'JavaScript Interop Overhead: Zero (Pure Blazor)' contradicts the pillar 'ships its own interop scripts' and the page's own IJSRuntime clipboard calls. | pending approval |
| SC-057 | `/` | 'Export to Excel' and 'New Request' only raise toasts; no workbook is produced despite the 'native ClosedXML Excel export' headline. | pending approval |
| SC-058 | `/` | 'Enterprise Reporting' tab is a static HTML mock, not ReportWriterControl; its code snippet uses Definition/Executor parameters that do not exist on ReportWriterControl. | pending approval |
| SC-059 | `/` | HomeFront jargon: window title 'Inbox — Custom Quotes (HomeFront Precision Estimating)', 'Division: 0100', communities 'MARGARITA VILLE', 'ELLIOT TRAINING'. | pending approval |
| SC-060 | `/` | Competitor name in shipped code: CssClass 'telerik-grid-theme', snippet comment 'Telerik-grade', markup comment 'Telerik Style'. | pending approval |
| SC-061 | `/` | Dead code: GetCategoryBadgeClass is never called; section comments number 1,2,3,5,6,7 (no 4). | pending approval |
| SC-062 | `/components` | Entire page is the same ComponentCatalogDirectory already rendered on Home. | pending approval |
| SC-063 | `/components` | No H1 or intro of its own; PageTitle 'All Components' is the only page-specific text. | pending approval |
| SC-064 | `/demo/advanced-charts` | PageTitle ("Advanced Charts & Gauges") and H2 ("Advanced Charts, Financials & Gauges") differ; menu label is "Advanced charts" | done |
| SC-065 | `/demo/advanced-charts` | Copy says "newly ported chart series types" — time-relative wording a buyer cannot interpret and that goes stale | pending approval |
| SC-066 | `/demo/advanced-charts` | No markup/code sample section, unlike Dialog, Grid, Layout, Pivot pages | pending approval |
| SC-067 | `/demo/advanced-charts` | Section 6 feeds the Linear gauge a series declared as ChartType.RadialGauge; works at runtime but misleads anyone copying the code | pending approval |
| SC-068 | `/demo/advanced-charts` | Gauges are shown here via ChartControl and again on /demo/counterparts via four dedicated gauge controls, with no cross-link explaining which to use | pending approval |
| SC-069 | `/demo/buttons` | Not linked from MainLayout nav, Home, footer or /components; the only link is in Layout/NavMenu.razor, which is not referenced anywhere | done |
| SC-070 | `/demo/buttons` | "Also in the family" links FabMenuControl and ChipControl to demo/new-components, but that page contains neither control | pending approval |
| SC-071 | `/demo/buttons` | RibbonControl link is href="" (resolves to home) with the text "on the home page" — fragile and unlabeled | done |
| SC-072 | `/demo/buttons` | PageTitle brand is "FlexCore UI" while every other page uses "FlexCore" or "FlexCore Showcase" | done |
| SC-073 | `/demo/buttons` | Copy names flexkit-themes.css — FlexKit is the internal library name, not FlexCore; also "Classic (VB6 / Win32)" and "For migrated VB6 forms" are migration jargon on a buyer page | pending approval |
| SC-074 | `/demo/buttons` | Code tab is labeled "ButtonsDemo.razor.cs" but no such file exists; the C# shown is a hand-written excerpt | pending approval |
| SC-075 | `/demo/buttons` | "Zero JS" badge is asserted, not demonstrated on the page | pending approval |
| SC-076 | `/demo/charts` | PageTitle 'Charts Overview' does not match H1 'ChartControl'. | pending approval |
| SC-077 | `/demo/charts` | Copy says '30+ visualization types' but the page renders 24; the ChartType enum has 42 (Treemap, Pyramid, Candlestick, Sankey, Bullet, LinearGauge, RangeBar etc. are not shown and the count is not displayed). | pending approval |
| SC-078 | `/demo/charts` | Sunburst panel duplicates /demo/charts/sunburst rather than showing ChartType.Sunburst via ChartControl. | pending approval |
| SC-079 | `/demo/charts/sunburst` | All headline numbers (56,956,080 cases; 1,397,672 deaths; 2.45% CFR; 210 countries; continent totals 15.9M/14.6M/13.5M/10.7M/2.1M/49.9K) are typed into markup, not computed from CovidDataService; they match the service constants today but will drift. | pending approval |
| SC-080 | `/demo/charts/sunburst` | No H1; PageTitle 'COVID-19 Sunburst Chart' differs from the on-page heading 'COVID-19 Global Sunburst Visualization'. | done |
| SC-081 | `/demo/charts/sunburst` | 'Top 10 Highest Cases' leaderboard ignores the Total Deaths metric toggle. | pending approval |
| SC-082 | `/demo/charts/sunburst` | Toolbar (metric toggle, continent filters, search box) uses Bootstrap buttons/inputs instead of FlexCore controls. | pending approval |
| SC-083 | `/demo/charts/sunburst` | External link to towardsdatascience.com in the page header. | pending approval |
| SC-084 | `/demo/charts/sunburst` | Menu entry is named after a pandemic dataset rather than the control a buyer is evaluating. | pending approval |
| SC-085 | `/demo/counterparts` | Menu labels do not match content: linked as "VB6 Migration" (top-level group and footer) and as "PDF viewer and spreadsheet" (under Reports) — the page has no migration content and no spreadsheet | done |
| SC-086 | `/demo/counterparts` | Route/file name "counterparts" (i.e. counterparts to Syncfusion) is internal framing; title "Dedicated controls" tells a buyer nothing about what is inside | pending approval |
| SC-087 | `/demo/counterparts` | Seven unrelated control areas (Docking, Gantt, PDF, AI prompt among them) share one route behind a toggle strip, so none can be linked from a menu or found by search | pending approval |
| SC-088 | `/demo/counterparts` | PDF "Save to host" only writes "Saved N PDF bytes to the host" to a status line; nothing is saved anywhere | pending approval |
| SC-089 | `/demo/counterparts` | No code samples or API notes for any of the 28 controls shown | pending approval |
| SC-090 | `/demo/counterparts` | All C# is compressed to one statement per line, which makes the page a poor copy source | pending approval |
| SC-091 | `/demo/data-control` | Menu label "Data control" under the Data Grid group implies a visual control; DataControl is a non-visual class and the page is an API reference | done |
| SC-092 | `/demo/data-control` | Framing "inspired by pandas DataFrame and PowerBuilder DataWindow" and the "DataWindow Methods" section are niche jargon most Blazor buyers will not know | pending approval |
| SC-093 | `/demo/data-control` | Six of ten sections (Metadata, DataFrame Methods, DataWindow Methods, Control Adapters, Support Types, Constructor) are static name/description lists with nothing executed or interactive | pending approval |
| SC-094 | `/demo/data-control` | Only one adapter (ToChartBars) is demonstrated although the intro promises feeding GridControl and ReportWriterControl; no GridControl on the page | pending approval |
| SC-095 | `/demo/data-control` | Uses the `docs-page` style rather than the demo card style of sibling pages | pending approval |
| SC-096 | `/demo/dialog` | Not linked from MainLayout nav, Home, footer or /components; only the unreferenced Layout/NavMenu.razor links it | done |
| SC-097 | `/demo/dialog` | Thin coverage: two dialogs only — no non-modal, draggable/resizable, nested, or default-button (DefaultButtonScope, referenced on the Buttons page) examples | pending approval |
| SC-098 | `/demo/dialog` | Footer buttons are positioned with inline styles; the page never shows whether DialogControl has a footer slot | pending approval |
| SC-099 | `/demo/dialog` | Intro phrase "configurable visibility" describes nothing a visitor can see | pending approval |
| SC-100 | `/demo/grid` | Three names for one page: PageTitle "Grid", H1 "GridControl", menu "Data Grid / Grid overview" | pending approval |
| SC-101 | `/demo/grid` | Link text states "100k Performance" and "50K Row" — numbers not measured on this page | pending approval |
| SC-102 | `/demo/grid` | Heading "Flagship Enterprise Showcase" leads with "AG Grid Showcase (4-in-1 Suite)" — a competitor's product name as the headline link, easy to misread as AG Grid itself | pending approval |
| SC-103 | `/demo/grid` | Links "Assembly", "Vendor Prices", "Items Catalog" are HomeFront business scenarios with no explanation of what they show | pending approval |
| SC-104 | `/demo/grid` | Only 10 rows, so grouping and filtering have little to act on | pending approval |
| SC-105 | `/demo/grid` | Links use absolute "/demo/..." paths while the rest of the site uses relative hrefs | pending approval |
| SC-106 | `/demo/grid/advanced` | Intro claims "auto-sizing" but no auto-fit/auto-size setting is used on either grid | pending approval |
| SC-107 | `/demo/grid/advanced` | Actions column renders Edit/Del buttons with no click handlers — dead controls | pending approval |
| SC-108 | `/demo/grid/advanced` | EditMode.Batch is enabled but the page exposes no save/discard control or changed-row indicator, so a visitor cannot tell batching is active (GridControl commits internally on row leave) | pending approval |
| SC-109 | `/demo/grid/advanced` | Second card header says "ID is a key; InternalCode is excluded; Total is read-only" — only the exclusion is visible; key/read-only effects are not demonstrated | pending approval |
| SC-110 | `/demo/grid/advanced` | No markup/code sample section; the annotation model (the point of the second grid) is not shown to the visitor | pending approval |
| SC-111 | `/demo/grid/ag-grid-showcase` | Title leads with the competitor's name ('AG Grid Showcase'); a buyer may read it as an AG Grid demo rather than a FlexCore one | pending approval |
| SC-112 | `/demo/grid/ag-grid-showcase` | 'Performance Benchmark' tab measures only data-generation time (Gen Time); render and scroll performance are not measured, and the page's own copy admits frame rate and scroll latency are not measured | pending approval |
| SC-113 | `/demo/grid/ag-grid-showcase` | 'Performance Comparison Matrix' contains no numbers; the '100k Rows Memory' row has identical text in both vendor columns | pending approval |
| SC-114 | `/demo/grid/ag-grid-showcase` | 'Offices: 5 Global Hubs' in the HR toolbar is hardcoded text, not computed from data | pending approval |
| SC-115 | `/demo/grid/ag-grid-showcase` | 'Master-Detail' badge: the detail pane is a plain HTML table, not a grid detail feature | pending approval |
| SC-116 | `/demo/grid/ag-grid-showcase` | _inventoryGrid @ref is assigned but never used | pending approval |
| SC-117 | `/demo/grid/ag-grid-showcase` | Razor code tab is a hand-abbreviated snippet (binds TreeGrid to _allHrEmployees, omits templates) rather than the page's actual markup | pending approval |
| SC-118 | `/demo/grid/assembly` | Intro promises 'editable line items' but no EditSettings is set; nothing is editable | pending approval |
| SC-119 | `/demo/grid/assembly` | Intro promises 'per-row formatting'; no row templates or conditional styling exist | pending approval |
| SC-120 | `/demo/grid/assembly` | 'widths persist' in Things to try: no layout persistence is configured; a reload resets widths | pending approval |
| SC-121 | `/demo/grid/assembly` | 'HomeFront's FAssembly form' is internal jargon | pending approval |
| SC-122 | `/demo/grid/blazor-optimizer` | Every number in the 'Live Engine Performance Telemetry' table is a literal: 14.8/3.2/12.4/0.9 ms, 94.2%/98.7% cache hits, 42/24/18/12 renders; nothing is measured | pending approval |
| SC-123 | `/demo/grid/blazor-optimizer` | Mode buttons change only _activeMode, which is never passed to the grid; GridControl exposes a real PerformanceMode parameter (GridPerformanceMode) that the page does not bind, so all four 'modes' render identically | pending approval |
| SC-124 | `/demo/grid/blazor-optimizer` | 'Capture Snapshot' adds 2 to a counter | pending approval |
| SC-125 | `/demo/grid/blazor-optimizer` | C# tab shows a PerformanceTelemetry class that exists nowhere in the solution | pending approval |
| SC-126 | `/demo/grid/blazor-optimizer` | 100 rows only; the 'high-frequency' claim is not exercised | pending approval |
| SC-127 | `/demo/grid/blazor-optimizer` | _grid @ref unused | pending approval |
| SC-128 | `/demo/grid/column-swap` | File is named Bench but nothing is timed or counted | pending approval |
| SC-129 | `/demo/grid/column-swap` | Doc claims batch-edit cells survive a layout swap; page has no EditSettings, so this cannot be tried (AllowEditing on columns has no effect) | pending approval |
| SC-130 | `/demo/grid/column-swap` | Doc claims scroll offset is retained; grid has 6 rows in 340px and never scrolls | pending approval |
| SC-131 | `/demo/grid/column-swap` | _grid and _gridColumns @refs are unused | pending approval |
| SC-132 | `/demo/grid/column-virtualization` | No visible indicator of mounted versus spacer columns, so a viewer cannot tell virtualization is active | pending approval |
| SC-133 | `/demo/grid/column-virtualization` | Card header text 'EnableColumnVirtualization=true; overscan=2' is developer syntax shown to the viewer | pending approval |
| SC-134 | `/demo/grid/column-virtualization` | Not built on SyncfusionDemoLayout; no source or documentation tabs, unlike sibling grid pages | pending approval |
| SC-135 | `/demo/grid/edit-items-db` | Title says 'Typing Bench' and summary promises 'typing responsiveness metrics'; only an edit counter exists, nothing is timed | pending approval |
| SC-136 | `/demo/grid/edit-items-db` | 'Database' in title and route; data is an in-memory list (BenchDataStore.EstimatingItems = 5,000 synthesized rows because estimating-items-full.json is absent on disk) | pending approval |
| SC-137 | `/demo/grid/edit-items-db` | Zoom applies an inline CSS transform:scale on the wrapper and also calls ZoomService.SetZoom; the transform does not reflow layout, so the grid overflows or shrinks inside its container, and the doc's --fx-zoom explanation is not what the viewer sees | pending approval |
| SC-138 | `/demo/grid/edit-items-db` | 'Memory usage remains constant' is asserted, not measured | pending approval |
| SC-139 | `/demo/grid/edit-items-db` | 'HomeFront's production Estimating Items catalog' is internal jargon | pending approval |
| SC-140 | `/demo/grid/edit-items-db` | _grid @ref unused | pending approval |
| SC-141 | `/demo/grid/enterprise-suite` | 'Show Filter Row' button toggles _showFilterRow, which is bound to nothing; GridControl has no filter-row parameter, so the control is dead | pending approval |
| SC-142 | `/demo/grid/enterprise-suite` | Summary promises multi-column sorting, Excel-style checklist filtering and frozen left columns; the grid sets none of AllowMultiColumnSorting, IsFrozen, or a checklist filter | pending approval |
| SC-143 | `/demo/grid/enterprise-suite` | 'Selected: N' reads _grid.GetSelectedRecords() during render with no RowSelected handler, so it does not refresh when rows are selected | pending approval |
| SC-144 | `/demo/grid/enterprise-suite` | AllowEditing=true on three columns with no EditSettings; nothing is editable | pending approval |
| SC-145 | `/demo/grid/enterprise-suite` | 'complete parity with high-end desktop data grids' is unmeasured marketing copy | pending approval |
| SC-146 | `/demo/grid/fdbgrid-veto` | 'FDBGrid', 'ported from VB6', 'HomeFront' are internal jargon; the route /fdbgrid-veto exposes it | pending approval |
| SC-147 | `/demo/grid/fdbgrid-veto` | Summary calls it a benchmark and the file is named Bench; nothing is measured | pending approval |
| SC-148 | `/demo/grid/fdbgrid-veto` | Doc table names RowSelectingEventArgs<T>, which does not exist in FlexCore; the live page uses RowSelectEventArgs<T> | pending approval |
| SC-149 | `/demo/grid/fdbgrid-veto` | Razor code tab uses args.RowData and Func delegates; live page uses args.Data and EventCallback.Factory, so the sample does not match the working API | pending approval |
| SC-150 | `/demo/grid/fdbgrid-veto` | Ledger rows carry Qty and Price columns ('Accounts Receivable' qty 14 at $86,450.50), which is not plausible ledger data | pending approval |
| SC-151 | `/demo/grid/items` | 'modeled after FItems' is internal jargon | pending approval |
| SC-152 | `/demo/grid/items` | Things to try says 'Group sums show per-category average'; the aggregates are Average and Count, there is no Sum | pending approval |
| SC-153 | `/demo/grid/items-provider-db` | File named Sqlite and C# tab shows a SqliteGridProvider with SQL; the page runs LINQ over a 5,000-row in-memory list with Task.Delay(25); no database is queried, so 'Query Database' and 'Total Matches in DB' mislead | pending approval |
| SC-154 | `/demo/grid/items-provider-db` | 'hundreds of thousands of records' and 'zero memory consumption on the Blazor server' are not demonstrated (5,000 in-memory rows) | pending approval |
| SC-155 | `/demo/grid/items-provider-db` | Summary says sort orders and filters are passed into the provider; the provider ignores request sort and the grid's own filters, applying only the external text box | pending approval |
| SC-156 | `/demo/grid/items-provider-db` | Doc table and code tab name GridItemsProviderRequest<T> and GridItemsProviderResult.From(), which do not exist; the live page uses GridItemsResult<T> | pending approval |
| SC-157 | `/demo/grid/items-provider-db` | Provider calls InvokeAsync(StateHasChanged) fire-and-forget from inside the fetch | pending approval |
| SC-158 | `/demo/grid/items-provider-db` | File named Bench; nothing is measured | pending approval |
| SC-159 | `/demo/grid/picklist-churn` | No modal or dialog is opened; 'Modal Opening' is simulated by regenerating a list | pending approval |
| SC-160 | `/demo/grid/picklist-churn` | 'Staged Baseline' time is the same Stopwatch plus a literal +35.0 ms; 'Renders' increments by literal 1 or 3; initial 1.8 ms is a literal, so the benchmark numbers are fabricated | pending approval |
| SC-161 | `/demo/grid/picklist-churn` | 'PickList', 'churn', 'FPickList', 'HomeFront' are internal jargon | pending approval |
| SC-162 | `/demo/grid/picklist-churn` | Doc claims 'Pre-Calculated Geometry' and 'Single-Pass Synchronization'; nothing on the page toggles or exercises them | pending approval |
| SC-163 | `/demo/grid/picklist-churn` | _grid @ref unused | pending approval |
| SC-164 | `/demo/grid/pm-entry` | 'Simulated Latency' buttons set _simulatedLatency, which nothing reads; no delay is injected, so 'Simulate network delays to see...' is false | pending approval |
| SC-165 | `/demo/grid/pm-entry` | 'Keystrokes' and 'Server Commits' are incremented together in OnCellEdit and always show the same number; keystrokes are not counted per key | pending approval |
| SC-166 | `/demo/grid/pm-entry` | 'cuts WebSocket network payload by up to 95%' is unmeasured | pending approval |
| SC-167 | `/demo/grid/pm-entry` | 'PM Entry', 'Project Managers entry grid', 'HomeFront' are internal jargon | pending approval |
| SC-168 | `/demo/grid/pm-entry` | Title says Bench; nothing is timed; five rows | pending approval |
| SC-169 | `/demo/grid/pm-entry` | _grid @ref unused | pending approval |
| SC-170 | `/demo/grid/provider-enterprise` | Copy opens with "This bench behaves like a remote database" but nothing is timed or measured; it is a feature demo, not a benchmark | pending approval |
| SC-171 | `/demo/grid/provider-enterprise` | Three names: PageTitle "Provider Grouping & Export", H2 "Remote grouping and complete-query export", menu "Server-side provider grid" | pending approval |
| SC-172 | `/demo/grid/provider-enterprise` | Copy is developer jargon throughout ("provider", "GroupChildren request", "paged GroupItems requests", "Export pages", "visible window") with no plain statement such as "groups load from the server on demand" | pending approval |
| SC-173 | `/demo/grid/provider-enterprise` | Data is a 12,000-row in-memory list with Task.Delay(90ms) simulating latency; the page does not say so, while the sibling items-provider-db page uses a real SQLite database | pending approval |
| SC-174 | `/demo/grid/radzen-comparison` | PageTitle names Radzen; layout title says 'Conventional Web Grid'; copy names Radzen, MudBlazor and Syncfusion; no competitor grid is rendered for comparison | pending approval |
| SC-175 | `/demo/grid/radzen-comparison` | All table figures are literals and unsourced: '350 KB - 1.8 MB', '~180 elements', '1,500 - 10,000+ elements', '< 15 ms', '120 - 450 ms', '100% parity' | pending approval |
| SC-176 | `/demo/grid/radzen-comparison` | '10,000 Rows' button: EstimatingItems holds 5,000 synthesized rows (JSON file absent), so Take(10000) yields 5,000 and the '10,000 rows' table row is never exercised | pending approval |
| SC-177 | `/demo/grid/radzen-comparison` | 'VB6 / Legacy ERP Port Parity', 'Veto gating', 'PageControl' are internal jargon | pending approval |
| SC-178 | `/demo/grid/radzen-comparison` | 'unmatched frame rates' is unmeasured | pending approval |
| SC-179 | `/demo/grid/radzen-comparison` | C# tab is a three-line comment, not code | pending approval |
| SC-180 | `/demo/grid/radzen-comparison` | _grid @ref unused | pending approval |
| SC-181 | `/demo/grid/reordering` | Copy is a QA checklist ('Use this page to validate first-drop reorder', 'Reload the page and repeat to validate consistent behavior'), not buyer-facing | pending approval |
| SC-182 | `/demo/grid/reordering` | Reorder-pipe color picker is a test knob with no product purpose | pending approval |
| SC-183 | `/demo/grid/row-selection-50k` | Title and summary say '50 Columns', 'Columns 7-50', '2.5 million potential cells'; the grid defines 12 columns (6 interactive + C07-C12); the code tab comment 'C07-C50 continue here' hides the gap | pending approval |
| SC-184 | `/demo/grid/row-selection-50k` | Summary lists a 'Dropdown' editor among columns 1-6; none exists (Category is plain text) | pending approval |
| SC-185 | `/demo/grid/row-selection-50k` | 'Build: N ms' times list generation only, not grid render; 'Renders' is incremented by hand; initial 2.4 ms is a literal | pending approval |
| SC-186 | `/demo/grid/row-selection-50k` | 'FAssembly gItems', 'HomeFront' are internal jargon | pending approval |
| SC-187 | `/demo/grid/row-selection-50k` | Doc's 'silky-smooth scrolling' and linear-memory claims are unmeasured | pending approval |
| SC-188 | `/demo/grid/userperms-lists` | Route and file say 'UserPerms', internal jargon | pending approval |
| SC-189 | `/demo/grid/userperms-lists` | Detail filter is fake: _allUsers.Take((GroupID % 4) + 3); users have no group relation, so each group shows the same leading users; the code tab shows u.GroupID == group.GroupID, which the page does not do (UserListItem has no GroupID) | pending approval |
| SC-190 | `/demo/grid/userperms-lists` | 'Create Group & Rename' adds a row but does not open the editor; doc claims it 'targets focus directly to the rename editor' | pending approval |
| SC-191 | `/demo/grid/userperms-lists` | 'Dirty' state is a manual checkbox, not derived from edits; editing Group Name does not set it | pending approval |
| SC-192 | `/demo/grid/userperms-lists` | Summary calls it a benchmark; nothing is measured | pending approval |
| SC-193 | `/demo/grid/userperms-lists` | Master 'Users' column (MemberCount) does not match the detail grid's row count | pending approval |
| SC-194 | `/demo/grid/vendor-prices` | 'modeled after FVendorPriceList' is internal jargon | pending approval |
| SC-195 | `/demo/grid/vendor-prices` | Structurally the same demo as /demo/grid/items; only the model differs | pending approval |
| SC-196 | `/demo/grid/virtualization-bench` | 'Last build: N ms' wraps a Stopwatch around a field assignment; initial 1.2 ms is a literal; 'Renders' is incremented by hand; title says Bench but nothing real is measured | pending approval |
| SC-197 | `/demo/grid/virtualization-bench` | 'Maintains consistent 60 FPS' and 'near-zero memory footprint' are unmeasured | pending approval |
| SC-198 | `/demo/grid/virtualization-bench` | Summary lists a 'dropdown' editor type; no dropdown column exists | pending approval |
| SC-199 | `/demo/grid/virtualization-bench` | Code tab uses GridEditSettings/GridEditMode/GridSelectionSettings and places BatchEditBehavior and AllowSingleCellColumnMassEdit on the settings object; the page uses EditSettings/EditMode/SelectionSettings and those two are GridControl parameters, so the sample does not compile against the real API | pending approval |
| SC-200 | `/demo/grid/virtualization-bench` | Doc says virtualization runs 'purely on .NET'; the AG Grid page says GridControl loads its own interop script on demand, so the messaging is inconsistent | pending approval |
| SC-201 | `/demo/layout` | Not linked from MainLayout nav, Home, footer or /components; only the unreferenced Layout/NavMenu.razor links it | done |
| SC-202 | `/demo/layout` | Markup section covers PageLayoutControl only; the dashboard grid has no code sample | pending approval |
| SC-203 | `/demo/layout` | Layout family is split across three pages (this, /demo/charts, /demo/counterparts) with no cross-links | pending approval |
| SC-204 | `/demo/layout` | Intro lists five controls; GridLayoutControl, AppBarControl and DockManagerControl, which are also layout controls, are absent | pending approval |
| SC-205 | `/demo/new-components` | Not linked from MainLayout nav, Home, footer or /components; only the unreferenced Layout/NavMenu.razor links it | done |
| SC-206 | `/demo/new-components` | Title "New" and copy "newly ported" are time-relative and meaningless to a buyer | pending approval |
| SC-207 | `/demo/new-components` | Intro promises Breadcrumbs, Carousels and FABs; none is on the page (BreadcrumbControl does not exist in FlexCore; CarouselControl and FabMenuControl exist but are unused) | pending approval |
| SC-208 | `/demo/new-components` | /demo/buttons links here for FabMenuControl and ChipControl, which are not on the page | pending approval |
| SC-209 | `/demo/new-components` | Sample alert text "All grid rows are rendered via pure SVG / native Blazor" is a marketing claim inside demo data, and grid rows are HTML, not SVG | pending approval |
| SC-210 | `/demo/new-components` | QR code encodes https://github.com/flexcore, which is not the project's repository (footer links wadoodachaudhary/FlexCore) | pending approval |
| SC-211 | `/demo/new-components` | No code samples for any of the 17 controls | pending approval |
| SC-212 | `/demo/notifications` | Not linked from MainLayout nav, Home, footer or /components; only the unreferenced Layout/NavMenu.razor links it | done |
| SC-213 | `/demo/notifications` | Page renders its own NotificationDisplayControl while MainLayout.razor already renders one, so each toast can appear twice; the page's own sample says "Place once near root layout" | pending approval |
| SC-214 | `/demo/notifications` | PageTitle "Notifications" vs H1 "NotificationService" (a service name, not a control) | pending approval |
| SC-215 | `/demo/notifications` | 29 lines: no position, duration, action, or manual-dismiss options shown | pending approval |
| SC-216 | `/demo/pivot-formulas` | Four names for one page: route "pivot-formulas", menu "Pivot formulas", PageTitle "Pivot Tables", H1 "Multi-Level Pivot Table"; "pivot formulas" is not a thing on the page | pending approval |
| SC-217 | `/demo/pivot-formulas` | Section 5 copy says "Count of Quantity" and its code sample shows Aggregation = Count, but the live config uses Sum of Quantity labelled "Units" | pending approval |
| SC-218 | `/demo/pivot-formulas` | Grid formula columns (section 6) are a GridControl feature shown nowhere else, buried at the bottom of a pivot page under a pivot title | pending approval |
| SC-219 | `/demo/pivot-formulas` | PivotControl API section is prose cards only; no parameter table like the Buttons page | pending approval |
| SC-220 | `/demo/reports/model-costs` | Intro says '4 levels of group footers'; 'What's happening' bullet says 'three group footers (Phase / Group / Model)'. The XML defines 4 groups. | pending approval |
| SC-221 | `/demo/reports/model-costs` | HomeFront-internal jargon: 'HomeFront's real Crystal Report', '@DivisionID', 'ShowcaseReportSessionContext', report name 'Model Costs'. | pending approval |
| SC-222 | `/demo/reports/model-costs` | Copy is entirely class names and Program.cs wiring; no buyer-facing statement of what the report engine does. | pending approval |
| SC-223 | `/demo/reports/model-costs` | Logger message typo: 'Loading demi report'. | pending approval |
| SC-224 | `/demo/reports/model-costs` | Catalog links 'Report Parameter Dialog' to this page, but it never prompts for parameters (PromptOptionalParameters not set). | pending approval |
| SC-225 | `/demo/reports/model-costs` | PageTitle 'Model Costs Report' vs H1 'Reports — Model Costs' vs nav label 'Model Costs'. | pending approval |
| SC-226 | `/demo/tree/tree-load-bench` | Copy claims it measures 'simulated database round trips, node construction, and the TreeGridControl first server render'; the Stopwatch wraps only GenerateTree() list construction. The 100 ms Task.Delay and the render are not timed. | pending approval |
| SC-227 | `/demo/tree/tree-load-bench` | Scale buttons say 40 / 180 / 480 nodes but the grid loads 50 / 210 / 540 (group rows not counted); the 'Loaded Nodes' counter shows the larger figure. | pending approval |
| SC-228 | `/demo/tree/tree-load-bench` | Documentation lists 'Lazy Load Support' via the Expanded callback, but OnNodeExpandedAsync is a no-op. | pending approval |
| SC-229 | `/demo/tree/tree-load-bench` | Jargon: 'Modeled after HomeFront's FItems hierarchical catalog tree', 'classic enterprise Windows/VB6 folder and line styles'. | pending approval |
| SC-230 | `/demo/tree/tree-load-bench` | Three different names: PageTitle 'TreeGrid First-Load Bench', H1 'TreeGrid Self-Referential Hierarchy & Load Bench', nav 'TreeGrid Hierarchy'. | pending approval |
| SC-231 | `/demo/tree/tree-load-bench` | This benchmark is the only Tree Grid entry point from the popular cards and directory; there is no plain TreeGrid feature page. | pending approval |
| SC-232 | `/demo/tree/treeview-explorer` | Summary and docs claim 'checkbox selection'; ShowCheckboxes is never set and no checkboxes render. | pending approval |
| SC-233 | `/demo/tree/treeview-explorer` | C# code tab shows 'public bool Expanded' but the real TreeNode model property is IsExpanded (TreeViewModels.cs:16); the page itself uses IsExpanded. | pending approval |
| SC-234 | `/demo/tree/treeview-explorer` | 'Classic VB6 Lines' / 'Classic VB6' theme naming is migration jargon for a buyer. | pending approval |
| SC-235 | `/demo/tree/treeview-explorer` | PageTitle omits '& Folder Styles' that the H1 carries. | pending approval |
| SC-236 | `/demo/workspace/page-control-bench` | Page never renders PageControl or TabControl; the tabs are plain <button>s toggling _activeTab. The Razor code tab shows '<PageControl NavigationGraph=...>' which is not what runs. | pending approval |
| SC-237 | `/demo/workspace/page-control-bench` | Route and copy call it a benchmark; nothing is measured. | pending approval |
| SC-238 | `/demo/workspace/page-control-bench` | Summary claims DropDownListControl and keyboard shortcuts (Ctrl+Tab, Alt+Letter); neither is present on the page. | pending approval |
| SC-239 | `/demo/workspace/page-control-bench` | Insurance tab switches are unbound HTML checkboxes; toggles do not persist when navigating vendors. | pending approval |
| SC-240 | `/demo/workspace/page-control-bench` | HomeFront jargon: 'BuildPro Digital Integration Active', GL/WC insurance flags from the vendor master. | pending approval |
| SC-241 | `/demo/workspace/page-control-bench` | Directory maps 'Tab Control' to this page, which contains no TabControl. | pending approval |
| SC-242 | `/demo/workspace/page-control-bench` | Three names: nav 'Tabbed PageControl', PageTitle 'PageControl Multi-Tab Workspace', H1 adds 'Enterprise ... & Navigation Graph'. | pending approval |
| SC-243 | `/get-started` | 'Once published to NuGet:' implies the package is not yet published, contradicting Home's CTA 'Add the FlexCore NuGet package to your project'. | pending approval |
| SC-244 | `/get-started` | '/downloads/FlexCore.zip' link 404s (no wwwroot/downloads folder). | pending approval |
| SC-245 | `/get-started` | No live control on the page; snippets only. | pending approval |
| SC-246 | `/get-started` | Service registration shows only NotificationService; the report adapters it names (IReportDataExecutor etc.) have no example. | pending approval |


## 5. Feature-page template rollout (started 2026-10-12)

Reader feedback on the Buttons page: too busy. Rule for converted pages (2026-10-12): at most three examples per page, one concept each, and one good example beats three mediocre ones; everything else lives in the vocabulary tables or a code tab.

| ID | Page | Status | Note |
|---|---|---|---|
| SC-247 | Shared: `FeatureDemoLayout`, `DemoExample`, `ApiTable`, `DemoApi.cs` under `Components/Shared` | done | BreadCrumbControl, CardControl, TabStripControl, ButtonControl inside; `SyncfusionDemoLayout` stays until the last page leaves it |
| SC-248 | `/demo/buttons` on the template: 10 sections → 5 examples (Variants, Icons and sizes, Dropdown, Theme, Related controls) + 4 vocabulary tables | done | removed demos: busy/focus, form types, BareStyle, Save/Cancel, Toolbar, zoom slider, 4-palette grid |
| SC-249 | Catalog: Toolbar → Control tour, Office Ribbon → home; Split/Toggle/Pill still deep-link into `/demo/buttons` | done | |
| SC-250 | Convert remaining feature pages (Dialogs, Toasts, Page layouts, Tree view, Charts, …) one commit each | pending approval | vocabulary written from each control's source |
| SC-251 | New `/demo/split-button` on the template: three examples (default action, item values, custom menu content), Parameters, SplitButtonItem and CSS tables | done | nav: Components ▾ → Split button; catalog Split Button row points here |
| SC-252 | Catalog rows FAB and Speed Dial Menu link to UI elements, which renders no FabMenuControl | done | both rows now point at `/demo/fab` (2026-10-12); Speed Dial is the same control open |
| SC-253 | Library gaps found while writing pages (owner-gated): SplitButtonControl has no public close for MenuContent and no arrow/Escape keys; TabStripControl tabs are divs, not keyboard-focusable; FabMenuControl has no rule for `BadgeVariant.Light` (no fill) and no Escape/arrow keys; MessageBoxControl with RequiredConfirmationText does not take focus (OK is disabled), so Escape goes to the page until the user clicks inside; NotificationDisplayControl has no aria-live region and its close button has no accessible name; SliderControl ignores the base `Enabled` and `Visible` (own `Disabled` only, renders `Style` not `ResolvedStyle`) and `ShowValue` has no format; SecurityCodeControl has no focus auto-advance, no paste splitting, no arrow keys and no per-box labels, and ignores base `Enabled`/`Visible`; TimeSpanPickerControl and BarcodeControl ignore base `Visible`; TabControl has no tablist/tab/tabpanel roles or arrow keys; TabStripControl tabs are not focusable (already listed); RatingControl star buttons have no accessible name, pressed state or arrow keys; CheckBoxControl `Indeterminate` is visual only (aria-checked=mixed, input.indeterminate not set); SwitchControl, RatingControl and CardControl ignore base `Enabled`/`Visible` GridControl grouping (2026-10-17): a group footer renders only the first AggregateColumn whose Field matches the column (`aggRow.Columns.FirstOrDefault(a => a.Field == col.Field)`, GridControl.razor.cs ~14535), so a second aggregate on the same field never reaches the footer; AggregateType.Count parses values as doubles and drops the rest, so Count on a text or boolean column yields nothing; the caption pills never get their styling: `.fx-group-caption-agg` and `.fx-group-count` are rules in the scoped GridControl.razor.css (compiled as `[b-…]` selectors) but the spans are emitted from GridControl.razor.cs (lines ~14123 and ~14140) with no scope attribute, so margin, padding and background never apply and the aggregates run together | pending approval | FlexCore, not Showcase |
| SC-254 | New `/demo/toggle-buttons` on the template: ToggleButtonControl + ButtonGroupControl, three examples (toggle, single, multiple vertical), six vocabulary tables | done | nav: Components ▾ → Toggle buttons; catalog Toggle Button row points here; layout accepts several source links |
| SC-255 | New `/demo/fab` on the template: FabMenuControl, two examples (action menu with Disabled switch, custom trigger and content), Parameters, FabMenuItem and CSS tables | done | instances pinned inside a stage with two scoped overrides, stated on the page |
| SC-256 | `/demo/dialog` rebuilt on the template: DialogControl, MessageBoxControl, InputDialogControl, IDialogService; four examples (modal with default button, non-modal window, classic look, service and message box), seven vocabulary tables | done | DialogService registered in Program.cs and DialogHostControl hosted in MainLayout, both new |
| SC-257 | Catalog cleanup: labels and routes follow the nav (Wide Grids, Column Sets, Batch Editing, Column Reorder, Master-Detail, Tree Grid route, Financial & Gauges); duplicates removed (Financial Navigator, MultiSelect Dual List, Pager); Sparkline → Charts, Progress Bar → Page layouts; Most Popular: Data Grid → grid overview, Dialogs and Buttons replace Scheduler & Gantt and Tree Grid | done | 2026-10-12 |
| SC-258 | Control tour accepts `?tab=` so 30 catalog rows land on the right tab (forms, selection, charts, docking, gantt, pdf, ai-prompt) | done | removed when phase 2 splits the tour |
| SC-259 | Catalog rows whose control is demonstrated nowhere: Rich Text Editor (EditorControl), Format Control, Diagram Flowchart, Property Grid, Scheduler, ListBox, Carousel; AppBar is shown only as the tour's own bar | pending approval | drop the rows or build demos; all seven controls exist in the library (Breadcrumb now shown on UI elements) |
| SC-260 | 1–2 examples rule applied to the button pages: Buttons 5 → 2 (variants, icons, sizes and pills in one card, theming on its CSS tab; dropdown), Split button 3 → 2 (Item values dropped), Toggle buttons 3 → 2 (group modes merged), FAB 2 | done | Dialogs still has 4; pending a decision |
| SC-261 | Rule revised to at most three examples; Dialogs 4 → 3 (classic look is now a switch on the modal example) | done | |
| SC-262 | `/demo/notifications` rebuilt on the template: one example (levels, detail, duration, sticky, clear), NotificationService and Notification tables, CSS classes; accessibility states there is no aria-live region | done | |
| SC-263 | `/demo/layout` rebuilt on the template: application shell (page layout with collapsible panels, splitter in the centre, collapse buttons inside the demo) and dashboard grid in CardControls; five vocabulary tables | done | catalog Splitter row now lands on a live splitter |
| SC-264 | Template applied to the remaining Components and Charts pages: UI elements (3 examples), Control tour (1, unchanged workbenches), Tree view (1, parameter table written), Charts (1, gallery), Financial & gauges (3), Sunburst (1) | done | vocabulary pending on UI elements, Control tour, Charts, Financial & gauges, Sunburst; Tree view keyboard notes pending |
| SC-265 | Claims removed while converting: Charts "no JavaScript dependencies", Sunburst "Pure SVG • Zero JS" badge, UI elements accordion and alert copy about the library | done | |
| SC-266 | `/demo/sliders` added (SliderControl + RangeSliderControl): markup slider with a computed price, budget range slider filtering a list; parameter, NumericRange, method, CSS and keyboard tables. SliderControl removed from UI elements; nav entry after Toasts; catalog row "Slider & Range Slider" points here | done | first of five pages split out of UI elements (next: security code, date & time, tabs & accordion, barcodes & QR) |
| SC-267 | `/demo/security-code` added: one verification-code example with OnComplete and Clear; parameter, enum, CSS and keyboard tables; copy states there is no auto-advance or paste handling | done | |
| SC-268 | `/demo/date-time` added (DatePicker, TimePicker, DateRangePicker, Calendar, TimeSpanPicker): appointment (date + time + duration), crane-hire range, multi-day calendar; six parameter tables (DatePicker grid-hosting parameters summarised in one row), methods, CSS, two keyboard tables. Catalog rows for calendar, date, time and time span point here | done | |
| SC-269 | `/demo/tabs-accordion` added (TabControl, TabStripControl, AccordionControl): estimate tabs, closable document strip with reopen, accordion with icons and a disabled item; six tables plus TabStrip CSS variables. Catalog "Tab Control" row still points at the PageControl bench; decide whether it should move here | done | catalog row open |
| SC-270 | `/demo/barcodes` added (BarcodeControl, QrCodeControl): symbology picker with per-type samples and live encoder errors, QR with error-correction level and size. UI elements trimmed to badges/alerts/skeletons, cards/timeline/breadcrumb, switch/rating (nine pending vocabularies). Nav: Security code and Date & time after Sliders, Tabs & accordion under Layout & navigation, new Display heading for UI elements and Barcodes & QR | done | |
| SC-271 | Code tabs added to the three pages that had none: UI elements (Razor per example, C# for the breadcrumb and input fields), Charts gallery (full ten-tab markup and every series field) and Control tour (full workbench markup and the whole code block). Each tab is a verbatim copy of the live markup, checked tag for tag by script | done | 2026-10-13 |
| SC-272 | `/demo/cards` added (CardControl, CardGroupControl): card with image (wwwroot/images/card-kitchen.svg, drawn for the demo), header, footer and an elevation switch; card group from a list with HeaderContent. Tables from source; cards removed from UI elements; nav under Display; catalog card rows point here | done | 2026-10-13 |
| SC-273 | `/demo/checks` added (CheckBoxControl, RadioGroupControl, SwitchControl, RatingControl): select-all with Indeterminate, radio group plus switches, rating with read-only twin; six tables and a keyboard table. Switch and rating removed from UI elements (now five controls, SC-264 list updated); nav after Date & time; catalog checkbox/radio/switch/rating rows point here | done | 2026-10-13 |
| SC-274 | Site stays open (owner decision 2026-10-13). Preview banner above the top bar (AlertControl, closable) linking NuGet and the new `/signup` page. Sign-up / question form built on FormControl + FormItemControl (name, email, company, message, download switch) with a honeypot and a three-per-visit limit; submissions append to a JSON-lines file (`$HOME/data/flexcore-signups.jsonl` on App Service, `App_Data/signups.jsonl` locally, `Signups:Path` overrides). Email delivery of the Community edition link is NOT wired: provider choice (Azure Communication Services or SendGrid) and its key are the owner's | done, email pending | footer and phone drawer link to Sign up |

## 6. One-example-plus-code push (2026-10-17 → 10-20) and what goes beyond it

Review of 2026-10-17 (three scripted audits: page completeness, library coverage, site as a product). Baseline: 44 routed demo pages; 20 complete on FeatureDemoLayout; 13 on SyncfusionDemoLayout with a live grid and a code tab that drops 3–12 attributes of the live markup; 11 with a live demo and no code. Definition of done for the push: FeatureDemoLayout, one live example, Razor and C# tabs verbatim from the live markup, vocabulary as a pending note. Schedule: Fri `/demo/grid`, `/demo/grid/grouping`; Sat `advanced`, `column-virtualization`, `reordering`, `/demo/reports/model-costs`; Mon `batch-editing`, `enterprise-suite`, `column-swap`, `typing-modes`, `unsaved-row-lock`, `dynamic-columns`, `tree-grid`. Everything below is what the schedule does not hold.

| ID | Item | Status | Note |
|---|---|---|---|
| SC-275 | Conversions that need a content decision first: `scenarios` (AgGridSuite, 1115 lines, four scenarios → pick one), `row-selection-50k` (code tab configures grouping/windowing the live grid lacks), `master-detail` (UserPerms, synthetic detail filter SC-189), `virtualization-bench` (unmeasured timings SC-014), `page-control-bench` (renders no PageControl, SC-038), `pivot-formulas` (three hand-typed snippets, SC-217/219), `provider-enterprise` (provider class needs a C# tab), `data-control` (docs-style page; decide whether a DemoExample fits) | reviewed | after Monday |
| SC-276 | Pages reachable only from sibling lists or the catalog: `assembly` and `vendor-prices` (merge into grouping per SC-035, or convert), `items-provider-db` (SqliteProvider, catalog only) | pending approval | merge decision is Baaria's |
| SC-277 | C# tabs missing where the example binds fields: Financial & gauges (five `_…Series` fields), Sunburst (data build, `_metricItems`, `SwitchMetric`; the leaderboard GridControl at L292-308 is in the Demo but not the code tab) | done | 2026-10-17: Financial & gauges got three C# tabs (the series fields, verbatim; the C# comment that sat in the Razor tab moved out). Sunburst: Razor tab is now the live chart markup verbatim (ternaries on `_useDeathsAsMetric`) plus the data-table GridControl; C# tab is a verbatim excerpt (metric items, fields, OnInitialized, LoadData, SwitchMetric, ResetToWorld, HandleNodeClick, HandleNodeHover); both tabs open with a comment naming what is left out. Checked in the browser |
| SC-278 | Grid vocabulary: no grid page has a parameters table. GridControl 218 `[Parameter]`s, TreeGridControl 92, PivotControl 18, DropDownGridControl 25, plus GridColumn, AggregateRow/AggregateColumn. Put GridControl tables on `/demo/grid` split by the partial-class files; feature pages link there | reviewed | biggest documentation hole by weight |
| SC-279 | Coverage: 115 of 191 library components never appear on the site (about 75 top-level controls). New pages by value per hour, one example each: Text inputs (TextBox, NumericTextBox, MaskedTextBox, TextArea, FloatingLabel, SearchBox); Dropdowns & lists (DropDownList, AutoComplete, DropDownTree, MultiSelectCombo, ListBox); Forms & validation (lift from `/signup`); Progress & loaders; Kanban; Gantt (lift from tour); Menus, toolbar & context menu; Diagram; Property grid + DataFilter + XmlViewer; Chips, avatars, segmented; Tooltips & popovers; PDF viewer; AI prompt (canned reply); Navigation (Stepper, SideNavigation, Wizard, BreadCrumb); Docking (DockManager, MdiHost, PageControl); Scheduler; Color pickers; Upload; Signature; List view; Maps; Editor (L); Spreadsheet (L); Report designer (L+, library paths still throw "not implemented") | pending approval | order proposed, Baaria picks |
| SC-280 | Catalog rows whose target does not render the control: TextBox, Numeric TextBox, TextArea → `counterparts?tab=forms` (renders none of them); Progress Bar → `demo/layout` (no ProgressBarControl anywhere); DropDownGrid → `demo/grid` (tag only on Home); Picklist Dropdown → `dynamic-columns` (no dropdown tag); Tab Control → PageControl bench (SC-269). Missing rows: DateRangePicker, Tooltip, MenuBar. Duplicates: Sparkline and "30+ SVG Charts Gallery" both → `demo/charts`. Nav "Export & templates" and "On-demand loading" absent from the catalog | reviewed | fix as the owning pages land |
| SC-281 | Control tour still renders RadioGroupControl, TimePickerControl and CalendarControl, which `/demo/checks` and `/demo/date-time` now own; split per SC-028/034 | reviewed | |
| SC-282 | Claims the code contradicts (owner decision): footer and home "No npm or third-party JavaScript" (MainLayout.razor:117, Home.razor:12) and the "JavaScript Interop Overhead: Zero (Pure Blazor)" row (Home.razor:568) versus 14 first-party scripts in FlexCore/wwwroot and pdf.js vendored in FlexCore.Documents, which this site references; h1 "sub-millisecond 60 FPS virtualization" (Home.razor:15) and the competitor column "$1,200+ / dev / year" are unmeasured; catalog subtitle "70+ … no third-party JavaScript"; FlexCore README "Zero JavaScript dependencies" (library repo, owner-gated) | pending approval | reword to "no npm build step; interop is the library's own scripts" |
| SC-283 | Licensing is unexplained: FlexCore.csproj says MIT, Home says free, the banner offers a "Community edition by email". One owner sentence on `/get-started` and `/signup` saying what NuGet gives versus the email | done | 2026-10-17 per Baaria: "Host-Agnostic & Free" (the only "free" claim on the site) became "MIT license on NuGet; a Community edition is coming by email"; Get Started gained a License section linking the repo LICENSE; the banner and the sign-up intro now say "under the MIT license". Rule from Baaria: whenever availability is mentioned, say MIT license plus upcoming Community edition. What the Community edition contains versus the NuGet package is still unstated (owner) |
| SC-284 | Quick wins under an hour each: `<PageTitle>` and `<h1>` on NotFound.razor; drop the "set ASPNETCORE_ENVIRONMENT=Development" boilerplate from Error.razor; neutral window title at Home.razor:31 ("HomeFront Precision Estimating"); meta description, Open Graph and canonical in App.razor; static robots.txt and a sitemap from SiteNav (both 404 live); Get Started registration steps add `<NotificationDisplayControl />` and `<DialogHostControl />` (toasts never show if followed literally); skip link before the header; delete unreferenced NavMenu.razor(.css) (SC-043); `<h1>` on `/components`; `/health` endpoint; check response compression with `curl --compressed` | reviewed | fit into slack time after the page push |
| SC-285 | Shell accessibility: burger is a label for an opacity-0 checkbox with no focus ring (MainLayout.razor:81); `.nav-dd-link:focus-visible{outline:none}` (MainLayout.razor.css:231); `h1:focus{outline:none}` (app.css:23); no `:root` tokens or `prefers-color-scheme` anywhere, so no dark mode (library has ThemeService) | reviewed | |
| SC-286 | Library gaps a demo would expose (log only, owner-gated; extends SC-253): two `TextAreaControl` types (root and `Editor/`); `SidebarControl` root class `command-panel` breaks the `fx-` prefix; headers citing GhostWriter (InlineCheckBox, Editor/TextArea) and HomeFront forms (CheckItem, SideNavigation, MultiLineEdit, MultiSelectDualList); no `///` summaries on about 35 controls (Avatar, Carousel, Chip, ContextMenu, DropDownTree, Explorer, FileBrowser, ListBox, ListView, MaskedTextBox, Popover, Popup, Scheduler, Signature, Upload, all five AI controls, Map, Spreadsheet, …); ReportDesigner Crystal toolbar commands answer "not implemented yet" and about 20 "not implemented" throws in the runtime; `DateTextControls` (22 params, no header) unclear if public; MapControl needs an external tile URL; ErrorBoundaryControl injects NotificationService | reviewed | for Wadood |
| SC-287 | PageTitle drift outside the SC-008 pass: Assembly "Grid — Assembly — FlexCore", VendorPrices "Grid — Vendor Prices — FlexCore", SqliteProvider "Async ItemsProvider & Database Pushdown — FlexCore DataGrid", PageControlBench "PageControl Multi-Tab Workspace — FlexCore Controls" | reviewed | fix on conversion |
| SC-288 | Vocabulary omissions (none invented; all 31 parameter tables verified against source 2026-10-17): Dialog table lacks VisibleChanged/WidthChanged/HeightChanged/PositionChanged; TreeView lacks the three `*IdsChanged`; MessageBox and InputDialog lack VisibleChanged | reviewed | |
| SC-289 | `/demo/grid` moved onto FeatureDemoLayout: one example (ten-row grid with sorting, filtering, grouping, resizing), Razor and C# tabs generated verbatim from the live markup and the record list, Related list still built from SiteNav, vocabulary pending (SC-278). Copy change: "right-click for menu" removed because GridControl only raises `OnContextMenu` when the host handles it and the page has no handler; "drag a header sideways to reorder" added (AllowColumnReorder defaults to true, GridControl.razor.cs:675) | done | 2026-10-17; first page of the one-example-plus-code push |
| SC-290 | `/demo/grid/grouping` moved onto FeatureDemoLayout: one example (item catalog grouped by Category with average and count aggregates, search bar), Razor and C# tabs generated verbatim (C# tab also carries the CatalogItem record from Data/DemoData.cs), Related links Grid overview, Pivot tables, Assembly and Vendor prices (the last two were otherwise orphaned). Corrections: `ShowGroupCount` dropped (the Count aggregate caption template already printed "(n items)", so captions showed the count twice); internal catalog name and "default sorts" left the intro (no sort is set); "footer repeats them" became "footer repeats the average" (see SC-253 note); a `.catalog` wrapper plus page CSS puts a gap between the caption aggregates | done | 2026-10-17 |
