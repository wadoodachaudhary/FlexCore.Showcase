#!/usr/bin/env python3
"""Build the GA-PE-210 deck, narration script, and 1080p briefing video."""

from __future__ import annotations

import argparse
import asyncio
import json
import subprocess
import sys
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter, ImageFont
from pptx import Presentation
from pptx.dml.color import RGBColor
from pptx.enum.shapes import MSO_SHAPE
from pptx.enum.text import MSO_ANCHOR, PP_ALIGN
from pptx.oxml.ns import qn
from pptx.util import Emu, Inches, Pt

sys.path.insert(0, str(Path(__file__).resolve().parent))
from course_content import COURSE_CODE, COURSE_TITLE, SLIDES, VOICE, VOICE_RATE  # noqa: E402

COURSE = Path("/workspace/courses/prompt-engineering")
ART_DIRS = [
    Path("/workspace/artifacts/prompt-engineering"),
    Path("/opt/cursor/artifacts/prompt-engineering"),
]
WORK = Path("/tmp/pe-course")
PORTRAIT = Path("/opt/cursor/artifacts/assets/instructor-portrait.png")
if not PORTRAIT.exists():
    PORTRAIT = COURSE / "assets" / "instructor-portrait.png"

W, H = 1920, 1080
FPS = 24
CARD_X, CARD_Y = 628, 28

NAVY = (14, 32, 48)
INK = (27, 40, 50)
GOLD = (166, 133, 52)
CREAM = (246, 243, 236)
MUTED = (78, 90, 102)
ALERT = (132, 54, 44)
TEAL = (24, 102, 98)
WHITE = (255, 255, 255)
RULE = (214, 206, 192)

FONT_DIR_INTER = Path("/usr/share/fonts/truetype/macos")
FONT_SERIF = Path("/usr/share/fonts/truetype/noto")
FONT_MONO = Path("/usr/share/fonts/truetype/jetbrains-mono/JetBrainsMono-Regular.ttf")


def font(path: Path, size: int) -> ImageFont.FreeTypeFont:
    return ImageFont.truetype(str(path), size)


F_SERIF = FONT_SERIF / "NotoSerif-Bold.ttf"
F_SERIF_REG = FONT_SERIF / "NotoSerif-Regular.ttf"
F_SANS = FONT_DIR_INTER / "Inter-Regular.ttf"
F_SANS_MED = FONT_DIR_INTER / "Inter-Medium.ttf"
F_SANS_BOLD = FONT_DIR_INTER / "Inter-Bold.ttf"


def wrap(draw: ImageDraw.ImageDraw, text: str, face: ImageFont.FreeTypeFont, max_w: int) -> list[str]:
    words = text.split()
    if not words:
        return []
    lines: list[str] = []
    cur = words[0]
    for word in words[1:]:
        trial = f"{cur} {word}"
        if draw.textlength(trial, font=face) <= max_w:
            cur = trial
        else:
            lines.append(cur)
            cur = word
    lines.append(cur)
    return lines


def text_h(lines: list[str], face: ImageFont.FreeTypeFont, gap: int) -> int:
    if not lines:
        return 0
    ascent, descent = face.getmetrics()
    line_h = ascent + descent + gap
    return line_h * len(lines) - gap


def draw_lines(draw, lines, face, xy, fill, gap, max_lines=None):
    x, y = xy
    ascent, descent = face.getmetrics()
    line_h = ascent + descent + gap
    shown = lines if max_lines is None else lines[:max_lines]
    for i, line in enumerate(shown):
        draw.text((x, y + i * line_h), line, font=face, fill=fill)
    return y + line_h * len(shown)


