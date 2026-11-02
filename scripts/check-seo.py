#!/usr/bin/env python3
"""Checks the page registry (Seo/SiteMeta.Pages.cs) against the pages (2026-10-29).

Every @page route must be a registry path or alias; every registry path or alias must be a route; each page's <PageTitle>
must equal its registry title; indexed pages need a description of 70-160 characters and a title of at most 60.
Run from the project root: python3 scripts/check-seo.py
"""
import pathlib, re, sys

root = pathlib.Path(__file__).resolve().parent.parent
registry = (root / 'Seo/SiteMeta.Pages.cs').read_text()
entries = []
for m in re.finditer(r'new\("([^"]*)",\s*"(?:[^"\\]|\\.)*",\s*"((?:[^"\\]|\\.)*)",\s*"((?:[^"\\]|\\.)*)",\s*"([^"]*)"(.*?)\),?\n', registry, re.S):
    path, title, desc, section, rest = m.groups()
    aliases = re.findall(r'"([^"]+)"', (re.search(r'Aliases:\s*new\[\]\s*\{([^}]*)\}', rest) or [None, ''])[1] if 'Aliases:' in rest else '')
    entries.append(dict(path=path, title=title.replace('\\"', '"'), desc=desc.replace('\\"', '"'), section=section,
                        aliases=aliases, index='Index: false' not in rest))

pages = {}
for f in sorted(root.glob('Components/Pages/**/*.razor')):
    text = f.read_text()
    routes = [r.strip('/') for r in re.findall(r'^@page "([^"]+)"', text, re.M)]
    if not routes:
        continue
    title = re.search(r'<PageTitle>(.*?)</PageTitle>', text, re.S)
    pages[f] = (routes, title.group(1).strip().replace('&amp;', '&') if title else None)

problems = []
known = {}
for e in entries:
    for p in [e['path']] + e['aliases']:
        if p in known:
            problems.append(f"registry: {p!r} listed twice")
        known[p] = e
for f, (routes, title) in pages.items():
    rel = f.relative_to(root)
    for r in routes:
        if r not in known:
            problems.append(f"{rel}: route {r!r} is not in the registry")
    e = known.get(routes[0]) or next((known[r] for r in routes if r in known), None)
    if e and title is not None and title != e['title']:
        problems.append(f"{rel}: <PageTitle> {title!r} != registry {e['title']!r}")
all_routes = {r for routes, _ in pages.values() for r in routes}
for p in known:
    if p not in all_routes:
        problems.append(f"registry: {p!r} is not a route of any page")
for e in entries:
    if not e['index']:
        continue
    if not 70 <= len(e['desc']) <= 160:
        problems.append(f"{e['path'] or '/'}: description is {len(e['desc'])} characters (want 70-160)")
    if len(e['title']) > 60:
        problems.append(f"{e['path'] or '/'}: title is {len(e['title'])} characters (want <= 60)")
    if re.search(r'\b(free|blazing|seamless|robust|powerful|cutting-edge|effortless|unleash|elevate|supercharge)\b', (e['title'] + ' ' + e['desc']), re.I):
        problems.append(f"{e['path'] or '/'}: wording to avoid in title/description")

for f in root.glob('Components/**/*.razor'):
    if f.name != 'SiteHead.razor' and '<HeadContent' in f.read_text():
        problems.append(f"{f.relative_to(root)}: has its own <HeadContent>, which would replace SiteHead's tags (Blazor keeps the last one)")

print(f"{len(entries)} registry entries, {len(pages)} pages, {len(all_routes)} routes")
for p in problems:
    print('  ' + p)
sys.exit(1 if problems else 0)
