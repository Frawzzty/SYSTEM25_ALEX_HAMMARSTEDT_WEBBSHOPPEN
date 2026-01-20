using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowStatistics
    {
        private static int tableRowLimit = 10; //Limit how many rows windows will display

        public static void Draw()
        {
            Helpers.DrawMenuText("Statistics", "Any key to go back");

            int leftGap = 8;

            int leftPos = 1;
            int topPos = 5;

            WinShopRevenue(leftPos, topPos);


            leftPos = 1;
            topPos = topPos + 4;
            leftPos += WinBestSellers(leftPos, topPos, 1) + 5;



            WinBestSellers(leftPos, topPos, 0);

            leftPos = 1;
            topPos = topPos + 12;
            leftPos += WinBestLocation(leftPos, topPos, 1) + 5;

            WinBestLocation(leftPos, topPos, 0);
        }


        // WINDOWS
        private static int WinShopRevenue(int leftPos, int topPos)
        {
            string header = "Total Revenue";

            decimal bestSellerValue = DapperServices.GetShopRevenue();
            List<string> textRows = new List<string>() { (bestSellerValue.ToString("N0")) + " SEK" };
            

            var window = new Window(header, leftPos, topPos, textRows);
            window.Draw(ConsoleColor.Green);

            return Helpers.GetMaxHorizontalLength(textRows); //Return how much space window takes up. For drawing window in-line
        }

        ///<summary> int table == 1; Orders by revenue else units sold</summary>
        /// <returns>Windows horizontal length</returns>
        private static int WinBestSellers(int leftPos, int topPos, int table)
        {
            var orderDetails = OrderDetailServices.GetAllOrderDetails();
            var groups = orderDetails //Group by product ID. And order by revenue
                .GroupBy(od => od.ProductId);

            string header = "";
            if (table == 1)
            {
                header = "Best Selling - Revenue";
                groups = groups.OrderByDescending(group => group.Sum(od => od.Price));
            }
            else
            {
                header = "Best Selling - Units Sold";
                groups = groups.OrderByDescending(group => group.Sum(od => od.UnitAmount));
            }

            //Limit to max rows;
            groups = groups.Take(tableRowLimit); 

            //Get padding for to columns
            int padUnitsSold = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.UnitAmount.ToString().Length)), 4);
            int padRevenue = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.Price.ToString().Length)), 2);

            //Create text row for window
            List<string> textRows = new List<string>();
            foreach (var group in groups)
            {
                int unitsSold = group.Sum(g => g.UnitAmount);
                decimal revenue = group.Sum(g => g.Price);

                textRows.Add(revenue.ToString().PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Product.Name);
            }

            //Draw window
            var window = new Window(header, leftPos, topPos, textRows);
            window.Draw(ConsoleColor.Yellow);

            //Return how much space the window take up to the left. For drawing In-line
            return Helpers.GetMaxHorizontalLength(textRows);
        }


        /// <summary> int table == 1; Country table else City table </summary>
        /// <returns>Windows horizontal length</returns>
        private static int WinBestLocation(int leftPos, int topPos, int table)
        {

            var orderDetails = OrderDetailServices.GetAllOrderDetails();
            IOrderedEnumerable<IGrouping<string, OrderDetail>> groups;

            string header = "";
            int padLocation = 0;
            if (table == 1)
            {
                groups = orderDetails.GroupBy(od => od.Order.Country)
                    .OrderByDescending(group => group.Sum(od => od.Price));

                header = "Country - Revenue";
                padLocation = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.Order.Country.Length)), 2);
            }
            else
            {
                groups = orderDetails.GroupBy(od => od.Order.City)
                    .OrderByDescending(group => group.Sum(od => od.Price));

                header = "City - Revenue";
                padLocation = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.Order.City.Length)), 2);
            }

            //Limit to max rows;
            var groupsLimited = groups.Take(tableRowLimit);

            //Get padding for to columns
            int padUnitsSold = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.UnitAmount.ToString().Length)), 4);
            int padRevenue = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.Price.ToString().Length)), 2);
            int padCountry = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.Order.Country.ToString().Length)), 2);
            int padCity = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.Order.City.ToString().Length)), 2);

            //Create text row for window
            List<string> textRows = new List<string>();
            foreach (var group in groupsLimited)
            {
                int unitsSold = group.Sum(g => g.UnitAmount);
                decimal revenue = group.Sum(g => g.Price);

                if(table == 1)
                {
                    textRows.Add(revenue.ToString().PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Order.Country.PadRight(padCountry));
                }
                else
                {
                    textRows.Add(revenue.ToString().PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Order.Country.PadRight(padCountry) + group.First().Order.City.PadRight(padCity));

                }

            }

            //Draw window
            var window = new Window(header, leftPos, topPos, textRows);
            window.Draw(ConsoleColor.Yellow);

            //Return how much space the window take up to the left. For drawing In-line
            return Helpers.GetMaxHorizontalLength(textRows);
        }




    }
}
