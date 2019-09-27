using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int delay = int.Parse(args[0]);
                while (true) {
                    //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                    //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.Move);
                    //Console.WriteLine("Clicked");
                    System.Threading.Thread.Sleep(delay * 1000);
                }
            }
            catch (Exception e) {
                Console.WriteLine("Could not parse argument as numeric");
                Console.ReadKey();
            }
        }
    }
}
