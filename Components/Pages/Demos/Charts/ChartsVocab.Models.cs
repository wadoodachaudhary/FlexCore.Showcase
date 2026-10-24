// 2026-10-24: chart model rows for the Charts page Documentation (meeting point 5). Every description was checked against
// the read sites in Charts/ChartControl.razor, so a property no renderer reads says so. ChartBar is the nested class
// ChartControl.ChartBar (Charts/ChartControl.razor, legacy Bars list), not a ChartModels.cs type. The read-only aliases on
// ChartDataPoint (Open/High/Low/Close, Actual/Target, Source/Destination) are documented inline on the fields and factories
// they forward to rather than as rows of their own. GaugeRange and GaugePointer are on the Gauges page.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos.Charts;

public static partial class ChartsVocab
{
    /// <summary>ChartSeries: one named list of points.</summary>
    public static readonly ApiRow[] Series =
    {
        new("Name", "The series name. Shown in the legend when the chart has more than one series, in the tooltips of grouped and stacked bars, and as the row label of a Heatmap, where each series is one row (cut at 10 characters).", "string", "\"\""),
        new("Color", "Overrides the palette colour for this series. Read by the Bar family (bars and the <code>ComboBarLine</code> line), the Line and Area family, Sparkline, Radar, Box Plot, Histogram, the gauge arc and the first group of a Sunburst. Pie, Donut, Funnel, Pyramid, Gantt and Dot Plot colour by point and only take it when there are several series; Waterfall, Heatmap, Candlestick, Ohlc, HighLow, Bullet, the Range charts and Sankey draw fixed colours and ignore it.", "string?", "null"),
        new("Type", "Declared, but never read: the control's own <code>Type</code> parameter decides how every series is drawn. <code>ComboBarLine</code> draws the first series as bars and the second as a line scaled to its own maximum, whatever their <code>Type</code>.", "ChartType", "ChartType.Bar"),
        new("DataPoints", "The points. The first series supplies the category labels for the Bar and Line families, Radar and the Heatmap columns. Pie, Donut, Sunburst, Funnel, Pyramid, Sparkline, Waterfall, Histogram, Box Plot, Dot Plot, Gantt, Bullet, the financial, Range and Sankey charts and the gauge read the first series only; grouped and stacked bars, the Line family, Scatter, Bubble, Radar and Heatmap read every series.", "List&lt;ChartDataPoint&gt;", "new()"),
        new("ChartSeries(string name, ChartType type, IEnumerable&lt;ChartDataPoint&gt;? points = null, string? color = null)", "Sets <code>Name</code>, <code>Type</code> and <code>Color</code> and copies the points into <code>DataPoints</code>; the parameterless constructor leaves the defaults.", "ChartSeries"),
    };

