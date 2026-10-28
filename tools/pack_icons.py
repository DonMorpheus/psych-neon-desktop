from pathlib import Path
from PIL import Image

SESSION = Path(r"C:\Users\Administrator\.grok\sessions\C%3A%5CUsers%5CAdministrator\01a182a9-62dc-7381-92f0-38a1302991d5\images")
OUT = Path(r"C:\Users\Administrator\src\psych-neon-desktop\assets\icons")
OUT.mkdir(parents=True, exist_ok=True)

MAP = {
    "34.jpg": "folder",
    "31.jpg": "file",
    "35.jpg": "pdf",
    "32.jpg": "txt",
    "30.jpg": "image",
    "28.jpg": "zip",
    "33.jpg": "video",
    "29.jpg": "audio",
}

SIZES = [256, 128, 64, 48, 32, 16]


def square(im: Image.Image) -> Image.Image:
    im = im.convert("RGBA")
    s = min(im.size)
    left = (im.width - s) // 2
    top = (im.height - s) // 2
    return im.crop((left, top, left + s, top + s))


def pack(src: Path, stem: str):
    im = square(Image.open(src))
    master = im.resize((1024, 1024), Image.Resampling.LANCZOS)
    png = OUT / f"{stem}.png"
    master.save(png, "PNG", optimize=True)
    icos = [master.resize((n, n), Image.Resampling.LANCZOS) for n in SIZES]
    ico = OUT / f"{stem}.ico"
    icos[0].save(ico, format="ICO", sizes=[(n, n) for n in SIZES], append_images=icos[1:])
    print(f"{stem}: {src.name} -> {png.name} {png.stat().st_size} {ico.name} {ico.stat().st_size}")


def main():
    for fn, stem in MAP.items():
        src = SESSION / fn
        if not src.exists():
            raise SystemExit(f"missing {src}")
        pack(src, stem)


if __name__ == "__main__":
    main()