def make_plate(src: Path) -> Image.Image:
    """1920x1080 plate with the instructor on the left and a quieter right side."""
    im = Image.open(src).convert("RGB")
    scaled_w = 2560
    scaled_h = int(scaled_w * im.height / im.width)
    im = im.resize((scaled_w, scaled_h), Image.Resampling.LANCZOS)
    # Skin-box center measured on the source portrait (1280-wide): x=472.
    face_x = 472 / 1280 * scaled_w
    crop_x = int(face_x - 390)
    crop_y = int((335 / 720 * scaled_h) - 460)
    crop_x = max(0, min(crop_x, scaled_w - W))
    crop_y = max(0, min(crop_y, scaled_h - H))
    plate = im.crop((crop_x, crop_y, crop_x + W, crop_y + H)).convert("RGB")

    # Soft navy wash, stronger on the right so the slide stays dominant.
    wash = Image.new("L", (W, 1))
    for x in range(W):
        t = max(0.0, (x - 280) / (W - 280))
        wash.putpixel((x, 0), int(255 * min(1.0, t ** 0.85)))
    wash = wash.resize((W, H))
    navy = Image.new("RGB", (W, H), (8, 20, 32))
    darkened = Image.composite(navy, plate, wash)
    # Keep the instructor column readable: blend back some of the original on the left.
    left_keep = Image.new("L", (W, 1))
    for x in range(W):
        # 0 = original, 255 = darkened
        if x < 460:
            v = int(40 + 80 * (x / 460))
        else:
            v = 255
        left_keep.putpixel((x, 0), v)
    left_keep = left_keep.resize((W, H))
    plate = Image.composite(darkened, plate, left_keep)

    vignette = Image.new("L", (W, H), 0)
    vd = ImageDraw.Draw(vignette)
    vd.rectangle((0, 0, W, H), fill=0)
    # slight bottom gradient via a translucent bar
    bar = Image.new("RGBA", (W, 160), (6, 14, 22, 0))
    bd = ImageDraw.Draw(bar)
    for i in range(160):
        bd.line((0, i, W, i), fill=(6, 14, 22, int(150 * (i / 160))))
    out = plate.convert("RGBA")
    out.alpha_composite(bar, (0, H - 160))
    return out.convert("RGB")


