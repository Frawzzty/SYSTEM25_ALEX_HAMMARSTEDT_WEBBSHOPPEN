using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebShop.Modles;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebShop.Services
{
    internal class GenericServices
    {
        public static void UpdateItemName2<T>(T item, string newName, Enum myEnum)
        {

        }

        public static bool UpdateItemName<T>(T item, string newName)
        {
            bool isSucess = false;

            //Transaction?
            if(item is Product product)
            {
                product.Name = newName;
            }
            else if (item is Category category)
            {
                category.Name = newName;
            }
            else if (item is Customer customer)
            {
                customer.Name = newName;
            }

            if (item != null && !string.IsNullOrWhiteSpace(newName))
            using (var db = new WebShopContext())
            {
                db.Update(item);
                 db.SaveChangesAsync();
                isSucess = true;
            }
            else
            {
                //Rollback?
                Console.WriteLine("Could not find Item or invalid name. Any key to continue...");
                Console.ReadKey(true);
            }
                return isSucess;
        }

        public static void PrintBaseClass<T>(List<T> items)
        {
            //Headers
            
            //Rows
        }

        public static bool DeleteDbItem<T>(T item)
        {
            bool success = false;

            using (var db = new WebShopContext()) 
            {
                try
                {
                    db.Remove(item);
                    db.SaveChanges(success);
                    success = true;
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine(ex.Message);
                    Helpers.MsgLeavingAnyKey();
                }

            }
            return success;

        }
    }
}
