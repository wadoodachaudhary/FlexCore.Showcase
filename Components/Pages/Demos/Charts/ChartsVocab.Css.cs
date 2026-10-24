// 2026-10-24: ChartControl CSS classes, CSS variables and the event/method surface for the Charts page Documentation
// (meeting point 5). Checked against Charts/ChartControl.razor.css and the markup in ChartControl.razor.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos.Charts;

public static partial class ChartsVocab
{
    // Notes for the page author (2026-10-24):
    // - ChartControl.razor.css is a scoped stylesheet (the FlexCore bundle compiles `.fx-chart-title[b-...]`), so its rules reach
    //   only the elements ChartControl renders itself; a page stylesheet can still target the class names below.
    // - The SVG modes put no class on any svg element: fill, stroke and font-size are attributes in viewBox units
    //   (SvgText, ChartControl.razor:472-488), so neither these rules nor --fx-zoom reach them. Only `.fx-chart svg`
    //   (display:block; width:100%) applies to the svg itself.
    // - Gauge, ArcGauge, LinearGauge and RadialGauge render a GaugeVisualControl (a bare <svg>, no classes); Sunburst renders a
    //   SunburstChartControl with its own `fx-sunburst-*` classes. Neither set is listed here.
    public static readonly ApiRow[] Css =
    {
        new("fx-chart", "The root <code>div</code> in every mode; <code>CssClass</code> is appended here and <code>Width</code> becomes its inline width. Position relative, full width, system font stack, 12px text, colour <code>#212529</code>. The descendant rule <code>.fx-chart svg</code> makes each chart's svg <code>display:block; width:100%</code>; the svg elements themselves carry no classes."),
        new("fx-chart-title", "The <code>Title</code> line above the chart, rendered only when <code>Title</code> is set: 14px, weight 600, <code>#1f2937</code>, 8px below."),
        new("fx-chart-empty", "The <code>No data</code> placeholder: <code>#9ca3af</code>, italic, centred, 24px padding. Shown in Bars mode when neither <code>Bars</code> nor a resolvable <code>DataSource</code> has rows, and in SVG mode when <code>Series</code> is empty for any type except the four gauges and <code>KpiCard</code>."),
        new("fx-chart-legend", "SVG mode only: the wrapping flex row under the drawing, centred, 12px gap, 8px vertical padding. Rendered when <code>Legend.Visible</code> is not false and there are at least two series; Pie and Donut always render one entry per slice whatever <code>Legend</code> says. <code>ChartLegend.Position</code> is not read, so the legend always sits below."),
        new("fx-chart-legend-item", "One legend entry: a flex row with a 4px gap, 12px text, <code>#4b5563</code>. Holds the swatch and a <code>span</code> with the series name (the slice label for Pie and Donut)."),
        new("fx-chart-legend-swatch", "The 12px colour square, 2px radius; its <code>background</code> is inline: the series colour, the <code>ColorPalette</code> entry or the <code>ChartDataPoint.Color</code> the chart used."),
        new("fx-chart-kpi", "<code>KpiCard</code> only (HTML, no svg): the centred card, 24px padding."),
        new("fx-chart-kpi-value", "The big number: <code>KpiValue</code>, or <code>--</code> when it is null. 48px, weight 800, <code>#0d3b66</code>."),
        new("fx-chart-kpi-label", "<code>KpiLabel</code> under the value, rendered only when set: 14px, <code>#6b7280</code>, 4px above."),
        new("fx-chart-kpi-trend", "<code>KpiTrend</code> under the label, rendered only when set: 14px, 8px above. Gets <code>up</code> (<code>#16a34a</code>, prefixed with an up arrow) when the text starts with <code>+</code> or contains <code>up</code>, <code>down</code> (<code>#dc2626</code>, down arrow) when it starts with <code>-</code> or contains <code>down</code>; any other text keeps the inherited colour and gets no arrow."),
        new("fx-chart-rows", "Bars mode only: the column of horizontal bars, 4px apart."),
        new("fx-chart-row", "One bar line: label, track and value in a flex row with an 8px gap, at least 22px high."),
        new("fx-chart-row-label", "The label cell, rendered when <code>ShowLabels</code> is true: 11px, <code>#444</code>, right-aligned, clipped with an ellipsis. <code>LabelStyle</code> (default <code>min-width:160px;</code>) is its inline style."),
        new("fx-chart-row-bar-container", "The track that fills the rest of the line, at least 18px high, with a 1px <code>#e5e7eb</code> line at its right edge drawn as a gradient."),
        new("fx-chart-row-bar", "The bar: 14px high, 2px radius, width animated over 0.2s. Its inline width is the value as a percentage of the largest bar and its inline background is <code>ChartBar.Color</code> or <code>BarColor</code>; the <code>title</code> tooltip reads <code>Label: value</code> in <code>N2</code>."),
        new("fx-chart-row-value", "The number after the bar, rendered when <code>ShowValues</code> is true and formatted with <code>ValueFormat</code>: 11px, <code>#555</code>, tabular numerals."),
        new("fx-chart-multi", "DataSource mode only: the column of grouped-bar rows, 6px apart."),
        new("fx-chart-multi-row", "One source row: label and bar group side by side, at least 60px high, 1px <code>#f0f0f0</code> rule underneath."),
        new("fx-chart-multi-label", "The row label (<code>LabelField</code>), rendered when <code>ShowLabels</code> is true: 11px, <code>#333</code>, vertically centred, ellipsis, full text in its <code>title</code>. <code>LabelStyle</code> is its inline style."),
        new("fx-chart-multi-bars", "The group of vertical bars for the row: bottom-aligned, 3px apart, at least 50px high, 1px <code>#cbd5e1</code> baseline."),
        new("fx-chart-bar-cell", "One bar column (one per <code>ValueFields</code> entry), equal width, minimum 4px; its <code>title</code> tooltip is the matching <code>ValueLabels</code> entry and the value in <code>N2</code>."),
        new("fx-chart-bar", "The vertical bar: full cell width, 1px top radius, height animated over 0.2s. Inline height is the value as a percentage of the largest value in the whole table and inline background is <code>BarColor</code>; there is no per-bar colour in this mode."),
        new("fx-chart-bar-value", "The number above a bar, rendered when <code>ShowValues</code> is true: 9px, <code>#555</code>, tabular numerals, line-height 1."),
        new("fx-chart-axis", "The bottom tick row, rendered when <code>ShowAxisLabels</code> is true and <code>ValueLabels</code> has entries: 10px, <code>#666</code>."),
        new("fx-chart-axis-label-spacer", "The empty cell under the label column. Its width is a fixed 160px, so a <code>LabelStyle</code> with another <code>min-width</code> puts the ticks out of line with the bars."),
        new("fx-chart-axis-bars", "The flex row holding the ticks, spaced like the bar cells (3px gap, space-between)."),
        new("fx-chart-axis-tick", "One tick per <code>ValueLabels</code> entry, centred, clipped with an ellipsis."),
    };

