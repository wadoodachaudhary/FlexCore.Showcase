using System.Diagnostics;
using FlexCore.Showcase.Data;

namespace FlexCore.Showcase.Services;

/// <summary>
/// In-memory data for the grid pages, synthesized once at startup (Program.cs calls <see cref="Load"/>): the tour operator's
/// rate card (5,000 lines over <see cref="TravelData.Destinations"/>), the supplier list with its contacts (Page navigation) and
/// the sales desks with their agents (Master-detail). Deterministic, so screenshots and timings repeat.
/// </summary>
public sealed class BenchDataStore
{
    private readonly object _gate = new();
    private bool _loaded;

    public IReadOnlyList<RateCardItem> RateCard { get; private set; } = [];
    public IReadOnlyList<SupplierRecord> Suppliers { get; private set; } = [];
    public IReadOnlyList<SupplierContactRecord> SupplierContacts { get; private set; } = [];
    public IReadOnlyList<SalesDesk> SalesDesks { get; private set; } = [];
    public IReadOnlyList<AgentListItem> Agents { get; private set; } = [];

    public string LoadReport { get; private set; } = "Initializing...";

    public void Load()
    {
        if (_loaded) return;
        lock (_gate)
        {
            if (_loaded) return;
            var sw = Stopwatch.StartNew();

            RateCard = SynthesizeRateCard(5000);
            Suppliers = SynthesizeSuppliers(200);
            SupplierContacts = SynthesizeSupplierContacts(Suppliers);
            SalesDesks = SynthesizeSalesDesks();
            Agents = SynthesizeAgents(SalesDesks);

            sw.Stop();
            LoadReport = $"Data synthesized in {sw.ElapsedMilliseconds} ms | Rate card: {RateCard.Count:N0} lines | Suppliers: {Suppliers.Count:N0} | Agents: {Agents.Count:N0}";
            _loaded = true;
        }
    }

    /// <summary>
    /// The rate card: for every destination, every supplier's services in every season. Supplier names are built from the
    /// destination's city (Hotel Lisbon Central, Kyoto Walks); the rates move with the region and the season; the margin
    /// depends on the category. Every seventeenth line is not bookable.
    /// </summary>
    private static List<RateCardItem> SynthesizeRateCard(int count)
    {
        // category → suppliers (a {0} placeholder takes the city) → services (description, unit, low rate, high rate)
        var catalogue = new (string Category, int Margin, (string Supplier, (string Desc, string Unit, int Low, int High)[] Services)[] Suppliers)[]
        {
            ("Transport", 10, new[]
            {
                ("Meridian Airways", new[] { ("Return flights, economy", "pax", 380, 620), ("Return flights, premium economy", "pax", 900, 1500), ("Checked baggage, 23 kg", "pax", 40, 60) }),
                ("{0} Airport Transfers", new[] { ("Airport to hotel, private car", "trip", 45, 90), ("Hotel to airport, private car", "trip", 45, 90), ("Shared shuttle", "pax", 18, 30) }),
                ("{0} Rail & Coach", new[] { ("Day-trip coach with driver", "day", 120, 260), ("Regional rail pass, 3 days", "pax", 90, 180), ("Car hire, compact", "day", 35, 70) }),
            }),
            ("Accommodation", 15, new[]
            {
                ("Hotel {0} Central", new[] { ("Double room with breakfast", "night", 110, 260), ("Twin room, room only", "night", 90, 220), ("Suite with breakfast", "night", 240, 480) }),
                ("{0} Boutique Stays", new[] { ("Room upgrade, view side", "night", 40, 90), ("Late checkout on departure day", "stay", 30, 60), ("Cot and high chair", "stay", 0, 15) }),
                ("{0} Apartments", new[] { ("Apartment, two bedrooms", "night", 130, 300), ("Welcome groceries", "stay", 25, 45) }),
            }),
            ("Experiences", 20, new[]
            {
                ("{0} Walks", new[] { ("Old town walking tour", "pax", 25, 45), ("Evening food tour", "pax", 65, 110) }),
                ("{0} Day Tours", new[] { ("Full-day excursion with lunch", "pax", 80, 160), ("Entrance tickets", "pax", 18, 40), ("Sunset boat trip", "pax", 45, 95) }),
                ("Taste of {0}", new[] { ("Cooking class with market visit", "pax", 70, 120), ("Wine tasting with transfer", "pax", 60, 120) }),
                ("{0} Adventures", new[] { ("Guided hike, half day", "pax", 40, 80), ("Kayak hire with guide", "pax", 55, 95) }),
            }),
            ("Services", 12, new[]
            {
                ("Guardian Travel Cover", new[] { ("Travel insurance, comprehensive", "pax", 38, 75) }),
                ("{0} Guide Co-op", new[] { ("Private guide, full day", "day", 180, 320), ("Airport meet and greet", "pax", 25, 45) }),
                ("Showcase Travel, head office", new[] { ("Booking fee", "booking", 45, 60), ("Documents and delivery", "booking", 12, 25), ("Restaurant reservations", "booking", 10, 20) }),
            }),
        };
        var notes = new[] { "", "", "Net rate", "Commissionable 10%", "Non-refundable", "Paid locally", "Pre-booked", "" };
        var random = new Random(42);
        var list = new List<RateCardItem>(count);

        for (var i = 1; i <= count; i++)
        {
            var (city, region, _) = TravelData.Destinations[i % TravelData.Destinations.Length];
            var cat = catalogue[(i / TravelData.Destinations.Length) % catalogue.Length];
            var sup = cat.Suppliers[random.Next(cat.Suppliers.Length)];
            var svc = sup.Services[random.Next(sup.Services.Length)];
            var season = TravelData.Seasons[random.Next(TravelData.Seasons.Length)];
            var regionFactor = region switch
            {
                "East Asia" => 1.15m, "Southern Africa" => 1.1m, "North Africa" => 0.85m, "Northern Europe" => 1.4m,
                "North America" => 1.3m, "Oceania" => 1.35m, "South America" => 1.05m, _ => 1.0m
            };
            var seasonFactor = season switch { "Summer" => 1.25m, "Autumn" => 1.05m, "Winter" => 0.85m, _ => 1.0m };
            var rate = Math.Round(random.Next(svc.Low * 100, svc.High * 100 + 1) / 100m * regionFactor * seasonFactor, 2);

            list.Add(new RateCardItem
            {
                Code = $"SRV-{i:D5}",
                Region = region,
                Destination = city,
                Category = cat.Category,
                Supplier = string.Format(sup.Supplier, city),
                Description = svc.Desc,
                Unit = svc.Unit,
                Rate = rate,
                MarginPercent = cat.Margin + (i % 3 == 0 ? 2 : 0) - (i % 5 == 0 ? 2 : 0),
                Season = season,
                Bookable = i % 17 != 0,
                Notes = notes[random.Next(notes.Length)]
            });
        }
        return list;
    }

