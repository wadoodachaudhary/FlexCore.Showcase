using System.Diagnostics;
using System.Text.Json;
using FlexCore.Showcase.Data;

namespace FlexCore.Showcase.Services;

/// <summary>
/// Central high-performance in-memory data store for showcase benches.
/// Loads authentic JSON exports if present under Data/, and dynamically synthesizes
/// rich, realistic enterprise datasets when running standalone or in lightweight distributions.
/// </summary>
public sealed class BenchDataStore
{
    public const string EstimatingFileName = "estimating-items-full.json";
    public const string AssemblyFileName = "assembly-details-full.json";
    public const string VendorsFileName = "vendors-200.json";
    public const string VendorContactsFileName = "vendor-contacts.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly string[] _probeRoots;
    private readonly object _gate = new();
    private bool _loaded;

    public BenchDataStore(IWebHostEnvironment env)
    {
        _probeRoots =
        [
            Path.Combine(AppContext.BaseDirectory, "Data"),
            Path.Combine(env.ContentRootPath, "Data"),
            Path.Combine(env.ContentRootPath, "..", "HomeFront", "FlexKitTester", "Data")
        ];
    }

    public IReadOnlyList<EstimatingItemRecord> EstimatingItems { get; private set; } = [];
    public IReadOnlyList<AssemblyDetailRecord> AssemblyDetails { get; private set; } = [];
    public IReadOnlyList<VendorRecord> Vendors { get; private set; } = [];
    public IReadOnlyList<VendorContactRecord> VendorContacts { get; private set; } = [];
    public IReadOnlyList<SecurityGroup> SecurityGroups { get; private set; } = [];
    public IReadOnlyList<UserListItem> Users { get; private set; } = [];

    public string LoadReport { get; private set; } = "Initializing...";
    public string? DataRoot { get; private set; }

    public void Load()
    {
        if (_loaded) return;
        lock (_gate)
        {
            if (_loaded) return;
            var sw = Stopwatch.StartNew();

            DataRoot = _probeRoots.FirstOrDefault(Directory.Exists);

            var items = ReadArray<EstimatingItemRecord>(EstimatingFileName);
            if (items.Count == 0)
            {
                items = SynthesizeEstimatingItems(5000);
            }
            EstimatingItems = items;

            var assemblies = ReadArray<AssemblyDetailRecord>(AssemblyFileName);
            if (assemblies.Count == 0)
            {
                assemblies = SynthesizeAssemblies(items);
            }
            AssemblyDetails = assemblies;

            var vendors = ReadArray<VendorRecord>(VendorsFileName);
            if (vendors.Count == 0)
            {
                vendors = SynthesizeVendors(200);
            }
            Vendors = vendors;

            var contacts = ReadArray<VendorContactRecord>(VendorContactsFileName);
            if (contacts.Count == 0)
            {
                contacts = SynthesizeContacts(vendors);
            }
            VendorContacts = contacts;

            SecurityGroups = SynthesizeSecurityGroups();
            Users = SynthesizeUsers();

            sw.Stop();
            LoadReport = $"Data loaded in {sw.ElapsedMilliseconds} ms | Estimating: {EstimatingItems.Count:N0} items | Assemblies: {AssemblyDetails.Count:N0} | Vendors: {Vendors.Count:N0}";
            _loaded = true;
        }
    }

