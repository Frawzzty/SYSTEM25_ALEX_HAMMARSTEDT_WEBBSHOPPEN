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

                Helpers.DrawMenuWindow(new MenuHomeMain(), menuHeader);

                WindowSaleProduct.Draw();
                WindowWelcome.Draw(); //Draw welcome window last to hide "connection lagging"
                

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuHomeMain)number)
                    {
                        case Enums.MenuHomeMain.Store:
                            Menus.MenuStore.MenuStoreMain();
                            break;

                        case Enums.MenuHomeMain.Cart:

                            break;

                        case Enums.MenuHomeMain.Admin:
                            Menus.MenuAdmin.MenuAdminMain();
                            break;

                        case Enums.MenuHomeMain.Exit:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }
    }
}
