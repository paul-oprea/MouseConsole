using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

public class ScreenOperations
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

    public static Image CaptureDesktop()
    {
        return CaptureWindow(GetDesktopWindow());
    }

    public static Bitmap CaptureActiveWindow()
    {
        return CaptureWindow(GetForegroundWindow());
    }

    public static Bitmap CaptureWindow(IntPtr handle)
    {
        var rect = new Rect();
        GetWindowRect(handle, ref rect);
        var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        var result = new Bitmap(bounds.Width, bounds.Height);

        using (var graphics = Graphics.FromImage(result))
        {
            graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        }

        return result;
    }

    public static Bitmap CaptureWindow(int left, int top, int right, int bottom)
    {
        var rect = new Rect();

        var bounds = new Rectangle(left, top, right - left, bottom - top);
        var result = new Bitmap(bounds.Width, bounds.Height);

        using (var graphics = Graphics.FromImage(result))
        {
            graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
        }


        return result;
    }

    /// <summary> Save an image to a specific file </summary>
    /// <param name="filename">Filename.
    /// <para>* If extension is omitted, it's calculated from the type of file</para>
    /// <para>* If path is omitted, defaults to %TEMP%</para>
    /// <para>* Use %NOW% to put a timestamp in the filename</para></param>
    /// <param name="format">Optional file save mode.  Default is PNG</param>
    /// <param name="image">Image to save.  Usually a BitMap, but can be any
    /// Image.</param>
    public static void ImageSave(string filename, ImageFormat format, Image image)
    {
        format = format ?? ImageFormat.Png;
        if (!filename.Contains("."))
            filename = filename.Trim() + "." + format.ToString().ToLower();

        if (!filename.Contains(@"\"))
            filename = Path.Combine(Environment.GetEnvironmentVariable("TEMP") ?? @"C:\Temp", filename);

        filename = filename.Replace("%NOW%", DateTime.Now.ToString("yyyy-MM-dd@hh.mm.ss"));
        image.Save(filename, format);
    }

}