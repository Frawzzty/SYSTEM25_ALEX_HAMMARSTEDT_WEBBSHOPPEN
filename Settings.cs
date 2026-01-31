using Microsoft.Extensions.Configuration;
using WebShop.DbServices;

namespace WebShop
{
    internal class Settings
    {
        private static bool isMongoDbLoggingEnabled = true;

        //Switch between Azure DB and Local DB. //FALSE == LOCAL
        private static bool isUsingAzureDb = true;

        //Auto login - If true Check customerID input in Program.cs
        private static bool isUsingDebug = false;



        private static int currentCustomerId = -1; //No user can have id -1
        public static Models.Customer GetCurrentCustomer()
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



        public static bool isMongoLoggingEnabled()
        {
            return isMongoDbLoggingEnabled;
        }

        public static bool isDebugEnabled()
        {
            return isUsingDebug;
        }

        public static string GetDbConnectionString()
        {
            var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

            //Return Azure or Local db
            return isUsingAzureDb == true ?  
                config["MySettings:ConnectionStringAzure"] : config["MySettings:ConnectionStringLocal"];

        }


        //SHOP SETTINGS

        //For News Feed Window
        public static List<string> GetNewsFeedText()
        {
            List<string> newsFeedTextRows = new List<string>()
            {
                "* Winter Sale is now Active", 
                "* Up to 50% off", 
                "* Newly restocked"
            };

            return newsFeedTextRows;
        }
    }
}