def render_card(slide: dict, index: int, total: int) -> Image.Image:
    cw, ch = 1252, 1024
    pad = 28
    canvas = Image.new("RGBA", (cw + pad * 2, ch + pad * 2), (0, 0, 0, 0))
    shadow = Image.new("RGBA", canvas.size, (0, 0, 0, 0))
    sd = ImageDraw.Draw(shadow)
    sd.rounded_rectangle((pad + 10, pad + 16, pad + 10 + cw, pad + 16 + ch), 22, fill=(0, 0, 0, 130))
    shadow = shadow.filter(ImageFilter.GaussianBlur(18))
    canvas = Image.alpha_composite(canvas, shadow)

    card = Image.new("RGBA", canvas.size, (0, 0, 0, 0))
    d = ImageDraw.Draw(card)
    x0, y0 = pad, pad
    x1, y1 = pad + cw, pad + ch
    rail = ALERT if slide["layout"] == "alert" else GOLD if slide["layout"] in {"title", "close", "section"} else NAVY
    d.rounded_rectangle((x0, y0, x1, y1), 18, fill=CREAM + (255,))
    d.rounded_rectangle((x0, y0, x0 + 12, y1), 6, fill=rail + (255,))
    d.rectangle((x0 + 6, y0, x0 + 12, y1), fill=rail + (255,))

    kicker_f = font(F_SANS_BOLD, 18)
    title_f = font(F_SERIF, 40 if slide["layout"] != "title" else 48)
    if slide["layout"] == "title":
        title_f = font(F_SERIF, 46)
    body_f = font(F_SANS, 26)
    body_b = font(F_SANS_MED, 26)
    small = font(F_SANS, 18)
    mono = font(FONT_MONO, 18)
    mono_b = font(FONT_MONO, 17)

    inner_l = x0 + 40
    inner_r = x1 - 36
    max_w = inner_r - inner_l
    y = y0 + 32

    kicker = slide["kicker"].upper()
    d.text((inner_l, y), kicker, font=kicker_f, fill=GOLD if slide["layout"] != "alert" else ALERT)
    y += 32
    d.line((inner_l, y, inner_l + 84, y), fill=GOLD, width=3)
    y += 18

    title_lines = wrap(d, slide["title"], title_f, max_w)
    y = draw_lines(d, title_lines, title_f, (inner_l, y), INK, 6)
    y += 8

    if slide.get("subtitle"):
        sub_f = font(F_SERIF_REG, 26)
        sub_lines = wrap(d, slide["subtitle"], sub_f, max_w)
        y = draw_lines(d, sub_lines, sub_f, (inner_l, y), MUTED, 4)
        y += 8

    layout = slide["layout"]
    if layout == "split":
        y += 10
        gap = 16
        col_w = (max_w - gap) // 2
        panels = [
            (slide["left_title"], slide["left_body"], (236, 228, 220), ALERT),
            (slide["right_title"], slide["right_body"], (226, 236, 232), TEAL),
        ]
        for i, (title, body, bg, accent) in enumerate(panels):
            px = inner_l + i * (col_w + gap)
            py = y
            ph = 430
            d.rounded_rectangle((px, py, px + col_w, py + ph), 12, fill=bg + (255,))
            d.rectangle((px, py, px + 8, py + ph), fill=accent + (255,))
            tf = font(F_SANS_BOLD, 22)
            bf = font(F_SANS, 24)
            d.text((px + 22, py + 18), title, font=tf, fill=accent)
            lines = wrap(d, body, bf, col_w - 44)
            draw_lines(d, lines, bf, (px + 22, py + 64), INK, 6)
        y += 450
    elif layout == "prompt":
        y += 6
        prompt = slide.get("prompt", "")
        # wrap each logical line
        wrapped: list[str] = []
        for raw in prompt.split("\n"):
            wrapped.extend(wrap(d, raw, mono_b, max_w - 48) or [""])
        line_h = mono_b.getmetrics()[0] + mono_b.getmetrics()[1] + 6
        box_h = 28 + line_h * len(wrapped)
        d.rounded_rectangle((inner_l, y, inner_r, y + box_h), 12, fill=NAVY + (255,))
        ty = y + 16
        for line in wrapped:
            label, _, rest = line.partition(":")
            if rest and label in {"Role", "Context", "Task", "Constraints", "Output", "Source", "Reader"} and line.startswith(label):
                d.text((inner_l + 20, ty), label + ":", font=mono_b, fill=GOLD)
                lw = d.textlength(label + ": ", font=mono_b)
                d.text((inner_l + 20 + lw, ty), rest[1:] if rest.startswith(" ") else rest, font=mono_b, fill=(236, 232, 222))
            else:
                d.text((inner_l + 20, ty), line, font=mono_b, fill=(236, 232, 222))
            ty += line_h
        y += box_h + 16
    else:
        y += 8
        bullets = slide.get("bullets") or []
        bfont = font(F_SANS, 28 if len(bullets) <= 4 else 26)
        for bullet in bullets:
            lines = wrap(d, bullet, bfont, max_w - 36)
            ascent, descent = bfont.getmetrics()
            d.rounded_rectangle((inner_l, y + 10, inner_l + 10, y + 20), 2, fill=GOLD + (255,))
            y = draw_lines(d, lines, bfont, (inner_l + 28, y), INK, 5)
            y += 14

    callout = slide.get("callout")
    if callout:
        cf = font(F_SANS_MED, 22)
        lines = wrap(d, callout, cf, max_w - 36)
        box_h = 22 + text_h(lines, cf, 4) + 8
        cy = min(y + 12, y1 - 78 - box_h)
        fill = (248, 236, 230, 255) if layout == "alert" else (236, 230, 214, 255)
        accent = ALERT if layout == "alert" else GOLD
        d.rounded_rectangle((inner_l, cy, inner_r, cy + box_h), 10, fill=fill)
        d.rectangle((inner_l, cy, inner_l + 8, cy + box_h), fill=accent + (255,))
        draw_lines(d, lines, cf, (inner_l + 22, cy + 12), INK, 4)

    footer = f"{COURSE_CODE}   ·   Unclassified training   ·   Not a Department of Labor publication   ·   {index} / {total}"
    d.line((inner_l, y1 - 48, inner_r, y1 - 48), fill=RULE, width=1)
    d.text((inner_l, y1 - 38), footer, font=small, fill=MUTED)

    canvas = Image.alpha_composite(canvas, card)
    return canvas


