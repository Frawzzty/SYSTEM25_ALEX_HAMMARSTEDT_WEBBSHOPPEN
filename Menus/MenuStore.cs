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
    internal class MenuStore
    { 
        //MAIN BRANCH
        public static void MenuStoreMain()
        {
            int productsPerPage = 3;
            string menuHeader = "Store";
            bool loop = true;
            while (loop)
            {
                
                Helpers.MenuWindow(new MenuStoreMain(),menuHeader);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuStoreMain)number)
                    {
                        case Enums.MenuStoreMain.Pants:
                            WindowBrowseProducts.BrowseProducts(ProductServices.GetProductsByCategory("Pants"), productsPerPage);
                            break;

                        case Enums.MenuStoreMain.Shirts:
                            WindowBrowseProducts.BrowseProducts(ProductServices.GetProductsByCategory("Shirts"), productsPerPage);
                            break;

                        case Enums.MenuStoreMain.Shoes:
                            WindowBrowseProducts.BrowseProducts(ProductServices.GetProductsByCategory("Shoes"), productsPerPage);
                            break;

                        case Enums.MenuStoreMain.Hats:
                            WindowBrowseProducts.BrowseProducts(ProductServices.GetProductsByCategory("Hats"), productsPerPage);
                            break;

                        case Enums.MenuStoreMain.Search_Product:
                            //Dapper?
                            break;

                        case Enums.MenuStoreMain.Back:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }
       
    }
}
