namespace RazorSharpGetaways.Data;

/// <summary>
/// A trip request at the quote desk: who asked, where to, the quote and its status. The Trip Requests grid edits these in place,
/// the Dashboard counts them, the Itinerary and Invoices pages open one.
/// </summary>
public sealed class TripRequest
{
    public int Id { get; set; }

    /// <summary>Request key: the destination's first three letters and a running number, e.g. LIS-0001.</summary>
    public string Request { get; set; } = "";

    public string Agent { get; set; } = "";

    /// <summary>Colour behind the agent's initials; one per agent.</summary>
    public string AvatarBg { get; set; } = "#0284c7";

    public DateTime DateRequested { get; set; }
    public string Destination { get; set; } = "";
    public string Traveller { get; set; } = "";
    public int Travellers { get; set; } = 2;
    public DateTime Departure { get; set; }

    /// <summary>Nights away: the destination package's length unless the booking overrides it (LIS-0586 is five).</summary>
    public int Nights { get; set; } = 7;

    public string TripType { get; set; } = "";
    public string TripTypeName { get; set; } = "";
    public string Itinerary { get; set; } = "";
    public string Notes { get; set; } = "";
    public decimal Quote { get; set; }
    public string Status { get; set; } = "New";

    /// <summary>Five weekly fare samples for the route; the last one is the quote.</summary>
    public List<int> FareHistory { get; set; } = new();

    /// <summary>Change from the first fare sample to the last, in whole per cent.</summary>
    public int FareDeltaPercent { get; set; }

    /// <summary>Editor copy, so Cancel leaves the row unchanged.</summary>
    public TripRequest Clone()
    {
        var copy = (TripRequest)MemberwiseClone();
        copy.FareHistory = new List<int>(FareHistory);
        return copy;
    }

    /// <summary>Copies the editor-changeable fields from an edited copy.</summary>
    public void CopyEditsFrom(TripRequest edited)
    {
        Agent = edited.Agent;
        AvatarBg = edited.AvatarBg;
        Destination = edited.Destination;
        Traveller = edited.Traveller;
        Travellers = edited.Travellers;
        TripType = edited.TripType;
        TripTypeName = edited.TripTypeName;
        Quote = edited.Quote;
        Status = edited.Status;
        DateRequested = edited.DateRequested;
        Itinerary = edited.Itinerary;
        Notes = edited.Notes;
    }
}

/// <summary>Trip-type lookup row for the grid's in-cell drop-down grid.</summary>
public sealed class TripTypeLookup
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Desk { get; set; } = "";
    public string LeadTime { get; set; } = "";
    public string Display => $"{Code} - {Name}";
}

/// <summary>An agent at the desk and the colour of their avatar.</summary>
public sealed record TravelAgent(string Name, string Color);
