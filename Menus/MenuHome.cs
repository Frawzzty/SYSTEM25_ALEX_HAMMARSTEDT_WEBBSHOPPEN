using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Enums;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuHome
    {
        private static int cursorPosTop = 0;
        private static ConsoleColor menuColor = ConsoleColor.White;
        public static void MenuHomeMain()
        {
            
            string menuHeader = "Home";
            bool loop = true;
            while (loop)
            {

                Console.SetCursorPosition(0, cursorPosTop);
                Console.WriteLine(menuHeader);
                foreach (int i in Enum.GetValues(typeof(Enums.MenuHome)))
                {
                    string menuText = "[" + i + "] " + Enum.GetName(typeof(Enums.MenuHome), i).Replace('_', ' ') + "  ";
                    Helpers.WriteInColor(menuColor, menuText);
                }

                WindowSaleProduct.Draw();
                WindowWelcome.Draw(); //Draw welcome window last to hide "connection lagging"
                
                
                

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuHome)number)
                    {
                        case Enums.MenuHome.Store:

                            break;

                        case Enums.MenuHome.Cart:

                            break;

                        case Enums.MenuHome.Admin:
                            Menus.MenuAdmin.MenuAdminMain();
                            break;

                        case Enums.MenuHome.Exit:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }
    }
}