    /// <summary>ChartDataPoint: one reading; the extra numbers change meaning per chart type.</summary>
    public static readonly ApiRow[] Point =
    {
        new("Label", "The category text. Axis label in the Bar family (cut at 10 characters), the Line family (8, every tenth point when there are many), Radar (8), Waterfall (8), Box Plot (8), Histogram (every eighth), the financial charts (8) and the Range charts (8); slice name in the Pie and Donut tooltip and legend; segment text in Funnel and Pyramid; leaf name in Sunburst; column header in Heatmap (8, from the first series); row label in Gantt (12), Dot Plot (12) and Bullet; source node in Sankey, also readable as <code>Source</code>. Scatter and Bubble parse it as the numeric x value with <code>double.TryParse</code> (invariant culture), so a name puts the point at x = 0. Not read by Sparkline, the gauges or the KPI card.", "string", "\"\""),
        new("Value", "The number every renderer plots: bar height, line y, slice share (absolute value), cell intensity in Heatmap, bar start in Gantt, lower whisker in Box Plot, <code>Open</code> in the financial charts, <code>Actual</code> in Bullet, range start in the Range charts, flow width in Sankey, the delta added to the running total in Waterfall, and the gauge reading when <code>GaugeValue</code> is not set (first point of the first series). <code>StackedBar100</code> turns it into a share of the column total.", "double", "0"),
        new("IsEmpty", "Marks a missing reading. The Bar family leaves the slot empty instead of drawing a zero bar and keeps the point out of the axis range; <code>Line</code> breaks the line at the gap, and the whole Line and Area family drops the point from the range. Every other type ignores it and plots <code>Value</code>.", "bool", "false"),
        new("Value2", "Second number, meaning per type: Bubble size (radius 4 to 24, scaled to the largest <code>Value2</code>; a point without one is radius 5); Box Plot upper whisker; <code>High</code> in Candlestick, Ohlc and HighLow; <code>Target</code> in Bullet; range end in RangeBar, RangeColumn and RangeArea; bar end in Gantt (null means start + 1); the secondary value of a Sunburst leaf, shown in its statistics panel. Where it is required and null the renderer uses <code>Value</code>. Other types ignore it.", "double?", "null"),
        new("Value3", "Box Plot first quartile (null falls back to <code>Value</code>, the lower whisker); <code>Low</code> in Candlestick, Ohlc and HighLow (null falls back to <code>Value</code>). Not read by any other type.", "double?", "null"),
        new("Value4", "Box Plot third quartile (null falls back to <code>Value2</code>, the upper whisker); <code>Close</code> in Candlestick and Ohlc (null falls back to <code>Value</code>; a close at or above the open draws green, below it red). Not read by any other type.", "double?", "null"),
        new("Value5", "Box Plot median (null falls back to the midpoint of the quartiles). Not read by any other type.", "double?", "null"),
        new("Color", "Colour of this point. Read directly by a single-series Bar, by Waterfall (replacing the green-for-positive, red-for-negative default) and by the Sunburst leaf, and through <code>GetPointColor</code> by Pie, Donut, Funnel, Pyramid, Gantt and Dot Plot, which otherwise take the series colour when there are several series, else the palette by point index. Not read by the Line family, grouped and stacked bars, Scatter, Bubble, Radar, Heatmap, Box Plot, Histogram, the financial, Bullet, Range and Sankey charts.", "string?", "null"),
        new("Category", "Sunburst groups points by it into the inner ring (empty means <code>General</code>); Sankey reads it as the target node, through <code>Destination</code>. Declared for Gantt resources too, but the Gantt renderer never reads it, and Heatmap rows come from the series names, not from here.", "string?", "null"),
        new("DateValue", "Declared, but never read by <code>ChartControl</code>. <code>StockChartControl</code> requires it on every point, sorts by it and uses it to pick the visible range; <code>ChartDataPoint.Financial</code> fills it.", "DateTime?", "null"),
        new("ChartDataPoint(string label, double value, string? color = null)", "Sets <code>Label</code>, <code>Value</code> and <code>Color</code>; the parameterless constructor leaves the defaults.", "ChartDataPoint"),
        new("ChartDataPoint.Financial(string label, double open, double high, double low, double close, DateTime? date = null)", "Open, high, low and close into <code>Value</code> to <code>Value4</code> and the date into <code>DateValue</code>. The same fields are readable as <code>Open</code>, <code>High</code>, <code>Low</code> and <code>Close</code>, each falling back to <code>Value</code> when null.", "ChartDataPoint"),
        new("ChartDataPoint.Bullet(string label, double actual, double target, string? category = null)", "Actual into <code>Value</code> (alias <code>Actual</code>) and target into <code>Value2</code> (alias <code>Target</code>); the category goes into <code>Category</code>, which the Bullet renderer never reads.", "ChartDataPoint"),
        new("ChartDataPoint.Range(string label, double min, double max, string? color = null)", "Min into <code>Value</code>, max into <code>Value2</code>, the colour into <code>Color</code>. The Range renderers draw a fixed blue, so the colour is stored but not drawn.", "ChartDataPoint"),
        new("ChartDataPoint.Sankey(string source, string target, double flow, string? color = null)", "Source into <code>Label</code> (alias <code>Source</code>), target into <code>Category</code> (alias <code>Destination</code>, empty when null), flow into <code>Value</code>; the colour is stored but the Sankey renderer draws fixed blues. Each point draws its own source and target node; equal names are not merged.", "ChartDataPoint"),
    };

