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
        public static void WriteInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }

        //Messages
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

        public static List<string> GetWindowProductText(Product product, string actionKey)
        {
            List<string> productWindowTexts = new List<string>();
            productWindowTexts.Add(product.Name);
            productWindowTexts.Add(product.Description);
            productWindowTexts.Add(product.SupplierName);

            if (product.UnitSalePrice > 0)
            {
                productWindowTexts.Add("Sale: " + product.UnitSalePrice.ToString() + " SEK");
            }
            else
            {
                productWindowTexts.Add(product.UnitPrice.ToString() + " SEK");
            }
            productWindowTexts.Add($"View more [{actionKey}]");

            return productWindowTexts;
        }

        //Menus things
        public static void DrawMenuWindow(Enum menuEnum, string menuHeader)
        {
            Type enumType = menuEnum.GetType(); //Get what enum was inserted
            string menuText = "";
            foreach (int i in Enum.GetValues(enumType))
            {
                menuText += "[" + i + "] " + Enum.GetName(enumType, i).Replace('_', ' ') + "  ";
            }

            var windowMenu = new Window(menuHeader, 1, 1, new List<string> { menuText });
            windowMenu.Draw(ConsoleColor.Yellow);
            Console.SetCursorPosition(0, 4);

        }
    }
}
