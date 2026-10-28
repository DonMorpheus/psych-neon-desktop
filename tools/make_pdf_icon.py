"""Classic high-contrast PDF page — readable at 16px, 4K master."""
from pathlib import Path
from PIL import Image, ImageDraw, ImageFont

OUT = Path(r"C:\Users\Administrator\src\psych-neon-desktop\assets\icons\stock256")
OUT.mkdir(parents=True, exist_ok=True)


def draw(size=1024):
    im = Image.new("RGBA", (size, size), (0, 0, 0, 0))
    d = ImageDraw.Draw(im)
    # drop shadow
    sh = int(size * 0.03)
    left, top = int(size * 0.22), int(size * 0.10)
    w, h = int(size * 0.56), int(size * 0.78)
    d.rounded_rectangle((left + sh, top + sh, left + w + sh, top + h + sh), radius=int(size * 0.03), fill=(0, 0, 0, 70))
    # page
    d.rounded_rectangle((left, top, left + w, top + h), radius=int(size * 0.02), fill=(255, 255, 255, 255), outline=(40, 40, 40, 255), width=max(2, size // 200))
    # red header
    head = int(h * 0.28)
    d.rectangle((left + 1, top + 1, left + w - 1, top + head), fill=(196, 32, 32, 255))
    # PDF letters — simple blocks so they stay sharp (no font dependency)
    # three white bars forming a chunky "PDF" silhouette via thick text if font exists
    try:
        font = ImageFont.truetype("arialbd.ttf", int(size * 0.14))
    except Exception:
        try:
            font = ImageFont.truetype("C:\\Windows\\Fonts\\arialbd.ttf", int(size * 0.14))
        except Exception:
            font = ImageFont.load_default()
    text = "PDF"
    bbox = d.textbbox((0, 0), text, font=font)
    tw, th = bbox[2] - bbox[0], bbox[3] - bbox[1]
    tx = left + (w - tw) // 2
    ty = top + (head - th) // 2 - bbox[1]
    d.text((tx, ty), text, font=font, fill=(255, 255, 255, 255))
    # body lines
    lx0 = left + int(w * 0.14)
    lx1 = left + w - int(w * 0.14)
    thick = max(6, size // 40)
    y0 = top + head + int(h * 0.12)
    for i in range(5):
        y = y0 + i * int(h * 0.10)
        right = lx1 if i < 4 else lx0 + int((lx1 - lx0) * 0.55)
        d.rounded_rectangle((lx0, y, right, y + thick), radius=thick // 2, fill=(55, 55, 55, 255))
    return im


def main():
    master = draw(1024)
    master.save(OUT / "pdf.png", "PNG")
    print("pdf", (OUT / "pdf.png").stat().st_size)


if __name__ == "__main__":
    main()