def render_lower_third() -> Image.Image:
    img = Image.new("RGBA", (520, 92), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    d.rounded_rectangle((0, 0, 500, 84), 10, fill=(10, 22, 34, 210))
    d.rectangle((0, 0, 8, 84), fill=GOLD + (255,))
    d.text((22, 12), "ILLUSTRATED INSTRUCTOR", font=font(F_SANS_BOLD, 16), fill=GOLD)
    d.text((22, 40), "Synthetic narration  ·  GovAI Academy", font=font(F_SANS, 18), fill=(236, 232, 222))
    return img


def composite_preview(plate: Image.Image, card: Image.Image, lower: Image.Image) -> Image.Image:
    frame = plate.convert("RGBA")
    frame.alpha_composite(card, (CARD_X - 28, CARD_Y - 28))
    frame.alpha_composite(lower, (36, H - 118))
    return frame.convert("RGB")


def build_background_loop(plate: Image.Image, dest: Path) -> None:
    large = plate.resize((2304, 1296), Image.Resampling.LANCZOS)
    src = WORK / "plate_large.png"
    large.save(src, quality=95)
    # 8-second subtle Ken Burns plus a slow vertical drift. Looped under the slides.
    frames = 8 * FPS
    vf = (
        f"zoompan="
        f"z='1.04+0.025*sin(2*PI*on/{frames})':"
        f"x='(iw-iw/zoom)/2+18*sin(2*PI*on/{frames})':"
        f"y='(ih-ih/zoom)/2+10*sin(2*PI*on/{frames // 2})':"
        f"d={frames}:s={W}x{H}:fps={FPS},"
        f"format=yuv420p"
    )
    cmd = [
        "ffmpeg", "-y", "-loop", "1", "-i", str(src),
        "-vf", vf, "-t", "8",
        "-c:v", "libx264", "-preset", "veryfast", "-crf", "20",
        "-pix_fmt", "yuv420p", "-r", str(FPS),
        str(dest),
    ]
    subprocess.run(cmd, check=True, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE)


def _set_run(paragraph, text, face, size, color, bold=False):
    paragraph.clear()
    run = paragraph.add_run()
    run.text = text
    run.font.name = face
    run.font.size = Pt(size)
    run.font.bold = bold
    run.font.color.rgb = RGBColor(*color)
    # Force latin typeface on both major/minor so PowerPoint does not substitute oddly.
    rpr = run._r.get_or_add_rPr()
    for tag in ("latin", "ea", "cs"):
        el = rpr.find(qn(f"a:{tag}"))
        if el is None:
            el = rpr.makeelement(qn(f"a:{tag}"), {})
            rpr.append(el)
        el.set("typeface", face)


def add_textbox(slide, l, t, w, h, text, face, size, color, bold=False, align=PP_ALIGN.LEFT):
    box = slide.shapes.add_textbox(Inches(l), Inches(t), Inches(w), Inches(h))
    tf = box.text_frame
    tf.word_wrap = True
    p = tf.paragraphs[0]
    p.alignment = align
    _set_run(p, text, face, size, color, bold)
    return box


def add_para(tf, text, face, size, color, bold=False, space_before=6, level=0):
    p = tf.add_paragraph()
    p.level = level
    p.space_before = Pt(space_before)
    p.alignment = PP_ALIGN.LEFT
    _set_run(p, text, face, size, color, bold)
    return p


def build_pptx(path: Path) -> None:
    prs = Presentation()
    prs.slide_width = Inches(13.333)
    prs.slide_height = Inches(7.5)
    prs.core_properties.title = COURSE_TITLE
    prs.core_properties.subject = f"{COURSE_CODE} unclassified instructor-led briefing"
    prs.core_properties.category = "Unclassified training"
    prs.core_properties.keywords = "prompt engineering, government officers, AI literacy, GA-PE-210"
    prs.core_properties.author = "GovAI Academy"
    blank = prs.slide_layouts[6]
    total = len(SLIDES)

    for i, slide in enumerate(SLIDES, start=1):
        s = prs.slides.add_slide(blank)
        # cream background
        bg = s.shapes.add_shape(MSO_SHAPE.RECTANGLE, 0, 0, prs.slide_width, prs.slide_height)
        bg.line.fill.background()
        bg.fill.solid()
        bg.fill.fore_color.rgb = RGBColor(*CREAM)
        rail_color = ALERT if slide["layout"] == "alert" else NAVY
        rail = s.shapes.add_shape(MSO_SHAPE.RECTANGLE, 0, 0, Inches(0.12), prs.slide_height)
        rail.line.fill.background()
        rail.fill.solid()
        rail.fill.fore_color.rgb = RGBColor(*rail_color)
        gold = s.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.48), Inches(0.92), Inches(0.7), Inches(0.045))
        gold.line.fill.background()
        gold.fill.solid()
        gold.fill.fore_color.rgb = RGBColor(*GOLD)

        add_textbox(
            s, 0.48, 0.38, 12.2, 0.36,
            slide["kicker"].upper(), "Calibri", 14, GOLD, True,
        )
        add_textbox(s, 0.48, 1.05, 12.3, 1.35, slide["title"], "Georgia", 32, INK, True)
        top = 2.45
        if slide.get("subtitle"):
            add_textbox(s, 0.48, 2.35, 12.2, 0.5, slide["subtitle"], "Georgia", 18, MUTED, False)
            top = 2.95

        if slide["layout"] == "split":
            for col, (title, body, accent) in enumerate([
                (slide["left_title"], slide["left_body"], ALERT),
                (slide["right_title"], slide["right_body"], TEAL),
            ]):
                left = 0.48 + col * 6.3
                panel = s.shapes.add_shape(MSO_SHAPE.ROUNDED_RECTANGLE, Inches(left), Inches(top), Inches(6.0), Inches(3.3))
                panel.line.fill.background()
                panel.fill.solid()
                panel.fill.fore_color.rgb = RGBColor(236, 228, 220) if col == 0 else RGBColor(226, 236, 232)
                add_textbox(s, left + 0.25, top + 0.2, 5.5, 0.4, title, "Calibri", 16, accent, True)
                add_textbox(s, left + 0.25, top + 0.75, 5.5, 2.3, body, "Calibri", 18, INK, False)
        elif slide["layout"] == "prompt":
            panel = s.shapes.add_shape(
                MSO_SHAPE.ROUNDED_RECTANGLE, Inches(0.48), Inches(top), Inches(12.35), Inches(3.55)
            )
            panel.line.fill.background()
            panel.fill.solid()
            panel.fill.fore_color.rgb = RGBColor(*NAVY)
            box = s.shapes.add_textbox(Inches(0.7), Inches(top + 0.18), Inches(11.95), Inches(3.2))
            tf = box.text_frame
            tf.word_wrap = True
            first = True
            for line in slide["prompt"].split("\n"):
                if first:
                    p = tf.paragraphs[0]
                    first = False
                else:
                    p = tf.add_paragraph()
                p.space_before = Pt(6)
                _set_run(p, line, "Consolas", 13, (236, 232, 222), False)
        else:
            box = s.shapes.add_textbox(Inches(0.48), Inches(top), Inches(12.3), Inches(3.6))
            tf = box.text_frame
            tf.word_wrap = True
            bullets = slide.get("bullets") or []
            if bullets:
                tf.paragraphs[0].text = ""
                # clear default empty by writing first bullet into paragraph 0
                first = True
                for b in bullets:
                    if first:
                        p = tf.paragraphs[0]
                        first = False
                    else:
                        p = tf.add_paragraph()
                    p.level = 0
                    p.space_before = Pt(10)
                    _set_run(p, "▸  " + b, "Calibri", 22, INK, False)

        if slide.get("callout"):
            bar = s.shapes.add_shape(MSO_SHAPE.ROUNDED_RECTANGLE, Inches(0.48), Inches(6.15), Inches(12.35), Inches(0.62))
            bar.line.fill.background()
            bar.fill.solid()
            bar.fill.fore_color.rgb = RGBColor(248, 236, 230) if slide["layout"] == "alert" else RGBColor(236, 230, 214)
            add_textbox(s, 0.7, 6.25, 12.0, 0.45, slide["callout"], "Calibri", 15, INK, True)

        footer = f"{COURSE_CODE}   ·   Unclassified training   ·   Not a Department of Labor publication   ·   {i} / {total}"
        add_textbox(s, 0.48, 6.95, 12.3, 0.32, footer, "Calibri", 12, MUTED, False)

        notes = s.notes_slide.notes_text_frame
        notes.text = slide["narration"]

    path.parent.mkdir(parents=True, exist_ok=True)
    prs.save(path)


