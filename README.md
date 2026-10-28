# PsychNeon Desktop

4K neon wallpaper + system file icons for Windows 10/11. One EXE, HKCU only, no admin.

Microsoft sprzedaje Ci „Personalizacja”: trzy suwaki, tapeta przeskalowana z 1080p, ikona TXT rozmazana na 16 px i uśmiech w Ustawieniach. To wstyd. Shell umie więcej od 1995. Oni dali ludziom panel, który nic nie rusza.

PsychNeon wchodzi tam, gdzie Settings się boi: **klasyfikacja plików, jumbo imagelist, DWM, `SHChangeNotify`**. Nie klikasz motywu. Nadpisujesz to, co Explorer czyta przy starcie.

**Za co płacisz:** [RAPORT.md](RAPORT.md)

## Jak głęboko edytujemy Windows

To nie skórka. To overlay na Shell.

| Warstwa | Co nadpisujemy | API / klucz |
|---|---|---|
| Pulpit | tapeta 3840×2160, Stretch | `user32!SystemParametersInfo(SPI_SETDESKWALLPAPER)` + `HKCU\Control Panel\Desktop` (`Wallpaper`, `WallpaperStyle=2`) |
| Foldery | ikona katalogu | `HKCU\Software\Classes\Directory\DefaultIcon`, `Folder\DefaultIcon` |
| TXT / log / md / ini / csv | klasyczna kartka B/W, master 4096 | `txtfile\DefaultIcon`, `SystemFileAssociations\.txt` (i reszta) |
| PDF / jpg / png / zip / mp4 / mp3 | DefaultIcon per typ | `SystemFileAssociations\.<ext>\DefaultIcon` + ProgID w HKCU |
| PDF handler-safe | ikona bez kradzieży open | `HKCU\Software\Classes\PsychNeon.pdf` + `.pdf` |
| Explorer refresh | bez killania `explorer.exe` | `shell32!SHChangeNotify(SHCNE_ASSOCCHANGED \| SHCNF_FLUSH)` |
| Źródło ikon | prawdziwe jumbo 256 z systemu | `PrivateExtractIcons` z `shell32.dll` / `imageres.dll` / `wmploc.dll` |
| 4K | 256 → 1024 → 4096 | Real-ESRGAN-ncnn-vulkan na GPU, potem LANCZOS do ICO 256…16 |

**Czego nie ruszamy (celowo):** ikony `.exe` (zostają z binarki), czułość myszy, HKLM, usługi, driverów. Zero UAC. Całość w **HKCU** — Twój user, Twój Shell.

Ustawienia Windows dają Ci JPEG 1920 i ikonę z 2006. My bierzemy ten sam Shell i karmimy go masterami 4K. Jak Microsoft nie umie zrobić kartki TXT, która nie wygląda jak gówno, to nie nasz problem — to ich.

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
