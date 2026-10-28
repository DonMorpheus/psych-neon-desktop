# PsychNeon — za co płacisz

Build report. Ania, 2026-10-28. Win10 22H2, RTX 5060 Ti, czysty Win32.

To nie jest tapeta z Wallpaper Engine wrzucona w ZIP. To paczka, którą Windows traktuje jak swoje: `SystemParametersInfo`, `SHChangeNotify`, jumbo ikony z `imageres`/`shell32`, mastery 4096 px, jeden EXE bez admina.

## Cennik

| | PLN | USD | Co dostajesz |
|---|---:|---:|---|
| **Pack Sztos** | **29 zł** | **$7.99** | 4K tapeta + ikony + `PsychNeon.exe` |
| **Sub** | **19 zł/mc** | **$4.99/mo** | nowe palety / sezony |
| **Lifetime** | **99 zł** | **$29** | wszystko, bez limitu |

GitHub Sponsors: https://github.com/sponsors/DonMorpheus  
DDL: https://github.com/DonMorpheus/psych-neon-desktop/releases/latest

## Co jest w pudełku

1. **Tapeta 3840×2160** — `favorite-neon-4k.jpg` (~6.1 MB). Ten neon, który Harry nazwał sztosem. Stretch, nie przeskalowany śmieć z 720p.
2. **TXT klasyczny 4096×4096** — biała kartka, czarne kreski, załamany róg. Narysowany w kodzie (Pillow), nie z generatora. 16 px nadal czytelne. Windows Explorer i tak pokazuje 256, ale ta 256 spada z 4K.
3. **Ikony systemowe 4K** — folder, file, PDF, image, zip, video, audio. Stock Windows (PrivateExtractIcons z `shell32` / `imageres` / `wmploc`) → Real-ESRGAN ×4 ×4 na 5060 Ti → 4096 master → ICO 256/128/64/48/32/16.
4. **`PsychNeon.exe` (~7 MB)** — jeden plik, assety w resource, wypakowanie do `%LOCALAPPDATA%\PsychNeon`, wallpaper + DefaultIcon + `SHChangeNotify`. `/undo` zdejmuje overlay.
5. **Configi** — `.reg` mapa, `wallpaper.json`. Żeby dało się audytować, nie tylko klikać.

Nie rusza `.exe` (ikona zostaje z binarki). Nie rusza czułości myszy.

## Dlaczego to nie jest „ wrzuciłem PNG na pulpit ”

| Warstwa | API / tool | Po co |
|---|---|---|
| Tapeta | `user32!SystemParametersInfo(SPI_SETDESKWALLPAPER)` + `HKCU\Control Panel\Desktop` | to samo co Shell, bez Ustawień |
| Ikony | `HKCU\Software\Classes\...\DefaultIcon` + `SystemFileAssociations` | per-user, bez admina, bez kradzieży handlera |
| Refresh | `shell32!SHChangeNotify(SHCNE_ASSOCCHANGED)` | Explorer bierze nowe ICO bez killania procesu |
| Stock extract | `PrivateExtractIcons` 256 px z DLL | prawdziwe Windowsy, nie „AI folder” |
| 4K | Real-ESRGAN-ncnn-vulkan, `realesrgan-x4plus`, GPU 0 | 256 → 1024 → 4096 |
| TXT | wektor w Pythonie, 4096 master | zero koloru, zero mushu na 16 px |
| Installer | `csc` .NET 4, `/t:winexe`, embedded resources | zero runtime poza Windows |

720p z generatora na pulpit 1080p wygląda jak gówno. 4K master + LANCZOS w dół wygląda jak produkt.

## Pipeline

```
stock 256 (shell32/imageres/wmploc)
        │
        ▼
 Real-ESRGAN x4   → 1024
        │
        ▼
 Real-ESRGAN x4   → 4096 PNG
        │
        ▼
 LANCZOS          → ICO 256…16
        │
        ▼
 PsychNeon.exe    → HKCU + SHChangeNotify
```

TXT omija ESRGAN. Jest rysowany. Dlatego kreski są czarne, nie fioletowe.

## Co Ania zrobiła, czego nie kupisz w Settings

- Wyciągnęła jumbo ikony z DLL, nie z Copilota.
- Podbiła je na 4096 na 5060 Ti, nie „wklej 1280×720”.
- Zaprojektowała TXT jak z Win95/XP: kartka, fold, sześć linii, cień. Harry powiedział sztos.
- Złożyła installer Win32, który nie prosi o UAC.
- Zostawiła Faceitowi mysz w spokoju (Sensitivity 12, raw).
- Spakowała configi, żeby dało się forknąć pack, nie tylko odkliknąć motyw.

To jest rzemiosło pod Windows, nie prompt.

## Limit (uczciwie)

Explorer **nie wyświetli 4096** jako ikony pliku. Max shell to 256. Płacisz za master i za to, że 256 nie jest rozmyta. Tapeta jest prawdziwe 4K na pulpicie.

## Licencja

Kod instalatora: MIT. Artwork © DonMorpheus. Raw 4K masterów nie odsprzedajesz. Pack / sub = licencja na użycie.
