using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;

namespace WebShop.Windows
{
    internal class CategoryPage
    {


        public static void DrawCategoryPage(List<Product> products, int productsPerPage)
        {
            productsPerPage = Math.Clamp(productsPerPage, 1, 8); //Clamp as inputs will be 1-8.

            int currentPage = 1;
            int maxPages = (products.Count() + productsPerPage -1 ) / productsPerPage; //Update not correct?
            if (maxPages == 0)
                maxPages++; //If no products add 1 to max page to avoid crash if user tries to scroll.
            

            bool isActive = true;
            while (isActive) 
            {
                Console.Clear();
                int gapBetweenWindows = 5;
                int topPos = 5;
                int leftPos = 1;

                //Draw control window
                var controlsWindow = new Window($"Page {currentPage} / {maxPages}", leftPos, 1, new List<string> { "[Q] Previous [E] Next  -  [9] Back " });
                controlsWindow.Draw(ConsoleColor.Yellow);

                //Get Products for current page
                List<Product> productsCurrentPage = new List<Product>();
                int productStartIndex = productsPerPage * currentPage;
                for (int i = 0; i < productsPerPage; i++)
                {
                    int productIndex = productStartIndex + i - productsPerPage; //Overcomplicated it?

                    if (productIndex < products.Count())
                        productsCurrentPage.Add(products[productIndex]);
                }

                //Draw product windows for current page
                int interactionKey = 1; //Interaction key displayed in product window. 1 up to 8 in this case
                foreach(Product product in productsCurrentPage)
                {
                    List<string> productText = Helpers.GetWindowProductText(product, interactionKey.ToString());

                    var productWindow = new Window(product.Category.Name, leftPos, topPos, productText);
                    productWindow.Draw(ConsoleColor.Red);

                    //Add spacing to leftPos for next window
                    leftPos += Helpers.GetProdcutWindowLeftLength(productText) + gapBetweenWindows;
                    interactionKey++;
                }

                //Handle inputs
                string key = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                switch (key)
                {
                    //Add to cart / view more
                    case "1": //ATC product [0]
                    case "2": //ATC product [1]
                    case "3": //ATC product [2]
                    case "4": //ATC product [3]
                    case "5": //ATC product [4]
                    case "6": //ATC product [5]
                    case "7": //ATC product [6]
                    case "8": //ATC product [7]
                        //AddToCart(productsCurrentPage[key - 1] //Todo
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
    }
}
