using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Modles;

namespace WebShop.Windows
{
    internal class WindowBrowseProducts
    {


        public static void BrowseProducts(List<Product> products, int productsPerPage)
        {
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
                    windowLeftPos += Helpers.GetProdcutWindowLeftLength(productText) + gapBetweenWindows;
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
                            AddToCart(productsCurrentPage[int.Parse(key) - 1]);
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

        public static void AddToCart(Product product)
        {
            string addToCartKey = "B";
            string windowHeader = "Selected product";
            List<string> productText = Helpers.GetProductTexLongForWindow(product, "Add To Cart" , addToCartKey);

            var productWindow = new Window(windowHeader, 1, 13, productText); //Fix, Postion too hardcoded?
            productWindow.Draw(ConsoleColor.Green);

            string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
            if(key == addToCartKey)
            {
                CartItemServices.AddCartItem(product.Id, Program.myCustomerId);
                Console.WriteLine("  Added to cart");
                Helpers.MsgContinueAnyKey();
                //Add product as cartitem
            }
            
        }
    }
}
