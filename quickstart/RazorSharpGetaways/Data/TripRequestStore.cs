namespace RazorSharpGetaways.Data;

/// <summary>
/// The agency's trip requests for one visit: 1,000 generated on first use and kept in memory, so an edit on the Trip Requests
/// page shows on the Dashboard at once. Registered as a scoped service in Program.cs, one per visitor. To run the app on your own
/// data, replace the generator here; the pages read only this class.
/// </summary>
public sealed class TripRequestStore
{
    /// <summary>The agent signed in to the app, who takes the new requests.</summary>
    public const string SignedInAgent = "Pippa Larkspur";

    public static readonly TravelAgent[] Agents =
    [
        new("Marigold Teacup", "#0284c7"),
        new("Percival Crumpet", "#7c3aed"),
        new("Tabitha Inkwell", "#10b981"),
        new("Horace Nutmeg", "#d97706"),
        new("Rosalind Fizzwick", "#e11d48"),
        new("Cornelius Quill", "#0891b2"),
        new("Pippa Larkspur", "#4f46e5"),
        new("Ambrose Hobnob", "#059669"),
    ];

    /// <summary>The trip types the agency sells; the grid's drop-down grid lists these four columns.</summary>
    public static readonly IReadOnlyList<TripTypeLookup> TripTypes =
    [
        new() { Code = "BEACH", Name = "Beach & island resorts", Desk = "Leisure", LeadTime = "6-8 wks" },
        new() { Code = "CITY", Name = "City breaks & culture", Desk = "Leisure", LeadTime = "2-3 wks" },
        new() { Code = "SKI", Name = "Ski & snowboard", Desk = "Adventure", LeadTime = "8-12 wks" },
        new() { Code = "SAFARI", Name = "Safari & wildlife", Desk = "Adventure", LeadTime = "12-16 wks" },
        new() { Code = "CRUISE", Name = "Ocean & river cruises", Desk = "Cruise", LeadTime = "10-14 wks" },
        new() { Code = "RAIL", Name = "Scenic rail journeys", Desk = "Touring", LeadTime = "4-6 wks" },
        new() { Code = "FAMILY", Name = "Family holidays", Desk = "Leisure", LeadTime = "6-8 wks" },
        new() { Code = "HONEYMOON", Name = "Honeymoons & celebrations", Desk = "Luxury", LeadTime = "8-10 wks" },
    ];

    public static readonly string[] Statuses = ["New", "Quoted", "Booked", "Declined"];

    /// <summary>All requests, newest additions first.</summary>
    public List<TripRequest> Requests { get; } = GenerateRequests(1000);

    /// <summary>The newest requests still waiting for a quote; the bell in the top bar shows four.</summary>
    public IEnumerable<TripRequest> Newest(int count) =>
        Requests.Where(r => r.Status == "New").OrderByDescending(r => r.DateRequested).Take(count);

    /// <summary>Booked trips that have not left yet, soonest first: the Dashboard's departures and the Invoices page's list.</summary>
    public IEnumerable<TripRequest> BookedDepartures =>
        Requests.Where(r => r.Status == "Booked" && r.Departure >= DateTime.Today).OrderBy(r => r.Departure);

    /// <summary>The trip the Itinerary page lays out: LIS-0586, a Lisbon honeymoon booked by the signed-in agent and leaving tomorrow.</summary>
    public TripRequest? ItineraryTrip => Requests.FirstOrDefault(r => r.Request == "LIS-0586") ?? Requests.FirstOrDefault();

    /// <summary>A blank request for the editor, taken by the signed-in agent.</summary>
    public TripRequest NewRequest() => new()
    {
        Agent = SignedInAgent, AvatarBg = AgentColor(SignedInAgent),
        DateRequested = DateTime.Today, Departure = DateTime.Today.AddDays(90),
        Destination = "Lisbon", Nights = TripPackages.ForDestination("Lisbon").Nights,
        TripType = "CITY", TripTypeName = "City breaks & culture",
        Travellers = 2, Status = "New"
    };

