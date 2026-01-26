using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Models;

namespace WebShop.Windows
{
    internal class WindowCheckout
    {

        public static void DrawPage(Order order)
        {

            int topPos = 5; //Start below menu window

            //Cart - Window
            List<string> CartItemsText = Helpers.GetCartItmesText(CartItemServices.GetCartItemsByCustomerId(Settings.GetCurrentCustomerId()));
            var windowCartItems = new Window("Cart", 1, topPos, CartItemsText);
            windowCartItems.Draw(ConsoleColor.Green);


            //Shipping info - Window
            List<string> shippingDetails = new List<string> {
                "Name:    " + order.Name,
                "Street:  " + order.Street,
                "City:    " + order.City,
                "Country: " + order.Country,
                " ",
                "Method:  " + order.ShippingMethod,
            };

            topPos += CartItemsText.Count + 2;
            var windowShippingDetails = new Window("Shipping", 1, topPos, shippingDetails);
            
            if(!string.IsNullOrWhiteSpace(order.Name) && !string.IsNullOrWhiteSpace(order.Street) && !string.IsNullOrWhiteSpace(order.City) && !string.IsNullOrWhiteSpace(order.Country))
                windowShippingDetails.Draw(ConsoleColor.Green);
            else
                windowShippingDetails.Draw(ConsoleColor.Red);


            //Payment info - Window
            List<string> paymentDetails = new List<string> {
                "Method:  " + order.PaymentMethod,
                " ",
                "Subtotal:     " +order.SubTotal + " SEK",
                "Of which VAT: " + ((order.SubTotal * (decimal)0.25)).ToString("#.##") + " SEK",
            };

            topPos += shippingDetails.Count + 2;
            var windowPaymentDetails = new Window("Payment", 1, topPos, paymentDetails);
            

            if (!string.IsNullOrWhiteSpace(order.PaymentMethod))
                windowPaymentDetails.Draw(ConsoleColor.Green);
            else
                windowPaymentDetails.Draw(ConsoleColor.Red);


        }
    }
}
