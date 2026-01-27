using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Models;

namespace WebShop.Windows
{
    internal class WindowHome
    {


        public static void DrawHome()
        {
            //Welcome window vars
            int leftPos = 1;
            int topPos = 5;

            Customer customer = CustomerServices.GetCustomerById(Settings.GetCurrentCustomerId()); //Get current active customer

            //Store welcome message
            List<string> welcomeWindowList = new List<string> { $"Welcome {customer.Name}", "We sell the best clothes!", new string(' ', 41) }; //Also add some spaces to make it take up more space.
            var windowWelcome = new Window("The Clothing Store", 0,0, welcomeWindowList);
            windowWelcome.headerColor = ConsoleColor.Blue;

            //News feed
            var windowNewsFeed = new Window("News", 0, 0, Settings.GetNewsFeedText());
            windowNewsFeed.headerColor = ConsoleColor.Yellow;

            //Draw Window
            List<Window> welcomeAndNewsFeed = new List<Window>() {windowWelcome, windowNewsFeed};
            Window.DrawWindowsInRow(welcomeAndNewsFeed, leftPos, topPos, 1);
        }
    }
}
