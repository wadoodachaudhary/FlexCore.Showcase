"""Builds the Showcase's airport snapshot from the OurAirports data dump (public domain, https://ourairports.com/data/).

Usage:  python3 build-snapshot.py <folder with airports.csv runways.csv countries.csv regions.csv> <snapshot date, e.g. 2026-10-26>
Writes airports.tsv and runways.tsv next to this script. Kept: airports with an IATA code and scheduled service whose type is
large, medium or small airport; their runways. Surfaces are folded into a few plain groups; the raw code is kept beside them.
"""
import csv, os, sys

src, date = sys.argv[1], sys.argv[2]
here = os.path.dirname(os.path.abspath(__file__))

def clean(v):
    return " ".join((v or "").replace("\t", " ").split())

countries = {r["code"]: clean(r["name"]) for r in csv.DictReader(open(f"{src}/countries.csv", encoding="utf-8"))}
regions = {r["code"]: clean(r["name"]) for r in csv.DictReader(open(f"{src}/regions.csv", encoding="utf-8"))}
types = {"large_airport": "Large", "medium_airport": "Medium", "small_airport": "Small"}

airports = []
for a in csv.DictReader(open(f"{src}/airports.csv", encoding="utf-8")):
    if not a["iata_code"].strip() or a["scheduled_service"] != "yes" or a["type"] not in types:
        continue
    airports.append(a)
by_ident = {a["ident"]: a for a in airports}

def surface_group(raw):
    s = raw.strip().upper()
    if s.startswith("WATER"): return "Water"
    if "/" in s or s in ("ASPH-CONC", "CON/ASP") or s.startswith(("PEM", "PER", "PAVED", "COM", "COP", "HARD")): return "Mixed paved"
    if s.startswith(("ASP", "BIT", "MAC")): return "Asphalt"
    if s.startswith("CON"): return "Concrete"
    if s.startswith(("GVL", "GRVL", "GRAVEL", "CRUSHED", "GRADED", "TREATED", "COR")): return "Gravel"
    if s.startswith(("GRS", "GRASS", "TURF", "SAND GRASS")): return "Grass"
    if s.startswith(("DIRT", "CLA", "SAN", "LAT", "UNPAVED")): return "Earth or sand"
    return "Unknown"

with open(os.path.join(here, "airports.tsv"), "w", encoding="utf-8", newline="\n") as f:
    f.write(f"# OurAirports snapshot {date}; public domain (https://ourairports.com/data/). Built by build-snapshot.py.\n")
    f.write("Code\tIcao\tName\tType\tCity\tCountryCode\tCountry\tContinent\tRegion\tLatitude\tLongitude\tElevationFt\tWikipedia\n")
    for a in sorted(airports, key=lambda a: a["iata_code"]):
        f.write("\t".join([
            clean(a["iata_code"]), clean(a["icao_code"] or a["gps_code"]), clean(a["name"]), types[a["type"]],
            clean(a["municipality"]), a["iso_country"], countries.get(a["iso_country"], a["iso_country"]), a["continent"],
            regions.get(a["iso_region"], ""), f'{float(a["latitude_deg"]):.4f}', f'{float(a["longitude_deg"]):.4f}',
            clean(a["elevation_ft"]), clean(a["wikipedia_link"])]) + "\n")

count = 0
with open(os.path.join(here, "runways.tsv"), "w", encoding="utf-8", newline="\n") as f:
    f.write(f"# OurAirports snapshot {date}; public domain (https://ourairports.com/data/). Built by build-snapshot.py.\n")
    f.write("AirportCode\tRunway\tLengthFt\tWidthFt\tSurface\tSurfaceCode\tLighted\tClosed\n")
    for r in csv.DictReader(open(f"{src}/runways.csv", encoding="utf-8")):
        a = by_ident.get(r["airport_ident"])
        if a is None:
            continue
        ends = "/".join(x for x in (clean(r["le_ident"]), clean(r["he_ident"])) if x)
        f.write("\t".join([clean(a["iata_code"]), ends, clean(r["length_ft"]), clean(r["width_ft"]),
                           surface_group(r["surface"]), clean(r["surface"]).upper(), r["lighted"].strip(), r["closed"].strip()]) + "\n")
        count += 1
print(f"{len(airports)} airports, {count} runways")
