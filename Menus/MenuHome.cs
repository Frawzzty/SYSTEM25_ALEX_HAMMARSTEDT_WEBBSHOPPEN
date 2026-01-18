using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
            List<Product> productsOnSale = new List<Product>();
            string menuHeader = "Home";
            bool loop = true;
            while (loop)
            {
                //WindowCustomer.SelectCustomer();

                
                Helpers.DrawMenuEnum(new MenuHomeMain(), menuHeader);
                WindowHome.DrawHome(); //Welcome message & Newsfeed

                productsOnSale = ProductServices.GetProductsOnSale();

                List<string> saleActionKeys = new List<string> { "Z", "X", "C", "V" }; //Will draw same amount of products windows
                WindowSaleProduct.DrawProductWindows(productsOnSale, saleActionKeys.Count, saleActionKeys);

                string input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int actionKeyIndex = Helpers.GetActionKeyIndex(input);
                //Handle Navbar menu
                Console.Clear();
                if (int.TryParse(input, out int number))
                {

                    switch ((Enums.MenuHomeMain)number)
                    {
                        case Enums.MenuHomeMain.Store:
                            Menus.MenuStore.MenuStoreMainDynamic();
                            break;

                        case Enums.MenuHomeMain.Cart:
                            Menus.MenuCart.MenuCartMain();
                            break;

                        case Enums.MenuHomeMain.Order_History:
                            Menus.MenuOrderHistory.MenuOrderHistoryMain();
                            break;

                        case Enums.MenuHomeMain.Switch_Customer:
                            Settings.SetCurrentCustomer(WindowCustomer.SelectCustomer());
                            break;

                        case Enums.MenuHomeMain.Admin:
                            Menus.MenuAdmin.MenuAdminMain();
                            break;

                        case Enums.MenuHomeMain.Exit:
                            loop = false;
                            break;
                    }
                }
                else if(actionKeyIndex >= 0) //Try add sale product to cart
                {
                        Helpers.TryAddProductOnSaleToCart(productsOnSale, actionKeyIndex);
                }

                Console.Clear();
            }
        }


    }
}