    /// <summary>
    /// The supplier list: every destination's hotels, transfer companies, tour operators and services, named from the city
    /// (Hotel Lisbon Central, Kyoto Walks) with a few operator-wide names. Deterministic; about a third have no contract yet.
    /// </summary>
    private static List<SupplierRecord> SynthesizeSuppliers(int count)
    {
        var patterns = new (string Name, string Category)[]
        {
            ("Hotel {0} Central", "Accommodation"), ("{0} Boutique Stays", "Accommodation"), ("{0} Apartments", "Accommodation"), ("{0} Villas", "Accommodation"),
            ("{0} Airport Transfers", "Transport"), ("{0} Rail & Coach", "Transport"), ("{0} Car Hire", "Transport"),
            ("{0} Walks", "Experiences"), ("{0} Day Tours", "Experiences"), ("Taste of {0}", "Experiences"), ("{0} Adventures", "Experiences"),
            ("{0} Harbour Cruises", "Experiences"), ("{0} Bike Tours", "Experiences"), ("{0} Cooking School", "Experiences"),
            ("{0} Guide Co-op", "Services"), ("{0} Concierge", "Services"), ("{0} Spa Retreat", "Services"),
        };
        var streets = new[] { "Harbour Road", "Market Street", "Station Square", "Old Town Lane", "Cathedral Close", "Riverside Walk", "Hill Road", "Garden Street" };
        var random = new Random(7);
        var list = new List<SupplierRecord>(count);
        for (var i = 0; i < count; i++)
        {
            var (city, _, country) = TravelData.Destinations[i % TravelData.Destinations.Length];
            var pattern = patterns[(i / TravelData.Destinations.Length) % patterns.Length];
            var slug = string.Format(pattern.Name, city).ToLowerInvariant().Replace(" & ", "-").Replace(' ', '-');
            list.Add(new SupplierRecord
            {
                Code = $"SUP-{i + 1:D3}",
                Name = string.Format(pattern.Name, city),
                Category = pattern.Category,
                Address = $"{random.Next(2, 180)} {streets[random.Next(streets.Length)]}",
                City = city,
                Country = country,
                PostalCode = $"{random.Next(1000, 9999)}-{random.Next(100, 999)}",
                Phone = $"+{random.Next(20, 99)} {random.Next(10, 99)} {random.Next(100, 999)} {random.Next(1000, 9999)}",
                Email = $"bookings@{slug}.example",
                Contracted = i % 3 != 2,
                InsuranceOnFile = i % 4 != 3,
                OnlineBooking = i % 2 == 0,
                Active = i % 23 != 22
            });
        }
        return list;
    }

