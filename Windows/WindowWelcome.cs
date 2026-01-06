using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Windows
{
    internal class WindowWelcome
    {


        public static void Draw()
        {
            int leftPos = 1;
            string headerWelcome = "Clothing.com";  int welcomeTopPos = 3;

            List<string> welcomeWindowList = new List<string> { "Welcome,", "Super cool clothes","" };
            var windowWelcome = new Window(headerWelcome, leftPos, welcomeTopPos, welcomeWindowList);
            windowWelcome.Draw(ConsoleColor.Blue);

        }
    }
}
