# PsychNeon Desktop

4K neon wallpaper + system file icons for Windows 10/11. One EXE, HKCU only, no admin.

**Za co płacisz:** [RAPORT.md](RAPORT.md) — pipeline Win32, mastery 4096, TXT narysowany w kodzie, nie z generatora.

## DDL

Release assets (direct download):

- **Installer:** https://github.com/DonMorpheus/psych-neon-desktop/releases/latest/download/PsychNeon.exe
- **Pack zip (wallpaper + ico + configs):** https://github.com/DonMorpheus/psych-neon-desktop/releases/latest/download/PsychNeon-pack.zip

## Install

```text
PsychNeon.exe
```

Unpacks to `%LOCALAPPDATA%\PsychNeon`, sets the 4K wallpaper (`SPI_SETDESKWALLPAPER`), writes file-type icons, then `SHChangeNotify`.

Undo icons:

```text
PsychNeon.exe /undo
```

## What you get

| Asset | Path in pack |
|---|---|
| 4K wallpaper 3840×2160 | `assets/wallpaper/favorite-neon-4k.jpg` |
| Folder / file / PDF / txt / image / zip / video / audio | `assets/icons/*.ico` (256…16) |
| Registry map | `configs/psych-neon-icons.reg` |
| Wallpaper config | `configs/wallpaper.json` |

Does **not** touch `.exe` icons (those stay per-app). Does **not** change mouse speed.

## Cennik

Płać na [GitHub Sponsors](https://github.com/sponsors/DonMorpheus) albo wypisz się na release — wrzucę Gumroad jak wstanie.

| | PLN | USD |
|---|---:|---:|
| **Pack Sztos** — 4K tapeta + ikony systemowe + `PsychNeon.exe` | 29 zł | $7.99 |
| **Sub miesięczna** — nowe palety / sezony | 19 zł/mc | $4.99/mo |
| **Lifetime** — wszystkie packi, bez limitu | 99 zł | $29 |

Core na GitHubie jest po to, żeby ktoś kliknął DDL i zobaczył. Kasa idzie za packi i suba.

## Subscription drops

Build from source:

```text
python tools/pack_icons.py
csc /nologo /optimize+ /t:winexe /out:dist\PsychNeon.exe src\PsychNeon.cs ^
  /resource:assets\wallpaper\favorite-neon-4k.jpg,wallpaper.jpg ^
  /resource:assets\icons\folder.ico,folder.ico ^
  ...
```

## License

Installer code: MIT. Artwork © DonMorpheus — use via the installer / a paid pack, do not resell the raw 4K masters.
