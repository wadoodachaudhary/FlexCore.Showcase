using System.Data;

namespace FlexCore.Showcase.Data;

/// <summary>
/// In-memory construction / home-building dataset for the showcase demos.
/// Fictional vendors, phases, items, and house models — no real customer data.
/// Shared by the Grid demos and the Reports demo so they tell a single story.
/// </summary>
public static class DemoData
{
    public static readonly string[] Vendors =
    {
        "ABC Lumber Co.",
        "Cornerstone Concrete",
        "BlueSky Electrical",
        "Hyland Plumbing Ltd.",
        "Marvel Roofing",
        "Pinnacle Drywall",
        "AzureGlass Windows",
        "Spectrum Cabinets",
        "EarthPro Excavating",
        "Apex HVAC Services",
    };

    public static readonly string[] Phases =
    {
        "010 Permits",
        "030 Excavation",
        "050 Foundation",
        "070 Framing",
        "090 Roofing",
        "120 Plumbing Rough",
        "150 Electrical Rough",
        "180 HVAC",
        "210 Insulation",
        "240 Drywall",
        "270 Cabinets",
        "300 Flooring",
        "330 Paint",
        "360 Finishes",
    };

    public static readonly string[] UnitOfMeasure = { "Each", "LF", "SF", "SY", "CY", "LS", "HR" };

    public static readonly HouseModel[] HouseModels =
    {
        new("Aurora",    3, 2.0, 1980, 285_000m),
        new("Brookside", 4, 2.5, 2450, 342_000m),
        new("Cascade",   3, 2.5, 2120, 318_000m),
        new("Devonshire",5, 3.5, 3280, 489_000m),
        new("Evergreen", 4, 3.0, 2760, 401_000m),
        new("Magnolia",  3, 2.0, 1840, 268_000m),
        new("Sunridge",  4, 2.5, 2580, 372_000m),
        new("Whitfield", 5, 4.0, 3640, 542_000m),
    };

    /// <summary>Generates an FAssembly-style item list: each row is a line item
    /// in an assembly with vendor, PO index, description, qty, UOM, item type,
    /// unit price, and computed total.</summary>
    public static List<AssemblyItem> GenerateAssemblyItems(int count = 40)
    {
        var rnd = new Random(42); // deterministic for screenshots
        var items = new List<AssemblyItem>();
        string[] itemPool =
        {
            "Sales Incentives", "Land Costs", "Fill Dirt and Material",
            "Permits and Fees", "Rough Grading", "Utility Connections",
            "Temporary Electric", "Sewer Service", "Gas Service", "Electric Service",
            "Building Permits", "Telephone Service", "Construction Finance Costs",
            "Closing Fees and Costs", "Appraisals", "HBA Assessments",
            "New Home Warranty Fees", "Architectural and Engineering", "Blueprints",
            "Surveys", "Site Work", "Lot Clearing", "Excavation and Foundation",
            "Waterproofing", "Termite Protection", "Earth Hauling",
            "Footing and Foundations", "Gravel", "Sand", "Rebar and Reinforcing Steel",
            "Structural Slabs", "Stairs", "Garage or Carport Slab", "Concrete Labor",
            "Rough Sheet Metal", "Gutters and Downspouts", "Metal Edge and Flashing",
            "Soffit and Gable Flashing", "Rough Plumbing", "Beams - Steel",
            "Trusses", "Floor Joists", "Framing Labor", "Concrete", "Formwork",
        };
        for (int i = 0; i < count; i++)
        {
            var phaseIdx = (i / 3) % Phases.Length;
            var vendor = i < 6 ? "Default" : Vendors[i % Vendors.Length];
            var phase = Phases[phaseIdx];
            var item = itemPool[i % itemPool.Length];
            var itemType = (i % 7 == 0) ? "Quote" : "Unit Price";
            var qty = rnd.Next(1, 35);
            decimal price = (i % 11 == 0) ? 0m
                          : (i < 5) ? rnd.Next(100, 35_000)
                          : (decimal)Math.Round(rnd.NextDouble() * 5000 + 50, 0);
            items.Add(new AssemblyItem(
                PriceLevel: i % 6 == 0 ? "Item DB" : "Global (any Community)",
                Vendor: vendor,
                POIndex: $"{phaseIdx * 10 + 10:D3} {phase.Split(' ')[1]}",
                Description: item,
                Qty: qty,
                UOM: UnitOfMeasure[i % UnitOfMeasure.Length],
                ItemType: itemType,
                Price: price,
                Total: qty * price));
        }
        return items;
    }

