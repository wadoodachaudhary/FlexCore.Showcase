namespace FlexCore.Showcase.Data;

/// <summary>
/// High-density record for AG Grid Performance stress-test benchmark (up to 100k rows).
/// </summary>
public sealed class AgPerformanceRow
{
    public int Id { get; set; }
    public string Project { get; set; } = "";
    public string Task { get; set; } = "";
    public string Assignee { get; set; } = "";
    public string Priority { get; set; } = "";
    public string Status { get; set; } = "";
    public int Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Budget { get; set; }
    public decimal ActualCost { get; set; }
    public decimal Variance => Budget - ActualCost;
    public string RiskLevel { get; set; } = "Low";
}

/// <summary>
/// Financial ticker row with live price updates and sparkline price history.
/// </summary>
public sealed class AgFinanceTickerRow
{
    public string Ticker { get; set; } = "";
    public string Name { get; set; } = "";
    public string Instrument { get; set; } = ""; // Stock, Bond, ETF, Crypto
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Price { get; set; }
    public decimal PreviousPrice { get; set; }
    public decimal Delta => Price - PreviousPrice;
    public decimal DeltaPercent => PreviousPrice == 0 ? 0 : Math.Round((Delta / PreviousPrice) * 100, 2);
    public decimal TotalValue => Quantity * Price;
    public decimal UnrealizedPL => Quantity * (Price - PurchasePrice);
    public List<double> Timeline { get; set; } = new();
    public int TickDirection { get; set; } // +1 for up, -1 for down, 0 for neutral
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Organizational hierarchy employee row for AG Grid HR Tree Data demo.
/// </summary>
public sealed class AgHrEmployeeRow
{
    public string EmployeeID { get; set; } = "";
    public string? ReportsToID { get; set; }
    public string FullName { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public string Department { get; set; } = ""; // Engineering, Product, Sales, Marketing, Finance, HR
    public string Office { get; set; } = ""; // New York, London, San Francisco, Tokyo, Sydney
    public string EmploymentType { get; set; } = "Full-Time";
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public int Rating { get; set; } // 1-5 stars
    public string AvatarBg { get; set; } = "#0284c7";
    public int SubordinatesCount { get; set; }
    public bool IsManager => SubordinatesCount > 0;
}

/// <summary>
/// Master inventory product row with expandable warehouse breakdown details.
/// </summary>
public sealed class AgInventoryProductRow
{
    public string SKU { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public int TotalOnHand { get; set; }
    public int TotalReserved { get; set; }
    public int AvailableStock => TotalOnHand - TotalReserved;
    public int ReorderPoint { get; set; }
    public decimal UnitCost { get; set; }
    public decimal InventoryValuation => TotalOnHand * UnitCost;
    public string StockStatus => AvailableStock <= 0 ? "Out of Stock" : AvailableStock <= ReorderPoint ? "Low Stock" : "In Stock";
    public bool IsExpanded { get; set; }
    public List<AgWarehouseStockDetail> WarehouseDetails { get; set; } = new();
}

/// <summary>
/// Child record detailing stock levels across individual warehouses.
/// </summary>
public sealed class AgWarehouseStockDetail
{
    public string WarehouseCode { get; set; } = "";
    public string WarehouseName { get; set; } = "";
    public string LocationBin { get; set; } = "";
    public int QuantityOnHand { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity => QuantityOnHand - ReservedQuantity;
    public int IncomingShipmentQty { get; set; }
    public DateTime NextDeliveryDate { get; set; }
    public string BatchLotNumber { get; set; } = "";
}
