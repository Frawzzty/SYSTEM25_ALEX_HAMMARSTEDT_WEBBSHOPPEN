using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebShop.Migrations;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowStatistics
    {
        private static int tableRowLimit = 10; //Limit how many rows windows will display

        public static void Draw()
        {
            Helpers.DrawMenuText("Statistics", "[Any key - Go Back]");

            int leftGap = 8;

            int leftPos = 1;
            int topPos = 5;

            Window WinTotalRevenue = WindowTotalRevenue();

            Window WinRevenueLastHour = WindowRevenueLastHour();
            List<Window> windowsRevenue = new List<Window>() { WinTotalRevenue, WinRevenueLastHour };
            Window.DrawWindowsInRow(windowsRevenue, 1, topPos, 1);


            Window WinProdcutsRevenue = WindowBestSellers(1);
            Window WinProdcutsUnitsSold = WindowBestSellers(0);
            List<Window> windowRowProducts = new List<Window>() { WinProdcutsRevenue, WinProdcutsUnitsSold };
            Window.DrawWindowsInRow(windowRowProducts, 1, topPos += (Window.GetWindowVerticalLength(WinTotalRevenue) + 1), 1);

            Window WinCountryRevenue = WindowBestLocation(1);
            Window WinCityRevenue = WindowBestLocation(0);
            List<Window> windowRowLocation = new List<Window>() { WinCountryRevenue, WinCityRevenue };
            Window.DrawWindowsInRow(windowRowLocation, 1, topPos += Window.GetWindowVerticalLength(WinProdcutsRevenue) + 1, 1);


            Window WinCategoryRevenue = WindowBestCategoryRevenue();
            Window WinCategoryRevenue2 = WindowBestCategoryUnitsSold();
            List<Window> windowRowCategory = new List<Window>() { WinCategoryRevenue, WinCategoryRevenue2 };
            Window.DrawWindowsInRow(windowRowCategory, 1, topPos += Window.GetWindowVerticalLength(WinCategoryRevenue) + 1, 1);


            Window WinLast7Days = WindowLast7Days();
            List<Window> WindowDailyBreakdow = new List<Window>() { WinLast7Days };
            Window.DrawWindowsInRow(WindowDailyBreakdow, 1, topPos += Window.GetWindowVerticalLength(WinCategoryRevenue) + 1, 1);

            //Category breakdown?


            //Shipping method popularity?

            //Monthly Revenue breakdown?


        }


        // WINDOWS
        private static Window WindowTotalRevenue()
        {
            string header = "Total Revenue";

            decimal value = DapperServices.GetShopRevenueTotal();
            List<string> textRows = new List<string>() { (value.ToString("N0")) + " SEK" };
            
            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Green;

            return window;
        }

        private static Window WindowRevenueLastHour()
        {
            string header = "Total Revenue -1 Hour";

            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddHours(-1);
            
            decimal value = DapperServices.GetShopRevenueL30D(startDate, endDate);
            
            List<string> textRows = new List<string>() { (value.ToString() + " SEK" )};

            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Green;

            return window;
        }

        ///<summary> int table == 1; Orders by revenue else units sold</summary>
        /// <returns>Windows horizontal length</returns>
        private static Window WindowBestSellers(int table)
        {
            var orderDetails = OrderDetailServices.GetAllOrderDetails();
            var groups = orderDetails //Group by product ID. And order by revenue
                .GroupBy(od => od.ProductId);

            string header = "";
            if (table == 1)
            {
                header = "Best Selling - Revenue";
                groups = groups.OrderByDescending(group => group.Sum(od => od.SubTotal));
            }
            else
            {
                header = "Best Selling - Units Sold";
                groups = groups.OrderByDescending(group => group.Sum(od => od.UnitAmount));
            }

            //Limit to max rows;
            groups = groups.Take(tableRowLimit);

            //Get padding for to columns
            int padUnitsSold = 0;
            int padRevenue = 0;

            if(orderDetails.Count > 0)
            {
                padUnitsSold = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.UnitAmount.ToString().Length)), 4);
                padRevenue = Helpers.GetHeaderMaxPadding(" ", groups.Max(group => group.Max(od => od.SubTotal.ToString().Length)), 2);
            }


            //Create text row for window
            List<string> textRows = new List<string>();
            if(orderDetails.Count > 0)
            {
                foreach (var group in groups)
                {
                    int unitsSold = group.Sum(g => g.UnitAmount);
                    decimal revenue = group.Sum(g => g.SubTotal);

                    textRows.Add(revenue.ToString("N0").PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Product.Name);
                }
            }
            else
            {
                textRows.Add("Table empty");
            }


                //Draw window
                var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Yellow;
 

            //Return how much space the window take up to the left. For drawing In-line
            return window;
        }


        /// <summary> int table == 1; Country table else City table </summary>
        /// <returns>Windows horizontal length</returns>
        private static Window WindowBestLocation(int table)
        {

            var orderDetails = OrderDetailServices.GetAllOrderDetails();
            IOrderedEnumerable<IGrouping<string, OrderDetail>> groups;

            string header = "";
            if (table == 1)
            {
                groups = orderDetails.GroupBy(od => od.Order.Country)
                    .OrderByDescending(group => group.Sum(od => od.SubTotal));

                header = "Country - Revenue";
                
            }
            else
            {
                groups = orderDetails.GroupBy(od => od.Order.City)
                    .OrderByDescending(group => group.Sum(od => od.SubTotal));

                header = "City - Revenue";
            }
            
            
            //Limit to max rows;
            var groupsLimited = groups.Take(tableRowLimit);

            int padUnitsSold = 0;
            int padRevenue = 0;
            int padCountry = 0;
            int padCity = 0;
            //Get padding for to columns
            if (orderDetails.Count > 0)
            {
                padUnitsSold = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.UnitAmount.ToString().Length)), 4);
                padRevenue = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.SubTotal.ToString().Length)), 2);
                padCountry = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.Order.Country.ToString().Length)), 2);
                padCity = Helpers.GetHeaderMaxPadding(" ", groupsLimited.Max(group => group.Max(od => od.Order.City.ToString().Length)), 2);
            }


            //Create text row for window
            List<string> textRows = new List<string>();
            if(orderDetails.Count > 0)
            {
                foreach (var group in groupsLimited)
                {
                    int unitsSold = group.Sum(g => g.UnitAmount);
                    decimal revenue = group.Sum(g => g.SubTotal);

                    if (table == 1)
                    {
                        textRows.Add(revenue.ToString("N0").PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Order.Country.PadRight(padCountry));
                    }
                    else
                    {
                        textRows.Add(revenue.ToString("N0").PadRight(padRevenue) + (unitsSold + "x").PadRight(padUnitsSold) + group.First().Order.Country.PadRight(padCountry) + group.First().Order.City.PadRight(padCity));

                    }
                }
            }
            else
            {
                textRows.Add("Table empty");
            }

            //Draw window
            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Yellow;

            //Return how much space the window take up to the left. For drawing In-line
            return window;
        }



        private static Window WindowBestCategoryRevenue()
        {

            List<string> textRows = new List<string>();
            using (var db = new WebShopContext())
            {
                var groups = db.OrderDetails
                    .Include(od => od.Product).ThenInclude(p => p.Category)
                    .ToList()
                    .GroupBy(od => od.Product.Category.Name)
                    .OrderByDescending(group => group.Sum(od => od.SubTotal));

                foreach (var group in groups)
                {
                    textRows.Add((group.Sum(g => g.SubTotal).ToString("N0").PadRight(15) + " " + group.First().Product.Category.Name));
                }

                if (textRows.Count == 0)
                    textRows.Add(" ");
             
            }

            string header = "Category Revenue";
            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Yellow;

            return window;
        }

        private static Window WindowBestCategoryUnitsSold()
        {

            List<string> textRows = new List<string>();
            using (var db = new WebShopContext())
            {
                var groups = db.OrderDetails
                    .Include(od => od.Product).ThenInclude(p => p.Category)
                    .ToList()
                    .GroupBy(od => od.Product.Category.Name)
                    .OrderByDescending(group => group.Sum(od => od.UnitAmount));

                foreach (var group in groups)
                {
                    textRows.Add((group.Sum(g => g.UnitAmount) + "x").PadRight(15) + group.First().Product.Category.Name);
                }

                if (textRows.Count == 0)
                    textRows.Add(" ");

            }

            string header = "Category Units Sold";
            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Yellow;

            return window;
        }

        private static Window WindowLast7Days()
        {
            List<DateOnly> dates = Helpers.GetDates(7);

            List<string> textRows = new List<string>();
            using (var db = new WebShopContext())
            {
                var orders = db.Orders
                    .Include(o => o.OrderDetails)
                    .ToList()
                    .OrderByDescending(o => o.SubTotal);

                foreach(var date in dates)
                {
                    var selectedOrders = orders //Select where date is in between
                        .Where(g => g.OrderDate >= date.ToDateTime(TimeOnly.MinValue))
                        .Where(g => g.OrderDate < date.AddDays(1).ToDateTime(TimeOnly.MinValue));

                    decimal subTotal = selectedOrders.Sum(o => o.SubTotal);
                    if (subTotal > 0)
                    {
                        textRows.Add(date.ToString().PadRight(12) + subTotal.ToString("N0"));
                    }
                    else
                    {
                        textRows.Add(date.ToString().PadRight(12) + "0");
                    }
                    
                }

                if (textRows.Count == 0)
                    textRows.Add(" ");

            }

            string header = "Revenue Last 7 Days";
            var window = new Window(header, 0, 0, textRows);
            window.headerColor = ConsoleColor.Yellow;

            return window;
        }

    }
}
