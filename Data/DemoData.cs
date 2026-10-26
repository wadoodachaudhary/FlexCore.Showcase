namespace FlexCore.Showcase.Data;

/// <summary>
/// In-memory data for the Grouping &amp; totals grid page: the tour operator's rate card and the cost lines of one of its
/// packages, both built on <see cref="TravelData"/>. Supplier and service names are invented; no real company is named.
/// Both generators are seeded, so screenshots repeat. The home page, the Report writer page and the larger grid pages take
/// their travel data from their own services.
/// </summary>
public static class DemoData
{
    /// <summary>Two travellers on every package costing.</summary>
    private const int Pax = 2;

    /// <summary>How far a destination is from home moves its rates; the same factors as the bench store's rate card.</summary>
    private static decimal RegionFactor(string region) => region switch
    {
        "East Asia" => 1.15m, "Southern Africa" => 1.1m, "North Africa" => 0.85m, "Northern Europe" => 1.4m,
        "North America" => 1.3m, "Oceania" => 1.35m, "South America" => 1.05m, _ => 1.0m
    };

    /// <summary>Departure season price factor.</summary>
    private static decimal SeasonFactor(string season) => season switch { "Summer" => 1.25m, "Autumn" => 1.05m, "Winter" => 0.85m, _ => 1.0m };

    /// <summary>The services on the rate card: category, supplier (a {0} placeholder takes the city), description, the unit the
    /// rate is quoted per, and the rate range in whole currency units before the region factor.</summary>
    private static readonly (string Category, string Supplier, string Description, string Unit, int Low, int High)[] RateCardServices =
    {
        ("Transport",     "Meridian Airways",             "Return flights, economy",         "pax",     380, 620),
        ("Transport",     "{0} Airport Transfers",        "Airport to hotel, private car",   "trip",     45,  90),
        ("Transport",     "{0} Rail & Coach",             "Day-trip coach with driver",      "day",     120, 260),
        ("Accommodation", "Hotel {0} Central",            "Double room with breakfast",      "night",   110, 260),
        ("Accommodation", "Hotel {0} Central",            "Twin room, room only",            "night",    90, 220),
        ("Accommodation", "Hotel {0} Central",            "Suite with breakfast",            "night",   240, 480),
        ("Experiences",   "{0} Walks",                    "Old town walking tour",           "pax",      25,  45),
        ("Experiences",   "{0} Day Tours",                "Full-day excursion with lunch",   "pax",      80, 160),
        ("Experiences",   "Taste of {0}",                 "Cooking class with market visit", "pax",      70, 120),
        ("Services",      "Guardian Travel Cover",        "Travel insurance, comprehensive", "pax",      38,  75),
        ("Services",      "FlexCore Travel Agency, head office", "Booking fee",                     "booking",  45,  60),
        ("Services",      "FlexCore Travel Agency, head office", "Documents and delivery",          "booking",  12,  25),
    };

    /// <summary>Generates the rate card: for every destination the operator sells, four to seven of the services above in turn,
    /// each with a rate drawn from its range and scaled by the region. Roughly one line in ten is not bookable.</summary>
    public static List<RateCardLine> GenerateRateCard()
    {
        var rnd = new Random(13);
        var lines = new List<RateCardLine>();
        int code = 1000;
        for (int d = 0; d < TravelData.Destinations.Length; d++)
        {
            var (city, region, _) = TravelData.Destinations[d];
            var regionFactor = RegionFactor(region);
            int perDestination = rnd.Next(4, 8);
            for (int k = 0; k < perDestination; k++)
            {
                var svc = RateCardServices[(d + k) % RateCardServices.Length];
                code++;
                lines.Add(new RateCardLine(
                    Code: $"RC-{code:D4}",
                    Description: svc.Description,
                    Supplier: string.Format(svc.Supplier, city),
                    Destination: city,
                    Category: svc.Category,
                    Unit: svc.Unit,
                    Rate: Math.Round(rnd.Next(svc.Low * 100, svc.High * 100 + 1) / 100m * regionFactor, 2),
                    Bookable: rnd.Next(0, 10) > 0));
            }
        }
        return lines;
    }