    /// <summary>ChartAxis: the XAxis and YAxis parameters. Only the Y axis affects scaling.</summary>
    public static readonly ApiRow[] Axis =
    {
        new("Title", "Axis caption. Drawn by the Bar renderer (Bar, HorizontalBar, GroupedBar, StackedBar, StackedBar100, Pareto, ComboBarLine and any type without a renderer of its own, such as Treemap) and the Line family (Line, MultiLine, Spline, StepLine, Area, StackedArea): <code>YAxis.Title</code> rotated along the left edge, <code>XAxis.Title</code> under the plot. No other renderer draws it.", "string?", "null"),
        new("Min", "Lower end of the value axis, read from <code>YAxis</code> only, by the Bar and Line families, Waterfall, Box Plot and Histogram. Null means 0, or 1.1 times the data minimum when that is negative. Scatter, Bubble, Dot Plot, Gantt, the financial and Range charts scale from the data and ignore it; <code>XAxis.Min</code> is never read.", "double?", "null"),
        new("Max", "Upper end of the value axis, same readers as <code>Min</code>. Null means 1.1 times the data maximum, so a <code>StackedBar100</code> tops out at 100 of 110 unless <code>Max</code> is set. <code>XAxis.Max</code> is never read.", "double?", "null"),
        new("Format", "Declared, but never read: tick labels are whole numbers everywhere except the financial charts, which print one decimal.", "string?", "null"),
        new("ShowGridLines", "Declared, but never read: the dashed grid lines are always drawn.", "bool", "true"),
        new("TickCount", "Number of tick labels on the value axis, first to last inclusive, read from <code>YAxis</code> by the Bar and Line families only; null means 5. Scatter, Bubble, Waterfall, Box Plot, Dot Plot and Histogram always draw 5; Gantt, the financial and Range charts always draw 4 intervals.", "int?", "null"),
    };

    /// <summary>ChartLegend: the Legend parameter.</summary>
    public static readonly ApiRow[] Legend =
    {
        new("Visible", "Shows the series legend under the chart. It only appears when there is more than one series, so a single-series chart shows nothing either way; Pie and Donut draw their own slice legend regardless. <code>ChartSvg.Markup</code> and the report analysis graphic set it from their own flags.", "bool", "true"),
        new("Position", "Declared, but never read: the legend is always a row of swatches under the chart. <code>LegendPosition</code> has <code>Top</code>, <code>Bottom</code>, <code>Left</code> and <code>Right</code>.", "LegendPosition", "LegendPosition.Bottom"),
    };

    /// <summary>ChartControl.ChartBar: one row of the legacy HTML bar list (the Bars parameter).</summary>
    public static readonly ApiRow[] Bar =
    {
        new("Label", "Text in the row label cell on the left, shown while <code>ShowLabels</code> is true, and in the bar tooltip.", "string", "\"\""),
        new("Value", "Bar length as a share of the largest <code>Value</code> in the list (a list with no positive value draws empty bars); printed after the bar in <code>ValueFormat</code> when <code>ShowValues</code> is true.", "double", "0"),
        new("Color", "Background of this bar; null falls back to the <code>BarColor</code> parameter (<code>#2563eb</code>).", "string?", "null"),
        new("ChartBar(string label, double value, string? color = null)", "Sets the three properties. The class is nested in the control, so write <code>ChartControl.ChartBar</code>; a list of them goes to the <code>Bars</code> parameter, which draws the legacy HTML bar list whenever <code>Series</code> is null.", "ChartBar"),
    };
}