    // The stylesheet declares no `--fx-chart-*` variable (grep of ChartControl.razor.css, 2026-10-24: the only var() it reads is
    // `--fx-zoom`, in eleven font-size rules). Colours are literal hex values, listed per class above; the site variables
    // `--fx-surface-color`, `--fx-text-color` and `--fx-border-color` (declared in FlexCore's wwwroot/flexkit-themes.css, which
    // the Showcase does not link) are not referenced, so the chart does not follow a site theme.
    public static readonly ApiRow[] CssVars =
    {
        new("--fx-zoom", "Multiplies the font size of the HTML text: root 12px, title 14px, legend item 12px, KPI value 48px, KPI label and trend 14px, legacy row label and value 11px, multi-row label 11px, bar value 9px and axis 10px, each written as <code>calc(Npx * var(--fx-zoom, 1))</code>. SVG text is unaffected: its sizes are <code>font-size</code> attributes of 9 to 11 viewBox units that scale with the drawing, not with the zoom. Read from the nearest ancestor that publishes it; <code>PageControl</code> does so with <code>ZoomService.ZoomScopeStyle</code>. The Showcase's app.css does not define it, so the charts on this page render at the fallback.", "1"),
    };

    // Events: none. ChartControl.razor declares no EventCallback parameter and no @on* handler (grep 2026-10-24); the SVG shapes
    // carry <title> tooltips only. Drill-down exists only inside the child SunburstChartControl, documented on its own page.
    // Methods: ChartControl itself has no public method; its public members are the 36 [Parameter] properties and the nested
    // ChartControl.ChartBar class (ChartControl.razor:1809-1816). The static ChartSvg.Markup helper (Charts/ChartSvg.cs:20) is the
    // one callable entry point, so it is the single row below; title that table "ChartSvg" rather than "ChartControl methods".
    public static readonly ApiRow[] Methods =
    {
        new("ChartSvg.Markup(ChartType type, IReadOnlyList&lt;ChartSeries&gt; series, string? title = null, bool showLegend = true, string? width = \"100%\", string? height = \"300px\")",
            "Static, in <code>Fx.ControlKit.Charts</code>. Renders a <code>ChartControl</code> with these six parameters through one process-wide <code>HtmlRenderer</code> (calls serialised by a lock) and returns its HTML, svg included, for callers that cannot host a component; report pagination uses it. <code>Legend</code> is <code>new ChartLegend { Visible = showLegend }</code>, <code>Animate</code> is false and a null title becomes an empty string. The string carries the class names and scope attributes but no stylesheet.",
            "string"),
    };
}
