using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowBrowseProducts
    {

        /// <summary>
        /// Insert List Prodcut or CategoryId
        /// </summary>

        public static void BroweProducts <T>(T item, int amountPerPage)
        {
            int currentPage = 1;

            bool isActive = true;
            while (isActive)
            {
                List<Product> products = new List<Product>();

                //Check item Type
                if (item is int)
                {
                    int categoryId = (int)(object)item;
                    products = CategoryServices.GetCategory(categoryId).Products.ToList();
                }
                else if (item is List<Product>)
                {
                    products = item as List<Product>;
                }

                //Filter items in stock
                products = products.Where(p => p.StockAmount > 0).ToList();

                //Get max page count based on how many products to display
                int maxPage = (products.Count() + amountPerPage - 1) / amountPerPage;
                int productStartIndex = amountPerPage * currentPage;

                //Get products to disaply on current page
                List<Product> productsCurrentPage = new List<Product>();
                for (int i = 0; i < amountPerPage; i++)
                {
                    int productIndex = productStartIndex + i - amountPerPage; //Overcomplicated it?

                    //Prevents out of bounds
                    if (productIndex < products.Count())
                    {
                        productsCurrentPage.Add(products[productIndex]);
                    }
                }

                //Draw graphics
                Helpers.DrawMenuText("Store - Browse " + currentPage + " / " + (maxPage > 0 ? maxPage : 1), "[Q] Previous [E] Next - [9] Back");
                int leftPos = 1;
                int topPos = 5;
                if (productsCurrentPage.Count() > 0)
                {
                    DrawProductWindows(productsCurrentPage, "view More", leftPos, topPos);
                }
                else
                {
                    var windowNoProducts = new Window("Products", leftPos, topPos, new List<string> { "No products found" });
                    windowNoProducts.Draw(ConsoleColor.Red);
                }

                //Handle Inputs
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int keyIndex = Helpers.GetActionKeyIndex(key);

                if (keyIndex != -1 && keyIndex < productsCurrentPage.Count)
                {
                    Helpers.ViewMoreWindow(productsCurrentPage[keyIndex]);
                }
                else if (key == "Q" && products.Count > 0)
                {
                    currentPage = Math.Clamp(currentPage -= 1, 1, maxPage);
                }
                else if (key == "E" && products.Count > 0)
                {
                    currentPage = Math.Clamp(currentPage += 1, 1, maxPage);
                }
                else if (key == "9")
                {
                    //Go back
                    isActive = false;
                }

                Console.Clear();
            }
        }

        private static void DrawProductWindows(List<Product> products, string actionText, int leftPos, int topPos)
        {
       
            List<Window> windows = new List<Window>();

            //Create windows to display
            int i = 0;
            foreach (Product product in products) 
            {
                string header = product.Category.Name;
                List<string> textRows = Helpers.GetProductTextShortForWindow(product, actionText, Helpers.GetActionKeys()[i]);

                Window window = new Window(header, 0, 0, textRows);
                window.headerColor = ConsoleColor.Red;

                windows.Add(window);
                i++;
            }
            //Draw windows
            Window.DrawWindowsInRow(windows, leftPos, topPos, 1);
        }


    }
}
