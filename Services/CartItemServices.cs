using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;
using WebShop.Services;

namespace WebShop.DbServices
{
    internal class CartItemServices
    {

        //GetCartItems
        public static List<CartItem> GetCartItemsByCustomerId(int customerId)
        {
            List<CartItem> cartItems;
            using (var db = new WebShopContext())
            {
                cartItems = db.CartItems.Include(ci => ci.Customer).Include(ci => ci.Product).Where(ci => ci.CustomerId == customerId).ToList();
            }
            return cartItems;
        }


        public static void AddCartItem(int productId, int customerId)
        {
            CartItem cartItem = null;

            // Check if cart item exists in customer cart
            Customer customer = CustomerServices.GetCustomerById(customerId);
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
                        cartItem = new CartItem(customerId, productId, 1);
                        db.Add(cartItem);


                    }
                    ProductServices.UpdateProductStock(productId, -1); //remove 1 from stock
                    db.SaveChanges();

                    
                    MongoDbServices.AddUserAction(new Models.UserAction(customerId, Enums.UserActions.Added_To_Cart, "Product ID: " + cartItem.ProductId));
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
        public static bool UpdateCartItem(CartItem cartItem, int value)
        {
            bool isRemoved = false;


            if (cartItem != null)
            {
                cartItem.UnitAmount += value;

                using (var db = new WebShopContext())
                {
                    if (cartItem.UnitAmount > 0)
                    {

                        db.Update(cartItem);
                    }
                    else
                    {
                        db.Remove(cartItem);
                        isRemoved = true;
                    }

                    db.SaveChanges();
                }
                //Update stock
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
            List<CartItem> cartItems = GetCartItemsByCustomerId(customerId);

            foreach (var cartItem in cartItems)
            {
                Console.WriteLine("Item ID: " + cartItem.Id + " " + cartItem.Product.Name);
            }
        }


        public static decimal GetCartValue(int customerId)
        {
            List<CartItem> cartItems = GetCartItemsByCustomerId(customerId);
            decimal totalValue = 0;

            foreach (var cartItem in cartItems)
            {
                totalValue += ProductServices.GetProductCurrentUnitPrice(cartItem.Product);
            }
            return totalValue;
        }

   
        public static void DeleteCartItems(List<CartItem> cartItems)
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
                    Console.WriteLine(ex.InnerException);

                    Console.ReadKey(true);
                }
            }
        }

        /// <summary>
        /// Method does not save changes.
        /// </summary>
        public static void DeleteCartItems(List<CartItem> cartItems, WebShopContext db)
        {
            try
            {
                db.CartItems.RemoveRange(cartItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleteing CartItems");
                Console.WriteLine("Any key to continue...\n");
                Console.WriteLine(ex.InnerException);

                Console.ReadKey(true);
            }
        }
    }

}
