using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Models;
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
                Helpers.DrawMenuEnum(new MenuCartMain(), menuHeader);
                WindowCart.ShowCartWindow(Settings.GetCurrentCustomerId());

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                //Inputs
                if (int.TryParse(input, out int number))
                {
                    int cartItemCount = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId()).Count;

                    switch ((Enums.MenuCartMain)number)
                    {
                        case Enums.MenuCartMain.Next_Shipping:
                            if (cartItemCount > 0)
                            {
                                isActive = !MenuCheckout.MenuCheckOutMain(); //If sucessfull purchase send back to home screen
                            }
                            break;

                        case Enums.MenuCartMain.Edit_Cart:
                            if(cartItemCount > 0)
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
                int totalItemsInCart = cartItems.Count - 1; //update every loop as it can update if items are removed
                if(totalItemsInCart == -1) //Leave if cart is empty
                {
                    break;
                }

                Helpers.DrawMenuEnum(new MenuCartEdit(), menuHeader);
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

                                CartItemServices.UpdateCartItem(cartItems[cartItemIndex], 1);
                                cartItems = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId());  //Update cartitems (To get new stock value)
                            break;

                        //Reduce units on specific product
                        case Enums.MenuCartEdit.Decrease:

                            CartItemServices.UpdateCartItem(cartItems[cartItemIndex], -1);
                            cartItems = CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId()); //Update cartitems (To get new stock value OR if item was removed)

                            if (cartItems.Count == cartItemIndex) //Can be true if item was deletd
                            {
                                cartItemIndex -= 1;
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

