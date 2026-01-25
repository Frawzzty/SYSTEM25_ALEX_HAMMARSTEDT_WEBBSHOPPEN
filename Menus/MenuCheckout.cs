using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Models;
using WebShop.Services;
using WebShop.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop.Menus
{
    internal class MenuCheckout
    {

        //MAIN BRANCH
        public static bool MenuCheckOutMain()
        {
            bool isPruchase = false;

            //Create order outside of loop so changes can be made to it.
            Order order = new Order(); 
            order.CustomerId = Settings.GetCurrentCustomerId();

            decimal cartTotalPrice = CartItemServices.GetCartValue(Settings.GetCurrentCustomerId());
            decimal shippingPrice = 0;
            
            bool isActive = true;
            while (isActive)
            {
                order.SubTotal = cartTotalPrice + shippingPrice; //Refresh incase shipping method changed.
                
                Helpers.DrawMenuEnum(new MenuCheckOutMain(), "Checkout");
                //Draw page based on order info
                WindowCheckout.DrawPage(order); 

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                //Inputs
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCheckOutMain)number)
                    {
                        case Enums.MenuCheckOutMain.Shipping_Info:
                            var values = SelectShippingDetails(order);
                            order = values.order;
                            shippingPrice = values.shippingPrice;

                            break;

                        case Enums.MenuCheckOutMain.Payment_Info:
                            order = SelectPaymentInfo(order);

                            break;

                        case Enums.MenuCheckOutMain.Pay:
                            order.OrderDate = DateTime.Now;
                            if (OrderServices.ValidateOrderForPurchase2(order))
                            {
                                OrderServices.CreateOrderAndDetailsAsync(order); //Will also delete customer Customer Cart Items
                                isPruchase = true;
                                isActive = false;
                            }
                            
                            break;

                        case Enums.MenuCheckOutMain.Back:
                            isActive = false;
                            break;
                    }
                }
                Console.Clear();
            }
            return isPruchase;
        }

        private static Order SelectPaymentInfo(Order order)
        {
            string paymentMethod = "";

            //Print menu
            int index = 1;
            Console.WriteLine("Select Payment method");
            foreach (int i in Enum.GetValues(typeof(Enums.PaymentOptions)))
            {
                Console.WriteLine("[" + index + "] " + Enum.GetName(typeof(Enums.PaymentOptions), i).Replace('_', ' '));
                index++;
            }

            //Input
            string input = Console.ReadKey(true).KeyChar.ToString();
            bool validInput = int.TryParse(input, out int number) && number > 0 && number <= Enum.GetValues(typeof(Enums.PaymentOptions)).Length;

            //set values
            PaymentOptions[] shippingOptions = Enum.GetValues<PaymentOptions>();
            if (validInput)
            {
                PaymentOptions selectedMethod = shippingOptions[number - 1];
                paymentMethod = selectedMethod.ToString().Replace('_', ' ');
            }

            order.PaymentMethod = paymentMethod;
            return order;
        }


        /// <summary>
        /// Lets user input shipping detials
        /// </summary>
        /// <returns>Input order and Shipping price as Orders do not have didcated column for shipping</returns>
        private static (Order order, decimal shippingPrice) SelectShippingDetails(Order order)
        {
            //Get values
            var shippingCarrier = GetShippingMethodAndPrice();
            Console.WriteLine();
            var shippingDetails = GetShipping();

            //Set values
            order.ShippingMethod = shippingCarrier.method;
            order.Name = shippingDetails.name;
            order.Street = shippingDetails.street;
            order.City = shippingDetails.city;
            order.Country = shippingDetails.country;

            return (order, shippingCarrier.price);
        }
        

        private static (string method, decimal price) GetShippingMethodAndPrice()
        {
            string shippingMethod = "";
            decimal shippingPrice = 0;

            //Print options
            Console.WriteLine("Choose Shipping Method");
            int index = 1;
            foreach (int i in Enum.GetValues(typeof(Enums.ShippingOptions)))
            {
                Console.WriteLine(
                    ("[" + index + "] " + Enum.GetName(typeof(Enums.ShippingOptions), i).Replace('_', ' ')).PadRight(15)
                    + i + " SEK" );
                index++;
            }

            //Get input
            string input = Console.ReadKey(true).KeyChar.ToString();
            bool validInput = int.TryParse(input, out int number) && number > 0 && number <= Enum.GetValues(typeof(Enums.ShippingOptions)).Length;

            //Set values
            ShippingOptions[] shippingOptions = Enum.GetValues<ShippingOptions>(); //Get array of enum values
            if (validInput)
            {
                ShippingOptions selectedMethod = shippingOptions[number - 1];
                shippingMethod = selectedMethod.ToString().Replace('_', ' ');
                shippingPrice = (decimal)selectedMethod;
            }

            return (shippingMethod,shippingPrice);
        }


        private static (string name, string street, string city, string country) GetShipping()
        {
            string name =       "";
            string street =     "";
            string city =       "";
            string country =    "";

            Console.WriteLine("Enter Shipping Info:");
            Console.WriteLine("[1] Auto Fill (Customer details) - [2] Enter Manually");
            int key = int.Parse(Console.ReadKey(true).KeyChar.ToString());

            //Enter manually
            if (key == 1)
            {
                Customer customer = Settings.GetCurrentCustomer();
                name = customer.Name;
                street = customer.Street;
                city = customer.City;
                country = customer.Country;
            }
            //Auto fill
            else if (key == 2)
            {
                Console.WriteLine();
                Console.Write("Name: ");
                name = Console.ReadLine();
                Console.Write("Street: ");
                street = Console.ReadLine();
                Console.Write("City: ");
                city = Console.ReadLine();
                Console.Write("Country: ");
                country = Console.ReadLine();
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }

            return (name, street, city, country);
        }


    }
}
