namespace FlexCore.Showcase.Data;

/// <summary>
/// One post at FlexCore Travel Agency. <see cref="ManagerId"/> is the solid reporting line and <see cref="DottedLineId"/> a second,
/// dotted one (a finance or HR partner who also answers to the head of the area they support). A vacant post keeps its place
/// in the tree, its people and its budgeted salary, but has no name, start date or rating.
/// </summary>
public sealed record StaffMember
{
    public required string Id { get; init; }
    public string? ManagerId { get; init; }
    public string? DottedLineId { get; init; }
    public required string Name { get; init; }        // "Vacant" for an open post
    public required string Title { get; init; }
    public int Grade { get; init; }                   // 1 trainee to 9 chief executive
    public required string Division { get; init; }
    public required string Department { get; init; }
    public required string Office { get; init; }
    public required string Country { get; init; }
    public required string Region { get; init; }
    public required string Contract { get; init; }    // Permanent, Fixed-term, Seasonal or Contractor
    public double Fte { get; init; } = 1;             // 1 full time; 0.5 to 0.8 part time
    public DateOnly? Started { get; init; }
    public decimal Salary { get; init; }              // USD a year at the post's FTE; the budget for a vacant post
    public int? Rating { get; init; }                 // 1 to 5; none for vacant posts and people who started under six months ago
    public string Languages { get; init; } = "";
    public string Email { get; init; } = "";
    public bool Vacant { get; init; }
    public bool OnLeave { get; init; }
    public int Level { get; init; }                   // 0 for the chief executive
    public int DirectReports { get; init; }           // posts reporting on the solid line, vacant ones included
    public int Headcount { get; init; }               // people below on the solid line, vacant posts not counted
}

/// <summary>
/// The staff of FlexCore Travel Agency, the imagined tour operator of the people demos: a head office in Boston, a ground team in
/// each destination of <see cref="TravelData"/>, contact centres in Lisbon, Manila and Boston, and the usual central functions.
/// Generated from a fixed seed, so every visitor and every build sees the same people. Every name is made up: the executive team
/// is fixed, everyone else gets a first name and a surname drawn from the two lists below.
/// </summary>
public static class TravelOrg
{
    /// <summary>The date the figures are as of: tenure, ratings and seasonal start dates count from it.</summary>
    public static readonly DateOnly AsOf = new(2026, 10, 1);

    public static IReadOnlyList<StaffMember> Staff => Built.Value;

    private static readonly Lazy<IReadOnlyList<StaffMember>> Built = new(Build);

    private sealed record Office(string City, string Country, string Region, double CostIndex, string Languages);

    private static readonly string[] FirstNames =
    [
        "Buttercup", "Scarlet", "Harry", "Albus", "Ronald", "Hermione", "Clementine", "Barnaby", "Percival", "Marigold", "Tabitha",
        "Horace", "Wilhelmina", "Bartholomew", "Rosalind", "Cornelius", "Pippa", "Ignatius", "Philippa", "Rupert", "Dorothea", "Hazel",
        "Ambrose", "Juniper", "Mortimer", "Petunia", "Thaddeus", "Winifred", "Archibald", "Lavender", "Montgomery", "Prudence", "Clover",
        "Ebenezer", "Delphine", "Ferdinand", "Ottoline", "Humphrey", "Bluebell", "Cuthbert", "Henrietta", "Leopold", "Matilda", "Ophelia",
        "Quentin", "Primrose", "Rufus", "Saffron", "Tobias", "Ursula", "Valentine", "Wendell", "Xanthe", "Yorick", "Zinnia", "Basil",
        "Poppy", "Rowan", "Fennel", "Sorrel", "Bramble", "Florian", "Mabel", "Jasper", "Imogen", "Crispin", "Octavia", "Gideon", "Nell",
        "Ezra", "Peregrine", "Wren", "Otto", "Dahlia", "Felix", "Beatrix", "Lysander", "Esme", "Hugo", "Agatha"
    ];

