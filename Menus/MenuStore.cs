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
        private static int cursorPosTop = 0;
        private static ConsoleColor menuColor = ConsoleColor.White;

        //MAIN BRANCH
        public static void MenuStoreMain()
        {
            string menuHeader = "Store";
            bool loop = true;
            while (loop)
            {
                
                Helpers.DrawMenuWindow(new MenuStoreMain(),menuHeader);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuStoreMain)number)
                    {
                        case Enums.MenuStoreMain.Pants:
                            CategoryPage.DrawCategoryPage(ProductServices.GetProductsByCategory("Pants"), 3);
                            break;

                        case Enums.MenuStoreMain.Shirts:
                            CategoryPage.DrawCategoryPage(ProductServices.GetProductsByCategory("Shirts"), 3);
                            break;

                        case Enums.MenuStoreMain.Shoes:
                            CategoryPage.DrawCategoryPage(ProductServices.GetProductsByCategory("Shoes"), 3);
                            break;

                        case Enums.MenuStoreMain.Hats:
                            CategoryPage.DrawCategoryPage(ProductServices.GetProductsByCategory("Hats"), 3);
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
