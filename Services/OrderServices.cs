using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public static List<Order> GetCustomerOrders(int customerId)
        {
            using (var db = new Connections.WebShopContext())
            {
                //Get obj with navigation properties available. Order by latest date
                List<Order> orders = db.Orders
                    .Include(od => od.OrderDetails).ThenInclude(p => p.Product)
                    .Where(c => c.CustomerId == customerId)
                    .OrderByDescending(o => o.OrderDate).ToList();
                return orders;
            } 
        }
        

        /// <summary>
        /// Validate order is ready for purchase
        /// </summary>
        /// <returns>True if all columns are filled in. Does not check for ID</returns>
        public static bool ValidateOrderForPurchase(Order order)
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

        public static bool ValidateOrderForPurchase2(Order order)
        {
            bool isValidCustomerDetails = true;

            if (string.IsNullOrWhiteSpace(order.Name))              return false;

            if (string.IsNullOrWhiteSpace(order.PaymentMethod))     return false;

            if (string.IsNullOrWhiteSpace(order.ShippingMethod))    return false;

            if (string.IsNullOrWhiteSpace(order.Street))            return false;

            if (string.IsNullOrWhiteSpace(order.City))              return false;

            if (string.IsNullOrWhiteSpace(order.Country))           return false;

            if (order.SubTotal <= 0)                                return false;

            if (order.OrderDate == null)                            return false;
           

            return isValidCustomerDetails;
        }


        /// <summary>
        /// Creates order. Gets cartItems from order.customerId. Creates Order details for each product in customers cart.
        /// Then Clears cart
        /// </summary>
        public static async void CreateOrderAndDetailsAsync(Order order)
        {
            using (var db = new Connections.WebShopContext())
            {
                var myTransaction = db.Database.BeginTransaction();
                //Add order
                try
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync(); //Save first so OrderId gets generated

                    var cartItems = CartItemServices.GetCartItemsByCustomerId(order.CustomerId);

                    foreach (var cartItem in cartItems)
                    {
                        OrderDetail orderDetail = new OrderDetail();

                        orderDetail.OrderId = order.Id;
                        orderDetail.ProductId = cartItem.ProductId;

                        //Get mormal price or sale price.
                        if (cartItem.Product.IsOnSale == true)
                        {
                            orderDetail.SubTotal = cartItem.Product.UnitSalePrice * cartItem.UnitAmount;
                        }
                        else
                        {
                            orderDetail.SubTotal = cartItem.Product.UnitPrice * cartItem.UnitAmount;
                        }

                        orderDetail.UnitAmount = cartItem.UnitAmount;

                        db.OrderDetails.Add(orderDetail);
                    }

                    //Clear cart. Use current db context so it stays on the same transaction chain
                    CartItemServices.DeleteCartItems(cartItems, db);

                    await db.SaveChangesAsync();
                    await myTransaction.CommitAsync();

                    await MongoDbServices.AddUserAction(new Models.UserAction(order.Customer.Id, Enums.UserActions.Pruchase, ("Order ID: " + order.Id)));
                }
                catch (Exception ex)
                {
                    await myTransaction.RollbackAsync();
                    Console.WriteLine("Error Adding order");
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine("\nAny key to continue...");
                    Console.ReadKey();
                }



            }
        }

    }
}
