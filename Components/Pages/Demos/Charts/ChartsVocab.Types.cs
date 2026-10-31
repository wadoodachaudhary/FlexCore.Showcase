// 2026-10-24: ChartType rows for the Charts page Documentation (meeting point 5). Each row was checked against the renderer
// branch in Charts/ChartControl.razor; a type that has no branch of its own says which branch draws it.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos.Charts;

public static partial class ChartsVocab
{
    public static readonly ApiRow[] Types =
    {
        // Comparison
        new("Bar",
            "Vertical columns, one per category; several series are drawn side by side as grouped columns. " +
            "Reads <code>Label</code> as the category text (taken from the first series) and <code>Value</code>; a point's <code>Color</code> is honoured only with a single series, and <code>IsEmpty</code> leaves the slot blank. " +
            "Draws on a 600x300 viewBox with Y ticks from <code>YAxis.TickCount</code> (default 5) and <code>YAxis.Min</code>/<code>Max</code> honoured; every category is labelled with no thinning, cut at 10 characters."),
        new("HorizontalBar",
            "Has no renderer of its own: the type is not in the renderer switch, so it falls to the default branch and draws exactly as <code>Bar</code>, with vertical columns. " +
            "Same fields, 600x300 viewBox, ticks and 10-character labels as <code>Bar</code>. Not in the gallery."),
        new("GroupedBar",
            "Side-by-side columns per category, one per series, coloured by <code>ChartSeries.Color</code> or the palette; a point's <code>Color</code> is ignored even with one series. " +
            "Reads <code>Label</code> from the first series and <code>Value</code> from each series; <code>IsEmpty</code> skips that column. " +
            "Drawn by the bar branch, which already groups any multi-series <code>Bar</code>; 600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), every category labelled, cut at 10 characters."),
        new("StackedBar",
            "Columns stacked per category, one segment per series in the series colour. " +
            "Reads <code>Label</code> from the first series and <code>Value</code> from each; the scale runs from 0 to the largest column total of absolute values, and a negative segment gets zero height. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), every category labelled, cut at 10 characters."),
        new("StackedBar100",
            "As <code>StackedBar</code>, but every segment becomes its percentage of the column total so each column reaches 100. " +
            "Reads <code>Label</code> and <code>Value</code> per series; the Y axis runs 0 to 110 (the fixed 100 plus the 1.1 padding) unless <code>YAxis.Max</code> is set. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), labels cut at 10 characters."),

        // Trend over time
        new("Line",
            "Straight segments through evenly spaced points with a 3px marker at each. " +
            "Reads <code>Label</code> as the X text and <code>Value</code>; X positions come from the point index, not from <code>Label</code> or <code>DateValue</code>; several series draw in the series colour; <code>IsEmpty</code> breaks the line (for this type only). " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), <code>YAxis.Min</code>/<code>Max</code> honoured. " +
            "X labels come from the first series and are thinned to every n/10th point from 20 points up, cut at 8 characters."),
        new("MultiLine",
            "Drawn by the line branch with no handling of its own, so it is <code>Line</code> with several series, which <code>Line</code> already draws. " +
            "The one difference is that the <code>IsEmpty</code> gap applies to <code>Line</code> only; here empty points are plotted at their <code>Value</code>. " +
            "Same 600x300 viewBox, ticks and label thinning as <code>Line</code>. Not in the gallery."),
        new("Spline",
            "A smooth cubic curve through the points (control points at the midpoints), straight when there are fewer than three points, with a marker at each point. " +
            "Reads <code>Label</code> and <code>Value</code> from one or several series, positioned by point index; <code>IsEmpty</code> points are plotted at their <code>Value</code>. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), X labels thinned from 20 points up and cut at 8 characters."),
        new("StepLine",
            "Horizontal-then-vertical steps between consecutive points, with a marker at each point. " +
            "Reads <code>Label</code> and <code>Value</code> from one or several series, positioned by point index. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), X labels thinned from 20 points up and cut at 8 characters. Not in the gallery."),
        new("Area",
            "A <code>Line</code> with the region down to the axis filled at 15 percent opacity, one fill per series. " +
            "Reads <code>Label</code> and <code>Value</code>, positioned by point index; <code>IsEmpty</code> points are plotted at their <code>Value</code>. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), X labels thinned from 20 points up and cut at 8 characters."),
        new("StackedArea",
            "Drawn exactly as <code>Area</code>: each series is filled from its own values down to the axis, so the series overlap and are not stacked. " +
            "Reads <code>Label</code> and <code>Value</code> per series, positioned by point index. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), X labels thinned from 20 points up and cut at 8 characters."),
        new("Sparkline",
            "A single 1.5px polyline with no axes, ticks, labels or markers. " +
            "Reads <code>Value</code> from the first series only, scaled between that series' own minimum and maximum rather than from zero; <code>Label</code> is unused. " +
            "Draws on its own 200x40 viewBox at 100 percent width, <code>Height</code> default 40px, in the series colour."),

