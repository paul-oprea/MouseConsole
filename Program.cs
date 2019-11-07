using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseConsole
{
    class Program
    {
        private Dictionary<string, string> cmdLineArgs;
        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> hashMap = new Dictionary<string, string>();
            foreach (string s in args)
            {
                string[] kv = s.Split("=".ToCharArray()[0]);
                if (kv.Length > 1 && !hashMap.ContainsKey(kv[0]))
                    hashMap.Add(kv[0], kv.Length < 2 ? null : kv[1]);
            }
            return hashMap;
        }

        static void Main(string[] args)
        {
            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.WindowWidth = 180;
            Console.WindowHeight = 32;

            Program instance = new Program();
            instance.cmdLineArgs = ParseArgs(args);

            if (instance.cmdLineArgs.ContainsKey("mode"))
            {
                switch (instance.cmdLineArgs["mode"].ToLower())
                {
                    case "replay":
                        int duration = 10;
                        if (instance.cmdLineArgs.ContainsKey("duration"))
                            duration = int.Parse(instance.cmdLineArgs["duration"]);
                        MouseOperations.ReplayMouse(
                        MouseOperations.CaptureMouse(duration * MouseOperations.__SECOND));

                        break;
                    case "parametric":
                        MouseOperations.MouseMovementSequence();
                        break;
                    case "antitimeout":
                        try
                        {
                            int delay = 10;
                            if (instance.cmdLineArgs.ContainsKey("wait"))
                                delay = int.Parse(instance.cmdLineArgs["wait"]);
                            while (true)
                            {
                                //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                                //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.Move);
                                //Console.WriteLine("Clicked");
                                System.Threading.Thread.Sleep(delay * MouseOperations.__SECOND);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not parse argument as numeric");
                            Console.ReadKey();
                        }
                        break;
                    default:
                        break;
                }
            }
            else
                System.Console.WriteLine("" +
                    "Usage:\n" +
                    "\tMouseConsole command=value [option1=value] [option2=value]..." +
                    "\n\tcommands:" +
                    "\n\t\tmode=[replay | parametric | antitimeout]" +
                    "\n\t\t\t- replay captures the mouse behaviour for a specified " +
                    "duration then plays it back;" +
                    "\n\t\t\t- parametric emulates movement along a curve" +
                    "\n\t\t\t- timeout will prevent screensaver by a small wriggle " +
                    "every \"wait\" seconds" +
                    "\n" +
                    "\n\toptions:" +
                    "\n\t\twait = n seconds\tseconds between mouse actions" +
                    "\n\t\tduration = n seconds\tseconds of mouse behaviour to capture then replay");

            System.Console.ReadKey();

        }
    }
}
