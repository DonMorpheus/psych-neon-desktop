"""High-contrast TXT icon: readable at 16px, still PsychNeon palette."""
from pathlib import Path
from PIL import Image, ImageDraw

OUT = Path(r"C:\Users\Administrator\src\psych-neon-desktop\assets\icons")
OUT.mkdir(parents=True, exist_ok=True)
SIZES = [256, 128, 64, 48, 32, 16]


def draw_txt(size=1024):
    im = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    d = ImageDraw.Draw(im)
    # tile
    pad = int(size * 0.06)
    r = int(size * 0.18)
    tile = (pad, pad, size - pad - 1, size - pad - 1)
    d.rounded_rectangle(tile, radius=r, fill=(18, 8, 16, 255), outline=(255, 64, 180, 255), width=max(6, size // 48))
    # inner gold rim
    inset = max(4, size // 80)
    inner = (pad + inset, pad + inset, size - pad - 1 - inset, size - pad - 1 - inset)
    d.rounded_rectangle(inner, radius=r - inset, outline=(255, 196, 64, 220), width=max(2, size // 120))

    # document page
    dw, dh = int(size * 0.42), int(size * 0.54)
    left = (size - dw) // 2
    top = (size - dh) // 2 + int(size * 0.02)
    page = (left, top, left + dw, top + dh)
    fold = int(dw * 0.28)
    # page body (polygon with folded corner)
    pts = [
        (left, top),
        (left + dw - fold, top),
        (left + dw, top + fold),
        (left + dw, top + dh),
        (left, top + dh),
    ]
    d.polygon(pts, fill=(255, 248, 252, 255), outline=(40, 10, 30, 255))
    # fold triangle
    d.polygon(
        [(left + dw - fold, top), (left + dw, top + fold), (left + dw - fold, top + fold)],
        fill=(255, 170, 210, 255),
        outline=(40, 10, 30, 255),
    )
    # text lines — thick, dark, high contrast
    lx0 = left + int(dw * 0.16)
    lx1 = left + dw - int(dw * 0.16)
    nlines = 4
    gap_top = top + int(dh * 0.32)
    gap_bot = top + dh - int(dh * 0.16)
    thick = max(8, size // 28)
    for i in range(nlines):
        y = gap_top + int((gap_bot - gap_top) * i / (nlines - 1))
        wline = lx1 if i < nlines - 1 else lx0 + int((lx1 - lx0) * 0.62)
        d.rounded_rectangle((lx0, y - thick // 2, wline, y + thick // 2), radius=thick // 2, fill=(40, 12, 28, 255))
    return im


def main():
    master = draw_txt(1024)
    png = OUT / "txt.png"
    master.save(png, "PNG")
    icos = [master.resize((n, n), Image.Resampling.LANCZOS) for n in SIZES]
    ico = OUT / "txt.ico"
    icos[0].save(ico, format="ICO", sizes=[(n, n) for n in SIZES], append_images=icos[1:])
    print("txt.png", png.stat().st_size, "txt.ico", ico.stat().st_size)


if __name__ == "__main__":
    main()
