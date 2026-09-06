namespace FlexCore.Showcase.Data;

/// <summary>
/// Record representing an estimating line item from the enterprise database.
/// </summary>
public sealed class EstimatingItemRecord
{
    public int DivisionID { get; set; } = 1;
    public string? Phase { get; set; }
    public string? Item { get; set; }
    public string? ItemDesc { get; set; }
    public string? PhaseDesc { get; set; }
    public string? CostCategory { get; set; }
    public string? Notes { get; set; }
    public string? POIndex { get; set; }
    public string? POIndexDescription { get; set; }
    public string? JCCostCode { get; set; }
    public string? JCCostCodeDesc { get; set; }
    public string? JCCategory { get; set; }
    public string? JCCategoryDesc { get; set; }
    public string? TaxGroup { get; set; }
    public int? WastePercent { get; set; }
    public int? RoundDir { get; set; }
    public decimal? RoundTo { get; set; }
    public decimal? Price { get; set; }
    public bool? IsQuote { get; set; }
    public string? OrderUOM { get; set; }
    public decimal? ConversionFactor { get; set; }
    public string? PartNumber { get; set; }
    public string? Formula { get; set; }
}

/// <summary>
/// Assembly detail row record.
/// </summary>
public sealed class AssemblyDetailRecord
{
    public int DivisionID { get; set; } = 1;
    public string? Community { get; set; }
    public string? Assembly { get; set; }
    public string? Model { get; set; }
    public string? OptionID { get; set; }
    public string? Phase { get; set; }
    public string? Item { get; set; }
    public string? ItemChart { get; set; }
    public int Sequence { get; set; }
    public decimal? TakeoffQty { get; set; }
    public decimal? OrderQty { get; set; }
}

/// <summary>
/// Vendor profile record.
/// </summary>
public sealed class VendorRecord
{
    public int DivisionID { get; set; } = 1;
    public string? Vendor_ID { get; set; }
    public string? Vendor_Name { get; set; }
    public string? Addr1 { get; set; }
    public string? Addr2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? WebUID { get; set; }
    public string? Password { get; set; }
    public string? WalletPayeeID { get; set; }
    public string? TaxID { get; set; }
    public string? VendorGroupID { get; set; }
    public string? TradeType { get; set; }
    public string? POFormat { get; set; }
    public bool? IsTBD { get; set; }
    public bool? BuildProEnabled { get; set; }
    public bool? InActive { get; set; }
    public double? HoldBackPercentage { get; set; }
    public bool? GLInsRequired { get; set; }
    public bool? WCInsRequired { get; set; }
    public bool? UmbInsRequired { get; set; }
    public bool? AutoInsRequired { get; set; }
}

/// <summary>
/// Vendor contact record.
/// </summary>
public sealed class VendorContactRecord
{
    public int DivisionID { get; set; } = 1;
    public int? ContactID { get; set; }
    public string? Role { get; set; }
    public string? FirstName { get; set; }
    public string? WorkPhone { get; set; }
    public string? CellPhone { get; set; }
    public string? Email { get; set; }
    public string? VendorCode { get; set; }
    public string? DisplayName { get; set; }
    public bool? Active { get; set; } = true;
}

/// <summary>
/// Security group for permissions benches.
/// </summary>
public sealed class SecurityGroup
{
    public int GroupID { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int MemberCount { get; set; }
}

/// <summary>
/// User list item for permissions benches.
/// </summary>
public sealed class UserListItem
{
    public string UserID { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}

/// <summary>
/// Row for FDBGrid veto bench.
/// </summary>
public sealed class LedgerRow
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Qty { get; set; }
    public decimal Price { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// TreeGrid bench node model.
/// </summary>
public sealed class BenchTreeNode
{
    public string NodeID { get; set; } = string.Empty;
    public string? ParentID { get; set; }
    public string Node { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool IsGroup { get; set; }
    public int PhaseCount { get; set; }
    public decimal Budget { get; set; }
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