async def _synth_one(sem, slide, dest: Path, rate: str, attempts: int = 4) -> None:
    import edge_tts
    async with sem:
        last = None
        for n in range(attempts):
            try:
                comm = edge_tts.Communicate(slide["narration"], VOICE, rate=rate)
                await comm.save(str(dest))
                if dest.stat().st_size < 1000:
                    raise RuntimeError("audio too small")
                return
            except Exception as exc:  # noqa: BLE001
                last = exc
                await asyncio.sleep(1.5 * (n + 1))
        raise RuntimeError(f"TTS failed for slide {slide['id']}: {last}")


async def synthesize(audio_dir: Path, rate: str) -> None:
    audio_dir.mkdir(parents=True, exist_ok=True)
    sem = asyncio.Semaphore(4)
    tasks = []
    for slide in SLIDES:
        dest = audio_dir / f"{slide['id']}.mp3"
        tasks.append(_synth_one(sem, slide, dest, rate))
    await asyncio.gather(*tasks)


def probe_duration(path: Path) -> float:
    out = subprocess.check_output(
        ["ffprobe", "-v", "error", "-show_entries", "format=duration", "-of", "csv=p=0", str(path)],
        text=True,
    ).strip()
    return float(out)


def fmt_ts(seconds: float) -> str:
    seconds = max(0.0, seconds)
    h = int(seconds // 3600)
    m = int((seconds % 3600) // 60)
    s = seconds % 60
    if h:
        return f"{h}:{m:02d}:{s:05.2f}"
    return f"{m:02d}:{s:05.2f}"


def write_script(durations: list[float], path: Path, vtt_path: Path, rate: str, atempo: float) -> dict:
    pad = 0.40
    cursor = 0.0
    blocks = []
    vtt = ["WEBVTT", ""]
    chapters = []
    for slide, dur in zip(SLIDES, durations):
        start = cursor
        end = cursor + dur
        # narration occupies dur; pad is silence after, included in the video segment
        blocks.append((slide, start, end))
        vtt.append(f"{fmt_ts(start).replace('.', ',')} --> {fmt_ts(end).replace('.', ',')}")
        vtt.append(slide["narration"])
        vtt.append("")
        chapters.append({"id": slide["id"], "title": slide["title"], "start": round(start, 3), "end": round(end + pad, 3)})
        cursor = end + pad

    lines = [
        f"# Narration script — {COURSE_TITLE}",
        "",
        f"Course {COURSE_CODE}. Unclassified training. Illustrated instructor, synthetic narration.",
        f"Voice: {VOICE} at rate {rate}. Playback tempo factor: {atempo:.3f}.",
        "Timestamps match the briefing video, including a short pause after each slide.",
        "This script is original academy material. It is not a Department of Labor publication.",
        "",
        f"Total video duration: {fmt_ts(cursor)} ({cursor:.1f} seconds).",
        "",
    ]
    for slide, start, end in blocks:
        lines.append(f"## Slide {slide['id']} — {slide['title']}")
        lines.append("")
        lines.append(f"Section: {slide['section']}  ·  On screen {fmt_ts(start)} – {fmt_ts(end + pad)}")
        lines.append("")
        lines.append(slide["narration"])
        lines.append("")
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text("\n".join(lines), encoding="utf-8")
    vtt_path.write_text("\n".join(vtt), encoding="utf-8")
    return {"total": cursor, "chapters": chapters, "pad": pad}


def encode_segments(audio_dir: Path, card_dir: Path, bg_loop: Path, durations: list[float], atempo: float, seg_dir: Path) -> list[Path]:
    seg_dir.mkdir(parents=True, exist_ok=True)
    lower = render_lower_third()
    lower_path = WORK / "lower.png"
    lower.save(lower_path)
    paths = []
    pad = 0.40
    for slide, dur in zip(SLIDES, durations):
        seg = seg_dir / f"{slide['id']}.mp4"
        audio = audio_dir / f"{slide['id']}.mp3"
        card = card_dir / f"{slide['id']}.png"
        seg_dur = dur / atempo + pad
        atempo_filter = f"atempo={atempo:.5f}," if abs(atempo - 1) > 0.01 else ""
        filter_complex = (
            f"[1:v]format=rgba[card];"
            f"[2:v]format=rgba[lower];"
            f"[0:v][card]overlay={CARD_X - 28}:{CARD_Y - 28}:format=auto[v1];"
            f"[v1][lower]overlay=36:{H - 118}:format=auto,format=yuv420p[v];"
            f"[3:a]{atempo_filter}apad=pad_dur={pad},aresample=48000,aformat=channel_layouts=stereo[a]"
        )
        cmd = [
            "ffmpeg", "-y",
            "-stream_loop", "-1", "-i", str(bg_loop),
            "-i", str(card),
            "-i", str(lower_path),
            "-i", str(audio),
            "-filter_complex", filter_complex,
            "-map", "[v]", "-map", "[a]",
            "-t", f"{seg_dur:.3f}",
            "-r", str(FPS),
            "-c:v", "libx264", "-preset", "veryfast", "-tune", "stillimage",
            "-crf", "26", "-pix_fmt", "yuv420p",
            "-c:a", "aac", "-b:a", "128k", "-ar", "48000", "-ac", "2",
            "-movflags", "+faststart",
            str(seg),
        ]
        print(f"encoding slide {slide['id']} ({seg_dur:.1f}s)", flush=True)
        proc = subprocess.run(cmd, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE)
        if proc.returncode != 0:
            sys.stderr.write(proc.stderr.decode("utf-8", "replace")[-2000:])
            raise SystemExit(f"ffmpeg failed on slide {slide['id']}")
        paths.append(seg)
    return paths


def concat_with_chapters(segments: list[Path], chapters: list[dict], dest: Path) -> None:
    list_path = WORK / "concat.txt"
    list_path.write_text("".join(f"file '{p}'\n" for p in segments), encoding="utf-8")
    silent = dest.with_suffix(".tmp.mp4")
    subprocess.run(
        ["ffmpeg", "-y", "-f", "concat", "-safe", "0", "-i", str(list_path), "-c", "copy", "-movflags", "+faststart", str(silent)],
        check=True, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE,
    )
    meta = [";FFMETADATA1", "title=Prompt Engineering for Government Officers"]
    # ffmetadata wants TIMEBASE milliseconds
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
    subprocess.run(
        ["ffmpeg", "-y", "-i", str(silent), "-i", str(meta_path), "-map_metadata", "1", "-codec", "copy", "-movflags", "+faststart", str(dest)],
        check=True, stdout=subprocess.DEVNULL, stderr=subprocess.PIPE,
    )
    silent.unlink(missing_ok=True)


def publish(paths: list[Path]) -> None:
    for folder in ART_DIRS:
        folder.mkdir(parents=True, exist_ok=True)
    for src in paths:
        # keep relative structure under the course folder when possible
        try:
            rel = src.relative_to(COURSE)
        except ValueError:
            rel = Path(src.name)
        for folder in ART_DIRS:
            dest = folder / rel
            dest.parent.mkdir(parents=True, exist_ok=True)
            if dest.resolve() == src.resolve():
                continue
            subprocess.run(["cp", "-f", str(src), str(dest)], check=True)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--preview", action="store_true")
    parser.add_argument("--skip-video", action="store_true")
    args = parser.parse_args()

    WORK.mkdir(parents=True, exist_ok=True)
    portrait_dest = COURSE / "assets" / "instructor-portrait.png"
    portrait_dest.parent.mkdir(parents=True, exist_ok=True)
    if PORTRAIT.exists() and PORTRAIT.resolve() != portrait_dest.resolve():
        subprocess.run(["cp", "-f", str(PORTRAIT), str(portrait_dest)], check=True)

    plate = make_plate(portrait_dest)
    plate.save(WORK / "plate.png", quality=95)

    card_dir = WORK / "cards"
    card_dir.mkdir(exist_ok=True)
    lower = render_lower_third()
    total = len(SLIDES)
    preview_ids = {"01", "02", "04", "17", "18", "32"}
    for i, slide in enumerate(SLIDES, start=1):
        card = render_card(slide, i, total)
        card.save(card_dir / f"{slide['id']}.png")
        if args.preview and slide["id"] in preview_ids:
            frame = composite_preview(plate, card, lower)
            frame.save(WORK / f"preview-{slide['id']}.jpg", quality=90)
            print("preview", WORK / f"preview-{slide['id']}.jpg")
    if args.preview:
        return

    pptx_path = COURSE / "slides" / "Prompt-Engineering-for-Government-Officers.pptx"
    build_pptx(pptx_path)
    print("pptx", pptx_path)

    audio_dir = WORK / "audio"
    rate = VOICE_RATE
    print("synthesizing", rate, flush=True)
    asyncio.run(synthesize(audio_dir, rate))
    durations = [probe_duration(audio_dir / f"{s['id']}.mp3") for s in SLIDES]
    raw = sum(durations)
    print(f"raw narration {raw:.1f}s ({raw/60:.2f} min) at {rate}")

    pad_total = 0.40 * len(SLIDES)
    # Aim the spoken track so pads still leave the video inside 29–31 minutes.
    desired_audio = 30.1 * 60 - pad_total
    spoken_rate = 0.12
    if raw > 31.2 * 60 - pad_total or raw < 28.4 * 60 - pad_total:
        new_r = (raw * (1 + spoken_rate) / desired_audio) - 1
        new_r = max(-0.05, min(0.40, new_r))
        rate = f"{new_r * 100:+.0f}%"
        print(f"re-synthesizing at {rate} to land near 30 minutes", flush=True)
        asyncio.run(synthesize(audio_dir, rate))
        durations = [probe_duration(audio_dir / f"{s['id']}.mp3") for s in SLIDES]
        raw = sum(durations)
        print(f"raw narration {raw:.1f}s ({raw/60:.2f} min) at {rate}")

    atempo = raw / desired_audio
    atempo = max(0.92, min(1.10, atempo))
    if abs(atempo - 1) <= 0.015:
        atempo = 1.0
    print(f"applying atempo {atempo:.3f}")
    projected = raw / atempo + pad_total
    print(f"projected video {projected:.1f}s ({projected/60:.2f} min)")

    script_path = COURSE / "narration" / "narration-script.md"
    vtt_path = COURSE / "narration" / "captions.vtt"
    meta = write_script([d / atempo for d in durations], script_path, vtt_path, rate, atempo)
    (COURSE / "narration" / "timing.json").write_text(json.dumps(meta, indent=2), encoding="utf-8")

    if args.skip_video:
        return

    bg = WORK / "bg-loop.mp4"
    print("background loop", flush=True)
    build_background_loop(plate, bg)
    segments = encode_segments(audio_dir, card_dir, bg, durations, atempo, WORK / "segments")
    video_path = COURSE / "video" / "GA-PE-210-prompt-engineering.mp4"
    video_path.parent.mkdir(parents=True, exist_ok=True)
    concat_with_chapters(segments, meta["chapters"], video_path)
    final = probe_duration(video_path)
    print(f"FINAL {video_path} {final:.2f}s ({final/60:.2f} min)")

    publish([
        pptx_path,
        script_path,
        vtt_path,
        COURSE / "narration" / "timing.json",
        video_path,
        portrait_dest,
        COURSE / "outline" / "course-outline.md",
        COURSE / "README.md",
        COURSE / "seed" / "GovAiAcademy.PromptEngineering.seed.cs",
        COURSE / "seed" / "ADD-TO-CATALOG.md",
    ])


if __name__ == "__main__":
    main()
