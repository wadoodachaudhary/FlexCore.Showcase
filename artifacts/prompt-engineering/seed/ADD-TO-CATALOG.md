# Add GA-PE-210 to the GovAI Academy catalog

The Blazor catalog pattern for an approved instructor-led course is:

- **Title** on the course record
- **Levels** (this offering is Applied; the prerequisite is Baseline literacy)
- **Exercises** required for completion
- **Attendance** required, with video-only completion turned off

## 1. Copy the seed

Copy `GovAiAcademy.PromptEngineering.seed.cs` into the academy project, for example:

`GovAiAcademy/Catalog/Seed/GovAiAcademy.PromptEngineering.seed.cs`

If `CatalogCourse` and the related types already exist, delete the contract block at the bottom of the file (everything after the seed class) and adjust property names only where your types differ. Keep these values:

| Field | Value |
| --- | --- |
| Title | Prompt Engineering for Government Officers |
| Code | GA-PE-210 |
| Delivery | InstructorLed |
| Approval | Approved |
| Primary level | Applied |
| Prerequisite | BaselineLiteracy (DOL TEN 07-25 floor) |
| RequiresAttendance | true |
| VideoOnlyCompletes | false |
| Exercises | three required worksheets |
| Classified / CUI / personal data | all false |

## 2. Register it where the catalog is built

In the seeder or `Program.cs` path that already loads approved courses:

```csharp
using GovAiAcademy.Catalog.Seed;

catalog.Upsert(PromptEngineeringCourseSeed.Create());
```

Use the project's existing add/upsert method if it is not named `Upsert`. The course should land in the approved instructor-led list, not in drafts.

## 3. Point materials at the package

The seed lists paths relative to this course folder:

- `slides/Prompt-Engineering-for-Government-Officers.pptx`
- `video/GA-PE-210-prompt-engineering.mp4`
- `narration/narration-script.md`
- `outline/course-outline.md`

Host those files wherever the academy stores course media, and keep the same titles in the catalog so section leads can find them.

## 4. Confirm the gates before you publish the tile

- Delivery is instructor-led.
- Attendance checkpoints are opening, start of Lab 1, and dismissal.
- Completion stays blocked until the three exercises are recorded.
- The catalog blurb states that exercises are fictional and that classified, controlled, and personal data are out of scope.
