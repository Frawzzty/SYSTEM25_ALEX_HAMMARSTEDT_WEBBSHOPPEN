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
            Helpers.DrawMenuText("Statistics", "[Any key] Go Back");

            int leftGap = 8;

            int leftPos = 1;
            int topPos = 5;

            Window WinTotalRevenue = WindowTotalRevenue();
            WinTotalRevenue.Left = leftPos; WinTotalRevenue.Top = topPos;
            WinTotalRevenue.Draw();
            
            Window WinProdcutsRevenue = WindowBestSellers(1);
            Window WinProdcutsUnitsSold = WindowBestSellers(0);
            List<Window> windowRowProducts = new List<Window>() { WinProdcutsRevenue, WinProdcutsUnitsSold };
            Window.DrawWindowsInRow(windowRowProducts, 1, topPos += (Window.GetWindowVerticalLength(WinTotalRevenue) + 1), 1);

            Window WinCountryRevenue = WindowBestLocation(1);
            Window WinCityRevenue = WindowBestLocation(0);
            List<Window> windowRowLocation = new List<Window>() { WinCountryRevenue, WinCityRevenue };
            Window.DrawWindowsInRow(windowRowLocation, 1, topPos + Window.GetWindowVerticalLength(WinProdcutsRevenue) + 1, 1);
        }


        // WINDOWS
        private static Window WindowTotalRevenue()
        {
            string header = "Total Revenue";

            decimal bestSellerValue = DapperServices.GetShopRevenue();
            List<string> textRows = new List<string>() { (bestSellerValue.ToString("N0")) + " SEK" };
            
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





    }
}
