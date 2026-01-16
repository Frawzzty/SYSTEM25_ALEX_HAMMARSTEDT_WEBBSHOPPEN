using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Modles;

namespace WebShop.Services
{
    internal class OrderServices
    {

        public static void PrintOderInfo(Order order)
        {
            
            Console.WriteLine("ID:" + order.Id);
            Console.WriteLine("Customer ID:" + order.CustomerId);
            Console.WriteLine("Steet:" + order.Name);

            Console.WriteLine("Payment Method:" + order.PaymentMethod);
            Console.WriteLine("Shipping Method: " + order.ShippingMethod);
            
            Console.WriteLine("Steet:" + order.Street);
            Console.WriteLine("City:" + order.City);
            Console.WriteLine("Country:" + order.Country);
            Console.WriteLine("Country:" + order.SubTotal);
            Console.WriteLine("Country:" + order.OrderDate.ToString());

        }


        public static List<Order> GetCustomerOrders(int customerId)
        {
            using (var db = new WebShopContext())
            {
                List<Order> orders = db.Orders.Include(od => od.OrderDetails).ThenInclude(p => p.Product).ToList();
                return orders;
            } 
        }
        

        public static bool ValidateForPurchase(Order order)
        {
            bool isReady = false;

            List<int> checkList = new List<int>();
            if (order != null)
            {
                if (order.CustomerId > 0)
                {
                    checkList.Add(1);
                }
                else
                {
                    checkList.Add(0);
                }

                checkList.Add(Helpers.ValidateString(order.Name));
                checkList.Add(Helpers.ValidateString(order.PaymentMethod));
                checkList.Add(Helpers.ValidateString(order.ShippingMethod));
                checkList.Add(Helpers.ValidateString(order.Street));
                checkList.Add(Helpers.ValidateString(order.City));
                checkList.Add(Helpers.ValidateString(order.Country));

                if (order.SubTotal > 0)
                {
                    checkList.Add(1);
                }
                else
                {
                    checkList.Add(0);
                }

                if (order.OrderDate != null)
                {
                    checkList.Add(1);
                }
                else
                {
                    checkList.Add(0);
                }

                int total = 0;
                foreach(int item in checkList)
                {
                    if (item == 1)
                        total++;
                }


                if(total == checkList.Count)
                    isReady = true;
            }

            return isReady;
        }

        public static async Task CreateOrder(Order order)
        {

            using (var db = new WebShopContext())
            {
                //Add order to webshop
                try
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Adding order");
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine("\nAny key to continue...");
                    Console.ReadKey();
                }

                //Get cartItems - Create one OrderDetail per product in cart
                var cartItems = CartItemServices.GetCartItemsByCustomerId(order.CustomerId);
                //Add order details
                try 
                {
                    Console.WriteLine("OrderId: " + order.Id + "\nNext key...");
                    
                    foreach (var cartItem in cartItems)
                    {
                        OrderDetail orderDetail = new OrderDetail();

                        orderDetail.OrderId = order.Id;
                        orderDetail.ProductId = cartItem.ProductId;

                        if (cartItem.Product.OnSale == true)
                        {
                            orderDetail.Price = cartItem.Product.UnitSalePrice * cartItem.UnitAmount;
                        }
                        else
                        {
                            orderDetail.Price = cartItem.Product.UnitPrice * cartItem.UnitAmount;
                        }
                           
                        orderDetail.UnitAmount = cartItem.UnitAmount;

                        db.OrderDetails.Add(orderDetail);
                        //OrderDetailServices.PrintOrderDetail(orderDetail);
                    }
                    await db.SaveChangesAsync();
                    
                    //Clear cart;
                    CartItemServices.ClearCart(order.CustomerId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Adding Order details");
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine("\nAny key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

}