    private static readonly string[] Surnames =
    [
        "Plum", "Underhill", "Potts", "Door", "Wiesel", "Ranger", "Peppercorn", "Thistlewood", "Bumblesby", "Crumpet", "Fiddlesticks",
        "Applebottom", "Wobblesworth", "Higglebottom", "Puddleby", "Marmalade", "Nettlefield", "Tumbleweed", "Quickfoot", "Brambleberry",
        "Featherstone", "Gooseberry", "Honeysuckle", "Inkwell", "Kettleby", "Mossgrove", "Nutmeg", "Oddfellow", "Pennywhistle", "Quill",
        "Raspberry", "Snodgrass", "Teacup", "Vellum", "Wigglesworth", "Yarrow", "Zephyr", "Butterworth", "Cobblestone", "Dimplewick",
        "Elderflower", "Figgis", "Gumboot", "Hobnob", "Inglenook", "Jumble", "Larkspur", "Muddlecombe", "Noodle", "Picklesby", "Rumbletum",
        "Tiddlywink", "Crabapple", "Puddingstone", "Lamplighter", "Gingersnap", "Fizzwick", "Buttonwood", "Mapleleaf", "Toadflax",
        "Candlewick", "Hollyhock", "Biscuit", "Sprocket", "Moonbeam", "Periwinkle", "Wafflestone", "Hedgerow", "Bilberry", "Pumpernickel"
    ];

    private static readonly Office Boston = new("Boston", "United States", "Americas", 1.00, "English");
    private static readonly Office Lisbon = new("Lisbon", "Portugal", "Europe", 0.62, "Portuguese, English");
    private static readonly Office Amalfi = new("Amalfi", "Italy", "Europe", 0.70, "Italian, English");
    private static readonly Office Santorini = new("Santorini", "Greece", "Europe", 0.58, "Greek, English");
    private static readonly Office Reykjavik = new("Reykjavik", "Iceland", "Europe", 0.95, "Icelandic, English");
    private static readonly Office Kyoto = new("Kyoto", "Japan", "Asia Pacific", 0.75, "Japanese, English");
    private static readonly Office HoiAn = new("Hoi An", "Vietnam", "Asia Pacific", 0.30, "Vietnamese, English");
    private static readonly Office Queenstown = new("Queenstown", "New Zealand", "Asia Pacific", 0.80, "English");
    private static readonly Office Manila = new("Manila", "Philippines", "Asia Pacific", 0.28, "Filipino, English");
    private static readonly Office CapeTown = new("Cape Town", "South Africa", "Africa", 0.40, "English, Afrikaans");
    private static readonly Office Marrakech = new("Marrakech", "Morocco", "Africa", 0.35, "Arabic, French, English");
    private static readonly Office Vancouver = new("Vancouver", "Canada", "Americas", 0.88, "English, French");
    private static readonly Office Banff = new("Banff", "Canada", "Americas", 0.85, "English, French");
    private static readonly Office ElCalafate = new("El Calafate", "Argentina", "Americas", 0.38, "Spanish, English");

    private static readonly string[] ExtraLanguages = ["Spanish", "French", "German", "Mandarin", "Italian", "Japanese", "Portuguese", "Dutch"];

    // Base salary in USD by grade at the Boston cost index; an office scales it by its CostIndex.
    private static readonly decimal[] BaseSalary = [0, 38_000, 48_000, 64_000, 82_000, 105_000, 140_000, 185_000, 260_000, 420_000];

