using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

internal static class ExtractJumbo
{
    static int Main(string[] args)
    {
        string outDir = args.Length > 0 ? args[0] : @"C:\Users\Administrator\src\psych-neon-desktop\assets\icons\stock256";
        Directory.CreateDirectory(outDir);
        string sys = Environment.GetFolderPath(Environment.SpecialFolder.System);

        // stock Win10 indices (negative = resource ID)
        Grab(outDir, "folder", Path.Combine(sys, "shell32.dll"), 3);
        Grab(outDir, "file", Path.Combine(sys, "imageres.dll"), 2);
        Grab(outDir, "txt", Path.Combine(sys, "imageres.dll"), -102);
        Grab(outDir, "image", Path.Combine(sys, "imageres.dll"), -72);
        Grab(outDir, "zip", Path.Combine(sys, "imageres.dll"), -174);
        Grab(outDir, "video", Path.Combine(sys, "wmploc.dll"), -730);
        Grab(outDir, "audio", Path.Combine(sys, "wmploc.dll"), -731);
        Grab(outDir, "pdf", Path.Combine(sys, "imageres.dll"), -14);
        return 0;
    }

    static void Grab(string dir, string name, string dll, int index)
    {
        Bitmap bmp = FromDll(dll, index, 256);
        if (bmp == null)
        {
            Console.WriteLine("FAIL " + name + " " + dll + " " + index);
            return;
        }
        string p = Path.Combine(dir, name + ".png");
        bmp.Save(p, ImageFormat.Png);
        Console.WriteLine(name + " " + bmp.Width + "x" + bmp.Height + " " + new FileInfo(p).Length + " from " + Path.GetFileName(dll) + "," + index);
        bmp.Dispose();
    }

    static Bitmap FromDll(string dll, int index, int size)
    {
        IntPtr[] icons = new IntPtr[1];
        uint[] ids = new uint[1];
        uint n = PrivateExtractIcons(dll, index, size, size, icons, ids, 1, 0);
        if (n == 0 || icons[0] == IntPtr.Zero)
            return null;
        Bitmap bmp;
        using (Icon ico = Icon.FromHandle(icons[0]))
        using (Bitmap raw = ico.ToBitmap())
        {
            bmp = new Bitmap(raw.Width, raw.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(raw, 0, 0, raw.Width, raw.Height);
            }
        }
        DestroyIcon(icons[0]);
        return bmp;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern uint PrivateExtractIcons(string lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, uint[] piconid, uint nIcons, uint flags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyIcon(IntPtr hIcon);
}