    /// <summary>Vendor-grouped price list — FVendorPriceList-inspired.</summary>
    public static List<VendorPrice> GenerateVendorPriceList(int rowsPerVendor = 5)
    {
        var rnd = new Random(7);
        var rows = new List<VendorPrice>();
        var partsByVendor = new[]
        {
            new[] { "Floor Joist 2x10x16", "Wall Stud 2x4x8", "Plywood 4x8 5/8\"", "OSB 4x8 7/16\"", "Ridge Beam LVL", "Engineered I-Joist 16\"" },
            new[] { "Footing Mix 3000psi", "Slab Mix 3500psi", "Wall Mix 4000psi", "Pump Truck Hour", "Rebar #4 20'", "Vapor Barrier" },
            new[] { "Service 200A", "Service 400A", "Outlet Standard", "Outlet GFCI", "Switch Single", "Switch 3-way", "Ceiling Light" },
            new[] { "Rough-in Bath", "Rough-in Kitchen", "Water Heater 50gal", "Sewer Connection", "Toilet Standard", "Faucet Fixture" },
            new[] { "Shingles 30yr", "Underlayment Roll", "Drip Edge 10'", "Flashing Step", "Ridge Vent 4'", "Soffit Panel" },
            new[] { "Drywall 4x8 1/2\"", "Drywall 4x12 5/8\"", "Joint Compound 5gal", "Tape 250'", "Texture Spray Hr" },
            new[] { "Window 36x60", "Window 48x60", "Sliding Door 6'", "Bay Window 8'", "Skylight 24x48" },
            new[] { "Base Cabinet 36\"", "Wall Cabinet 30\"", "Pantry Tall 24\"", "Crown Molding LF", "Granite Counter SF" },
            new[] { "Excavate Basement", "Backfill", "Grade Lot Hr", "Topsoil Spread CY", "Compaction Hr" },
            new[] { "Furnace 80k BTU", "AC Unit 3-ton", "Duct Run 8\"", "Thermostat Smart", "Heat Pump 3-ton" },
        };
        for (int v = 0; v < Vendors.Length; v++)
        {
            var pool = partsByVendor[v];
            var uomPool = new[] { "Each", "LF", "SF", "HR", "EA", "CY" };
            for (int j = 0; j < rowsPerVendor; j++)
            {
                var part = pool[j % pool.Length];
                rows.Add(new VendorPrice(
                    Vendor: Vendors[v],
                    Item: part,
                    UOM: uomPool[j % uomPool.Length],
                    Price: (decimal)Math.Round(rnd.NextDouble() * 800 + 12, 2),
                    EffectiveDate: new DateTime(2025, 1, 1).AddDays(rnd.Next(0, 365)),
                    Notes: j % 4 == 0 ? "Bulk discount available" : ""));
            }
        }
        return rows.OrderBy(r => r.Vendor).ThenBy(r => r.Item).ToList();
    }

    /// <summary>Generic item catalog — FItems-inspired.</summary>
    public static List<CatalogItem> GenerateItemCatalog()
    {
        var rnd = new Random(13);
        var categories = new[] { "M (Material)", "L (Labor)", "S (Subcontract)", "E (Equipment)", "O (Overhead)" };
        var items = new List<CatalogItem>();
        int id = 1000;
        for (int p = 0; p < Phases.Length; p++)
        {
            var phase = Phases[p];
            int perPhase = rnd.Next(3, 7);
            for (int k = 0; k < perPhase; k++)
            {
                id++;
                items.Add(new CatalogItem(
                    ItemId: $"ITM-{id:D4}",
                    Description: $"{phase.Split(' ', 2)[1]} component {k + 1}",
                    Phase: phase,
                    Category: categories[(p + k) % categories.Length],
                    UOM: UnitOfMeasure[(p + k) % UnitOfMeasure.Length],
                    DefaultCost: (decimal)Math.Round(rnd.NextDouble() * 500 + 25, 2),
                    IsActive: rnd.Next(0, 10) > 0));
            }
        }
        return items;
    }

    /// <summary>Builds a DataTable summarising estimated cost per house model
    /// broken down by phase — fed straight into ReportWriterControl.ShowDataTableReport
    /// for the Reports demo.</summary>
    public static DataTable BuildModelCostReport()
    {
        var rnd = new Random(99);
        var dt = new DataTable("Model Cost Breakdown");
        dt.Columns.Add("Model",       typeof(string));
        dt.Columns.Add("Bedrooms",    typeof(int));
        dt.Columns.Add("Bathrooms",   typeof(double));
        dt.Columns.Add("SqFt",        typeof(int));
        dt.Columns.Add("Phase",       typeof(string));
        dt.Columns.Add("Vendor",      typeof(string));
        dt.Columns.Add("Cost",        typeof(decimal));
        foreach (var m in HouseModels)
        {
            foreach (var ph in Phases.Take(8))
            {
                var vendor = Vendors[rnd.Next(Vendors.Length)];
                decimal cost = (decimal)Math.Round(rnd.NextDouble() * 18_000 + 1_500, 0);
                dt.Rows.Add(m.Name, m.Bedrooms, m.Bathrooms, m.SqFt, ph, vendor, cost);
            }
        }
        return dt;
    }
}

public record AssemblyItem(string PriceLevel, string Vendor, string POIndex, string Description,
    int Qty, string UOM, string ItemType, decimal Price, decimal Total);

public record VendorPrice(string Vendor, string Item, string UOM, decimal Price,
    DateTime EffectiveDate, string Notes);

public record CatalogItem(string ItemId, string Description, string Phase, string Category,
    string UOM, decimal DefaultCost, bool IsActive);

public record HouseModel(string Name, int Bedrooms, double Bathrooms, int SqFt, decimal BasePrice);
