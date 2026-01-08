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
            int leftPosWelcome = 1;
            string headerWelcome = "The Clothing Store";  int welcomeTopPos = 3;

            List<string> welcomeWindowList = new List<string> { "Welcome 'NAME?',", "We sell the best clothes...", new string(' ', 41) };
            var windowWelcome = new Window(headerWelcome, leftPosWelcome, welcomeTopPos, welcomeWindowList);
            windowWelcome.Draw(ConsoleColor.Blue);

            int leftPosNewsFeed = 1 + 46;
            string headerNewsFeed = "News"; int newsFeedTopPos = 3;

            List<string> newsFeedWindowList = new List<string> { "* Winter Sale is now Active", "* Up to 50% off", "* Newly restocked" };
            var windowNewsFeed = new Window(headerNewsFeed, leftPosNewsFeed, newsFeedTopPos, newsFeedWindowList);
            windowNewsFeed.Draw(ConsoleColor.Yellow);

        }
    }
}
