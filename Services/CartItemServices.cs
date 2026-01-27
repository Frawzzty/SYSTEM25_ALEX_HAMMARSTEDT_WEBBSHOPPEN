using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Connections;
using WebShop.Enums;
using WebShop.Services;

namespace WebShop.DbServices
{
    internal class CartItemServices
    {

        //GetCartItems
        public static List<Models.CartItem> GetCartItemsByCustomerId(int customerId)
        {
            List<Models.CartItem> cartItems;
            using (var db = new WebShopContext())
            {
                cartItems = db.CartItems.Include(ci => ci.Customer).Include(ci => ci.Product).Where(ci => ci.CustomerId == customerId).ToList();
            }
            return cartItems;
        }


        public static async Task AddCartItem(int productId, int customerId)
        {
            Models.CartItem cartItem = null;

            // Check if cart item exists in customer cart
            Models.Customer customer = CustomerServices.GetCustomerById(customerId);
            foreach (var item in customer.CartItems)
            {
                if (productId == item.ProductId)
                {
                    cartItem = item;
                    break;
                }
            }
            
            using (var db = new WebShopContext())
            {
                try
                {
                    if (cartItem != null)
                    {
                        // + 1 to existing cartitem
                        cartItem.UnitAmount++;
                        db.Update(cartItem);

                    }
                    else
                    {
                        // create new cartitem
                        cartItem = new Models.CartItem(customerId, productId, 1);
                        db.Add(cartItem);

                    }

                    //remove 1 from stock
                    ProductServices.UpdateProductStock(productId, -1);

                    Models.UserAction userAction = new Models.UserAction(customerId, Enums.UserActions.Added_To_Cart, "Product ID: " + cartItem.ProductId);
                    userAction.TimeElapsedMS = Helpers.GetDbSaveChangesTime(db);
                    await MongoDbServices.AddUserActionAsync(userAction);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Could not add CartItem\n");
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                }

            }
        }


        //Increase / decrease cartItem unitAmount
        public static bool UpdateCartItem(Models.CartItem cartItem, int value)
        {
            bool isRemoved = false;


            if (cartItem != null)
            {
                cartItem.UnitAmount += value;

                using (var db = new WebShopContext())
                {
                    try
                    {
                        Models.UserAction userAction = new Models.UserAction();
                        if (cartItem.UnitAmount > 0)
                        {

                            db.Update(cartItem);
                            db.SaveChanges();
                        }
                        else
                        {

                            db.Remove(cartItem);

                            isRemoved = true;

                            //Saving to DB & Mongo logging
                            userAction.CustomerId = cartItem.CustomerId;
                            userAction.Action = UserActions.Remove_From_Cart.ToString().Replace("_", " ");
                            userAction.Details = "ProductID: " + cartItem.ProductId;

                            userAction.TimeElapsedMS = Helpers.GetDbSaveChangesTime(db);
                            MongoDbServices.AddUserActionAsync(userAction);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong when reducing item count\n" +
                            "Any key to continue");
                        Console.ReadKey(true);
                    }

                }
                //Update Prodcut Stock in DB
                if (value < 0)
                {
                    ProductServices.UpdateProductStock(cartItem.ProductId, Math.Abs(value)); //Add 1 to stock.
                }
                else
                {
                    ProductServices.UpdateProductStock(cartItem.ProductId, -Math.Abs(value)); //Remove 1 from stock
                }

            }
            return isRemoved;
        }


        public static void PrintCartItems(int customerId)
        {
            List<Models.CartItem> cartItems = GetCartItemsByCustomerId(customerId);

            foreach (var cartItem in cartItems)
            {
                Console.WriteLine((string)("Item ID: " + cartItem.Id + " " + cartItem.Product.Name));
            }
        }


        public static decimal GetCartValue(int customerId)
        {
            List<Models.CartItem> cartItems = GetCartItemsByCustomerId(customerId);
            decimal totalValue = 0;

            foreach (var cartItem in cartItems)
            {
                totalValue += ProductServices.GetProductCurrentUnitPrice(cartItem.Product) * cartItem.UnitAmount;
            }
            return totalValue;
        }

   
        //REMOVE?
        public static void DeleteCartItems(List<Models.CartItem> cartItems)
        {

            using (var db = new WebShopContext())
            {
                try
                {
                    db.CartItems.RemoveRange(cartItems);
                    if(db.SaveChanges() > 0)
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleteing CartItems");
                    Console.WriteLine("Any key to continue...\n");
                    Console.WriteLine(ex.Message);

                    Console.ReadKey(true);
                }
            }
        }

        /// <summary>
        /// Method does not save changes.
        /// </summary>
        public static void DeleteCartItems(List<Models.CartItem> cartItems, WebShopContext db)
        {
            try
            {
                db.CartItems.RemoveRange(cartItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleteing CartItems");
                Console.WriteLine("Any key to continue...\n");
                Console.WriteLine(ex.Message);

                Console.ReadKey(true);
            }
        }
    }

}
