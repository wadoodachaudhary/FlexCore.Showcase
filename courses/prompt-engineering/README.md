# Prompt Engineering for Government Officers

GovAI Academy course **GA-PE-210**. Unclassified, instructor-led briefing and labs for officers whose work is non-technical or only partly technical.

The briefing is original academy material. It sits **above** the baseline in the U.S. Department of Labor AI Literacy Framework (Training and Employment Notice 07-25, February 13, 2026). It is not a Department of Labor publication.

## Five-minute pilot (review this first)

`video/GA-PE-210-prompt-engineering-5min.mp4` is the current review cut: **5:16** (ffprobe 315.8 seconds), 1920×1080, H.264 + AAC.

The instructor is a seated talking head built from the supplied reference photos (formal bow-tie look), with a **male** American English voice (`en-US-AndrewNeural`). Mouth movement is lip-synced to that soundtrack with Wav2Lip. Slides sit in front, with the instructor visible on the left. This is the cut to approve for look, voice, and lip-sync.

The full thirty-minute lip-synced briefing is **not** in this file. It will be produced after that approval. The earlier `video/GA-PE-210-prompt-engineering.mp4` is the previous static-instructor cut and is not the look to approve.

Companion script: `narration/narration-script-5min.md`.

A live facilitator still takes attendance and runs the labs. The video alone does not complete the course.

## File list

| File | What it is |
| --- | --- |
| `outline/course-outline.md` | Full instructor-led session: rules, agenda, attendance, three labs, rubrics |
| `slides/Prompt-Engineering-for-Government-Officers.pptx` | 16:9 deck. Speaker notes are the narration |
| `narration/narration-script.md` | Spoken script with timestamps matched to the video |
| `narration/captions.vtt` | Captions for the briefing |
| `narration/timing.json` | Slide start and end times in seconds |
| `video/GA-PE-210-prompt-engineering-5min.mp4` | **Pilot to review.** 1080p, 5:16 (315.8 s), 25 fps, H.264 + AAC, lip-synced male narration |
| `narration/narration-script-5min.md` | Spoken script for the five-minute pilot, with timestamps |
| `narration/captions-5min.vtt` | Captions for the pilot |
| `video/GA-PE-210-prompt-engineering.mp4` | Earlier 30:07 cut with a static illustrated instructor. Not the approved look |
| `seed/GovAiAcademy.PromptEngineering.seed.cs` | Catalog seed: title, levels, exercises, attendance |
| `seed/ADD-TO-CATALOG.md` | How to register the course as approved and instructor-led |
| `assets/instructor-portrait.png` | Illustrated instructor used in the video background |

A copy of this package is also placed under `artifacts/prompt-engineering/` for transfer to a laptop.

## How to open

**Outline and script.** Any markdown reader, or Word / a browser. Start with `outline/course-outline.md` if you are facilitating.

**Slides.** Microsoft PowerPoint, PowerPoint for the web, Keynote, or LibreOffice Impress. Open `slides/Prompt-Engineering-for-Government-Officers.pptx`. Use Notes view if you teach the deck live instead of playing the video. Fonts are Calibri, Georgia, and Consolas so a typical government Windows laptop can open the file without extra installs.

**Video.** Open `video/GA-PE-210-prompt-engineering-5min.mp4` in any current player. It is 1920×1080 H.264 with AAC audio. The seated instructor is on the left and speaks with the narration; the slide card sits in front with a soft shadow. Chapter markers follow the eight slides. Captions are in `narration/captions-5min.vtt`.

**Catalog.** Follow `seed/ADD-TO-CATALOG.md`. The course is instructor-led, approved in the seed, requires attendance, and does not treat the video alone as completion.

## Gov-safe use

Do not add classified information, controlled unclassified information, or personal data to the examples, the labs, or any practice tool. Worksheets use fictional Harbor Town facts. If your agency has not approved a tool for training, run the labs on paper.

## Rebuild (optional)

From this folder, with Python 3, `python-pptx`, Pillow, `edge-tts`, and `ffmpeg`:

```bash
python3 build/build_all.py
```

That regenerates the deck, the timestamped script, and the video.
