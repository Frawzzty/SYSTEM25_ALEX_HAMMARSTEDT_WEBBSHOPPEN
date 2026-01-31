using WebShop.DbServices;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Menus
{
    internal class MenuOrderHistory
    {
        /// <summary>
        /// If no customer parameter, defaults to current customer
        /// </summary>
        /// <param name="customerId"></param>
        public static void MenuOrderHistoryMain(int customerId = -1)
        {
            Customer customer;
            if (customerId == -1)
            {
                customer = Settings.GetCurrentCustomer();
            }
            else
            {
                customer = CustomerServices.GetCustomerById(customerId);
            }
                
            if(customer == null)
            {
                Console.WriteLine("Customer ID invalid: " + customerId);
                return;
            }


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

                        case 9:
                            loop = false;

                            break;
                    }
                }
                Console.Clear();
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