    /// <summary>Adds a request filled in by the editor at the top of the list and returns it.</summary>
    public TripRequest Add(TripRequest request)
    {
        Complete(request);
        request.Id = Requests.Count + 1;
        request.Request = RequestKey(request.Destination, request.Id);
        request.FareHistory = Enumerable.Repeat((int)request.Quote, 5).ToList();
        request.FareDeltaPercent = 0;
        Requests.Insert(0, request);
        return request;
    }

    /// <summary>Writes an edited copy back onto the stored request.</summary>
    public void Save(TripRequest stored, TripRequest edited)
    {
        Complete(edited);
        stored.CopyEditsFrom(edited);
        ApplyQuoteToFares(stored);
    }

    // The trip type's name and the agent's colour follow what was typed in the editor
    private static void Complete(TripRequest request)
    {
        request.TripTypeName = TripTypes.FirstOrDefault(t => t.Code.Equals(request.TripType, StringComparison.OrdinalIgnoreCase))?.Name ?? request.TripType;
        request.AvatarBg = AgentColor(request.Agent);
    }

    /// <summary>The colour of an agent's avatar; grey for a name that is not on the team.</summary>
    public static string AgentColor(string name) =>
        Agents.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Color ?? "#64748b";

    /// <summary>The last of the five weekly fare samples is the quote, so a new quote moves the sparkline's end and its percentage.</summary>
    public static void ApplyQuoteToFares(TripRequest r)
    {
        if (r.FareHistory.Count == 0) return;
        r.FareHistory[^1] = (int)r.Quote;
        r.FareDeltaPercent = (int)Math.Round((r.FareHistory[^1] - r.FareHistory[0]) / (decimal)Math.Max(1, r.FareHistory[0]) * 100);
    }

    /// <summary>Request keys are the first three letters of the destination plus a running number, e.g. LIS-0001.</summary>
    public static string RequestKey(string destination, int id)
    {
        var word = destination.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "";
        var code = new string(word.Where(char.IsLetter).Take(3).ToArray()).ToUpperInvariant().PadRight(3, 'X');
        return $"{code}-{id:D4}";
    }

