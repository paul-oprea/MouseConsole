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
            if ( args.Length == 0 )
                SequencedCapture();
            
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
        /// <summary> Performs a sequence of screen captures </summary>
        /// <para>* This function is hand-written for a particular scenario and it is only interesting for the bits of code inside, not so much for reuse</para>
        static void SequencedCapture(){
            for( int i = 0; i < 200; i++){
                ScreenOperations.ImageSave("%NOW%"
                    , System.Drawing.Imaging.ImageFormat.Png
                    , ScreenOperations.CaptureWindow(80, 297, 2040, 2850));
                    
                System.Threading.Thread.Sleep( 2000 );
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                    MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
            } 
        }
    }
}
