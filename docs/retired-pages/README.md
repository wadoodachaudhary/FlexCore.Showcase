# Retired pages

Copies of demo pages removed from `Components/Pages` on 2026-10-20, kept as `.txt` so the build ignores them (git history has
them too). Each was replaced by an example on a FeatureDemoLayout page or dropped as a duplicate:

| File | Replaced by |
|---|---|
| `VirtualizationBenchDemo.razor.txt` | `/demo/grid/virtualization-bench` rebuilt on FeatureDemoLayout (Scroll vs paging) |
| `OptimizerBenchDemo.razor.txt` | nothing; the page had no route since its retirement (its address forwards to Scroll vs paging) |
| `AssemblyDemo.razor.txt` | second example on Grouping & totals (`/demo/grid/items`) |
| `VendorPricesDemo.razor.txt` | dropped; it repeated the Grouping & totals catalog example on other data |
| `SqliteProviderBenchDemo.razor.txt` | second example on On-demand loading (`/demo/grid/provider-enterprise`) |
| `NavMenu.razor.txt` | nothing; the side menu had no references |
| `PageControlBenchDemo.razor.txt` | `/demo/page-navigation` (the old address forwards): the library's PageControl is a keyboard navigation region, not a tab control, so the page became its page |
| `AgGridSuiteDemo.razor.txt` | retired 2026-10-23 (SC-338): its four scenarios became Live updates (`/demo/grid/live-updates`), the org chart on Tree grid (`#org`), the warehouse detail on Master-detail (`#warehouses`) and the 100,000 setting on 100k rows; both old addresses forward to Live updates |
| `DataControlDemo.razor.txt` | `/demo/data-control` rebuilt as one example (grid, chart and CSV from one frame) with documentation tables |
| `RadzenComparisonDemo.razor.txt` | retired 2026-10-06 (no competitor grid rendered, unsourced figures) and moved out of the build on 2026-10-24; its old address aliases the Grid overview |
| `CounterpartsDemo.razor.txt` (+ `.razor.css.txt`) | retired 2026-10-25: the Control tour's tabs had moved out one by one (Forms, Pickers, Gauges, Dock manager, Gantt, PDF viewer); the last, the AI prompt framed by an app bar and toolbar, became `/demo/ai-prompt`, which also answers `/demo/counterparts` |
