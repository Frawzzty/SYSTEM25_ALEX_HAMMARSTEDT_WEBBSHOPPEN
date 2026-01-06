using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.Windows
{
    internal class WindowSaleProduct
    {
        public static void Draw()
        {
            List<Product> productsOnSale = new List<Product>();
            using (var db = new WebShopContext()) 
            {
                productsOnSale = db.Products.Where(p => p.OnSale == true).ToList();
            }

            DrawProductWindows(productsOnSale, 10);

        }

        static void DrawProductWindows(List<Product> productsOnSale, int topPos)
        {
            string windowHeader = "Special offer";
            int leftPos = 1;
            string[] offerKeys = { "Z", "X", "C", "V", "B", "N", "M" };
            int i = 0;
            foreach (var product in productsOnSale)
            {
                List<string> offerText = new List<string> { product.Name, product.Description, product.UnitSalePrice.ToString() + " SEK", $"Add to Cart [{offerKeys[i]}]" };
                var windowOffer = new Window(windowHeader, leftPos, topPos, offerText);
                windowOffer.Draw(ConsoleColor.Red);
                leftPos += 25;

                i++;
                //Dont draw more than 3x Sale windows
                if (i > 3)
                    break;

            }
        }
    }
}
