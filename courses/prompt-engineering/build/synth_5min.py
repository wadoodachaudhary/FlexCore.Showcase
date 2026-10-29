#!/usr/bin/env python3
"""Expressive male narration for the five-minute pilot.

Sentences are spoken separately so pitch and pace can move, then joined
with a short breath. The base voice is en-US-GuyNeural.
"""

from __future__ import annotations

import asyncio
import re
import subprocess
import sys
from pathlib import Path

import edge_tts

sys.path.insert(0, str(Path(__file__).resolve().parent))
from cut5_content import SLIDES, VOICE  # noqa: E402

OUT = Path("/tmp/pe5/audio")
VOICE_NAME = VOICE

# One contour per slide, shifted so warnings sit lower and examples sit brighter.
# GuyNeural already moves pitch inside a paragraph; do not chop every sentence.
SLIDE_STYLE = {
    "01": ("+16%", "-4Hz"),
    "02": ("+14%", "-2Hz"),
    "03": ("+10%", "-8Hz"),
    "04": ("+16%", "-3Hz"),
    "05": ("+12%", "-6Hz"),
    "06": ("+18%", "-1Hz"),
    "07": ("+16%", "-3Hz"),
    "08": ("+12%", "-6Hz"),
}


def sentences(text: str) -> list[str]:
    parts = re.split(r"(?<=[.?!])\s+", text.strip())
    return [p.strip() for p in parts if p.strip()]


async def synth_slide(slide: dict) -> None:
    sid = slide["id"]
    rate, pitch = SLIDE_STYLE[sid]
    out = OUT / f"{sid}.mp3"
    await edge_tts.Communicate(slide["narration"], VOICE_NAME, rate=rate, pitch=pitch).save(str(out))
    print(f"  {sid} {rate} {pitch}", flush=True)


async def main() -> None:
    OUT.mkdir(parents=True, exist_ok=True)
    for slide in SLIDES:
        print("slide", slide["id"], flush=True)
        await synth_slide(slide)
    lst = OUT / "all.txt"
    lst.write_text("".join(f"file '{OUT / (s['id'] + '.mp3')}'\n" for s in SLIDES), encoding="utf-8")
    wav = Path("/tmp/pe5/narration.wav")
    subprocess.run(
        ["ffmpeg", "-y", "-f", "concat", "-safe", "0", "-i", str(lst), "-ar", "16000", "-ac", "1", str(wav)],
        check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL,
    )
    dur = subprocess.check_output(
        ["ffprobe", "-v", "error", "-show_entries", "format=duration", "-of", "csv=p=0", str(wav)],
        text=True,
    ).strip()
    print("NARRATION", dur, "seconds")


if __name__ == "__main__":
    asyncio.run(main())