    private List<T> ReadArray<T>(string fileName)
    {
        var path = _probeRoots.Select(root => Path.Combine(root, fileName)).FirstOrDefault(File.Exists);
        if (path is null) return [];

        try
        {
            var bytes = File.ReadAllBytes(path);
            var length = StripChunkBreaks(bytes);
            return JsonSerializer.Deserialize<List<T>>(new ReadOnlySpan<byte>(bytes, 0, length), JsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    private static int StripChunkBreaks(byte[] buffer)
    {
        var read = buffer.Length >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF ? 3 : 0;
        var write = 0;
        for (; read < buffer.Length; read++)
        {
            var b = buffer[read];
            if (b is (byte)'\n' or (byte)'\r') continue;
            buffer[write++] = b;
        }
        return write;
    }

    private static List<EstimatingItemRecord> SynthesizeEstimatingItems(int count)
    {
        var list = new List<EstimatingItemRecord>(count);
        var phases = new[]
        {
            ("0100", "Sitework & Excavation"),
            ("0200", "Foundation & Concrete"),
            ("0300", "Framing & Timber"),
            ("0400", "Roofing & Siding"),
            ("0500", "Plumbing Rough-In"),
            ("0600", "Electrical & Lighting"),
            ("0700", "HVAC & Insulation"),
            ("0800", "Drywall & Texturing"),
            ("0900", "Interior Trim & Doors"),
            ("1000", "Flooring & Tile"),
            ("1100", "Paint & Finish"),
            ("1200", "Cabinets & Countertops")
        };

        var uoms = new[] { "EA", "LF", "SF", "CY", "HRS", "BOX" };
        var random = new Random(42);

        for (var i = 1; i <= count; i++)
        {
            var p = phases[i % phases.Length];
            var uom = uoms[i % uoms.Length];
            var price = Math.Round((decimal)(random.NextDouble() * 850 + 15), 2);
            list.Add(new EstimatingItemRecord
            {
                DivisionID = 1,
                Phase = p.Item1,
                PhaseDesc = p.Item2,
                Item = $"ITM-{i:D5}",
                ItemDesc = $"{p.Item2} Specification {i}",
                CostCategory = (i % 3 == 0) ? "Subcontract" : (i % 3 == 1) ? "Material" : "Labor",
                OrderUOM = uom,
                Price = price,
                WastePercent = (i % 5 == 0) ? 10 : 5,
                POIndex = $"PO-{(i % 40) + 1:D3}",
                POIndexDescription = $"Purchase Group {(i % 40) + 1}",
                JCCostCode = $"{p.Item1}-{(i % 8) + 1:D2}",
                JCCostCodeDesc = $"{p.Item2} Task {(i % 8) + 1}",
                IsQuote = (i % 4 == 0),
                PartNumber = $"SKU-{(100000 + i)}",
                Notes = $"Standard specification item for {p.Item2}"
            });
        }
        return list;
    }

    private static List<AssemblyDetailRecord> SynthesizeAssemblies(List<EstimatingItemRecord> items)
    {
        var list = new List<AssemblyDetailRecord>();
        var models = new[] { "Birchwood", "Cedarbrook", "Oakmont", "Pinecrest", "Sycamore" };

        for (var i = 0; i < Math.Min(items.Count, 300); i++)
        {
            var item = items[i];
            list.Add(new AssemblyDetailRecord
            {
                DivisionID = 1,
                Community = "Meadowlands",
                Assembly = $"ASM-{(i % 25) + 1:D3}",
                Model = models[i % models.Length],
                Phase = item.Phase,
                Item = item.Item,
                Sequence = (i % 10) + 1,
                TakeoffQty = Math.Round((decimal)(new Random(i).NextDouble() * 50 + 1), 2),
                OrderQty = Math.Round((decimal)(new Random(i).NextDouble() * 55 + 1), 2)
            });
        }
        return list;
    }

    private static List<VendorRecord> SynthesizeVendors(int count)
    {
        var list = new List<VendorRecord>(count);
        var trades = new[] { "Framing", "Concrete", "Electrical", "Plumbing", "Drywall", "Roofing", "Painting", "HVAC" };
        var cities = new[] { "Austin", "Dallas", "Houston", "San Antonio", "Fort Worth", "Plano" };

        for (var i = 1; i <= count; i++)
        {
            var trade = trades[i % trades.Length];
            var city = cities[i % cities.Length];
            list.Add(new VendorRecord
            {
                DivisionID = 1,
                Vendor_ID = $"VND-{i:D3}",
                Vendor_Name = $"{city} {trade} Contractors #{i}",
                Addr1 = $"{100 + i * 12} Industrial Blvd",
                City = city,
                State = "TX",
                Zip = $"7500{i % 10}",
                Phone = $"(512) 555-{(1000 + i):D4}",
                Fax = $"(512) 555-{(2000 + i):D4}",
                TradeType = trade,
                HoldBackPercentage = 10.0,
                GLInsRequired = true,
                WCInsRequired = true,
                BuildProEnabled = (i % 2 == 0),
                InActive = false
            });
        }
        return list;
    }

    private static List<VendorContactRecord> SynthesizeContacts(List<VendorRecord> vendors)
    {
        var list = new List<VendorContactRecord>();
        var firstNames = new[] { "Michael", "Sarah", "David", "Jessica", "James", "Emily", "Robert", "Jennifer" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis" };

        for (var i = 0; i < vendors.Count; i++)
        {
            var v = vendors[i];
            var fn = firstNames[i % firstNames.Length];
            var ln = lastNames[i % lastNames.Length];
            list.Add(new VendorContactRecord
            {
                DivisionID = 1,
                ContactID = i + 1,
                VendorCode = v.Vendor_ID,
                FirstName = fn,
                DisplayName = $"{fn} {ln}",
                Role = (i % 2 == 0) ? "Project Manager" : "Accounts Payable",
                WorkPhone = v.Phone,
                Email = $"{fn.ToLower()}.{ln.ToLower()}@vendorcorp.example",
                Active = true
            });
        }
        return list;
    }

    private static List<SecurityGroup> SynthesizeSecurityGroups()
    {
        return
        [
            new SecurityGroup { GroupID = 1, Description = "Executive Management", IsAdmin = true, MemberCount = 6 },
            new SecurityGroup { GroupID = 2, Description = "Project Managers", IsAdmin = false, MemberCount = 24 },
            new SecurityGroup { GroupID = 3, Description = "Estimating Specialists", IsAdmin = false, MemberCount = 12 },
            new SecurityGroup { GroupID = 4, Description = "Purchasing & Subcontracts", IsAdmin = false, MemberCount = 18 },
            new SecurityGroup { GroupID = 5, Description = "Accounting & Payroll", IsAdmin = false, MemberCount = 15 },
            new SecurityGroup { GroupID = 6, Description = "Field Superintendents", IsAdmin = false, MemberCount = 42 },
            new SecurityGroup { GroupID = 7, Description = "Customer Care & Warranty", IsAdmin = false, MemberCount = 9 }
        ];
    }

    private static List<UserListItem> SynthesizeUsers()
    {
        return
        [
            new UserListItem { UserID = "asmith", UserName = "Alex Smith", Email = "asmith@enterprise.local", Active = true },
            new UserListItem { UserID = "bjenkins", UserName = "Bradley Jenkins", Email = "bjenkins@enterprise.local", Active = true },
            new UserListItem { UserID = "cclark", UserName = "Catherine Clark", Email = "cclark@enterprise.local", Active = true },
            new UserListItem { UserID = "ddavis", UserName = "David Davis", Email = "ddavis@enterprise.local", Active = true },
            new UserListItem { UserID = "eevans", UserName = "Elena Evans", Email = "eevans@enterprise.local", Active = true },
            new UserListItem { UserID = "ffoster", UserName = "Frank Foster", Email = "ffoster@enterprise.local", Active = true },
            new UserListItem { UserID = "ggarcia", UserName = "Grace Garcia", Email = "ggarcia@enterprise.local", Active = true },
            new UserListItem { UserID = "hharris", UserName = "Henry Harris", Email = "hharris@enterprise.local", Active = true }
        ];
    }

    // ══════════════════════════════════════════════════════════════════════
    // ── AG GRID SHOWCASE DATA GENERATORS ──────────────────────────────────
    // ══════════════════════════════════════════════════════════════════════

    public static List<AgPerformanceRow> GeneratePerformanceRows(int count)
    {
        var list = new List<AgPerformanceRow>(count);
        var projects = new[] { "Skyline Tower Phase II", "Metropolitan Transit Hub", "Apex Data Center", "Harborview Condominiums", "Summit Medical Facility", "Riverfront Esplanade" };
        var tasks = new[] { "Structural Steel Erection", "HVAC Ductwork & Chiller Install", "Foundation Pour & Post-Tension", "Underground Utility Tie-Ins", "Curtain Wall Glazing", "Drywall & Fire-Rating", "Final Inspection & Commissioning", "Electrical Substation Wiring" };
        var assignees = new[] { "Marcus Vance", "Elena Rostova", "Devon Chen", "Sarah Jenkins", "Liam O'Connor", "Priya Patel", "Gabriel Santos", "Chloe Dubois" };
        var priorities = new[] { "Critical", "High", "Medium", "Low" };
        var statuses = new[] { "In Progress", "Completed", "On Hold", "Review Needed", "Approved" };
        var riskLevels = new[] { "Low", "Moderate", "High", "Critical" };
        var baseDate = new DateTime(2026, 1, 15);
        var rand = new Random(42);

        for (int i = 1; i <= count; i++)
        {
            var budget = 10000m + (decimal)(rand.Next(50, 850) * 100);
            var varianceRatio = (decimal)(rand.NextDouble() * 0.4 - 0.2); // -20% to +20%
            var actual = Math.Round(budget * (1m + varianceRatio), 2);
            var duration = rand.Next(14, 180);
            var start = baseDate.AddDays(rand.Next(0, 90));

            list.Add(new AgPerformanceRow
            {
                Id = i,
                Project = projects[rand.Next(projects.Length)],
                Task = $"{tasks[rand.Next(tasks.Length)]} #{i:D4}",
                Assignee = assignees[rand.Next(assignees.Length)],
                Priority = priorities[rand.Next(priorities.Length)],
                Status = statuses[rand.Next(statuses.Length)],
                Progress = rand.Next(5, 101),
                StartDate = start,
                DueDate = start.AddDays(duration),
                Budget = budget,
                ActualCost = actual,
                RiskLevel = riskLevels[rand.Next(riskLevels.Length)]
            });
        }
        return list;
    }

    public static List<AgFinanceTickerRow> GetFinanceTickers()
    {
        var rand = new Random(101);
        var assets = new (string Ticker, string Name, string Instrument, decimal Price, int Qty)[]
        {
            ("AAPL", "Apple Inc.", "Stock", 232.50m, 150),
            ("MSFT", "Microsoft Corp.", "Stock", 448.20m, 120),
            ("NVDA", "NVIDIA Corp.", "Stock", 128.40m, 400),
            ("GOOGL", "Alphabet Inc.", "Stock", 176.90m, 200),
            ("AMZN", "Amazon.com Inc.", "Stock", 185.30m, 250),
            ("META", "Meta Platforms Inc.", "Stock", 512.10m, 100),
            ("TSLA", "Tesla Inc.", "Stock", 218.70m, 180),
            ("BTC-USD", "Bitcoin USD", "Crypto", 61450.00m, 4),
            ("ETH-USD", "Ethereum USD", "Crypto", 2740.00m, 25),
            ("SOL-USD", "Solana USD", "Crypto", 145.20m, 120),
            ("US10Y", "U.S. 10-Year Treasury Note", "Bond", 99.85m, 1000),
            ("CAD30Y", "Canada 30-Year Gov Bond", "Bond", 96.40m, 600),
            ("FRN2027", "France Gov Bond 2027", "Bond", 102.15m, 500),
            ("SPY", "SPDR S&P 500 ETF Trust", "ETF", 552.30m, 80),
            ("QQQ", "Invesco QQQ Trust", "ETF", 478.60m, 90),
            ("GLD", "SPDR Gold Shares ETF", "ETF", 231.80m, 110),
            ("MUB", "iShares National Muni Bond", "ETF", 108.40m, 300),
            ("JPM", "JPMorgan Chase & Co.", "Stock", 214.60m, 140),
            ("V", "Visa Inc.", "Stock", 278.30m, 110),
            ("WMT", "Walmart Inc.", "Stock", 74.50m, 350),
            ("DIS", "Walt Disney Co.", "Stock", 91.20m, 220),
            ("NFLX", "Netflix Inc.", "Stock", 685.40m, 60)
        };

        var list = new List<AgFinanceTickerRow>();
        foreach (var a in assets)
        {
            var timeline = new List<double>(24);
            double curr = (double)a.Price;
            for (int t = 0; t < 24; t++)
            {
                curr += (rand.NextDouble() - 0.49) * (curr * 0.025);
                timeline.Add(Math.Round(curr, 2));
            }

            var prev = Math.Round(a.Price * (1m + (decimal)(rand.NextDouble() * 0.04 - 0.02)), 2);
            var delta = a.Price - prev;
            var purchase = Math.Round(a.Price * (decimal)(0.85 + rand.NextDouble() * 0.25), 2);

            list.Add(new AgFinanceTickerRow
            {
                Ticker = a.Ticker,
                Name = a.Name,
                Instrument = a.Instrument,
                Quantity = a.Qty,
                PurchasePrice = purchase,
                Price = a.Price,
                PreviousPrice = prev,
                Timeline = timeline,
                TickDirection = delta > 0 ? 1 : delta < 0 ? -1 : 0
            });
        }
        return list;
    }

    public static List<AgHrEmployeeRow> GetHrHierarchy()
    {
        return
        [
            // CEO & Executive
            new AgHrEmployeeRow { EmployeeID = "EMP-001", ReportsToID = null, FullName = "Arthur Pendelton", JobTitle = "Chief Executive Officer", Department = "Executive", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2014, 3, 1), Salary = 385000m, Rating = 5, AvatarBg = "#0f172a", SubordinatesCount = 4 },
            new AgHrEmployeeRow { EmployeeID = "EMP-002", ReportsToID = "EMP-001", FullName = "Claire Sterling", JobTitle = "VP of Engineering", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2016, 6, 15), Salary = 265000m, Rating = 5, AvatarBg = "#0284c7", SubordinatesCount = 5 },
            new AgHrEmployeeRow { EmployeeID = "EMP-003", ReportsToID = "EMP-001", FullName = "Marcus Vance", JobTitle = "VP of Product", Department = "Product", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2017, 2, 10), Salary = 245000m, Rating = 4, AvatarBg = "#7c3aed", SubordinatesCount = 3 },
            new AgHrEmployeeRow { EmployeeID = "EMP-004", ReportsToID = "EMP-001", FullName = "Helena Zhang", JobTitle = "VP of Sales & Growth", Department = "Sales", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2018, 9, 1), Salary = 250000m, Rating = 5, AvatarBg = "#059669", SubordinatesCount = 3 },
            new AgHrEmployeeRow { EmployeeID = "EMP-005", ReportsToID = "EMP-001", FullName = "Devon Montgomery", JobTitle = "VP of People & Culture", Department = "Human Resources", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 1, 20), Salary = 210000m, Rating = 4, AvatarBg = "#d97706", SubordinatesCount = 2 },

            // Engineering Directors & Leads
            new AgHrEmployeeRow { EmployeeID = "EMP-010", ReportsToID = "EMP-002", FullName = "Julian Becker", JobTitle = "Director of Core Infrastructure", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2018, 4, 12), Salary = 205000m, Rating = 5, AvatarBg = "#2563eb", SubordinatesCount = 4 },
            new AgHrEmployeeRow { EmployeeID = "EMP-011", ReportsToID = "EMP-002", FullName = "Aisha Khan", JobTitle = "Director of Frontend Architecture", Department = "Engineering", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 7, 8), Salary = 198000m, Rating = 5, AvatarBg = "#4f46e5", SubordinatesCount = 3 },
            new AgHrEmployeeRow { EmployeeID = "EMP-012", ReportsToID = "EMP-002", FullName = "Carlos Mendez", JobTitle = "Lead Data Architect", Department = "Engineering", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 2, 15), Salary = 185000m, Rating = 4, AvatarBg = "#0891b2", SubordinatesCount = 2 },

            // Engineering Staff
            new AgHrEmployeeRow { EmployeeID = "EMP-020", ReportsToID = "EMP-010", FullName = "Dmitri Volkov", JobTitle = "Principal Distributed Systems Engineer", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 8, 1), Salary = 175000m, Rating = 5, AvatarBg = "#0369a1", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-021", ReportsToID = "EMP-010", FullName = "Seraphina Lin", JobTitle = "Senior Site Reliability Engineer", Department = "Engineering", Office = "Tokyo", EmploymentType = "Full-Time", HireDate = new DateTime(2021, 3, 22), Salary = 158000m, Rating = 4, AvatarBg = "#0284c7", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-022", ReportsToID = "EMP-011", FullName = "Liam Gallagher", JobTitle = "Senior Blazor Specialist", Department = "Engineering", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2021, 5, 14), Salary = 162000m, Rating = 5, AvatarBg = "#6366f1", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-023", ReportsToID = "EMP-011", FullName = "Maya Takahashi", JobTitle = "Design Systems Engineer", Department = "Engineering", Office = "Tokyo", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 1, 10), Salary = 145000m, Rating = 4, AvatarBg = "#8b5cf6", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-024", ReportsToID = "EMP-012", FullName = "Kofi Mensah", JobTitle = "Database Performance Specialist", Department = "Engineering", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 6, 1), Salary = 152000m, Rating = 4, AvatarBg = "#06b6d4", SubordinatesCount = 0 },

            // Product & Design
            new AgHrEmployeeRow { EmployeeID = "EMP-030", ReportsToID = "EMP-003", FullName = "Sophie Dubois", JobTitle = "Head of Enterprise UX", Department = "Product", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 11, 4), Salary = 175000m, Rating = 5, AvatarBg = "#9333ea", SubordinatesCount = 2 },
            new AgHrEmployeeRow { EmployeeID = "EMP-031", ReportsToID = "EMP-003", FullName = "Lucas Moretti", JobTitle = "Staff Product Manager", Department = "Product", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 5, 18), Salary = 168000m, Rating = 4, AvatarBg = "#a855f7", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-032", ReportsToID = "EMP-030", FullName = "Emma Watson", JobTitle = "Senior UI Designer", Department = "Product", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 9, 1), Salary = 132000m, Rating = 4, AvatarBg = "#c084fc", SubordinatesCount = 0 },

            // Sales & Growth
            new AgHrEmployeeRow { EmployeeID = "EMP-040", ReportsToID = "EMP-004", FullName = "Jack Reynolds", JobTitle = "Director of Strategic Accounts", Department = "Sales", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 8, 15), Salary = 190000m, Rating = 5, AvatarBg = "#15803d", SubordinatesCount = 2 },
            new AgHrEmployeeRow { EmployeeID = "EMP-041", ReportsToID = "EMP-004", FullName = "Ananya Sharma", JobTitle = "EMEA Regional Enterprise Lead", Department = "Sales", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 10, 1), Salary = 172000m, Rating = 4, AvatarBg = "#16a34a", SubordinatesCount = 0 },
            new AgHrEmployeeRow { EmployeeID = "EMP-042", ReportsToID = "EMP-040", FullName = "Tyler Brooks", JobTitle = "Senior Account Executive", Department = "Sales", Office = "Sydney", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 2, 1), Salary = 140000m, Rating = 4, AvatarBg = "#22c55e", SubordinatesCount = 0 },

            // HR & Talent
            new AgHrEmployeeRow { EmployeeID = "EMP-050", ReportsToID = "EMP-005", FullName = "Hannah Kim", JobTitle = "Head of Global Talent Acquisition", Department = "Human Resources", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 3, 9), Salary = 155000m, Rating = 5, AvatarBg = "#ea580c", SubordinatesCount = 1 },
            new AgHrEmployeeRow { EmployeeID = "EMP-051", ReportsToID = "EMP-050", FullName = "Noah Campbell", JobTitle = "Technical Recruiter", Department = "Human Resources", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2023, 4, 15), Salary = 108000m, Rating = 4, AvatarBg = "#f97316", SubordinatesCount = 0 }
        ];
    }

    public static List<AgInventoryProductRow> GetInventoryProducts()
    {
        var warehouses = new[]
        {
            ("DAL-01", "Dallas Central Hub", "Aisle 4, Shelf C"),
            ("CHI-02", "Chicago Logistics Center", "Bay 12, Bin 8"),
            ("ATL-03", "Atlanta East Terminal", "Sector 3, Rack A"),
            ("PHX-04", "Phoenix Inland Depot", "Aisle 9, Shelf F"),
            ("SEA-05", "Seattle Port Facility", "Dockside Bay 2")
        };

        var products = new (string SKU, string Name, string Cat, decimal Cost, int Reorder)[]
        {
            ("PRD-1001", "Titanium Alloy Fastener Assortment (1,000 pk)", "Hardware", 145.50m, 120),
            ("PRD-1002", "Ultra-Torque Brushless Cordless Drill 20V", "Industrial Tools", 229.00m, 80),
            ("PRD-1003", "Fiber Optic Backbone Transceiver Module 100G", "Electronics", 380.00m, 50),
            ("PRD-1004", "Structural Grade Reinforcing Rebar #5 (60ft)", "Raw Materials", 42.75m, 300),
            ("PRD-1005", "Industrial Smart Fall-Arrest Safety Harness", "Safety Gear", 185.00m, 60),
            ("PRD-1006", "Laser Distance Meter 200m with Bluetooth", "Industrial Tools", 165.00m, 75),
            ("PRD-1007", "Cat6A Shielded Plenum Cable (1,000ft Spool)", "Electronics", 215.00m, 100),
            ("PRD-1008", "Heavy-Duty Hydraulic Floor Jack 4-Ton", "Industrial Tools", 310.00m, 40),
            ("PRD-1009", "Respirator Mask N95 Valved Case (240 pk)", "Safety Gear", 112.50m, 150),
            ("PRD-1010", "Galvanized High-Tensile Steel Conduit 2in", "Raw Materials", 28.90m, 250),
            ("PRD-1011", "Digital Multi-Meter CAT IV 1000V True-RMS", "Electronics", 175.00m, 90),
            ("PRD-1012", "ANSI Class 3 High-Visibility Winter Parka", "Safety Gear", 124.00m, 70),
            ("PRD-1013", "Precision Pneumatic Framing Nailer 21-Deg", "Industrial Tools", 249.99m, 45),
            ("PRD-1014", "Pre-Cast Concrete Anchor Bolts 3/4x12in (50 pk)", "Hardware", 89.50m, 180),
            ("PRD-1015", "Circuit Breaker 200A 3-Phase Industrial Panel", "Electronics", 495.00m, 35)
        };

        var rand = new Random(33);
        var list = new List<AgInventoryProductRow>();
        var baseDelivery = DateTime.UtcNow.Date.AddDays(3);

        foreach (var p in products)
        {
            var details = new List<AgWarehouseStockDetail>();
            int totalOnHand = 0;
            int totalReserved = 0;

            int whCount = rand.Next(3, warehouses.Length + 1);
            for (int w = 0; w < whCount; w++)
            {
                var wh = warehouses[w];
                int onHand = rand.Next(20, 180);
                int reserved = rand.Next(2, (int)(onHand * 0.45));
                int incoming = rand.Next(0, 80);

                totalOnHand += onHand;
                totalReserved += reserved;

                details.Add(new AgWarehouseStockDetail
                {
                    WarehouseCode = wh.Item1,
                    WarehouseName = wh.Item2,
                    LocationBin = $"{wh.Item3} - Bin {rand.Next(10, 99)}",
                    QuantityOnHand = onHand,
                    ReservedQuantity = reserved,
                    IncomingShipmentQty = incoming,
                    NextDeliveryDate = baseDelivery.AddDays(rand.Next(1, 14)),
                    BatchLotNumber = $"LOT-2026-{(rand.Next(1000, 9999))}"
                });
            }

            list.Add(new AgInventoryProductRow
            {
                SKU = p.SKU,
                ProductName = p.Name,
                Category = p.Cat,
                TotalOnHand = totalOnHand,
                TotalReserved = totalReserved,
                ReorderPoint = p.Reorder,
                UnitCost = p.Cost,
                IsExpanded = false,
                WarehouseDetails = details
            });
        }
        return list;
    }
}