    private static IReadOnlyList<StaffMember> Build()
    {
        var b = new Builder(new Random(2026));

        // Executive office
        var ceo = b.Add(null, "Chief Executive Officer", 9, "Executive office", "Executive office", Boston, fixedName: "Albus Door");
        b.Add(ceo, "Executive Assistant", 3, "Executive office", "Executive office", Boston);
        var staff = b.Add(ceo, "Chief of Staff", 6, "Executive office", "Strategy", Boston);
        b.Many(2, staff, "Strategy Analyst", 4, "Executive office", "Strategy", Boston);
        b.Add(staff, "Internal Communications Manager", 5, "Executive office", "Strategy", Boston);

        // Operations: four regions, a ground team in every destination, and customer care
        var coo = b.Add(ceo, "Chief Operating Officer", 8, "Operations", "Operations leadership", Boston, fixedName: "Hermione Ranger");
        var safety = b.Add(coo, "Travel Safety Manager", 5, "Operations", "Travel safety", Boston);
        b.Many(2, safety, "Travel Safety Specialist", 3, "Operations", "Travel safety", Boston);
        var regions = new (string Name, Office Base, (Office Office, double Size)[] Offices)[]
        {
            ("Europe", Lisbon, [(Lisbon, 1.4), (Amalfi, 0.9), (Santorini, 0.8), (Reykjavik, 0.7)]),
            ("Asia Pacific", Kyoto, [(Kyoto, 1.2), (HoiAn, 0.8), (Queenstown, 0.8)]),
            ("Africa", CapeTown, [(CapeTown, 1.0), (Marrakech, 0.9)]),
            ("Americas", Vancouver, [(Vancouver, 1.0), (Banff, 0.7), (ElCalafate, 0.6)]),
        };
        var regionHeads = new Dictionary<string, string>();
        foreach (var (name, home, offices) in regions)
        {
            var director = b.Add(coo, $"Regional Director, {name}", 7, "Operations", $"{name} region", home);
            regionHeads[name] = director;
            b.Add(director, "Regional Operations Analyst", 4, "Operations", $"{name} region", home);
            foreach (var (office, size) in offices)
            {
                var dept = $"{office.City} destination team";
                var manager = b.Add(director, "Destination Manager", 6, "Operations", dept, office);
                var ground = b.Add(manager, "Ground Operations Lead", 4, "Operations", dept, office);
                b.Many(Scale(4, size) + 1, ground, "Operations Coordinator", 3, "Operations", dept, office);
                var leadGuide = b.Add(manager, "Lead Guide", 4, "Operations", dept, office);
                b.Many(Scale(26, size), leadGuide, "Tour Guide", 2, "Operations", dept, office, seasonal: 0.45, partTime: 0.25, extraLanguage: 0.6);
                var guest = b.Add(manager, "Guest Services Lead", 4, "Operations", dept, office);
                b.Many(Scale(8, size), guest, "Guest Services Representative", 2, "Operations", dept, office, seasonal: 0.2, partTime: 0.2, extraLanguage: 0.3);
                var transfers = b.Add(manager, "Transfers Lead", 4, "Operations", dept, office);
                b.Many(Scale(11, size), transfers, "Driver", 2, "Operations", dept, office, contractor: 0.35, seasonal: 0.15, partTime: 0.15);
            }
        }

        var care = b.Add(coo, "Director of Customer Care", 7, "Customer care", "Customer care leadership", Lisbon);
        var quality = b.Add(care, "Quality and Training Manager", 5, "Customer care", "Quality and training", Lisbon);
        b.Many(3, quality, "Quality Analyst", 3, "Customer care", "Quality and training", Lisbon);
        b.Many(3, quality, "Contact Centre Trainer", 3, "Customer care", "Quality and training", Lisbon);
        foreach (var (site, teams) in new[] { (Lisbon, 6), (Manila, 9), (Boston, 3) })
        {
            var dept = $"Contact centre {site.City}";
            var siteManager = b.Add(care, "Contact Centre Manager", 6, "Customer care", dept, site);
            for (var t = 0; t < teams; t++)
            {
                var leader = b.Add(siteManager, "Team Leader", 4, "Customer care", dept, site);
                b.Many(9 + b.Next(5), leader, "Customer Care Agent", 2, "Customer care", dept, site, fixedTerm: 0.1, partTime: 0.15, extraLanguage: 0.45);
            }
        }

        // Sales and marketing
        var cco = b.Add(ceo, "Chief Commercial Officer", 8, "Sales and marketing", "Commercial leadership", Boston, fixedName: "Scarlet Underhill");
        var sales = b.Add(cco, "Head of Sales", 7, "Sales and marketing", "Sales", Boston);
        foreach (var (desk, home, size) in new[] { ("Leisure desk", Boston, 14), ("Groups and incentives desk", Boston, 9), ("Luxury desk", Boston, 7), ("Adventure desk", Vancouver, 10), ("Trade partners desk", Lisbon, 8) })
        {
            var deskManager = b.Add(sales, "Desk Manager", 5, "Sales and marketing", desk, home);
            b.Many(2, deskManager, "Senior Travel Consultant", 4, "Sales and marketing", desk, home, extraLanguage: 0.3);
            b.Many(size - 2, deskManager, "Travel Consultant", 3, "Sales and marketing", desk, home, partTime: 0.15, extraLanguage: 0.3);
        }
        var marketing = b.Add(cco, "Head of Marketing", 7, "Sales and marketing", "Marketing", Boston);
        var brand = b.Add(marketing, "Brand Manager", 5, "Sales and marketing", "Marketing", Boston);
        b.Many(2, brand, "Graphic Designer", 3, "Sales and marketing", "Marketing", Boston, contractor: 0.3);
        var digital = b.Add(marketing, "Digital Marketing Manager", 5, "Sales and marketing", "Marketing", Boston);
        b.Many(4, digital, "Digital Marketing Specialist", 3, "Sales and marketing", "Marketing", Lisbon);
        var content = b.Add(marketing, "Content Lead", 5, "Sales and marketing", "Marketing", Lisbon);
        b.Many(3, content, "Travel Writer", 3, "Sales and marketing", "Marketing", Lisbon, contractor: 0.5, partTime: 0.3, extraLanguage: 0.5);
        var crm = b.Add(marketing, "CRM Manager", 5, "Sales and marketing", "Marketing", Boston);
        b.Many(2, crm, "CRM Analyst", 3, "Sales and marketing", "Marketing", Boston);
        var partnerships = b.Add(cco, "Head of Partnerships", 7, "Sales and marketing", "Partnerships", Lisbon);
        foreach (var (name, home, _) in regions)
        {
            b.Add(marketing, $"Regional Marketing Manager, {name}", 5, "Sales and marketing", "Marketing", home, dotted: regionHeads[name]);
            var contracting = b.Add(partnerships, $"Contracting Manager, {name}", 5, "Sales and marketing", "Partnerships", home, dotted: regionHeads[name]);
            b.Many(3 + b.Next(3), contracting, "Contracting Executive", 3, "Sales and marketing", "Partnerships", home, extraLanguage: 0.4);
        }

        // Product
        var cpo = b.Add(ceo, "Chief Product Officer", 8, "Product", "Product leadership", Boston, fixedName: "Harry Potts");
        var design = b.Add(cpo, "Head of Trip Design", 6, "Product", "Trip design", Boston);
        foreach (var (name, home, _) in regions)
        {
            var pm = b.Add(design, $"Product Manager, {name}", 5, "Product", "Trip design", home);
            b.Many(2, pm, "Product Executive", 3, "Product", "Trip design", home);
        }
        var revenue = b.Add(cpo, "Head of Revenue Management", 6, "Product", "Pricing and revenue", Boston);
        b.Many(4, revenue, "Revenue Analyst", 4, "Product", "Pricing and revenue", Boston);
        b.Many(2, revenue, "Pricing Executive", 3, "Product", "Pricing and revenue", Lisbon);

        // Technology
        var cto = b.Add(ceo, "Chief Technology Officer", 8, "Technology", "Technology leadership", Boston, fixedName: "Ronald Wiesel");
        var engineering = b.Add(cto, "Director of Engineering", 7, "Technology", "Engineering", Lisbon);
        foreach (var (team, home, size) in new[] { ("Booking platform", Boston, 7), ("Payments", Boston, 5), ("Mobile apps", Lisbon, 6), ("Partner integrations", Lisbon, 6), ("Website", Lisbon, 5) })
        {
            var em = b.Add(engineering, "Engineering Manager", 6, "Technology", team, home);
            b.Many(2, em, "Senior Software Engineer", 5, "Technology", team, home, contractor: 0.15);
            b.Many(size - 2, em, "Software Engineer", 4, "Technology", team, home, contractor: 0.15);
            b.Add(em, "Quality Engineer", 4, "Technology", team, home);
        }
        var data = b.Add(cto, "Head of Data", 6, "Technology", "Data and analytics", Boston);
        b.Many(3, data, "Data Engineer", 4, "Technology", "Data and analytics", Boston);
        b.Many(4, data, "Data Analyst", 3, "Technology", "Data and analytics", Lisbon);
        var it = b.Add(cto, "Head of IT", 6, "Technology", "IT and security", Boston);
        var security = b.Add(it, "Security Lead", 5, "Technology", "IT and security", Boston);
        b.Many(2, security, "Security Analyst", 4, "Technology", "IT and security", Boston);
        var serviceDesk = b.Add(it, "Service Desk Lead", 4, "Technology", "IT and security", Manila);
        b.Many(5, serviceDesk, "IT Support Technician", 2, "Technology", "IT and security", Manila, fixedTerm: 0.2);

        // Finance
        var cfo = b.Add(ceo, "Chief Financial Officer", 8, "Finance", "Finance leadership", Boston, fixedName: "Buttercup Plum");
        var controller = b.Add(cfo, "Financial Controller", 7, "Finance", "Accounting", Boston);
        var accounting = b.Add(controller, "Accounting Manager", 5, "Finance", "Accounting", Boston);
        b.Many(5, accounting, "Accountant", 3, "Finance", "Accounting", Boston);
        var ledger = b.Add(controller, "Payables and Receivables Lead", 4, "Finance", "Accounting", Manila);
        b.Many(6, ledger, "Accounts Clerk", 2, "Finance", "Accounting", Manila, fixedTerm: 0.15);
        var payroll = b.Add(controller, "Payroll Manager", 5, "Finance", "Payroll", Lisbon);
        b.Many(3, payroll, "Payroll Administrator", 3, "Finance", "Payroll", Lisbon, partTime: 0.3);
        var fpa = b.Add(cfo, "Head of Financial Planning", 6, "Finance", "Financial planning", Boston);
        b.Many(3, fpa, "Financial Analyst", 4, "Finance", "Financial planning", Boston);
        foreach (var (name, home, _) in regions)
            b.Add(fpa, $"Finance Partner, {name}", 5, "Finance", "Financial planning", home, dotted: regionHeads[name]);
        var treasury = b.Add(cfo, "Treasury Manager", 5, "Finance", "Treasury", Boston);
        b.Add(treasury, "Treasury Analyst", 3, "Finance", "Treasury", Boston);

        // People
        var people = b.Add(ceo, "Chief People Officer", 8, "People", "People leadership", Boston);
        foreach (var (supports, home) in new[] { (coo, Boston), (care, Lisbon), (cco, Boston), (cto, Lisbon) })
            b.Add(people, "HR Business Partner", 5, "People", "HR business partners", home, dotted: supports);
        var talent = b.Add(people, "Head of Talent Acquisition", 6, "People", "Talent acquisition", Boston);
        b.Many(2, talent, "Recruiter", 3, "People", "Talent acquisition", Lisbon);
        b.Many(2, talent, "Recruiter", 3, "People", "Talent acquisition", Manila);
        b.Add(talent, "Recruiter", 3, "People", "Talent acquisition", Boston);
        b.Add(talent, "Seasonal Recruitment Coordinator", 3, "People", "Talent acquisition", Lisbon, fixedTerm: 1);
        var learning = b.Add(people, "Learning and Development Manager", 5, "People", "Learning and development", Boston);
        b.Many(3, learning, "Learning Specialist", 3, "People", "Learning and development", Lisbon, partTime: 0.3);
        var hrOps = b.Add(people, "HR Operations Lead", 4, "People", "HR operations", Lisbon);
        b.Many(4, hrOps, "HR Administrator", 2, "People", "HR operations", Lisbon);

        // Legal
        var counsel = b.Add(ceo, "General Counsel", 8, "Legal", "Legal and compliance", Boston);
        b.Many(2, counsel, "Senior Counsel", 6, "Legal", "Legal and compliance", Boston);
        var compliance = b.Add(counsel, "Compliance Manager", 5, "Legal", "Legal and compliance", Lisbon);
        b.Many(2, compliance, "Compliance Analyst", 3, "Legal", "Legal and compliance", Lisbon);
        b.Add(counsel, "Data Protection Officer", 5, "Legal", "Legal and compliance", Lisbon);
        b.Add(counsel, "Paralegal", 3, "Legal", "Legal and compliance", Boston);

        return b.Finish();
    }

