using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Services;

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
                Console.SetCursorPosition(1, cursorPosTop);
                Console.WriteLine(menuHeader);
                Console.SetCursorPosition(1, cursorPosTop + 1);

                foreach (int i in Enum.GetValues(typeof(MenuStoreMain)))
                {
                    string menuText = "[" + i + "] " + Enum.GetName(typeof(MenuStoreMain), i).Replace('_', ' ') + "  ";
                    Helpers.WriteInColor(menuColor, menuText);
                }

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((MenuStoreMain)number)
                    {
                        case Enums.MenuStoreMain.Pants:

                            break;

                        case Enums.MenuStoreMain.Shirts:

                            break;

                        case Enums.MenuStoreMain.Shoes:
                            
                            break;

                        case Enums.MenuStoreMain.Hats:

                            break;

                        case Enums.MenuStoreMain.Search_Product:

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
