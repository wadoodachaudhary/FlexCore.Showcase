#!/usr/bin/env python3
"""Overlay course slides on the lip-synced instructor cut."""

from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont

sys.path.insert(0, str(Path(__file__).resolve().parent))
import build_all  # noqa: E402
from cut5_content import COURSE_CODE, SLIDES, VOICE, VOICE_RATE  # noqa: E402

TALK = Path("/tmp/pe5/talking.mp4")
AUDIO_DIR = Path("/tmp/pe5/audio")
WORK = Path("/tmp/pe5/comp")
COURSE = Path("/workspace/courses/prompt-engineering")
OUT = COURSE / "video" / "GA-PE-210-prompt-engineering-5min.mp4"
W, H = 1920, 1080


def probe(path: Path) -> float:
    return float(subprocess.check_output(
        ["ffprobe", "-v", "error", "-show_entries", "format=duration", "-of", "csv=p=0", str(path)],
        text=True,
    ).strip())


def fmt(seconds: float) -> str:
    m = int(seconds // 60)
    s = seconds % 60
    return f"{m:02d}:{s:05.2f}"


def lower_third() -> Path:
    img = Image.new("RGBA", (460, 78), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    d.rounded_rectangle((0, 0, 448, 70), 8, fill=(10, 22, 34, 210))
    d.rectangle((0, 0, 7, 70), fill=(166, 133, 52, 255))
    bold = ImageFont.truetype("/usr/share/fonts/truetype/macos/Inter-Bold.ttf", 15)
    reg = ImageFont.truetype("/usr/share/fonts/truetype/macos/Inter-Regular.ttf", 16)
    d.text((20, 10), "INSTRUCTOR", font=bold, fill=(166, 133, 52, 255))
    d.text((20, 36), "GovAI Academy  ·  GA-PE-210", font=reg, fill=(236, 232, 222, 255))
    dest = WORK / "lower.png"
    img.save(dest)
    return dest


def main() -> None:
    if not TALK.exists():
        raise SystemExit(f"missing {TALK}")
    WORK.mkdir(parents=True, exist_ok=True)
    durations = [probe(AUDIO_DIR / f"{s['id']}.mp3") for s in SLIDES]
    talk_dur = probe(TALK)
    audio_sum = sum(durations)
    # Keep slide cuts inside the talking-head timeline.
    scale = min(1.0, (talk_dur - 0.05) / audio_sum)
    durations = [d * scale for d in durations]
    print(f"talk {talk_dur:.2f}s audio {audio_sum:.2f}s scale {scale:.4f}")

    cards = []
    for i, slide in enumerate(SLIDES, start=1):
        card = build_all.render_card(slide, i, len(SLIDES))
        path = WORK / f"{slide['id']}.png"
        card.save(path)
        cards.append(path)

    lower = lower_third()
    seg_dir = WORK / "seg"
    seg_dir.mkdir(exist_ok=True)
    cursor = 0.0
    segments = []
    chapters = []
    for slide, dur, card in zip(SLIDES, durations, cards):
        seg = seg_dir / f"{slide['id']}.mp4"
        # Accurate seek so the mouth stays with the words.
        filt = (
            "[0:v]scale=760:1014,pad=1920:1080:0:33:color=0x0C1A28[bg];"
            "[1:v]format=rgba[card];"
            "[2:v]format=rgba[lower];"
            "[bg][card]overlay=548:18:format=auto[v1];"
            "[v1][lower]overlay=28:980:format=auto,format=yuv420p[v]"
        )
        cmd = [
            "ffmpeg", "-y",
            "-i", str(TALK),
            "-i", str(card),
            "-i", str(lower),
            "-ss", f"{cursor:.3f}",
            "-t", f"{dur:.3f}",
            "-filter_complex", filt,
            "-map", "[v]", "-map", "0:a",
            "-r", "25",
            "-c:v", "libx264", "-preset", "veryfast", "-crf", "20", "-pix_fmt", "yuv420p",
            "-c:a", "aac", "-b:a", "160k", "-ar", "48000", "-ac", "2",
            "-movflags", "+faststart",
            str(seg),
        ]
        print("segment", slide["id"], f"{cursor:.2f}+{dur:.2f}", flush=True)
        proc = subprocess.run(cmd, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE)
        if proc.returncode != 0:
            sys.stderr.write(proc.stderr.decode("utf-8", "replace")[-2000:])
            raise SystemExit(f"segment {slide['id']} failed")
        chapters.append({"id": slide["id"], "title": slide["title"], "start": cursor, "end": cursor + dur})
        segments.append(seg)
        cursor += dur

    lst = WORK / "list.txt"
    lst.write_text("".join(f"file '{p}'\n" for p in segments), encoding="utf-8")
    joined = WORK / "joined.mp4"
    subprocess.run(
        ["ffmpeg", "-y", "-f", "concat", "-safe", "0", "-i", str(lst), "-c", "copy", "-movflags", "+faststart", str(joined)],
        check=True, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE,
    )
    meta = [";FFMETADATA1", "title=GA-PE-210 Prompt Engineering — 5-minute pilot"]
    for ch in chapters:
        meta += [
            "[CHAPTER]",
            "TIMEBASE=1/1000",
            f"START={int(ch['start'] * 1000)}",
            f"END={int(ch['end'] * 1000)}",
            f"title=Slide {ch['id']}: {ch['title']}",
        ]
    meta_path = WORK / "chapters.txt"
    meta_path.write_text("\n".join(meta) + "\n", encoding="utf-8")
    OUT.parent.mkdir(parents=True, exist_ok=True)
    subprocess.run(
        ["ffmpeg", "-y", "-i", str(joined), "-i", str(meta_path), "-map_metadata", "1", "-codec", "copy", "-movflags", "+faststart", str(OUT)],
        check=True, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE,
    )
    final = probe(OUT)
    print(f"FINAL {OUT} {final:.2f}s")

    lines = [
        f"# Narration script — {COURSE_CODE} five-minute pilot",
        "",
        "Companion to `video/GA-PE-210-prompt-engineering-5min.mp4`.",
        "This is the review cut for look, voice, and lip-sync. The full thirty-minute lip-synced briefing is not in this file.",
        f"Voice: {VOICE} at {VOICE_RATE}. Male American English. Mouth movement is Wav2Lip driven by this soundtrack.",
        "Unclassified training. Fictional Harbor Town examples. Not a Department of Labor publication.",
        "",
        f"Duration: {fmt(final)} ({final:.1f} seconds).",
        "",
    ]
    vtt = ["WEBVTT", ""]
    t = 0.0
    for slide, dur in zip(SLIDES, durations):
        lines += [
            f"## Slide {slide['id']} — {slide['title']}",
            "",
            f"On screen {fmt(t)} – {fmt(t + dur)}",
            "",
            slide["narration"],
            "",
        ]
        vtt += [
            f"{fmt(t).replace('.', ',')} --> {fmt(t + dur).replace('.', ',')}",
            slide["narration"],
            "",
        ]
        t += dur
    script = COURSE / "narration" / "narration-script-5min.md"
    script.write_text("\n".join(lines), encoding="utf-8")
    (COURSE / "narration" / "captions-5min.vtt").write_text("\n".join(vtt), encoding="utf-8")
    (COURSE / "narration" / "timing-5min.json").write_text(
        json.dumps({"duration": final, "voice": VOICE, "rate": VOICE_RATE, "chapters": chapters}, indent=2),
        encoding="utf-8",
    )


if __name__ == "__main__":
    main()