    /// <summary>The services costed for a package: cost group (lettered the way the Report writer's cost sheet letters them),
    /// supplier (a {0} placeholder takes the package's city), description, unit, the rate range before the season factor, the
    /// rate basis (Contract or Spot) and whether the line is priced per person or per booking.</summary>
    private static readonly (string CostGroup, string Supplier, string Description, string Unit, int Low, int High, string RateBasis, string Pricing)[] PackageServices =
    {
        ("A Transport",     "Meridian Airways",             "Return flights, economy",         "pax",     380, 620, "Contract", "Per person"),
        ("A Transport",     "Meridian Airways",             "Checked baggage, 23 kg",          "pax",      40,  60, "Contract", "Per person"),
        ("A Transport",     "{0} Airport Transfers",        "Airport to hotel, private car",   "trip",     45,  90, "Spot",     "Per booking"),
        ("A Transport",     "{0} Airport Transfers",        "Hotel to airport, private car",   "trip",     45,  90, "Spot",     "Per booking"),
        ("A Transport",     "{0} Rail & Coach",             "Day-trip coach with driver",      "day",     120, 260, "Spot",     "Per booking"),
        ("B Accommodation", "Hotel {0} Central",            "Double room with breakfast",      "night",   110, 260, "Contract", "Per booking"),
        ("B Accommodation", "Hotel {0} Central",            "Late checkout on departure day",  "stay",     30,  60, "Spot",     "Per booking"),
        ("C Experiences",   "{0} Walks",                    "Old town walking tour",           "pax",      25,  45, "Contract", "Per person"),
        ("C Experiences",   "{0} Walks",                    "Evening food tour",               "pax",      65, 110, "Spot",     "Per person"),
        ("C Experiences",   "{0} Day Tours",                "Full-day excursion with lunch",   "pax",      80, 160, "Contract", "Per person"),
        ("C Experiences",   "{0} Day Tours",                "Entrance tickets",                "pax",      18,  40, "Spot",     "Per person"),
        ("C Experiences",   "Taste of {0}",                 "Cooking class with market visit", "pax",      70, 120, "Contract", "Per person"),
        ("D Services",      "Guardian Travel Cover",        "Travel insurance, comprehensive", "pax",      38,  75, "Contract", "Per person"),
        ("D Services",      "FlexCore Travel Agency, head office", "Booking fee",                     "booking",  45,  60, "Contract", "Per booking"),
        ("D Services",      "FlexCore Travel Agency, head office", "Documents and delivery",          "booking",  12,  25, "Contract", "Per booking"),
    };

    /// <summary>Generates the cost lines of the first package in <see cref="TravelData.Packages"/> (Lisbon Explorer): the services
    /// above, costed once per departure season in turn, for <see cref="Pax"/> travellers and the package's nights, so the default
    /// sixty lines are the fifteen services in each of the four seasons. A contract rate is drawn once per service and only moves
    /// with the season; a spot rate is drawn afresh for every line.</summary>
    public static List<PackageCostLine> GeneratePackageCostLines(int count = 60)
    {
        var rnd = new Random(42);
        var package = TravelData.Packages[0];
        var regionFactor = RegionFactor(package.Region);
        var contractRates = PackageServices.Select(s => rnd.Next(s.Low * 100, s.High * 100 + 1) / 100m).ToArray();
        var lines = new List<PackageCostLine>();
        for (int i = 0; i < count; i++)
        {
            int s = i % PackageServices.Length;
            var svc = PackageServices[s];
            var season = TravelData.Seasons[(i / PackageServices.Length) % TravelData.Seasons.Length];
            var baseRate = svc.RateBasis == "Contract" ? contractRates[s] : rnd.Next(svc.Low * 100, svc.High * 100 + 1) / 100m;
            var rate = Math.Round(baseRate * regionFactor * SeasonFactor(season), 2);
            int qty = svc.Pricing == "Per person" ? Pax : svc.Unit == "night" ? package.Nights : 1;
            lines.Add(new PackageCostLine(
                Season: season,
                RateBasis: svc.RateBasis,
                Supplier: string.Format(svc.Supplier, package.City),
                CostGroup: svc.CostGroup,
                Description: svc.Description,
                Qty: qty,
                Unit: svc.Unit,
                Pricing: svc.Pricing,
                Rate: rate,
                Total: qty * rate));
        }
        return lines;
    }
}

public record RateCardLine(string Code, string Description, string Supplier, string Destination,
    string Category, string Unit, decimal Rate, bool Bookable);

public record PackageCostLine(string Season, string RateBasis, string Supplier, string CostGroup, string Description,
    int Qty, string Unit, string Pricing, decimal Rate, decimal Total);
