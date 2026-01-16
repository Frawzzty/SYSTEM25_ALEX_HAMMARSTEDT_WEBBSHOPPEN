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
                //Graphics
                Helpers.MenuWindow(new MenuCartMain(), menuHeader);
                WindowCart.ShowCartWindow(Settings.GetCurrentCustomerId());

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                //Inputs
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCartMain)number)
                    {
                        case Enums.MenuCartMain.Next_Shipping:
                            if (CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId()).Count > 0)
                            {
                                MenuCheckout.MenuCheckOutMain();
                                isActive = false;
                            }
                            break;

                        case Enums.MenuCartMain.Edit_Cart:
                            if(CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId()).Count > 0)
                            {
                                MenuCartEditCart();
                            }
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

            int cartItemIndex = 0;
            List<CartItem> cartItems = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId());
            

            string menuHeader = "Edit Cart";
            bool isActive = true;
            while (isActive)
            {
;               bool isProductDeleted = false;
                int totalItemsInCart = cartItems.Count - 1; //update every loop as it can update if items are removed

                Helpers.MenuWindow(new MenuCartEdit(), menuHeader);
                WindowCart.EditCartPage(Settings.GetCurrentCustomerId(), cartItemIndex);


                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCartEdit)number)
                    {
                        //Go to previous item in cart
                        case Enums.MenuCartEdit.Previous_Item:
                            cartItemIndex = Math.Clamp(cartItemIndex -= 1, 0, totalItemsInCart);
                            break;

                        //Go to next item in cart
                        case Enums.MenuCartEdit.Next_Item:
                            cartItemIndex = Math.Clamp(cartItemIndex += 1, 0, totalItemsInCart);
                            break;


                        //Increase units on specific product
                        case Enums.MenuCartEdit.Increase:
                            if(totalItemsInCart != -1) 
                            {
                                isProductDeleted = CartItemServices.UpdateCartItem(cartItems[cartItemIndex], 1);
                                cartItems = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId());
                            }
                            else
                            {
                                isActive = false;  //If cart empty, leave edit cart page
                            }
                            break;

                        //Reduce units on specific product
                        case Enums.MenuCartEdit.Decrease:
                            if (totalItemsInCart != -1) 
                            {
                                isProductDeleted = CartItemServices.UpdateCartItem(cartItems[cartItemIndex], -1);
                                cartItems = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId());
                            }
                            else
                            {
                                isActive = false; //If cart empty, leave edit cart page
                            }
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

