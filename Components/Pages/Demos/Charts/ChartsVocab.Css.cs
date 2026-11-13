// 2026-10-24: ChartControl CSS classes, CSS variables, events and methods for the Charts page Documentation, from
// Charts/ChartControl.razor.css and the ChartControl.razor markup. 2026-11-13: hover rows (fx-pt to fx-chart-tooltip-row) added.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos.Charts;

public static partial class ChartsVocab
{
    // Notes (2026-10-24):
    // - ChartControl.razor.css is scoped (`.fx-chart-title[b-...]`): its rules reach only elements ChartControl renders; a page
    //   stylesheet can still target the class names below.
    // - SVG modes put no class on svg elements except `fx-pt` on hoverable shapes and `fx-pt-hit` on financial hit rects
    //   (2026-10-31): fill, stroke and font-size are viewBox-unit attributes (SvgText in ChartControl.razor), so neither these
    //   rules nor --fx-zoom reach them. Only `.fx-chart svg` (display:block; width:100%) and `.fx-pt` hover rules reach the svg.
    // - Gauge, ArcGauge, LinearGauge, RadialGauge render a GaugeVisualControl (bare <svg>, no classes); Sunburst renders
    //   SunburstChartControl (own `fx-sunburst-*` classes). Neither set is listed here.
    public static readonly ApiRow[] Css =
    {
        new("fx-chart", "The root <code>div</code> in every mode; <code>CssClass</code> is appended here and <code>Width</code> becomes its inline width. Position relative, full width, system font stack, 12px text, colour <code>#212529</code>. The scoped rule <code>.fx-chart svg</code> makes the svg ChartControl draws itself <code>display:block; width:100%</code>; the gauge and Sunburst svgs come from child components and are not reached. Inside ChartControl's own svg only the hover shapes carry classes (<code>fx-pt</code>, <code>fx-pt-hit</code>). Leaving the root hides the hover card."),
        new("fx-chart-title", "The <code>Title</code> line above the chart, rendered only when <code>Title</code> is set: 14px, weight 600, <code>#1f2937</code>, 8px below."),
        new("fx-chart-empty", "The <code>No data</code> placeholder: <code>#9ca3af</code>, italic, centred, 24px padding. Shown in Bars mode when neither <code>Bars</code> nor a resolvable <code>DataSource</code> has rows, and in SVG mode when <code>Series</code> is empty for any type except the four gauges and <code>KpiCard</code>."),
        new("fx-chart-legend", "SVG mode only: the wrapping flex row under the drawing, centred, 12px gap, 8px vertical padding. Rendered when <code>Legend.Visible</code> is not false and there are at least two series; Pie and Donut always render one entry per slice whatever <code>Legend</code> says. <code>ChartLegend.Position</code> is not read, so the legend always sits below."),
        new("fx-chart-legend-item", "One legend entry: a flex row with a 4px gap, 12px text, <code>#4b5563</code>. Holds the swatch and a <code>span</code> with the series name (the slice label for Pie and Donut)."),
        new("fx-chart-legend-swatch", "The 12px colour square, 2px radius; its <code>background</code> is inline: the series colour, the <code>ColorPalette</code> entry or the <code>ChartDataPoint.Color</code> the chart used."),
        new("fx-pt", "SVG mode, hover on (<code>ShowTooltips</code> true, the default, or a <code>PointHover</code> delegate): the class on each data point's shape (bar, slice, marker, cell, segment, flow). Hovered: <code>brightness(1.12) saturate(1.08)</code>, and paths and polygons (slices, funnel segments, Sankey flows) take <code>stroke-width: 3</code>; the svg's other <code>fx-pt</code> shapes drop to 0.72 opacity. Filter and opacity ease over 0.12s; the stroke change is instant. Not set on gauges, <code>KpiCard</code>, <code>Sparkline</code>, <code>Sunburst</code>, the Bars and DataSource modes, <code>Candlestick</code>, <code>Ohlc</code> and <code>HighLow</code> (they use <code>fx-pt-hit</code>), or charts drawn as a bitmap (<code>CategoryLegend</code>, <code>ThreeDimensional</code>, a supported <code>ReportPlot</code>, or both print sizes set on a type with a printed stage image)."),
        new("fx-pt-hit", "<code>Candlestick</code>, <code>Ohlc</code> and <code>HighLow</code> with hover on: a transparent rect over each price slot (80% of its width, high to low, at least 4 units tall) that takes the hover. The candle or bar under it has no class, so it is not highlighted. No CSS rule."),
        new("fx-chart-tooltip", "The hover card: an HTML <code>div</code> with <code>role=\"status\"</code> after the drawing (after the slice legend for Pie and Donut), rendered while the pointer is on a shape and <code>ShowTooltips</code> is true. <code>position: fixed</code> where the pointer entered the shape (inline <code>left</code>/<code>top</code>; it does not follow the pointer within the shape), offset 14px right and down, z-index 1100, <code>pointer-events: none</code>. Background <code>rgba(17, 24, 39, 0.94)</code>, white 12px text, 8px radius, <code>8px 11px</code> padding, shadow, 120 to 280px wide, no wrapping. Holds <code>TooltipTemplate</code> when set, else the title line and rows below."),
        new("fx-tip-left", "Added to <code>fx-chart-tooltip</code> when the shape's anchor point lies in the right 45% of the viewBox: the card opens to the left. The anchor is the top centre of a bar, box, cell or candle, the centre of a marker or segment, the mid-point of a slice."),
        new("fx-tip-up", "Added to <code>fx-chart-tooltip</code> when the shape's anchor point (see <code>fx-tip-left</code>) lies in the bottom 40% of the viewBox: the card opens above. With <code>fx-tip-left</code>, above and to the left."),
        new("fx-chart-tooltip-title", "Default card only: the first line, the swatch and <code>ChartTooltipContext.Title</code>. Flex row, 8px gap, weight 600, 2px below. <code>TooltipFormat</code> and <code>TooltipFormatter</code> replace the rows, not this line."),
        new("fx-chart-tooltip-swatch", "The 10px colour square in the title line, 2px radius; its inline <code>background</code> is the shape's colour (<code>ChartTooltipContext.Color</code>)."),
        new("fx-chart-tooltip-row", "One row of the default card: the name in a <code>span</code>, left out when empty, and the value in <code>b</code> (weight 600), at the two edges, at least 16px apart. Rows come from the text of <code>TooltipFormatter</code>, or of <code>TooltipFormat</code> when the formatter is unset or returns null, one nameless row per line; otherwise from <code>ChartTooltipContext.Fields</code>."),
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

    // No `--fx-chart-*` variable: the only var() in ChartControl.razor.css is `--fx-zoom`, in font-size rules (2026-10-24).
    // Colours are literals, listed per class above; `--fx-surface-color`, `--fx-text-color`, `--fx-border-color` (FlexCore's
    // wwwroot/flexkit-themes.css, not linked by the Showcase) are not referenced, so the chart does not follow a site theme.
    public static readonly ApiRow[] CssVars =
    {
        new("--fx-zoom", "Multiplies the font size of the HTML text: root 12px, title 14px, legend item 12px, tooltip card 12px, KPI value 48px, KPI label and trend 14px, legacy row label and value 11px, multi-row label 11px, bar value 9px and axis 10px, each written as <code>calc(Npx * var(--fx-zoom, 1))</code>. SVG text is unaffected: its sizes are <code>font-size</code> attributes of 9 to 11 viewBox units (6.5 in the Bar print layout) that scale with the drawing, not with the zoom. Read from the nearest ancestor that publishes it; <code>PageControl</code> does so with <code>ZoomService.ZoomScopeStyle</code>. The Showcase's app.css does not define it, so the charts on this page render at the fallback.", "1"),
    };

    // Events: PointHover (2026-10-31) is the only EventCallback; it is a row in Params with the tooltip parameters. Drill-down
    // exists only in the child SunburstChartControl (own page).
    // Methods: none public on ChartControl; its public members are the 51 [Parameter] properties and the nested
    // ChartControl.ChartBar class. The static ChartSvg.Markup (Charts/ChartSvg.cs:16) is the one callable entry point, the
    // single row below; title that table "ChartSvg", not "ChartControl methods".
    public static readonly ApiRow[] Methods =
    {
        new("ChartSvg.Markup(ChartType type, IReadOnlyList&lt;ChartSeries&gt; series, string? title = null, bool showLegend = true, string? width = \"100%\", string? height = \"300px\")",
            "A string, so it carries no hover tooltips. Static, in <code>Fx.ControlKit.Charts</code>. Renders a <code>ChartControl</code> with these six parameters on a renderer of its own, disposed before it returns, and returns its HTML, svg included, for callers that cannot host a component. Nothing is shared between calls. <code>Legend</code> is <code>new ChartLegend { Visible = showLegend }</code>, <code>Animate</code> is false and a null title becomes an empty string. The string carries the class names and scope attributes but no stylesheet.",
            "string"),
    };
}
