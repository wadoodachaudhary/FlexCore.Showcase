# Prompt Engineering for Government Officers

GovAI Academy course **GA-PE-210**. Unclassified, instructor-led briefing and labs for officers whose work is non-technical or only partly technical.

The briefing is original academy material. It sits **above** the baseline in the U.S. Department of Labor AI Literacy Framework (Training and Employment Notice 07-25, February 13, 2026). It is not a Department of Labor publication.

The video shows an **illustrated instructor in the background** and the **slides in the foreground**, with synthetic narration. A live facilitator still takes attendance and runs the labs.

## File list

| File | What it is |
| --- | --- |
| `outline/course-outline.md` | Full instructor-led session: rules, agenda, attendance, three labs, rubrics |
| `slides/Prompt-Engineering-for-Government-Officers.pptx` | 16:9 deck. Speaker notes are the narration |
| `narration/narration-script.md` | Spoken script with timestamps matched to the video |
| `narration/captions.vtt` | Captions for the briefing |
| `narration/timing.json` | Slide start and end times in seconds |
| `video/GA-PE-210-prompt-engineering.mp4` | 1080p briefing, 30:07 (1806.9 seconds by ffprobe), 24 fps, H.264 + AAC |
| `seed/GovAiAcademy.PromptEngineering.seed.cs` | Catalog seed: title, levels, exercises, attendance |
| `seed/ADD-TO-CATALOG.md` | How to register the course as approved and instructor-led |
| `assets/instructor-portrait.png` | Illustrated instructor used in the video background |

A copy of this package is also placed under `artifacts/prompt-engineering/` for transfer to a laptop.

## How to open

**Outline and script.** Any markdown reader, or Word / a browser. Start with `outline/course-outline.md` if you are facilitating.

**Slides.** Microsoft PowerPoint, PowerPoint for the web, Keynote, or LibreOffice Impress. Open `slides/Prompt-Engineering-for-Government-Officers.pptx`. Use Notes view if you teach the deck live instead of playing the video. Fonts are Calibri, Georgia, and Consolas so a typical government Windows laptop can open the file without extra installs.

**Video.** Any current player (Films & TV, VLC, QuickTime, the browser). The file is 1920×1080 H.264 with AAC audio. The instructor remains visible on the left; the slide card sits in front with a soft shadow. Chapter markers follow the slides. You can load `narration/captions.vtt` if the player does not pick them up automatically.

**Catalog.** Follow `seed/ADD-TO-CATALOG.md`. The course is instructor-led, approved in the seed, requires attendance, and does not treat the video alone as completion.

## Gov-safe use

Do not add classified information, controlled unclassified information, or personal data to the examples, the labs, or any practice tool. Worksheets use fictional Harbor Town facts. If your agency has not approved a tool for training, run the labs on paper.

## Rebuild (optional)

From this folder, with Python 3, `python-pptx`, Pillow, `edge-tts`, and `ffmpeg`:

```bash
python3 build/build_all.py
```

That regenerates the deck, the timestamped script, and the video.
