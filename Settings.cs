using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebShop.DbServices;
using WebShop.Models;

namespace WebShop
{
    internal class Settings
    {
        private static bool mongoDbLoggingEnabled = false;

        //Switch between Azure DB and Local DB. //FALSE == LOCAL
        private static bool isUsingAzureDb = false;

        //Auto login if TRUE - Check customerID input in Program.cs
        private static bool debugEnabled = true;



        private static int currentCustomerId = -1; //No user can have id -1
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



        public static bool GetMongoLoggingStatus()
        {
            return mongoDbLoggingEnabled;
        }


        public static bool GetDebugStatus()
        {
            return debugEnabled;
        }

        public static string GetDbConnectionString()
        {
            var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

            //Return Azure or Local db
            return isUsingAzureDb == true ?  
                config["MySettings:ConnectionStringAzure"]
                : config["MySettings:ConnectionStringLocal"];

        }
    }
}
