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
            int leftPosWelcome = 1;
            string headerWelcome = "The Clothing Store";  int welcomeTopPos = 5;
            Customer customer = CustomerServices.GetCustomerById(Settings.GetCurrentCustomerId()); //Get current active customer

            //Store welcome message
            List<string> welcomeWindowList = new List<string> { $"Welcome {customer.Name}", "We sell the best clothes!", new string(' ', 41) }; //Also add som epmpty string to make it take up more space.
            var windowWelcome = new Window(headerWelcome, leftPosWelcome, welcomeTopPos, welcomeWindowList);
            windowWelcome.Draw(ConsoleColor.Blue);



            //Newsfeed window vars
            int leftPosNewsFeed = Helpers.GetMaxHorizontalLength(welcomeWindowList) + 6; // +6 for borders
            string headerNewsFeed = "News"; int newsFeedTopPos = 5;

            //News feed
            List<string> newsFeedWindowList = new List<string> { "* Winter Sale is now Active", "* Up to 50% off", "* Newly restocked" };
            var windowNewsFeed = new Window(headerNewsFeed, leftPosNewsFeed, newsFeedTopPos, newsFeedWindowList);
            windowNewsFeed.Draw(ConsoleColor.Yellow);

        }
    }
}
