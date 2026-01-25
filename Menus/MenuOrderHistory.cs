using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Enums;
using WebShop.Models;
using WebShop.Services;
using WebShop.Windows;

namespace WebShop.Menus
{
    internal class MenuOrderHistory
    {
        public static void MenuOrderHistoryMain()
        {

            Customer customer = Settings.GetCurrentCustomer();

            
            int orderIndex = 0;
            int orderCount = customer.Orders.Count;
            string menuText = "Order History ";


            bool loop = true;
            while (loop)
            {
                //Draw Menu
                string windowHeader = ""; 
                if (orderCount == 0)
                {
                    windowHeader = menuText + (orderIndex) + " / " + (orderCount); //No orders
                }
                else
                {
                    windowHeader = menuText + (orderIndex + 1) + " / " + (orderCount);
                }


                var windowMenu = new Window(windowHeader, 1, 1, new List<string> { "[1] Previous [2] Next [9] Back" });
                windowMenu.Draw(ConsoleColor.Yellow);
                Console.SetCursorPosition(0, 4); //Set cursor below menu window

                
                if(orderCount != 0) //Only Draw if there are any orders
                {
                    OrderWindow(OrderServices.GetCustomerOrders(customer.Id)[orderIndex]);
                }
                else
                {
                    var windowNoOrders = new Window("Order", 1, 5, new List<string> { "No order history found" });
                    windowNoOrders.Draw(ConsoleColor.Red);
                }



                string input = Console.ReadKey(true).KeyChar.ToString();
                Console.Clear();
                if (int.TryParse(input, out int number))
                {
                    switch (number)
                    {
                        case 1:
                            if(orderCount != 0)
                            {
                                orderIndex = Math.Clamp(orderIndex -= 1, 0, orderCount - 1);
                            }
                            
                            break;

                        case 2:

                            if (orderCount != 0)
                            {
                                orderIndex = Math.Clamp(orderIndex += 1, 0, orderCount - 1);
                            }
                            break;

                        case 3:
                            PrintOrders(customer.Id);
                            Console.ReadKey();
                            break;

                        case 9:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
            }
        }


        private static void PrintOrders(int customerId)
        {
            List<Order> orders = OrderServices.GetCustomerOrders(customerId);
            foreach (var order in orders) 
            {
                Console.WriteLine("Order ID: " + order.Id);
                Console.WriteLine();
                Console.WriteLine("Customer ID: " + order.CustomerId);
                Console.WriteLine("Name: " + order.Name);
                Console.WriteLine();
                Console.WriteLine("Street: " + order.Street);
                Console.WriteLine("City: " + order.City);
                Console.WriteLine("Country: " + order.Country);
                Console.WriteLine("Shipping Method: " + order.ShippingMethod);
                Console.WriteLine();
                Console.WriteLine("Payment Method:" + order.PaymentMethod);
                Console.WriteLine("Subtotal: " + order.SubTotal);
                Console.WriteLine();
                Console.WriteLine("Order Date: " + order.OrderDate);
                Console.WriteLine();
                Console.WriteLine();

                foreach(var orderDetail in order.OrderDetails)
                {
                    Console.WriteLine((string)(orderDetail.UnitAmount + "x " + orderDetail.Product.Name + " - " + orderDetail.SubTotal + " SEK"));
                }
                Console.WriteLine();
                Console.WriteLine("------------------------------");

            }
        }

        private static void OrderWindow(Order order)
        {

            List<string> orderText = new List<string> {
                "Order ID:      " + order.Id,
                "Date:          " + order.OrderDate.Date.ToShortDateString(),
                "Time:          " + order.OrderDate.ToShortTimeString(),
                " ",
                "Customer ID:   " + order.CustomerId,
                "Name:          " + order.Name,
                " ",
                "Street:        " + order.Street,
                "City:          " + order.City,
                "Country:       " + order.Country,
                "Shipping:      " + order.ShippingMethod,
                " ",
                "Payment:       " + order.PaymentMethod,
                "Subtotal:      " + order.SubTotal + " SEK",
                " ",
                "Product Details",
            };

            foreach (var od in order.OrderDetails)
            {
                orderText.Add((string)(od.UnitAmount + "x " + od.Product.Name + " - " + od.SubTotal + " SEK"));
            }

            var windowMenu = new Window("Order", 1, 5, orderText);
            windowMenu.Draw(ConsoleColor.Yellow);
        }

        
    }

}