        // Part-to-whole
        new("Pie",
            "Slices clockwise from 12 o'clock, each slice's share taken from the absolute <code>Value</code> over the total. " +
            "Reads <code>Label</code> and <code>Value</code> from the first series only; colours per point from <code>Color</code> or the palette by point index. " +
            "300x300 viewBox; nothing is drawn when the total is zero, no labels are drawn inside the chart, and a legend of every slice is rendered below it regardless of <code>Legend.Visible</code>; the tooltip carries value and percentage."),
        new("Donut",
            "As <code>Pie</code> with the centre cut out at <code>InnerRadius</code> times the radius (default 0.6). " +
            "Reads <code>Label</code> and <code>Value</code> from the first series; per-point <code>Color</code> or the palette by index. " +
            "300x300 viewBox, always-on legend below, no in-chart labels."),
        new("Treemap",
            "Has no renderer: the name appears nowhere but the enum and a comment in <code>SunburstModels.cs</code>, so it falls to the default branch and draws as <code>Bar</code>. " +
            "Not in the gallery."),
        new("Funnel",
            "Trapezoid segments top to bottom in data order, each width proportional to the absolute <code>Value</code> over the maximum (80 percent of the width at most); each segment's bottom edge matches the next segment's width, the last tapers to 20 percent, and all segments share one height. " +
            "Reads <code>Label</code> and <code>Value</code> from the first series; colour per point from <code>Color</code> or the palette. " +
            "Draws on a 400x300 viewBox with no axes; labels are centred inside each segment in white 11px text and are not cut."),
        new("Pyramid",
            "The funnel branch with the order reversed and the widths swapped, so segments widen downward and the largest value forms the base. " +
            "Same inputs as <code>Funnel</code> (first series, <code>Label</code>, absolute <code>Value</code>, per-point <code>Color</code>), 400x300 viewBox, white centred labels. Not in the gallery."),
        new("Sunburst",
            "Builds a two-level tree and delegates to <code>SunburstChartControl</code> (700x700 viewBox, <code>Height</code> default 360px, drill-down and breadcrumb switched on). " +
            "Reads the first series: points are grouped by <code>Category</code> (empty becomes <code>General</code>) into inner-ring nodes coloured from the palette, and each point becomes a leaf with <code>Label</code>, <code>Value</code>, <code>Value2</code> as the secondary value (default 0) and <code>Color</code> or a darkened group colour; the root is named from <code>Title</code> or <code>Total</code>. " +
            "The delegate's <code>CenterTitle</code>, <code>MetricName</code> and <code>SecondaryMetricName</code> are not passed through, so the centre reads <code>TOTAL CASES</code> and the tooltip says <code>Total Cases</code> and <code>Total Deaths</code>. Not in the gallery."),

