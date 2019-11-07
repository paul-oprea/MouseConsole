using System;
using System.Runtime.InteropServices;

public class MouseOperations
{
    public static int __SECOND = 1000;
    public static int __RES_MS = 10;

    [Flags]
    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010
    }

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    public static void SetCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    public static void SetCursorPosition(MousePoint point)
    {
        SetCursorPos(point.X, point.Y);
    }

    public static MousePoint GetCursorPosition()
    {
        MousePoint currentMousePoint;
        var gotPoint = GetCursorPos(out currentMousePoint);
        if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
        return currentMousePoint;
    }

    public static void MouseMovementSequence() {
        int y;
        for (int x = 0; x < 360; x++)
        {
            y = (int)( Math.Sin( x / Math.PI / 2 ) * 10 );
//            System.Console.WriteLine(y);
            mouse_event
                            ((int)MouseEventFlags.Move,
                             1,
                             y,
                             0,
                             0)
                            ;
            System.Threading.Thread.Sleep(__RES_MS);

        };
    }

    public static MousePoint[] CaptureMouse(int Duration) {
        int d = -1;
        decimal c = Duration / __RES_MS;
        MousePoint[] coordinates = new MousePoint[(int)Math.Ceiling(c) + 1];
        
        while (d < c) {
            coordinates[ ++d ] = GetCursorPosition();
            System.Console.Clear();
            System.Console.Write(coordinates[d].X);
            System.Console.Write(" ");
            System.Console.WriteLine(coordinates[d].Y);
            System.Console.Write("Seconds remaining: {0}", (c-d)* __RES_MS/__SECOND );
 
            System.Threading.Thread.Sleep(__RES_MS);
        }
        return coordinates;
    }

    public static void ReplayMouse(MousePoint[] coordinates) {
        for (int i = 1; i < coordinates.Length; i++)
        {
            mouse_event
                            ((int)MouseEventFlags.Move,
                             (coordinates[i].X - coordinates[i-1].X)/3,
                             (coordinates[i].Y - coordinates[i-1].Y)/3,
                             0,
                             0)
                            ;
            System.Threading.Thread.Sleep(__RES_MS);
        }

    }

    public static void MouseEvent(MouseEventFlags value)
    {
        MousePoint position = GetCursorPosition();

        if (value == MouseEventFlags.Move) {
            mouse_event
                ((int)value,
                 100,
                 100,
                 0,
                 0)
                ;
            mouse_event
                ((int)value,
                 -100,
                 -100,
                 0,
                 0)
                ;
        }
        else
        mouse_event
            ((int)value,
             position.X,
             position.Y,
             0,
             0)
            ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}