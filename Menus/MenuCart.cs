using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuCart
    {

        //MAIN BRANCH
        public static void MenuCartMain()
        {

            string menuHeader = "Cart";
            bool isActive = true;
            while (isActive)
            {

                Helpers.DrawMenuWindow(new MenuCartMain(), menuHeader);
                WindowCart.ShowCartPage(Program.myCustomerId);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCartMain)number)
                    {
                        case Enums.MenuCartMain.Next_Shipping:

                            break;

                        case Enums.MenuCartMain.Edit_Cart:
                            MenuCartEditCart();
                            
                            break;


                        case Enums.MenuCartMain.Back:
                            isActive = false;
                            break;
                    }
                }
                Console.Clear();
            }
        }

        //SUB Branch
        public static void MenuCartEditCart()
        {
            
            string menuHeader = "Edit Cart";
            bool isActive = true;
            while (isActive)
            {
;
                Helpers.DrawMenuWindow(new MenuCartEdit(), menuHeader);
                WindowCart.ShowEditCartPage(Program.myCustomerId);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCartEdit)number)
                    {
                        case Enums.MenuCartEdit.Previous_Item:

                            break;

                        case Enums.MenuCartEdit.Increase:
                            
                            break;

                        case Enums.MenuCartEdit.Decrease:
                            
                            break;

                        case Enums.MenuCartEdit.Next_Item:

                            break;

                        case Enums.MenuCartEdit.Back:
                            isActive = false;
                            break;
                    }
                }
                Console.Clear();
            }
        }
    }
}

