using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Modles;

namespace WebShop
{
    internal class Settings
    {
        private static int currentCustomerId = -1; //Set as negative instead of null or zero
        public static Customer GetCurrentCustomer()
        {
            return CustomerServices.GetCustomerById(currentCustomerId);
        }

        public static int GetCurrentCustomerId()
        {
            return currentCustomerId;
        }

        public static void SetCurrentCustomer(int id)
        {
            currentCustomerId = id;
        }
    }
}
