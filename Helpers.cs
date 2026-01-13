using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Enums;
using WebShop.Modles;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop
{
    internal class Helpers
    {
        //Console.Write with color input
        public static void WriteInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }

        //Console.WriteLine with color input
        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }

        //Message methods for commonly occuring messages
        public static void MsgLeavingAnyKey()
        {
            Console.WriteLine("Leaving, nothing updated... Any key to continue");
            Console.ReadKey(true);
        }

        public static void MsgContinueAnyKey()
        {
            Console.WriteLine("Any key to continue...");
            Console.ReadKey(true);
        }

        public static void MsgBadInputsAnyKey()
        {
            Console.WriteLine("Bad inputs. Any key to continue...");
            Console.ReadKey(true);
        }

        public static int GetHeaderMaxPadding(string header, int textLength, int columnSpacing)
        {
            int padding = 0;
            if(textLength >= header.Length)
            {
                padding = textLength;
            }
            else
            {
                padding = header.Length;
            }
            return padding + columnSpacing;
        }

        //Windows
        //Gets the max text length of the windows text. Used in calculating the spacing of windows that are displayed in-line.
        public static int GetProdcutWindowLeftLength(List<string> windowTexts)
        {
            int windowSize = 0;
            foreach (string windowText in windowTexts) 
            {
                if (windowText.Length >= windowSize)
                {
                    windowSize = windowText.Length;
                }
            }
            
            return windowSize;
        }

        //Returns List used when creating windows
        public static List<string> GetProductTextShortForWindow(Product product, string actionText, string actionKey)
        {
            List<string> productWindowTexts = new List<string>();
            productWindowTexts.Add(product.Name);
            productWindowTexts.Add(product.UnitPrice.ToString() + " SEK");
            if (product.OnSale == true && product.UnitSalePrice > 0)
            {
                productWindowTexts.Add("Sale: " + product.UnitSalePrice.ToString() + " SEK");
            }
            else
            {
                productWindowTexts.Add(""); //Add empty line to keep windows equal size
            }
            productWindowTexts.Add($"{actionText} [{actionKey}]");

            return productWindowTexts;
        }

        public static List<string> GetProductTexLongForWindow(Product product, string actionText, string actionKey)
        {
            List<string> productWindowTexts = new List<string>();
            productWindowTexts.Add(product.Category.Name + " / " + product.SupplierName);
            productWindowTexts.Add(product.Name);
            productWindowTexts.Add(product.Description);
            productWindowTexts.Add(" ");
            productWindowTexts.Add("Amount in stock: " + product.StockAmount);
            productWindowTexts.Add(product.UnitPrice.ToString() + " SEK");
            if (product.OnSale == true && product.UnitSalePrice > 0)
            {
                productWindowTexts.Add("Sale: " + product.UnitSalePrice.ToString() + " SEK");
            }
            else
            {
                productWindowTexts.Add(""); //Add empty line to keep windows equal size
            }
            productWindowTexts.Add(" ");
            productWindowTexts.Add($"{actionText} [{actionKey}]");

            return productWindowTexts;
        }

        //Menus things
        public static void DrawMenuWindow(Enum menuEnum, string menuHeader)
        {
            Type myEnum = menuEnum.GetType(); //Get what enum was inserted as parameter
            string menuText = "";
            foreach (int i in Enum.GetValues(myEnum)) //Join the each enum value (button) and each enum name into one string. Used in displaying the menu window.
            {
                menuText += "[" + i + "] " + Enum.GetName(myEnum, i).Replace('_', ' ') + "  ";
            }

            var windowMenu = new Window(menuHeader, 1, 1, new List<string> { menuText });
            windowMenu.Draw(ConsoleColor.Yellow);
            Console.SetCursorPosition(0, 4); //Set cursor below menu window

        }

    }
}