    private static List<TripRequest> GenerateRequests(int count)
    {
        // Each trip type goes only to places that offer it, and both itineraries of a type read true for every one of its places.
        var tripTypes = new (string Code, string Name, int Low, int High, string ItineraryA, string ItineraryB, string[] Places)[]
        {
            ("BEACH", "Beach & island resorts", 1800, 5200,
                "Seven nights in a sea-view room, two adults, late August",
                "Five nights by the beach with a snorkelling day trip",
                ["Santorini", "Amalfi Coast", "Cape Town", "Hoi An"]),
            ("CITY", "City breaks & culture", 600, 2400,
                "Long weekend with museum passes and a guided food tour",
                "Four nights near the old town with a cooking class for two",
                ["Lisbon", "Kyoto", "Marrakech", "Vancouver", "Reykjavik"]),
            ("SKI", "Ski & snowboard", 2200, 6800,
                "Ski-in ski-out chalet for six, lessons for two beginners",
                "Week of lift passes with a half-day guided off-piste tour",
                ["Banff", "Queenstown", "Vancouver", "Patagonia"]),
            ("SAFARI", "Safari & wildlife", 5500, 14500,
                "Four nights at a wildlife lodge with a guided dawn trek",
                "Whale and bird watching by boat, three nights at the lodge",
                ["Cape Town", "Patagonia", "Banff", "Reykjavik"]),
            ("CRUISE", "Ocean & river cruises", 2800, 9800,
                "Ten-night cruise, balcony cabin, all excursions included",
                "Coastal cruise for two with the drinks package",
                ["Santorini", "Amalfi Coast", "Lisbon", "Vancouver", "Reykjavik"]),
            ("RAIL", "Scenic rail journeys", 1500, 5400,
                "First-class rail pass with two overnight sleeper legs",
                "Scenic railway loop with three nights in small towns",
                ["Kyoto", "Banff", "Vancouver", "Hoi An"]),
            ("FAMILY", "Family holidays", 2400, 7600,
                "Half-term week with a kids' club and a family suite",
                "Theme park passes for four with a connecting-rooms hotel",
                ["Lisbon", "Vancouver", "Queenstown", "Cape Town", "Kyoto"]),
            ("HONEYMOON", "Honeymoons & celebrations", 4200, 12800,
                "Private villa with a sunset dinner and a couples' spa day",
                "Ten nights in two boutique hotels, champagne on arrival",
                ["Santorini", "Amalfi Coast", "Lisbon", "Hoi An", "Kyoto", "Marrakech"])
        };

        // 200 first names from many places; a first name may repeat, a surname may not
        var firstNames = new[]
        {
            "Clementine", "Barnaby", "Juniper", "Mortimer", "Petunia", "Thaddeus", "Winifred", "Archibald", "Lavender", "Ebenezer",
            "Delphine", "Rufus", "Octavia", "Bartholomew", "Cornelia", "Florian", "Hyacinth", "Ignatius", "Leopold", "Millicent",
            "Rosamund", "Silas", "Tobias", "Ursula", "Wilhelmina", "Augustus", "Beatrix", "Cuthbert", "Dorothea", "Edmund",
            "Felicity", "Gideon", "Henrietta", "Ottoline", "Quentin", "Seraphina", "Amara", "Kwame", "Zainab", "Chidi",
            "Nia", "Tendai", "Folake", "Kofi", "Imani", "Sefu", "Adaeze", "Thabo", "Wanjiru", "Jabari",
            "Ayodele", "Makena", "Oluwaseun", "Zuri", "Babajide", "Nkechi", "Priya", "Arjun", "Ananya", "Rohan",
            "Kavya", "Vikram", "Meera", "Aarav", "Ishaan", "Lakshmi", "Farhan", "Nadia", "Sanjay", "Divya",
            "Rahul", "Anjali", "Kiran", "Tara", "Dev", "Sunita", "Kenji", "Mei", "Hiroshi", "Yuna",
            "Jiho", "Lian", "Takumi", "Sakura", "Wei", "Haruka", "Minjun", "Xiaoling", "Daichi", "Soo-ah",
            "Ren", "Aiko", "Jun", "Hana", "Bao", "Yuki", "Linh", "Arief", "Siti", "Malia",
            "Thanh", "Dewi", "Kalani", "Rizal", "Intan", "Narong", "Ploy", "Bayani", "Mateo", "Valentina",
            "Santiago", "Camila", "Diego", "Lucía", "Joaquín", "Ximena", "Rafael", "Paloma", "Emilio", "Renata",
            "Tomás", "Inés", "Alejandro", "Catalina", "Bruno", "Marisol", "Gonzalo", "Itzel", "Layla", "Omar",
            "Yasmin", "Karim", "Leila", "Tariq", "Samira", "Rami", "Dalia", "Hassan", "Noor", "Faris",
            "Amira", "Youssef", "Zara", "Idris", "Astrid", "Lars", "Ingrid", "Sven", "Freya", "Matteo",
            "Giulia", "Luca", "Chiara", "Aurélie", "Étienne", "Margaux", "Henrik", "Saoirse", "Niamh", "Cormac",
            "Siobhan", "Declan", "Anouk", "Joris", "Katarzyna", "Mikołaj", "Zofia", "Dmitri", "Anastasia", "Elif",
            "Emre", "Selin", "Nikos", "Eleni", "Moana", "Tane", "Aroha", "Keoni", "Leilani", "Nalani",
            "Wiremu", "Ahanu", "Kai", "Harper", "Jasper", "Willa", "Felix", "Hazel", "Theo", "Ivy",
            "Rowan", "Maeve", "Otis", "Poppy", "Ezra", "Wren", "Arlo", "Iris", "Milo", "Esme"
        };
        var statuses = new[] { "New", "New", "Quoted", "Booked", "Declined" };
        var notes = new[]
        {
            "Flexible on dates by a few days", "Prefers a morning departure", "Repeat client, knows the agent",
            "Asked for a travel insurance quote", "Needs an accessible room", "Travelling with an infant"
        };

        var rnd = new Random(42);
        var list = new List<TripRequest>(count);

        // Every lead traveller has a surname no one else has: two requests under one surname would read as one family booking
        // twice. 40 starts by 25 endings make 1,000 made-up surnames, dealt out in a shuffled order; none is an agent's surname.
        var starts = new[]
        {
            "Thistle", "Goose", "Bumble", "Apple", "Feather", "Higgle", "Parsnip", "Wobble", "Pepper", "Puddle",
            "Butter", "Crumble", "Dapple", "Fiddle", "Ginger", "Marrow", "Kettle", "Nettle", "Pickle", "Quince",
            "Ripple", "Toffee", "Whistle", "Bramble", "Cobble", "Dimple", "Gumdrop", "Muffin", "Noodle", "Pebble",
            "Rumble", "Sprocket", "Tumble", "Waffle", "Biscuit", "Doodle", "Jingle", "Mumble", "Wiggle", "Pudding"
        };
        var endings = new[]
        {
            "wood", "bottom", "berry", "sby", "worth", "stone", "by", "ford", "field", "brook", "wick", "ton", "ridge",
            "more", "well", "dale", "shaw", "cott", "mere", "ley", "holt", "thorpe", "combe", "hurst", "gate"
        };
        var surnames = starts.SelectMany(st => endings.Select(en => st + en)).OrderBy(_ => rnd.Next()).ToArray();
        // Requests from the last 300 days, so the Dashboard always shows this month
        var startDate = DateTime.Today.AddDays(-300);

        for (int i = 1; i <= count; i++)
        {
            var agent = Agents[rnd.Next(Agents.Length)];
            var type = tripTypes[rnd.Next(tripTypes.Length)];
            var destination = type.Places[rnd.Next(type.Places.Length)];
            var quote = rnd.Next(type.Low / 10, type.High / 10) * 10m;
            var requested = startDate.AddMinutes(rnd.Next(0, 300 * 24 * 60));
            var travellers = type.Code switch { "FAMILY" => rnd.Next(3, 7), "HONEYMOON" => 2, _ => rnd.Next(1, 5) };

            // five weekly fare samples for the route; the last one is the quote
            var first = (int)(quote * (100 + rnd.Next(-12, 13)) / 100);
            var fares = new List<int> { first };
            for (int k = 1; k < 4; k++)
                fares.Add(first + (int)(((int)quote - first) * k / 4m) + rnd.Next(-150, 151));
            fares.Add((int)quote);
            int delta = (int)Math.Round((fares[4] - fares[0]) / (decimal)Math.Max(1, fares[0]) * 100);

            list.Add(new TripRequest
            {
                Id = i,
                Request = RequestKey(destination, i),
                Agent = agent.Name,
                AvatarBg = agent.Color,
                DateRequested = requested,
                Destination = destination,
                Traveller = $"{firstNames[rnd.Next(firstNames.Length)]} {surnames[(i - 1) % surnames.Length]}",
                Travellers = travellers,
                Departure = requested.Date.AddDays(rnd.Next(30, 200)),
                Nights = TripPackages.ForDestination(destination).Nights,
                TripType = type.Code,
                TripTypeName = type.Name,
                Itinerary = rnd.Next(2) == 0 ? type.ItineraryA : type.ItineraryB,
                Notes = notes[rnd.Next(notes.Length)],
                Quote = quote,
                Status = statuses[rnd.Next(statuses.Length)],
                FareHistory = fares,
                FareDeltaPercent = delta
            });
        }

        // LIS-0586 is the trip the Itinerary page lays out: a Lisbon honeymoon for two, booked by the signed-in agent and leaving
        // tomorrow, so it heads the Dashboard's departures and the Invoices page opens on it.
        if (list.Count >= 586)
        {
            var trip = list[585];
            trip.Request = "LIS-0586";
            trip.Destination = "Lisbon";
            trip.Traveller = $"Wei {surnames[585]}";
            trip.Travellers = 2;
            trip.Agent = SignedInAgent;
            trip.AvatarBg = AgentColor(SignedInAgent);
            trip.TripType = "HONEYMOON";
            trip.TripTypeName = "Honeymoons & celebrations";
            trip.Status = "Booked";
            trip.DateRequested = DateTime.Today.AddDays(-40);
            trip.Departure = DateTime.Today.AddDays(1);
            trip.Nights = 5;
            trip.Itinerary = "Five nights in Alfama with a day in Sintra, a fado dinner and a sunset sail on the Tagus";
            trip.Notes = "Celebrating their wedding; asked for a quiet room";
            trip.Quote = 6480;
            trip.FareHistory = [6120, 6260, 6350, 6420, 6480];
            trip.FareDeltaPercent = 6;
        }

        return list;
    }
}
