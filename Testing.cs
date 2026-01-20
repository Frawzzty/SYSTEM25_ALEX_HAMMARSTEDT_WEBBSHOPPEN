using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
    internal class Testing
    {

        public static void WindowTesting()
        {
            var window1 = new Window("Heaaasdasdasd", 1, 1, new List<string>()
            {
                "J",
                " ",
                "ä"

            });

            window1.headerColor = ConsoleColor.Red;


            var window2 = new Window("Haar", 1, 1, new List<string>()
            {
                "Ja",
                "Jaaaaaag",
                "ämt"

            });
            window2.headerColor = ConsoleColor.Green;

            var window3 = new Window("Headeasdasdasr", 1, 1, new List<string>()
            {
                "Ja",
                "Jaaaaaag",
                "ämt"

            });
            window3.headerColor = ConsoleColor.Blue;

            List<Window> windows = new List<Window>()
            { 
                window1,
                window2,
                window3
            };


            int vPos = 20;
            var window5 = new Window("Heaaasdasdasd", 1, 20, new List<string>()
            {
                "J",
                " ",
                "ä"

            });

            Window.DrawWindowsInColumn(windows, 1, 1, 0);
            Window.DrawWindowsInColumn(windows, 1, 10, 0);
            

            Console.ReadKey(true);

        }
    }
}
