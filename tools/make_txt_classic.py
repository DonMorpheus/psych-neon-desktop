"""Classic B/W TXT page — folded corner, black lines, readable at 16px."""
from pathlib import Path
from PIL import Image, ImageDraw

OUT = Path(r"C:\Users\Administrator\src\psych-neon-desktop\assets\icons")
OUT.mkdir(parents=True, exist_ok=True)
SIZES = [256, 128, 64, 48, 32, 16]


def draw(size=1024):
    im = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    d = ImageDraw.Draw(im)

    margin = int(size * 0.16)
    left = margin
    top = int(size * 0.10)
    right = size - margin
    bottom = size - int(size * 0.10)
    fold = int((right - left) * 0.28)
    outline = max(3, size // 90)

    # soft drop shadow
    sh = max(4, size // 64)
    d.polygon(
        [
            (left + sh, top + sh),
            (right - fold + sh, top + sh),
            (right + sh, top + fold + sh),
            (right + sh, bottom + sh),
            (left + sh, bottom + sh),
        ],
        fill=(0, 0, 0, 50),
    )

    page = [
        (left, top),
        (right - fold, top),
        (right, top + fold),
        (right, bottom),
        (left, bottom),
    ]
    d.polygon(page, fill=(255, 255, 255, 255), outline=(0, 0, 0, 255))
    d.line(page + [page[0]], fill=(0, 0, 0, 255), width=outline)

    # dog-ear
    d.polygon(
        [
            (right - fold, top),
            (right, top + fold),
            (right - fold, top + fold),
        ],
        fill=(230, 230, 230, 255),
        outline=(0, 0, 0, 255),
    )
    d.line([(right - fold, top), (right - fold, top + fold), (right, top + fold)], fill=(0, 0, 0, 255), width=outline)

    # text lines
    pad_x = int((right - left) * 0.14)
    x0 = left + pad_x
    x1 = right - pad_x
    y0 = top + fold + int((bottom - top) * 0.16)
    y1 = bottom - int((bottom - top) * 0.12)
    n = 6
    thick = max(6, size // 32)
    for i in range(n):
        y = y0 + int((y1 - y0) * i / float(n - 1))
        # last two lines shorter, like a real paragraph
        if i == n - 1:
            x_end = x0 + int((x1 - x0) * 0.52)
        elif i == n - 2:
            x_end = x0 + int((x1 - x0) * 0.78)
        else:
            x_end = x1
        d.rectangle((x0, y, x_end, y + thick), fill=(0, 0, 0, 255))

    return im


def main():
    master = draw(4096)
    png4k = OUT / "txt-4k.png"
    master.save(png4k, "PNG")
    png = OUT / "txt.png"
    master.resize((1024, 1024), Image.Resampling.LANCZOS).save(png, "PNG")
    icos = [master.resize((n, n), Image.Resampling.LANCZOS) for n in SIZES]
    ico = OUT / "txt.ico"
    icos[0].save(ico, format="ICO", sizes=[(n, n) for n in SIZES], append_images=icos[1:])
    print("wrote", png4k, png4k.stat().st_size, png, png.stat().st_size, ico, ico.stat().st_size)


if __name__ == "__main__":
    main()