    private static int Scale(int n, double size) => Math.Max(1, (int)Math.Round(n * size));

    private sealed class Builder(Random rand)
    {
        private readonly List<StaffMember> _posts = [];
        private readonly HashSet<string> _names = new(StringComparer.Ordinal);
        private readonly HashSet<string> _emails = new(StringComparer.Ordinal);

        public int Next(int max) => rand.Next(max);

        public void Many(int count, string? manager, string title, int grade, string division, string department, Office office,
            double seasonal = 0, double contractor = 0, double fixedTerm = 0, double partTime = 0, double extraLanguage = 0)
        {
            for (var i = 0; i < count; i++)
                Add(manager, title, grade, division, department, office, null, seasonal, contractor, fixedTerm, partTime, extraLanguage);
        }

        public string Add(string? manager, string title, int grade, string division, string department, Office office,
            string? dotted = null, double seasonal = 0, double contractor = 0, double fixedTerm = 0, double partTime = 0, double extraLanguage = 0,
            string? fixedName = null)
        {
            var id = $"ST-{_posts.Count + 1:D4}";
            var roll = rand.NextDouble();
            var contract = roll < seasonal ? "Seasonal"
                : roll < seasonal + contractor ? "Contractor"
                : roll < seasonal + contractor + fixedTerm ? "Fixed-term"
                : "Permanent";
            var fte = rand.NextDouble() < partTime ? new[] { 0.5, 0.6, 0.8 }[rand.Next(3)] : 1.0;
            var salary = Math.Round(BaseSalary[grade] * (decimal)office.CostIndex * (decimal)fte * (decimal)(0.9 + rand.NextDouble() * 0.2) / 100) * 100;
            var vacant = fixedName is null && grade <= 7 && rand.NextDouble() < 0.03;

            var languages = office.Languages;
            if (rand.NextDouble() < extraLanguage)
            {
                var extra = ExtraLanguages[rand.Next(ExtraLanguages.Length)];
                if (!languages.Contains(extra, StringComparison.Ordinal)) languages += ", " + extra;
            }

            if (vacant)
            {
                _posts.Add(new StaffMember
                {
                    Id = id, ManagerId = manager, DottedLineId = dotted, Name = "Vacant", Title = title, Grade = grade, Division = division,
                    Department = department, Office = office.City, Country = office.Country, Region = office.Region, Contract = contract,
                    Fte = fte, Salary = salary, Languages = office.Languages, Vacant = true
                });
                return id;
            }

            var name = fixedName ?? PickName();
            _names.Add(name);
            var started = StartDate(grade, contract);
            int? rating = started > TravelOrg.AsOf.AddMonths(-6) ? null : RatingRoll();
            _posts.Add(new StaffMember
            {
                Id = id, ManagerId = manager, DottedLineId = dotted, Name = name, Title = title, Grade = grade, Division = division,
                Department = department, Office = office.City, Country = office.Country, Region = office.Region, Contract = contract,
                Fte = fte, Started = started, Salary = salary, Rating = rating, Languages = languages, Email = Email(name),
                OnLeave = rand.NextDouble() < 0.02
            });
            return id;
        }

