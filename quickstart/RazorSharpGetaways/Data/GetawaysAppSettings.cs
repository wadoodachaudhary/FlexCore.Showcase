using System.Text;
using Fx.ControlKit.Charts;
using Fx.ControlKit.Grid;
using Fx.ControlKit.Scheduling;

namespace RazorSharpGetaways.Data;

// Settings page state. Scoped service (Program.cs): one instance per visit; a reload restores the defaults. The FlexCore site's
// Settings C# tab prints the lines between the markers. Each scheme's chart colors were picked with a color-blindness check in
// stack order; Razor Sharp keeps the logo's colors.
// code-tab:settings
/// <summary>One color scheme: getaways.css holds its colors under the class scheme-{Key}.</summary>
public sealed record GetawaysScheme(string Key, string Name, string Accent, string Blurb, string[] ChartColors);

/// <summary>Where a dashboard card sits. Beside is true for the figures and the chart when they share a row.</summary>
public sealed record GetawaysCardSlot(string Key, string Title, int Row, int Column, int Span, bool Beside);

public sealed class GetawaysAppSettings
{
    public const string Figures = "figures", Chart = "chart", Departures = "departures";

    public static readonly GetawaysScheme[] Schemes =
    [
        new("razor", "Razor Sharp", "#4f46e5", "Indigo accents and a soft lavender menu.", ["#10b981", "#0284c7", "#38bdf8", "#6366f1"]),
        new("harbour", "Harbor", "#0369a1", "Deep blue accents and a navy menu.", ["#0369a1", "#059669", "#38bdf8", "#d97706"]),
        new("evergreen", "Evergreen", "#047857", "Green accents and a pale mint menu.", ["#047857", "#65a30d", "#0284c7", "#d97706"]),
        new("sunset", "Sunset", "#be123c", "Rose accents and a warm sand menu.", ["#be123c", "#d97706", "#7c3aed", "#0284c7"]),
    ];

    // Look
    public GetawaysScheme Scheme { get; set; } = Schemes[0];
    public int TextSize { get; set; } = 100;

    // Views
    public bool SidebarIcons { get; set; }
    public string RequestsView { get; set; } = "All";
    public SchedulerView ItineraryView { get; set; } = SchedulerView.MultiDay;
    public ChartType DashboardChart { get; set; } = ChartType.StackedBar;

    // Grids: a null theme follows the scheme via the gw-grid-theme class; any other is a built-in grid theme
    public GridTheme? GridTheme { get; set; }
    public GridDensity GridDensity { get; set; } = GridDensity.Default;
    public GridLines GridLines { get; set; } = GridLines.Both;
    public bool AlternateRows { get; set; } = true;

    // Dashboard layout
    public bool SideBySide { get; set; } = true;
    public List<string> CardOrder { get; private set; } = [Figures, Chart, Departures];
    public HashSet<string> HiddenCards { get; } = [];
    public bool Customizing { get; set; }

    /// <summary>Raised after a Settings change; the app window and its open page repaint.</summary>
    public event Action? Changed;
    public void NotifyChanged() => Changed?.Invoke();

    /// <summary>App window class that carries the scheme's colors.</summary>
    public string SchemeClass => $"scheme-{Scheme.Key}";

    /// <summary>App window inline style: --fx-zoom for FlexCore controls, plus a base font size for inherited text.</summary>
    public string WindowStyle => TextSize == 100
        ? ""
        : FormattableString.Invariant($"--fx-zoom: {TextSize / 100.0:0.##}; font-size: calc(1em * var(--fx-zoom, 1))");

    public string GridCss => GridTheme is null ? "gw-grid-theme gw-grid" : "gw-grid";
    public GridTheme GridThemeValue => GridTheme ?? Fx.ControlKit.Grid.GridTheme.Default;

    /// <summary>The layout preset the arrangement matches, or "" once cards are moved by hand.</summary>
    public string Preset => (SideBySide, CardOrder[0], CardOrder[1]) switch
    {
        (true, Figures, Chart) => "side",
        (false, Figures, Chart) => "stacked",
        (false, Chart, Figures) => "chart",
        (false, Departures, Figures) => "departures",
        _ => ""
    };

    public void ApplyPreset(string key)
    {
        SideBySide = key == "side";
        CardOrder = key switch
        {
            "chart" => [Chart, Figures, Departures],
            "departures" => [Departures, Figures, Chart],
            _ => [Figures, Chart, Departures]
        };
    }

    public static string CardTitle(string key) => key switch
    {
        Figures => "Figures",
        Chart => "Trip requests by month",
        _ => "Upcoming departures"
    };

    private List<string> VisibleCards => CardOrder.Where(k => !HiddenCards.Contains(k)).ToList();

    private bool FiguresBesideChart => VisibleCards is var v && SideBySide
        && Enumerable.Range(0, Math.Max(0, v.Count - 1)).Any(i => IsPair(v[i], v[i + 1]));

    private static bool IsPair(string a, string b) => (a, b) is (Figures, Chart) or (Chart, Figures);

    /// <summary>GridLayoutControl.Columns for the current arrangement. The chart stops at 640 px: it draws on a fixed 2:1 canvas.</summary>
    public string DashboardColumns
    {
        get
        {
            if (!FiguresBesideChart) return "minmax(0, 1fr)";
            return VisibleCards.IndexOf(Figures) < VisibleCards.IndexOf(Chart)
                ? "minmax(220px, 1fr) minmax(0, 640px)"
                : "minmax(0, 640px) minmax(220px, 1fr)";
        }
    }

