using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.Windows
{
    internal class WindowCart
    {
        public static void ShowCartPage(int customerId)
        {
            Customer customer = CustomerServices.GetCustomerById(customerId); //Get current active customer
            List<CartItem> cartItems = CartItemServices.GetCartItemsByCustomerId(customerId);

            //Window vars
            int windowLeftPos = 1;
            int windowTopPos = 1;


            //Draw cartItem window
            windowTopPos += 4; //Add previous window height
            List<string> CartItemsText = GetCartItmesText(cartItems);
            var windowCartItems = new Window(customer.Name + "'s Cart", windowLeftPos, windowTopPos, CartItemsText);
            windowCartItems.Draw(ConsoleColor.White);

            //Draw totals window
            windowTopPos += CartItemsText.Count() + 3; //Add previous window height. + 3 for borders
            var windowTotal = new Window("Total w/o Shipping", windowLeftPos, windowTopPos, new List<string> { GetCartTotalPrice(cartItems).ToString() + " SEK" });
            windowTotal.Draw(ConsoleColor.White);
        }

        public static void ShowEditCartPage(int customerId)
        {

            List<CartItem> cartItems = CartItemServices.GetCartItemsByCustomerId(customerId);
            int itemIndex = 0;
            int windowLeftPos = 1;
            int windowTopPos = 4;

            var controlsWindow = new Window($"Product {itemIndex + 1} / {cartItems.Count()}", windowLeftPos, windowTopPos, new List<string> { cartItems[itemIndex].UnitAmount + "x " + cartItems[itemIndex].Product.Name});
            controlsWindow.Draw(ConsoleColor.White);

            Console.ReadKey(true);
 

        }


        private static List<string> GetCartItmesText(List<CartItem> cartItems)
        {
            List<string> cartText = new List<string>();
            int padProductName = 0;

            if (cartItems.Count > 0) //Will crash if cart is empty
            {
                padProductName = Helpers.GetHeaderMaxPadding("", cartItems.Max(item => item.Product.Name.Length), 3); //Make price text start on the same LeftPos
                foreach (var item in cartItems)
                {
                    decimal price = item.Product.OnSale == false ? (item.UnitAmount * item.Product.UnitPrice) : (item.UnitAmount * item.Product.UnitSalePrice);
                    cartText.Add($"{item.UnitAmount}x {item.Product.Name.PadRight(padProductName)} {price} SEK");
                }
            }
            else
            {
                cartText.Add("Cart Empty");
            }

            return cartText;
        }

        private static decimal GetCartTotalPrice(List<CartItem> cartItems)
        {
            decimal totalPrice = 0;

            foreach (var item in cartItems)
            {
                totalPrice += item.Product.OnSale == false ? item.UnitAmount * item.Product.UnitPrice : item.UnitAmount * item.Product.UnitSalePrice;
                
            }
            return totalPrice;
        }

    }
}