    /// <summary>One or two contacts per supplier: reservations first, then accounts for every other supplier.</summary>
    private static List<SupplierContactRecord> SynthesizeSupplierContacts(IReadOnlyList<SupplierRecord> suppliers)
    {
        string[] firstNames = ["Ana", "Bruno", "Chiara", "Daniel", "Elif", "Farid", "Greta", "Hiro", "Ines", "Jonas", "Keiko", "Luis", "Mai", "Nadia", "Omar", "Paula", "Quang", "Rosa", "Sven", "Thandi"];
        string[] lastNames = ["Almeida", "Bianchi", "Costa", "Dubois", "Eriksen", "Fujimoto", "Gomez", "Haddad", "Ivanova", "Jensen", "Khan", "Lund", "Mbeki", "Nakamura", "Okafor", "Pereira", "Quinn", "Rossi", "Santos", "Tran"];
        var list = new List<SupplierContactRecord>();
        var id = 1;
        for (var i = 0; i < suppliers.Count; i++)
        {
            var s = suppliers[i];
            var domain = s.Email[(s.Email.IndexOf('@') + 1)..];
            var count = i % 2 == 0 ? 2 : 1;
            for (var k = 0; k < count; k++)
            {
                var first = firstNames[(i * 3 + k * 7) % firstNames.Length];
                var last = lastNames[(i * 5 + k * 11) % lastNames.Length];
                list.Add(new SupplierContactRecord
                {
                    ContactID = id++,
                    SupplierCode = s.Code,
                    DisplayName = $"{first} {last}",
                    Role = k == 0 ? "Reservations" : "Accounts",
                    Phone = s.Phone,
                    Email = $"{first.ToLowerInvariant()}.{last.ToLowerInvariant()}@{domain}",
                    Active = (i + k) % 9 != 8
                });
            }
        }
        return list;
    }

    private static List<SalesDesk> SynthesizeSalesDesks()
    {
        return
        [
            new SalesDesk { DeskID = 1, Name = "Leisure desk", GroupBookings = false, AgentCount = 24 },
            new SalesDesk { DeskID = 2, Name = "Adventure desk", GroupBookings = true, AgentCount = 12 },
            new SalesDesk { DeskID = 3, Name = "Cruise desk", GroupBookings = true, AgentCount = 9 },
            new SalesDesk { DeskID = 4, Name = "Touring desk", GroupBookings = true, AgentCount = 15 },
            new SalesDesk { DeskID = 5, Name = "Luxury desk", GroupBookings = false, AgentCount = 6 },
            new SalesDesk { DeskID = 6, Name = "Customer care", GroupBookings = false, AgentCount = 18 },
            new SalesDesk { DeskID = 7, Name = "Head office", GroupBookings = false, AgentCount = 8 }
        ];
    }

    /// <summary>
    /// One agent per <see cref="SalesDesk.AgentCount"/>, so the Agents column of the master grid and the detail grid's row
    /// count agree. Names cycle through two lists; logins are made unique with a numeric suffix. Every ninth agent is inactive.
    /// </summary>
    private static List<AgentListItem> SynthesizeAgents(IEnumerable<SalesDesk> desks)
    {
        string[] firstNames =
        [
            "Alex", "Bruna", "Catherine", "Dario", "Elena", "Femi", "Grace", "Hugo", "Isabel", "Jonah", "Keira", "Liam", "Maya",
            "Noah", "Olivia", "Priya", "Quentin", "Rosa", "Samuel", "Tessa", "Umar", "Vera", "Wesley", "Ximena", "Yusuf", "Zoe"
        ];
        string[] lastNames =
        [
            "Silva", "Jenkins", "Clark", "Dias", "Evans", "Foster", "Garcia", "Harris", "Ibarra", "Jones", "Kim", "Lopez", "Murphy",
            "Nguyen", "Ortiz", "Patel", "Quinn", "Reed", "Singh", "Turner", "Usman", "Vargas", "Walsh", "Xu", "Young", "Zimmer"
        ];
        var agents = new List<AgentListItem>();
        var taken = new HashSet<string>(StringComparer.Ordinal);
        var n = 0;
        foreach (var desk in desks)
        {
            for (var i = 0; i < desk.AgentCount; i++, n++)
            {
                var first = firstNames[n % firstNames.Length];
                var last = lastNames[(n * 7 + n / firstNames.Length) % lastNames.Length];
                var login = $"{char.ToLowerInvariant(first[0])}{last.ToLowerInvariant()}";
                for (var suffix = 2; !taken.Add(login); suffix++)
                    login = $"{char.ToLowerInvariant(first[0])}{last.ToLowerInvariant()}{suffix}";
                agents.Add(new AgentListItem
                {
                    Login = login,
                    FullName = $"{first} {last}",
                    Email = $"{login}@showcase-travel.example",
                    Active = n % 9 != 4,
                    DeskID = desk.DeskID
                });
            }
        }
        return agents;
    }

