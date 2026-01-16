using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Modles;
using WebShop.Services;
using WebShop.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop.Menus
{
    internal class MenuCheckout
    {



        //MAIN BRANCH
        public static void MenuCheckOutMain()
        {
            Order order = new Order(); //Create new order outside of loop so changes will be saved.
            order.CustomerId = Settings.GetCurrentCustomerId();
            decimal cartTotalPrice = CartItemServices.GetCartValue(Settings.GetCurrentCustomerId());
            decimal shippingPrice = 0;
            
            string menuHeader = "Checkout";
            bool isActive = true;
            while (isActive)
            {
                order.SubTotal = cartTotalPrice + shippingPrice; //Refresh incase shipping method changed.

                //Graphics
                Helpers.MenuWindow(new MenuCheckOutMain(), menuHeader);
                WindowCheckout.DrawPage(order);

                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                //Inputs
                if (int.TryParse(input, out int number))
                {
                    switch ((Enums.MenuCheckOutMain)number)
                    {
                        case Enums.MenuCheckOutMain.Shipping_Info:
                            var values = GetShippingDetails(order);
                            order = values.order;
                            shippingPrice = values.shippingPrice;

                            break;

                        case Enums.MenuCheckOutMain.Payment_Info:
                            order = GetPaymentInfo(order);

                            break;

                        case Enums.MenuCheckOutMain.Pay:
                            order.OrderDate = DateTime.Now;
                            if (OrderServices.ValidateForPurchase(order))
                            {
                                OrderServices.CreateOrder(order);
                            }
                            //OrderServices.PrintOderInfo(order);
                            Console.ReadKey(true);

                            //Create Order
                            //Create OrderDetails
                            //If success, DeleteCartItems
                            isActive = false;
                            break;


                        case Enums.MenuCheckOutMain.Back:
                            isActive = false;
                            break;
                    }
                }
                Console.Clear();
            }
        }

        private static Order GetPaymentInfo(Order order)
        {
            string paymentMethod = "";

            //Print menu
            int index = 1;
            foreach (int i in Enum.GetValues(typeof(Enums.PaymentOption)))
            {
                Console.WriteLine(index + ". " + Enum.GetName(typeof(Enums.PaymentOption), i).Replace('_', ' '));
                index++;
            }

            //Input
            string input = Console.ReadKey(true).KeyChar.ToString();
            bool validInput = int.TryParse(input, out int number) && number > 0 && number <= Enum.GetValues(typeof(Enums.PaymentOption)).Length;

            //set values
            PaymentOption[] shippingOptions = Enum.GetValues<PaymentOption>();
            if (validInput)
            {
                PaymentOption selectedMethod = shippingOptions[number - 1];
                paymentMethod = selectedMethod.ToString().Replace('_', ' ');
            }

            order.PaymentMethod = paymentMethod;
            return order;
        }



        private static (Order order, decimal shippingPrice) GetShippingDetails(Order order)
        {
            //Get values
            var shippingCarrier = GetShippingMethodAndPrice(); //Returns "Error", 0   if invalid selection
            Console.WriteLine();
            var shippingDetails = GetShipping(); //Returns (street: "", city: "", country: "") if inputs failed

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

            Console.WriteLine("Choose Shipping Method");
            //Print menu
            int index = 1;
            foreach (int i in Enum.GetValues(typeof(Enums.ShippingOption)))
            {
                Console.WriteLine(index + ". " + Enum.GetName(typeof(Enums.ShippingOption), i).Replace('_', ' '));
                index++;
            }

            //Input
            string input = Console.ReadKey(true).KeyChar.ToString();
            bool validInput = int.TryParse(input, out int number) && number > 0 && number <= Enum.GetValues(typeof(Enums.ShippingOption)).Length;

            //set values
            ShippingOption[] shippingOptions = Enum.GetValues<ShippingOption>();
            if (validInput)
            {
                ShippingOption selectedMethod = shippingOptions[number - 1];
                shippingMethod = selectedMethod.ToString().Replace('_', ' ');
                shippingPrice = (decimal)selectedMethod;
            }

            return (shippingMethod,shippingPrice);
        }

        private static (string name, string street, string city, string country) GetShipping()
        {
            string name =       "";    //Set inital as error
            string street =     "";    //Set inital as error
            string city =       "";    //Set inital as error
            string country =    "";    //Set inital as error

            Console.WriteLine("Enter Shipping Info:");
            Console.WriteLine("[1] Enter Manually - [2] Auto Fill (Customer details) ");
            int key = int.Parse(Console.ReadKey(true).KeyChar.ToString());

            //Enter manually
            if (key == 1)
            {
                Console.Write("Name: ");
                name = Console.ReadLine();
                Console.Write("Street: ");
                street = Console.ReadLine();
                Console.Write("City: ");
                city = Console.ReadLine();
                Console.Write("Country: ");
                country = Console.ReadLine();

            }
            //Auto fill
            else if (key == 2)
            {
                Customer customer = Settings.GetCurrentCustomer();
                name = customer.Name;
                street = customer.Street;
                city = customer.City;
                country = customer.Country;
            }
            else
            {
                Helpers.MsgBadInputsAnyKey();
            }

            return (name, street, city, country);
        }


    }
}
