# AI Atlas Sample Pack

An original, editable report collection for FlexCore Report Studio. This is a
demonstration dataset, not a commercial market report or investment forecast.

## Reports

1. **A brief history of intelligence**: seven sourced milestones, 1956-2022.
2. **The AI ecosystem**: an illustrative selection of research, model, platform
   and infrastructure organizations, not an exhaustive or ranked market map.
3. **Capability is a portfolio**: eight fictional models and three fictional
   benchmark suites. Names, context sizes and scores are invented; they do not
   describe real products, even where the organization name is real.
4. **From pilots to practice**: synthetic adoption and hours-saved figures for
   eight industries, 2022-2030. The year parameter changes detail data. The
   overview chart is explicitly a fixed 2026 baseline snapshot.
5. **The productivity dividend**: cautious, balanced and accelerated 2030
   scenarios in illustrative USD billions. Net value equals gross value less
   transition cost. These are not GDP, employment or investment predictions.
6. **The physical side of AI**: fictional sites, capacities and energy mixes.
   Organization names do not imply real commitments or facilities.

The report layouts, commentary, charts and synthetic records were authored for
this showcase. No proprietary third-party RPT file is redistributed. The sample
definitions are XML, not Crystal binary files. Uploaded genuine `.rpt` files
are converted by FlexCore's native C# converter.

## Data Model

`data/atlas.db` contains 12 tables with foreign keys:

| Table | Purpose |
| --- | --- |
| Sources | Provenance and synthetic-data notice |
| Countries | Country/region lookup |
| Organizations | Illustrative AI ecosystem |
| Milestones | Sourced historical events |
| Models | Fictional model profiles |
| Benchmarks | Fictional evaluation suites |
| ModelScores | Model-to-benchmark scores |
| Industries | Use cases and adoption constraints |
| AdoptionMetrics | Industry/year scenarios |
| Scenarios | Economic assumptions |
| EconomicImpacts | Industry/scenario gross, cost and net value |
| ComputeSites | Fictional capacity portfolio |

Six joined views expose the data to the reports. The web app opens SQLite
read-only and rejects changes, attachments, external databases and unapproved
functions. The public sample database contains no secrets or private uploads.
The database is created by an offline seed tool, never migrated by the web app.

## Historical Sources

Sources were checked on 2026-09-17. Only the historical report uses these as
evidence. They do not support the fictional metrics elsewhere in the pack.

- The 1956 research meeting: [Dartmouth](https://home.dartmouth.edu/about/artificial-intelligence-ai-coined-dartmouth).
- The 1997 chess match: [IBM's Deep Blue history](https://www.ibm.com/history/deep-blue).
- The 2012 vision paper: [Krizhevsky, Sutskever and Hinton, NeurIPS](https://papers.nips.cc/paper_files/paper/2012/hash/c399862d3b9d6b76c8436e924a68c45b-Abstract.html).
- The 2016 Go match: [Google DeepMind's AlphaGo research](https://deepmind.google/research/alphago/).
- The 2017 architecture paper: [Vaswani et al., Attention Is All You Need](https://arxiv.org/abs/1706.03762).
- The 2020 protein-folding result: [Google DeepMind's AlphaFold announcement](https://deepmind.google/blog/alphafold-a-solution-to-a-50-year-old-grand-challenge-in-biology/).
- The 2022 conversational research preview: [OpenAI's ChatGPT introduction](https://openai.com/index/chatgpt/).

## Workspace Behavior

Public samples are immutable. The designer edits private temporary copies.
Save retains the working copy for this page's lifetime; Download XML keeps an
editable self-contained copy. Uploads accept RPT/XML up to 32 MB each, six per
selection, twelve per workspace. Leaving the page cleans up its owned files.

An uploaded business report does not magically acquire an AI database schema.
It opens in explicit **Layout only** mode with no detail rows. Use the Database
Expert to map it to the sample schema, then select **AI Atlas SQLite** to run.
Native conversion never searches for a corresponding previously converted XML.

## Rebuilding The Pack

From the JavaToCSharp workspace:

```sh
dotnet run --project tools/ReportAtlas.Seed -- /path/to/FlexCore.Showcase/wwwroot/reports/ai-atlas
dotnet run --project tools/ReportAtlas.Tests
```

The seed tool replaces only its generated database, catalog and six definitions.
First-page PNG previews are captures of actual rendered baseline reports.
