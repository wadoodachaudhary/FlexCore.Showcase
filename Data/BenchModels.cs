namespace FlexCore.Showcase.Data;

/// <summary>
/// One line of the tour operator's rate card: a bookable service from one supplier at one destination, with the
/// net rate the operator pays and the margin it adds. Shared by Batch editing, Export &amp; templates and On-demand loading.
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
/// A supplier on the operator's books: a hotel, transfer company, tour operator or service at one destination. Page navigation.
/// </summary>
public sealed class SupplierRecord
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    /// <summary>A signed rate agreement is on file.</summary>
    public bool Contracted { get; set; }
    /// <summary>The supplier's liability insurance certificate has been received.</summary>
    public bool InsuranceOnFile { get; set; }
    /// <summary>The supplier takes bookings through its own online system.</summary>
    public bool OnlineBooking { get; set; }
    public bool Active { get; set; } = true;
}

/// <summary>A person at a supplier; <see cref="SupplierCode"/> is the <see cref="SupplierRecord.Code"/>.</summary>
public sealed class SupplierContactRecord
{
    public int ContactID { get; set; }
    public string SupplierCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}

/// <summary>A sales desk at the operator; the master rows on Master-detail. <see cref="AgentCount"/> is the number of agents on it.</summary>
public sealed class SalesDesk
{
    public int DeskID { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>The desk takes group bookings as well as individual ones.</summary>
    public bool GroupBookings { get; set; }
    public int AgentCount { get; set; }
}

/// <summary>An agent on a sales desk; the detail rows on Master-detail. <see cref="DeskID"/> is the <see cref="SalesDesk.DeskID"/>.</summary>
public sealed class AgentListItem
{
    public string Login { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public int DeskID { get; set; }
}

/// <summary>
/// One row of the Tree grid page's supplier tree: a supplier (a root row with its category, service count and bookings total)
/// or one of the services it sells (a child row with its unit, net rate and bookings). The page generates the flat list; the
/// grid folds it by <see cref="ParentId"/>.
/// </summary>
public sealed class SupplierNode
{
    public string Id { get; set; } = string.Empty;
    public string? ParentId { get; set; }             // the supplier's Id on a service row; null on a supplier row
    public string Name { get; set; } = string.Empty;  // the supplier's name, or the service's description
    public string Category { get; set; } = string.Empty;
    public int Level { get; set; }                    // 1 supplier, 2 service
    public bool IsSupplier { get; set; }
    public int? ServiceCount { get; set; }            // supplier rows only
    public string Unit { get; set; } = string.Empty;  // service rows only: night, pax, trip, day, stay, hour, group or bag
    public decimal? Rate { get; set; }                // service rows only: the net rate per Unit
    public int Bookings { get; set; }                 // the service's bookings, or the supplier's total
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
