using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuHome
    {
        public static void MenuHomeMain()
        {
            
            string menuHeader = "Home";
            bool loop = true;
            while (loop)
            {
                //WindowCustomer.SelectCustomer();
                Helpers.DrawMenuWindow(new MenuHomeMain(), menuHeader);


                WindowSaleProduct.Draw();
                WindowHome.DrawHome(); //Draw welcome window last to hide "connection lagging"
                

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
                            Menus.MenuCart.MenuCartMain();
                            break;

                        case Enums.MenuHomeMain.Switch_Customer:
                            Program.myCustomerId = WindowCustomer.SelectCustomer();
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
