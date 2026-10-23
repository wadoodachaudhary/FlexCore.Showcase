namespace FlexCore.Showcase.Data;

/// <summary>
/// One instrument of the Live updates ticker; Timeline is the sparkline history.
/// </summary>
public sealed class TickerRow
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
/// One person of the org chart on the Tree grid page; ReportsToID names the manager.
/// </summary>
public sealed class EmployeeRow
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
/// One product of the warehouse detail example on Master-detail; WarehouseDetails is the detail grid.
/// </summary>
public sealed class InventoryProduct
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
    public List<WarehouseStock> WarehouseDetails { get; set; } = new();
}

/// <summary>
/// Stock of one product at one warehouse.
/// </summary>
public sealed class WarehouseStock
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