    /// <summary>The visible cards in order, each with its GridLayoutItemControl position.</summary>
    public IReadOnlyList<GetawaysCardSlot> Slots
    {
        get
        {
            var visible = VisibleCards;
            var span = FiguresBesideChart ? 2 : 1;
            var slots = new List<GetawaysCardSlot>();
            for (int i = 0, row = 1; i < visible.Count; row++)
            {
                if (SideBySide && i + 1 < visible.Count && IsPair(visible[i], visible[i + 1]))
                {
                    slots.Add(new(visible[i], CardTitle(visible[i]), row, 1, 1, true));
                    slots.Add(new(visible[i + 1], CardTitle(visible[i + 1]), row, 2, 1, true));
                    i += 2;
                }
                else
                {
                    slots.Add(new(visible[i], CardTitle(visible[i]), row, 1, span, false));
                    i++;
                }
            }
            return slots;
        }
    }

    public bool CanMove(string key, int step)
    {
        var at = VisibleCards.IndexOf(key) + step;
        return at >= 0 && at < VisibleCards.Count;
    }

    /// <summary>Swaps a card with its visible neighbor (Move up / Move down).</summary>
    public void MoveCard(string key, int step)
    {
        if (!CanMove(key, step)) return;
        var other = VisibleCards[VisibleCards.IndexOf(key) + step];
        int a = CardOrder.IndexOf(key), b = CardOrder.IndexOf(other);
        (CardOrder[a], CardOrder[b]) = (CardOrder[b], CardOrder[a]);
    }

    /// <summary>A dragged card takes the place of the card it was dropped on.</summary>
    public void PlaceCard(string key, string target)
    {
        if (key == target) return;
        var to = CardOrder.IndexOf(target);
        CardOrder.Remove(key);
        CardOrder.Insert(to, key);
    }

    public void SetCardShown(string key, bool shown)
    {
        if (shown) HiddenCards.Remove(key);
        else HiddenCards.Add(key);
    }

    public void ResetLayout()
    {
        ApplyPreset("side");
        HiddenCards.Clear();
    }

    public void Reset()
    {
        Scheme = Schemes[0];
        TextSize = 100;
        SidebarIcons = false;
        RequestsView = "All";
        ItineraryView = SchedulerView.MultiDay;
        DashboardChart = ChartType.StackedBar;
        GridTheme = null;
        GridDensity = GridDensity.Default;
        GridLines = GridLines.Both;
        AlternateRows = true;
        Customizing = false;
        ResetLayout();
    }

    /// <summary>One series per status for each month with requests, in the scheme's chart colors.</summary>
    public List<ChartSeries> MonthlySeries(IEnumerable<TripRequest> requests, int months, bool withDeclined)
    {
        var all = requests.ToList();
        var today = DateTime.Today;
        var shown = Enumerable.Range(0, months).Select(i => today.AddMonths(-i)).Reverse()
            .Where(m => all.Any(r => SameMonth(r.DateRequested, m))).ToList();
        string[] statuses = withDeclined ? ["Booked", "Quoted", "New", "Declined"] : ["Booked", "Quoted", "New"];
        return statuses.Select((status, i) => new ChartSeries(status, DashboardChart,
            shown.Select(m => new ChartDataPoint(m.ToString("MMM"), all.Count(r => r.Status == status && SameMonth(r.DateRequested, m)))),
            Scheme.ChartColors[i])).ToList();
    }

    private static bool SameMonth(DateTime a, DateTime b) => a.Year == b.Year && a.Month == b.Month;

    /// <summary>Settings page live panel: the control parameters for the current choices.</summary>
    public string ToRazor()
    {
        var code = new StringBuilder();
        code.AppendLine("@* Trip Requests and Upcoming departures *@");
        code.AppendLine(GridTheme is null
            ? "<GridControl CssClass=\"gw-grid-theme\""
            : $"<GridControl Theme=\"GridTheme.{GridTheme}\"");
        code.AppendLine($"             Density=\"GridDensity.{GridDensity}\" GridLines=\"GridLines.{GridLines}\" EnableAltRow=\"{(AlternateRows ? "true" : "false")}\" … />");
        code.AppendLine();
        code.AppendLine("@* Dashboard *@");
        code.AppendLine($"<GridLayoutControl Columns=\"{DashboardColumns}\" Gap=\"14px\">");
        foreach (var slot in Slots)
        {
            var span = slot.Span > 1 ? $" ColumnSpan=\"{slot.Span}\"" : "";
            code.AppendLine($"    <GridLayoutItemControl Row=\"{slot.Row}\" Column=\"{slot.Column}\"{span}>{slot.Title}</GridLayoutItemControl>");
        }
        code.AppendLine("</GridLayoutControl>");
        code.AppendLine($"<ChartControl Type=\"ChartType.{DashboardChart}\" Series=\"@series\" … />   @* series colors: {string.Join(", ", Scheme.ChartColors)} *@");
        code.AppendLine();
        code.AppendLine("@* Itinerary *@");
        code.AppendLine($"<SchedulerControl View=\"SchedulerView.{ItineraryView}\" … />");
        code.AppendLine();
        code.AppendLine("/* The app window: FlexCore's buttons, segments and text read these variables */");
        code.AppendLine(".gw-app {");
        code.AppendLine($"    --fx-accent: {Scheme.Accent};");
        code.AppendLine(FormattableString.Invariant($"    --fx-zoom: {TextSize / 100.0:0.##};"));
        code.Append('}');
        return code.ToString();
    }
}
// /code-tab