        private string PickName()
        {
            for (var attempt = 0; ; attempt++)
            {
                var name = $"{FirstNames[rand.Next(FirstNames.Length)]} {Surnames[rand.Next(Surnames.Length)]}";
                if (attempt > 20) name = name.Replace(" ", $" {(char)('A' + rand.Next(26))}. ");
                if (!_names.Contains(name)) return name;
            }
        }

        private DateOnly StartDate(int grade, string contract)
        {
            if (contract == "Seasonal") return new DateOnly(2026, 3 + rand.Next(3), 1 + rand.Next(28));
            var maxDays = (int)((2 + grade * 1.6) * 365);
            var started = TravelOrg.AsOf.AddDays(-(30 + rand.Next(maxDays)));
            var founded = new DateOnly(2009, 4, 1);
            return started < founded ? founded.AddDays(rand.Next(400)) : started;
        }

        private int RatingRoll()
        {
            var r = rand.NextDouble();
            return r < 0.03 ? 1 : r < 0.13 ? 2 : r < 0.55 ? 3 : r < 0.88 ? 4 : 5;
        }

        private string Email(string name)
        {
            var parts = name.Replace(".", "").ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var local = $"{parts[0]}.{string.Join("", parts.Skip(1))}";
            var email = $"{local}@flexcore-travel.example";
            for (var n = 2; !_emails.Add(email); n++) email = $"{local}{n}@flexcore-travel.example";
            return email;
        }

        // Second pass: depth, direct reports and the people below each post.
        public IReadOnlyList<StaffMember> Finish()
        {
            var children = _posts.Where(p => p.ManagerId is not null).ToLookup(p => p.ManagerId!);
            var result = new List<StaffMember>(_posts.Count);
            var byId = new Dictionary<string, StaffMember>();
            int Below(string id) => children[id].Sum(c => (c.Vacant ? 0 : 1) + Below(c.Id));
            void Walk(StaffMember post, int level)
            {
                var done = post with { Level = level, DirectReports = children[post.Id].Count(), Headcount = Below(post.Id) };
                byId[post.Id] = done;
                foreach (var child in children[post.Id]) Walk(child, level + 1);
            }
            Walk(_posts[0], 0);
            foreach (var post in _posts) result.Add(byId[post.Id]);
            return result;
        }
    }
}