        // Distribution
        new("Histogram",
            "Touching columns, one per point: no binning is computed, so each point is a bin the caller has already built. " +
            "Reads <code>Label</code> and <code>Value</code> from the first series in the series colour (a point's <code>Color</code> is ignored); the Y axis runs from 0 to the maximum. " +
            "600x300 viewBox, Y ticks fixed at 5; X labels are thinned to every n/8th column from 16 points up and are not cut."),
        new("BoxPlot",
            "One box with whiskers per point. " +
            "Reads from the first series: <code>Value</code> = low whisker, <code>Value2</code> = high whisker (default <code>Value</code>), <code>Value3</code> = Q1 (default low), <code>Value4</code> = Q3 (default high), <code>Value5</code> = median (default the midpoint of Q1 and Q3); the box takes the series colour at 30 percent fill, whiskers are a fixed grey, a point's <code>Color</code> is ignored. " +
            "600x300 viewBox, Y ticks fixed at 5, <code>YAxis.Min</code>/<code>Max</code> honoured, every point labelled, cut at 8 characters."),

        // Correlation
        new("Scatter",
            "A 5px circle per point. " +
            "Reads <code>Label</code> parsed as the numeric X (invariant culture; text that does not parse becomes 0) and <code>Value</code> as Y, several series in the series colour; <code>Value2</code> and a point's <code>Color</code> are ignored. " +
            "600x300 viewBox, both axes scaled to the data extent rather than zero with <code>YAxis</code> ignored, Y ticks fixed at 5, and no X tick labels or axis titles are drawn."),
        new("Bubble",
            "As <code>Scatter</code> with the radius taken from <code>Value2</code>: 4 plus 20 times <code>Value2</code> over the largest <code>Value2</code>, or 5 when <code>Value2</code> is null. " +
            "Reads <code>Label</code> as the numeric X and <code>Value</code> as Y, several series in the series colour at 70 percent opacity. " +
            "600x300 viewBox, Y ticks fixed at 5, no X tick labels."),
        new("Heatmap",
            "A grid of cells: one row per series (the series <code>Name</code> is the row label), one column per <code>Label</code> of the first series, each cell shaded by <code>Value</code> between the data minimum and maximum: from a 10 percent tint of <code>ColorPalette</code> entry 0 to the colour itself when that entry is a hex colour, otherwise on a fixed white-to-blue ramp. " +
            "<code>Category</code>, a point's <code>Color</code> and the series colours are not read (each row is a series). " +
            "600x300 viewBox with no colour scale or legend; every row label is cut at 10 characters and every column label at 8, with no thinning."),

        // Ranking
        new("DotPlot",
            "Horizontal rows, one per point, with a 5px dot at <code>Value</code> on a shared X axis and a thin leader line from the left edge. " +
            "Reads <code>Label</code> and <code>Value</code> from the first series, colour per point from <code>Color</code> or the palette; the X axis spans the data extent, not from zero. " +
            "600x300 viewBox, X ticks fixed at 5, every row labelled, cut at 12 characters. Not in the gallery."),

        // Multivariate
        new("Radar",
            "A polygon per series on spokes taken from the first series' <code>Label</code>s, over four fixed rings. " +
            "Reads <code>Label</code> and <code>Value</code> from one or several series in the series colour at 20 percent fill; the scale runs from 0 to the maximum over all series. " +
            "300x300 viewBox; nothing is drawn with fewer than three spokes, ring values are not printed, and spoke labels are cut at 8 characters."),

