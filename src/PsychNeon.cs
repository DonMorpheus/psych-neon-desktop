using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

internal static class PsychNeon
{
    static readonly string AppDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PsychNeon");
    static readonly string WallName = "favorite-neon-4k.jpg";

    static readonly string[] IconNames =
    {
        "folder", "file", "pdf", "txt", "image", "zip", "video", "audio"
    };

    static int Main(string[] args)
    {
        try
        {
            Directory.CreateDirectory(AppDir);
            ExtractAssets();
            if (args.Length > 0 && (args[0] == "/undo" || args[0] == "--undo"))
            {
                Undo();
                Notify();
                return 0;
            }
            ApplyWallpaper();
            ApplyIcons();
            Notify();
            return 0;
        }
        catch (Exception ex)
        {
            File.AppendAllText(Path.Combine(AppDir, "install.log"), DateTime.Now + " " + ex + Environment.NewLine);
            return 1;
        }
    }

    static void ExtractAssets()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        string[] names = asm.GetManifestResourceNames();
        foreach (string name in names)
        {
            string leaf = name;
            int dot = name.LastIndexOf('.');
            if (name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                leaf = WallName;
            else if (name.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
            {
                string[] parts = name.Split('.');
                if (parts.Length >= 2)
                    leaf = parts[parts.Length - 2] + ".ico";
            }
            else
                continue;
            string dest = Path.Combine(AppDir, leaf);
            using (Stream s = asm.GetManifestResourceStream(name))
            using (var fs = File.Create(dest))
            {
                s.CopyTo(fs);
            }
        }

        string beside = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets");
        if (Directory.Exists(beside))
        {
            string wsrc = Path.Combine(beside, "wallpaper", WallName);
            if (File.Exists(wsrc))
                File.Copy(wsrc, Path.Combine(AppDir, WallName), true);
            string idir = Path.Combine(beside, "icons");
            if (Directory.Exists(idir))
            {
                foreach (string f in Directory.GetFiles(idir, "*.ico"))
                    File.Copy(f, Path.Combine(AppDir, Path.GetFileName(f)), true);
            }
        }
    }

    static void ApplyWallpaper()
    {
        string wall = Path.Combine(AppDir, WallName);
        if (!File.Exists(wall))
            throw new FileNotFoundException(wall);
        Native.SystemParametersInfo(Native.SPI_SETDESKWALLPAPER, 0, wall,
            Native.SPIF_UPDATEINIFILE | Native.SPIF_SENDWININICHANGE);
        using (RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
        {
            if (k != null)
            {
                k.SetValue("Wallpaper", wall);
                k.SetValue("WallpaperStyle", "2");
                k.SetValue("TileWallpaper", "0");
            }
        }
    }

    static void ApplyIcons()
    {
        SetIcon(@"Software\Classes\Directory\DefaultIcon", "folder.ico");
        SetIcon(@"Software\Classes\Folder\DefaultIcon", "folder.ico");
        SetIcon(@"Software\Classes\Directory\Background\DefaultIcon", "folder.ico");

        SetIcon(@"Software\Classes\txtfile\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\text\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.txt\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.log\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.md\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.ini\DefaultIcon", "txt.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.csv\DefaultIcon", "txt.ico");

        SetIcon(@"Software\Classes\jpegfile\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\pngfile\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\giffile\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\Paint.Picture\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.jpg\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.jpeg\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.png\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.gif\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.webp\DefaultIcon", "image.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.bmp\DefaultIcon", "image.ico");

        SetIcon(@"Software\Classes\7-Zip.zip\DefaultIcon", "zip.ico");
        SetIcon(@"Software\Classes\CompressedFolder\DefaultIcon", "zip.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.zip\DefaultIcon", "zip.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.7z\DefaultIcon", "zip.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.rar\DefaultIcon", "zip.ico");

        SetIcon(@"Software\Classes\WMP11.AssocFile.MP4\DefaultIcon", "video.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.mp4\DefaultIcon", "video.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.mkv\DefaultIcon", "video.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.avi\DefaultIcon", "video.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.webm\DefaultIcon", "video.ico");

        SetIcon(@"Software\Classes\WMP11.AssocFile.MP3\DefaultIcon", "audio.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.mp3\DefaultIcon", "audio.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.wav\DefaultIcon", "audio.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.flac\DefaultIcon", "audio.ico");

        SetIcon(@"Software\Classes\htmlfile\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.html\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.htm\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.json\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.js\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.py\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.cs\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\docxfile\DefaultIcon", "file.ico");
        SetIcon(@"Software\Classes\Unknown\DefaultIcon", "file.ico");

        using (RegistryKey ext = Registry.CurrentUser.CreateSubKey(@"Software\Classes\.pdf"))
        {
            ext.SetValue(null, "PsychNeon.pdf");
        }
        SetIcon(@"Software\Classes\PsychNeon.pdf\DefaultIcon", "pdf.ico");
        SetIcon(@"Software\Classes\SystemFileAssociations\.pdf\DefaultIcon", "pdf.ico");
        SetIcon(@"Software\Classes\.pdf\DefaultIcon", "pdf.ico");
    }

    static void SetIcon(string keyPath, string icoName)
    {
        string ico = Path.Combine(AppDir, icoName);
        if (!File.Exists(ico))
            return;
        using (RegistryKey k = Registry.CurrentUser.CreateSubKey(keyPath))
        {
            k.SetValue(null, ico + ",0");
        }
    }

    static void Undo()
    {
        string[] keys =
        {
            @"Software\Classes\Directory\DefaultIcon",
            @"Software\Classes\Folder\DefaultIcon",
            @"Software\Classes\txtfile\DefaultIcon",
            @"Software\Classes\jpegfile\DefaultIcon",
            @"Software\Classes\pngfile\DefaultIcon",
            @"Software\Classes\PsychNeon.pdf\DefaultIcon",
            @"Software\Classes\.pdf\DefaultIcon"
        };
        foreach (string key in keys)
        {
            try { Registry.CurrentUser.DeleteSubKeyTree(key, false); } catch { }
        }
    }

    static void Notify()
    {
        Native.SHChangeNotify(Native.SHCNE_ASSOCCHANGED, Native.SHCNF_IDLIST | Native.SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
        IntPtr unused;
        Native.SendMessageTimeout(Native.HWND_BROADCAST, Native.WM_SETTINGCHANGE, IntPtr.Zero, "Environment",
            Native.SMTO_ABORTIFHUNG, 1000, out unused);
    }
}

internal static class Native
{
    public const int SPI_SETDESKWALLPAPER = 20;
    public const int SPIF_UPDATEINIFILE = 0x01;
    public const int SPIF_SENDWININICHANGE = 0x02;
    public const uint SHCNE_ASSOCCHANGED = 0x08000000;
    public const uint SHCNF_IDLIST = 0x0000;
    public const uint SHCNF_FLUSH = 0x1000;
    public const int WM_SETTINGCHANGE = 0x001A;
    public const int SMTO_ABORTIFHUNG = 0x0002;
    public static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool SystemParametersInfo(int uiAction, int uiParam, string pvParam, int fWinIni);

    [DllImport("shell32.dll")]
    public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, int fuFlags, int uTimeout, out IntPtr lpdwResult);
}