    // ══════════════════════════════════════════════════════════════════════
    // ── AG GRID SHOWCASE DATA GENERATORS ──────────────────────────────────
    // ══════════════════════════════════════════════════════════════════════

    public static List<TickerRow> GetFinanceTickers()
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

        var list = new List<TickerRow>();
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

            list.Add(new TickerRow
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

    public static List<EmployeeRow> GetHrHierarchy()
    {
        return
        [
            // CEO & Executive
            new EmployeeRow { EmployeeID = "EMP-001", ReportsToID = null, FullName = "Arthur Pendelton", JobTitle = "Chief Executive Officer", Department = "Executive", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2014, 3, 1), Salary = 385000m, Rating = 5, AvatarBg = "#0f172a", SubordinatesCount = 4 },
            new EmployeeRow { EmployeeID = "EMP-002", ReportsToID = "EMP-001", FullName = "Claire Sterling", JobTitle = "VP of Engineering", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2016, 6, 15), Salary = 265000m, Rating = 5, AvatarBg = "#0284c7", SubordinatesCount = 5 },
            new EmployeeRow { EmployeeID = "EMP-003", ReportsToID = "EMP-001", FullName = "Marcus Vance", JobTitle = "VP of Product", Department = "Product", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2017, 2, 10), Salary = 245000m, Rating = 4, AvatarBg = "#7c3aed", SubordinatesCount = 3 },
            new EmployeeRow { EmployeeID = "EMP-004", ReportsToID = "EMP-001", FullName = "Helena Zhang", JobTitle = "VP of Sales & Growth", Department = "Sales", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2018, 9, 1), Salary = 250000m, Rating = 5, AvatarBg = "#059669", SubordinatesCount = 3 },
            new EmployeeRow { EmployeeID = "EMP-005", ReportsToID = "EMP-001", FullName = "Devon Montgomery", JobTitle = "VP of People & Culture", Department = "Human Resources", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 1, 20), Salary = 210000m, Rating = 4, AvatarBg = "#d97706", SubordinatesCount = 2 },

            // Engineering Directors & Leads
            new EmployeeRow { EmployeeID = "EMP-010", ReportsToID = "EMP-002", FullName = "Julian Becker", JobTitle = "Director of Core Infrastructure", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2018, 4, 12), Salary = 205000m, Rating = 5, AvatarBg = "#2563eb", SubordinatesCount = 4 },
            new EmployeeRow { EmployeeID = "EMP-011", ReportsToID = "EMP-002", FullName = "Aisha Khan", JobTitle = "Director of Frontend Architecture", Department = "Engineering", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 7, 8), Salary = 198000m, Rating = 5, AvatarBg = "#4f46e5", SubordinatesCount = 3 },
            new EmployeeRow { EmployeeID = "EMP-012", ReportsToID = "EMP-002", FullName = "Carlos Mendez", JobTitle = "Lead Data Architect", Department = "Engineering", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 2, 15), Salary = 185000m, Rating = 4, AvatarBg = "#0891b2", SubordinatesCount = 2 },

            // Engineering Staff
            new EmployeeRow { EmployeeID = "EMP-020", ReportsToID = "EMP-010", FullName = "Dmitri Volkov", JobTitle = "Principal Distributed Systems Engineer", Department = "Engineering", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 8, 1), Salary = 175000m, Rating = 5, AvatarBg = "#0369a1", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-021", ReportsToID = "EMP-010", FullName = "Seraphina Lin", JobTitle = "Senior Site Reliability Engineer", Department = "Engineering", Office = "Tokyo", EmploymentType = "Full-Time", HireDate = new DateTime(2021, 3, 22), Salary = 158000m, Rating = 4, AvatarBg = "#0284c7", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-022", ReportsToID = "EMP-011", FullName = "Liam Gallagher", JobTitle = "Senior Blazor Specialist", Department = "Engineering", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2021, 5, 14), Salary = 162000m, Rating = 5, AvatarBg = "#6366f1", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-023", ReportsToID = "EMP-011", FullName = "Maya Takahashi", JobTitle = "Design Systems Engineer", Department = "Engineering", Office = "Tokyo", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 1, 10), Salary = 145000m, Rating = 4, AvatarBg = "#8b5cf6", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-024", ReportsToID = "EMP-012", FullName = "Kofi Mensah", JobTitle = "Database Performance Specialist", Department = "Engineering", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 6, 1), Salary = 152000m, Rating = 4, AvatarBg = "#06b6d4", SubordinatesCount = 0 },

            // Product & Design
            new EmployeeRow { EmployeeID = "EMP-030", ReportsToID = "EMP-003", FullName = "Sophie Dubois", JobTitle = "Head of Enterprise UX", Department = "Product", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 11, 4), Salary = 175000m, Rating = 5, AvatarBg = "#9333ea", SubordinatesCount = 2 },
            new EmployeeRow { EmployeeID = "EMP-031", ReportsToID = "EMP-003", FullName = "Lucas Moretti", JobTitle = "Staff Product Manager", Department = "Product", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 5, 18), Salary = 168000m, Rating = 4, AvatarBg = "#a855f7", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-032", ReportsToID = "EMP-030", FullName = "Emma Watson", JobTitle = "Senior UI Designer", Department = "Product", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 9, 1), Salary = 132000m, Rating = 4, AvatarBg = "#c084fc", SubordinatesCount = 0 },

            // Sales & Growth
            new EmployeeRow { EmployeeID = "EMP-040", ReportsToID = "EMP-004", FullName = "Jack Reynolds", JobTitle = "Director of Strategic Accounts", Department = "Sales", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2019, 8, 15), Salary = 190000m, Rating = 5, AvatarBg = "#15803d", SubordinatesCount = 2 },
            new EmployeeRow { EmployeeID = "EMP-041", ReportsToID = "EMP-004", FullName = "Ananya Sharma", JobTitle = "EMEA Regional Enterprise Lead", Department = "Sales", Office = "London", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 10, 1), Salary = 172000m, Rating = 4, AvatarBg = "#16a34a", SubordinatesCount = 0 },
            new EmployeeRow { EmployeeID = "EMP-042", ReportsToID = "EMP-040", FullName = "Tyler Brooks", JobTitle = "Senior Account Executive", Department = "Sales", Office = "Sydney", EmploymentType = "Full-Time", HireDate = new DateTime(2022, 2, 1), Salary = 140000m, Rating = 4, AvatarBg = "#22c55e", SubordinatesCount = 0 },

            // HR & Talent
            new EmployeeRow { EmployeeID = "EMP-050", ReportsToID = "EMP-005", FullName = "Hannah Kim", JobTitle = "Head of Global Talent Acquisition", Department = "Human Resources", Office = "San Francisco", EmploymentType = "Full-Time", HireDate = new DateTime(2020, 3, 9), Salary = 155000m, Rating = 5, AvatarBg = "#ea580c", SubordinatesCount = 1 },
            new EmployeeRow { EmployeeID = "EMP-051", ReportsToID = "EMP-050", FullName = "Noah Campbell", JobTitle = "Technical Recruiter", Department = "Human Resources", Office = "New York", EmploymentType = "Full-Time", HireDate = new DateTime(2023, 4, 15), Salary = 108000m, Rating = 4, AvatarBg = "#f97316", SubordinatesCount = 0 }
        ];
    }

    public static List<InventoryProduct> GetInventoryProducts()
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
        var list = new List<InventoryProduct>();
        var baseDelivery = DateTime.UtcNow.Date.AddDays(3);

        foreach (var p in products)
        {
            var details = new List<WarehouseStock>();
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

                details.Add(new WarehouseStock
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

            list.Add(new InventoryProduct
            {
                SKU = p.SKU,
                ProductName = p.Name,
                Category = p.Cat,
                TotalOnHand = totalOnHand,
                TotalReserved = totalReserved,
                ReorderPoint = p.Reorder,
                UnitCost = p.Cost,
                WarehouseDetails = details
            });
        }
        return list;
    }
}
