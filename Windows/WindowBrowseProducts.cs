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


        public static void BrowseProducts(List<Product> products, int productsPerPage)
        {
            products = products.Where(p => p.StockAmount > 0).ToList(); //Only display products in stock

            productsPerPage = Math.Clamp(productsPerPage, 1, 8); //Clamp as inputs will be 1-8.
            
            int currentPage = 1;
            //Calculate how many pages there will be based on how many products are displayed
            int maxPages = (products.Count() + productsPerPage -1 ) / productsPerPage; //Update not correct?
            if (maxPages == 0)
                maxPages++; //If no products add 1 to max page to avoid crash if user tries to scroll.
            

            bool isActive = true;
            while (isActive) 
            {
                Console.Clear();
                int gapBetweenWindows = 5;
                int windowLeftPos = 1;
                int windowTopPos = 5;

                //Draw control window
                var controlsWindow = new Window($"Page {currentPage} / {maxPages}", windowLeftPos, 1, new List<string> { "[Q] Previous [E] Next  -  [9] Back " });
                controlsWindow.Draw(ConsoleColor.Yellow);

                //Put products displayed on current page in a list
                List<Product> productsCurrentPage = new List<Product>();
                int productStartIndex = productsPerPage * currentPage;
                for (int i = 0; i < productsPerPage; i++)
                {
                    int productIndex = productStartIndex + i - productsPerPage; //Overcomplicated it?

                    if (productIndex < products.Count())
                    {
                        productsCurrentPage.Add(products[productIndex]);
                    }
                }

                //Draw product windows for current page
                int interactionKey = 1; //Interaction key displayed in product window. 1 up to 8 in this case. Increments in foreachloop
                foreach(Product product in productsCurrentPage)
                {
                    List<string> productText = Helpers.GetProductTextShortForWindow(product, "View more",  interactionKey.ToString());

                    var productWindow = new Window(product.Category.Name, windowLeftPos, windowTopPos, productText);
                    productWindow.Draw(ConsoleColor.Red);

                    //Add spacing to leftPos for next window
                    windowLeftPos += Helpers.GetMaxHorizontalLength(productText) + gapBetweenWindows;
                    interactionKey++;
                }

                //Handle inputs
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                switch (key)
                {
                    //View more --> AddToCart
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        //Viewmore. Make sure selection is in rage of the displayed products
                        if((int.Parse(key)) != 0 && (int.Parse(key)) <= productsCurrentPage.Count)
                            Helpers.ViewMoreWindow(productsCurrentPage[int.Parse(key) - 1]);
                        break;

                    //Navigation keys
                    case "Q":
                        currentPage = Math.Clamp(currentPage -= 1, 1, maxPages);
                        break;

                    case "E":
                        currentPage = Math.Clamp(currentPage += 1, 1, maxPages);
                        break;

                    case "9":
                        isActive = false;
                        break;
                }
            }

        }


        public static void ProductPageByCategory(int categoryId, int productsPerPage)
        {
            int currentPage = 1;

            bool isActive = true;
            while (isActive)
            {
                List<Product> products = CategoryServices.GetCategory(categoryId).Products.ToList(); //Update everyloop. Incase items are no longer in stock
                products = products.Where(p => p.StockAmount > 0).ToList();

                //Get max page count based on how many products to display
                int maxPage = (products.Count() + productsPerPage - 1) / productsPerPage;

                int productStartIndex = productsPerPage * currentPage;

                //Get products to disaply on current page
                List<Product> productsCurrentPage = new List<Product>();
                for (int i = 0; i < productsPerPage; i++)
                {
                    int productIndex = productStartIndex + i - productsPerPage; //Overcomplicated it?

                    //Prevents out of bounds
                    if (productIndex < products.Count())
                    {
                        productsCurrentPage.Add(products[productIndex]);
                    }
                }
                
                //Draw graphics
                Helpers.DrawMenuText("Store - Browse " + currentPage + " / " + (maxPage > 0 ? maxPage: 1), "[Q] Previous [E] Next - [9] Back");
                if(products.Count() > 0)
                    DrawProductWindowsInLine(productsCurrentPage, "View More");
                else
                {
                    var windowNoProducts = new Window("Products", 1, 5, new List<string> { "No products found" });
                    windowNoProducts.Draw(ConsoleColor.Red);
                }

                //Handle Inputs
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int keyIndex = Helpers.GetActionKeyIndex(key);
                
                if (keyIndex != -1 && keyIndex < productsCurrentPage.Count) 
                {
                    Helpers.ViewMoreWindow(productsCurrentPage[keyIndex]);
                }
                else if(key == "Q" && products.Count > 0)
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

        public static void ProductPageByList(List<Product> products, int productsPerPage)
        {
            int currentPage = 1;

            bool isActive = true;
            while (isActive)
            {
                products = products.Where(p => p.StockAmount > 0).ToList();

                //Get max page count based on how many products to display
                int maxPage = (products.Count() + productsPerPage - 1) / productsPerPage;

                int productStartIndex = productsPerPage * currentPage;

                //Get products to disaply on current page
                List<Product> productsCurrentPage = new List<Product>();
                for (int i = 0; i < productsPerPage; i++)
                {
                    int productIndex = productStartIndex + i - productsPerPage; //Overcomplicated it?

                    //Prevents out of bounds
                    if (productIndex < products.Count())
                    {
                        productsCurrentPage.Add(products[productIndex]);
                    }
                }

                //Draw graphics
                Helpers.DrawMenuText("Store - Browse " + currentPage + " / " + (maxPage > 0 ? maxPage : 1), "[Q] Previous [E] Next - [9] Back");
                if (products.Count() > 0)
                    DrawProductWindowsInLine(productsCurrentPage, "View More");
                else
                {
                    var windowNoProducts = new Window("Products", 1, 5, new List<string> { "No products found" });
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

        public static void DrawProductWindowsInLine(List<Product> products, string productActionText)
        {

            int gapBetweenWindows = 5;
            int windowLeftPos = 1;
            int windowTopPos = 5;

            for (int i = 0; i < products.Count; i++) 
            {
                List<string> productText = Helpers.GetProductTextShortForWindow(products[i], productActionText, Helpers.GetActionKeys()[i]);

                var productWindow = new Window(products[i].Category.Name, windowLeftPos, windowTopPos, productText);
                productWindow.Draw(ConsoleColor.Red);

                windowLeftPos += Helpers.GetMaxHorizontalLength(productText) + gapBetweenWindows; //Caluclate where to put next window in line
            }
        }


    }
}
