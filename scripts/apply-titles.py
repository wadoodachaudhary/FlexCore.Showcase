#!/usr/bin/env python3
"""Writes each page's <PageTitle> from the registry (Seo/SiteMeta.Pages.cs) so the title tag and the registry cannot drift.
Skips files given as arguments (another session may be editing them). Run from the project root."""
import pathlib, re, sys, html

root = pathlib.Path(__file__).resolve().parent.parent
registry = (root / 'Seo/SiteMeta.Pages.cs').read_text()
titles = {}
for m in re.finditer(r'new\("([^"]*)",\s*"(?:[^"\\]|\\.)*",\s*"((?:[^"\\]|\\.)*)",\s*"(?:[^"\\]|\\.)*",\s*"[^"]*"(.*?)\),?\n', registry, re.S):
    path, title, rest = m.groups()
    title = title.replace('\\"', '"')
    titles[path] = title
    for a in re.findall(r'"([^"]+)"', (re.search(r'Aliases:\s*new\[\]\s*\{([^}]*)\}', rest) or [None, ''])[1] if 'Aliases:' in rest else ''):
        titles[a] = title
skip = {root / s for s in sys.argv[1:]}
changed = 0
for f in sorted(root.glob('Components/Pages/**/*.razor')):
    if f in skip:
        continue
    text = f.read_text()
    routes = [r.strip('/') for r in re.findall(r'^@page "([^"]+)"', text, re.M)]
    if not routes or routes[0] not in titles:
        continue
    new_title = html.escape(titles[routes[0]], quote=False)   # & -> &amp; as the pages already write it
    new = re.sub(r'<PageTitle>.*?</PageTitle>', f'<PageTitle>{new_title}</PageTitle>', text, count=1, flags=re.S)
    if new != text:
        f.write_text(new); changed += 1; print(f'  {f.relative_to(root)}: {titles[routes[0]]}')
print(f'{changed} files changed')
