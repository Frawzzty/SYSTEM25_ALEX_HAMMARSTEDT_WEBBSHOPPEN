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
                //Draw menu and Home screen
                Helpers.DrawMenuEnum(new MenuHomeMain(), menuHeader);
                WindowHome.DrawHome(); 

                //Draw product windows
                productsOnSale = ProductServices.GetProductsOnSale();
                List<string> saleActionKeys = Helpers.GetActionKeys().Take(4).ToList(); //Get actions keys for prodcut windows. Take(x) to limit how many windows are drawn.
                WindowSaleProduct.DrawProductWindows(productsOnSale, saleActionKeys.Count, saleActionKeys);

                //Inputs
                string input = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                int actionKeyIndex = Helpers.GetActionKeyIndex(input); //Check if input was a valid key for prodcut windows
                
                Console.Clear();
                //Navbar menu
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

                        case Enums.MenuHomeMain.Logout:
                            Settings.SetCurrentCustomer(-1); //Set current customer to invalid id
                            loop = false;
                            break;

                        case Enums.MenuHomeMain.Admin:
                            Menus.MenuAdmin.MenuAdminMain();
                            break;

                        case Enums.MenuHomeMain.Exit:
                            loop = false;
                            break;
                    }
                }
                //Add to cart
                else if(actionKeyIndex >= 0) //Try add sale product to cart
                {
                        Helpers.TryAddProductOnSaleToCart(productsOnSale, actionKeyIndex);
                }

                Console.Clear();
            }
        }


    }
}
