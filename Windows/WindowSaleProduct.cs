using WebShop.Models;

namespace WebShop.Windows
{
    internal class WindowSaleProduct
    {
   
        public static void DrawProductWindows(List<Product> products, int windowCount, List<string> actionKeys)
        {
            //Sort products? Revenu or volume sold.

            products = products.Take(windowCount).ToList(); //Limit List count to amount of windows

            int windowLeftPos = 1;
            int windowTopPos = 12;
            int windowSpacing = 5;

            int index = 0; //used for displaying correct action key
            foreach (var product in products) 
            {
                List<string> windowText = new List<string> { product.Name, product.Description, product.UnitSalePrice.ToString() + " SEK", "Stock: " + product.StockAmount, $"Add to Cart [{actionKeys[index]}]" };

                var window = new Window("Sale", windowLeftPos, windowTopPos, windowText);
                window.Draw(ConsoleColor.Red);
                windowLeftPos += Helpers.GetMaxHorizontalLength(windowText) + windowSpacing; //Add current window to left pos to create spacing
                index++;
                //Console.WriteLine(product.Name + ": " + product.OnSale);
            }
        }
    }
}
