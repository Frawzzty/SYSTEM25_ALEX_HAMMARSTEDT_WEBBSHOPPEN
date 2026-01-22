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
        /// T: Either List Prodcut or CategoryId
        /// </summary>

        public static void BroweProducts <T>(T item, int amountProductsPerPage)
        {
            int currentPage = 1;

            bool isActive = true;
            while (isActive)
            {
                List<Product> products = new List<Product>();

                //Check item Type
                if (item is int) //Is Category ID
                {
                    int categoryId = (int)(object)item;
                    products = CategoryServices.GetCategory(categoryId).Products.ToList();
                }
                else if (item is List<Product>) //Is list with products
                {
                    products = item as List<Product>;
                }

                //Select only items in stock
                products = products.Where(p => p.StockAmount > 0).ToList();

                //Used in menu graphics:  Get max page count based on how many products to display
                int maxPage = (products.Count() + amountProductsPerPage - 1) / amountProductsPerPage;
                

                //Get products to disaply on current page
                List<Product> productsCurrentPage = new List<Product>();
                int productStartIndex = amountProductsPerPage * currentPage;
                for (int i = 0; i < amountProductsPerPage; i++)
                {
                    int productIndex = productStartIndex + i - amountProductsPerPage; //Overcomplicated it?

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
                
                if (productsCurrentPage.Count() > 0) //Products found
                {
                    DrawProductWindows(productsCurrentPage, "view More", leftPos, topPos);
                }
                else //No prodcuts found
                {
                    var windowNoProducts = new Window("Products", leftPos, topPos, new List<string> { "No products found" });
                    windowNoProducts.Draw(ConsoleColor.Red);
                }

                //Handle Inputs
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int keyIndex = Helpers.GetActionKeyIndex(key);

                //User selected a view more key
                if (keyIndex != -1 && keyIndex < productsCurrentPage.Count) 
                {
                    Helpers.ViewMoreWindow(productsCurrentPage[keyIndex]);
                }
                //Page scroll Left
                else if (key == "Q" && products.Count > 0)
                {
                    currentPage = Math.Clamp(currentPage -= 1, 1, maxPage);
                }
                //Page scroll Right
                else if (key == "E" && products.Count > 0)
                {
                    currentPage = Math.Clamp(currentPage += 1, 1, maxPage);
                }
                //Go back
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
