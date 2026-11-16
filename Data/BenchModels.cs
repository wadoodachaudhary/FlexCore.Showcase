namespace FlexCore.Showcase.Data;

/// <summary>
/// Rate card line: one supplier's bookable service at one destination, with net rate and margin. Used by Batch editing,
/// Export &amp; templates and On-demand loading.
/// </summary>
public sealed class RateCardItem
{
    public string Code { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int MarginPercent { get; set; }
    public string Season { get; set; } = string.Empty;
    public bool Bookable { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Column specification for dynamic swapping bench.
/// </summary>
public sealed class ColumnSpec
{
    public string Field { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public string Width { get; set; } = "120px";
    public bool Visible { get; set; } = true;
    public bool Editable { get; set; }
}

/// <summary>
/// Layout configuration for column swapping bench.
/// </summary>
public sealed class ColumnLayoutConfig
{
    public string Name { get; set; } = string.Empty;
    public List<ColumnSpec> Columns { get; set; } = [];
}