        // Single value / KPI and gauges
        new("Gauge",
            "A 270-degree arc from the lower left with a needle, or a full ring without a needle when <code>GaugeCircular</code> is set, drawn by <code>GaugeVisualControl</code>. " +
            "Reads <code>GaugeValue</code>, falling back to the first point's <code>Value</code> of the first series and then 0; <code>GaugeMin</code>/<code>GaugeMax</code> default 0 and 100; <code>GaugeRanges</code> adds coloured bands and <code>GaugePointers</code> extra needles; the value colour is the series colour or the palette. " +
            "300x300 viewBox, <code>Height</code> default 260px, tick labels from <code>GaugeTickCount</code> (default 5, allowed 1 to 100) in <code>GaugeLabelFormat</code>, the centre value in <code>ValueFormat</code> (default N2). " +
            "Throws <code>ArgumentException</code> when <code>Max</code> is not above <code>Min</code> or a value is not finite; not gated by the <code>No data</code> message. Not in the gallery: the gauges have <a href=\"demo/gauges\">their own page</a>, built on the gauge controls."),
        new("KpiCard",
            "An HTML card rather than SVG: <code>KpiValue</code> (or <code>--</code>) in large type, <code>KpiLabel</code> beneath, and <code>KpiTrend</code> with an up arrow when it starts with + or contains the word up, a down arrow when it starts with - or contains down. " +
            "Reads no series data at all and ignores <code>Height</code>. Not gated by the <code>No data</code> message."),
        new("LinearGauge",
            "A horizontal bar gauge drawn by <code>GaugeVisualControl</code> through the same branch as <code>Gauge</code>: a grey track, <code>GaugeRanges</code> as coloured bands, a value bar, <code>GaugePointers</code> as vertical ticks, and the value printed above. " +
            "Reads <code>GaugeValue</code> (falling back to the first point's <code>Value</code>, then 0) between <code>GaugeMin</code> and <code>GaugeMax</code>. " +
            "Draws on a 320x105 viewBox, <code>Height</code> default 260px, with <code>GaugeTickCount</code> + 1 tick labels. Not in the gallery."),
        new("ArcGauge",
            "The 270-degree arc (360 when <code>GaugeCircular</code>) with the value shown as a filled sweep and no needle; <code>GaugePointers</code> still draw as needles. " +
            "Same inputs as <code>Gauge</code>: <code>GaugeValue</code> or the first point's <code>Value</code>, <code>GaugeMin</code>/<code>GaugeMax</code>, <code>GaugeRanges</code>. " +
            "300x300 viewBox; the value sweep narrows from 16 to 7 units when ranges are present. Not in the gallery."),
        new("RadialGauge",
            "The same arc as <code>ArcGauge</code> plus a needle, which is drawn whether or not <code>GaugeCircular</code> is set. " +
            "Same inputs as <code>Gauge</code>. 300x300 viewBox, <code>Height</code> default 260px. Not in the gallery."),
        new("Bullet",
            "One horizontal row per point: three fixed grey bands at 60, 85 and 100 percent of the width, a bar for the actual value (dark slate unless a point's <code>Color</code>, the series <code>Color</code> or <code>ColorPalette</code> entry 0 is set) and a target line (red unless <code>ColorPalette</code> entry 1 is set). " +
            "Reads from the first series <code>Value</code> as actual and <code>Value2</code> as target (default <code>Value</code>); the scale ends at 1.2 times the largest of the two; <code>Category</code> is not read. " +
            "Draws on a 600 by max(120, rows x 60) viewBox with rows 50 apart and prints no numbers at all: no ticks, no values, only each row's <code>Label</code>, not cut. Not in the gallery."),

        // Financial
        new("Candlestick",
            "A wick from high to low and a body from open to close per point, green when close is at or above open and red otherwise, or <code>UpColor</code> and <code>DownColor</code> when set. " +
            "Reads from the first series <code>Value</code> = open, <code>Value2</code> = high, <code>Value3</code> = low, <code>Value4</code> = close (each defaulting to <code>Value</code>); <code>DateValue</code>, a point's <code>Color</code> and <code>ColorPalette</code> are not read. " +
            "Draws on a 600x320 viewBox, <code>Height</code> default 320px, Y ticks fixed at 5 with one decimal, the scale from the lowest low to the highest high with no padding, every point labelled, cut at 8 characters. Not in the gallery."),
        new("Ohlc",
            "A vertical high-low line with a left tick at open and a right tick at close, green or red by direction (<code>UpColor</code>, <code>DownColor</code>), from the financial branch. " +
            "Same fields as <code>Candlestick</code>: <code>Value</code> open, <code>Value2</code> high, <code>Value3</code> low, <code>Value4</code> close, first series only. " +
            "600x320 viewBox, Y ticks fixed at 5, labels cut at 8 characters. Not in the gallery."),
        new("HighLow",
            "A single 2.5px vertical line per point from <code>Value3</code> (low) to <code>Value2</code> (high) in blue, or a point's <code>Color</code>, the series <code>Color</code> or <code>ColorPalette</code> entry 0; open and close are not drawn. " +
            "Reads the first series. " +
            "600x320 viewBox, Y ticks fixed at 5, labels cut at 8 characters. Not in the gallery."),

        // Range intervals
        new("RangeBar",
            "A vertical bar per point from <code>Value</code> (minimum) to <code>Value2</code> (maximum, default <code>Value</code>), at least 3 units tall, at 85 percent opacity in blue, or a point's <code>Color</code>, the series <code>Color</code> or <code>ColorPalette</code> entry 0. " +
            "Reads the first series; the Y axis spans the data extent, widened by 10 either side when flat. " +
            "600x300 viewBox, Y ticks fixed at 5, every point labelled, cut at 8 characters. " +
            "Despite the name the bars are vertical: the range branch has no horizontal variant. Not in the gallery."),
        new("RangeColumn",
            "Drawn by the range branch with no check on the type, so it is identical to <code>RangeBar</code>: vertical minimum-to-maximum bars from <code>Value</code> and <code>Value2</code> of the first series, same colours, 600x300 viewBox, Y ticks fixed at 5, labels cut at 8 characters. " +
            "Not in the gallery."),
        new("RangeArea",
            "Also identical to <code>RangeBar</code>: the range branch draws bars for all three range types and has no area or band variant. " +
            "Same fields, viewBox and limits. Not in the gallery."),

        // Flow and project
        new("Sankey",
            "A ribbon per point from a left source node to a right target node, with curved sides, in blues unless coloured: source nodes and ribbons take the series <code>Color</code> or <code>ColorPalette</code> entry 0, target nodes entry 1, and a point's <code>Color</code> paints its own ribbon. " +
            "Reads the first series: <code>Label</code> is the source, <code>Category</code> the destination, <code>Value</code> the flow (ribbon height is the flow's share of 80 percent of the plot height, at least 8 units). " +
            "Draws on a 650x340 viewBox, <code>Height</code> default 340px, labels not cut. " +
            "Every flow gets its own source and target node, so a name that appears in several flows is drawn once per flow rather than merged, and the column grows by 10 units per flow without being clamped to the height. Not in the gallery."),
        new("Gantt",
            "A horizontal bar per task on a numeric timeline with five vertical ticks. " +
            "Reads the first series: <code>Label</code> as the row label (cut at 12 characters), <code>Value</code> as the start, <code>Value2</code> as the end (default start + 1), colour per point from <code>Color</code> or the palette; <code>DateValue</code> and <code>Category</code> are not read, so dates must be supplied as numbers. " +
            "600x300 viewBox; rows are capped at 30 units tall, so a few tasks leave the lower part of the plot empty, and bars are at least 2 units wide. Not in the gallery: the <a href=\"demo/gantt\">Gantt chart page</a> uses <code>GanttControl</code>."),

        // Statistical
        new("Waterfall",
            "Floating bars that step along a running total, joined by dashed connectors. " +
            "Reads <code>Label</code> and <code>Value</code> from the first series; every point is added to the running total, so there is no way to mark a point as a subtotal or total bar. " +
            "Positive steps are green and negative ones red, or <code>UpColor</code> and <code>DownColor</code>; a point's <code>Color</code> wins over both and <code>ColorPalette</code> is not read. " +
            "600x300 viewBox, Y ticks fixed at 5, <code>YAxis.Min</code>/<code>Max</code> honoured, every point labelled, cut at 8 characters."),
        new("Pareto",
            "Falls to the bar branch: columns of the first series in data order (not sorted) plus a cumulative-percentage polyline in red (or <code>ColorPalette</code> entry 1 when the palette has one), scaled so that 100 percent is the top of the plot, with no second axis. " +
            "Reads <code>Label</code> and <code>Value</code>; with more than one series the extra series become grouped columns while the line still follows the first. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), every category labelled, cut at 10 characters."),

        // Combination
        new("ComboBarLine",
            "Falls to the bar branch: with two or more series every series is first drawn as grouped columns, then the second series is drawn again as a line with markers in its own colour, scaled to its own maximum with no second axis. " +
            "Reads <code>Label</code> from the first series and <code>Value</code> from each; with a single series only columns appear. " +
            "600x300 viewBox, Y ticks from <code>YAxis.TickCount</code> (default 5), labels cut at 10 characters. Not in the gallery."),
    };
}
