using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services
{
    internal class MongoDbServices
    {
        
        //Get UserAction Collection
        public static IMongoCollection<UserAction> GetUserActionCollection()
        {
            var client = Connections.ConnectionMongoDb.GetClient();

            var dataBase = client.GetDatabase("UserActionDB"); //Creats new DB if does not exist
            var userActionCollection = dataBase.GetCollection<Models.UserAction>("UserActionCollection"); //Creates new Collection in DB if does not exists

            return userActionCollection; 
        }


        //Add UserAction
        public static async Task AddUserAction(UserAction userAction)
        {
            if (Settings.GetMongoLoggingStatus()) //Only logg if enabled
            {
                var userActionCollection = GetUserActionCollection();
                await userActionCollection.InsertOneAsync(userAction);
            }
        }
        
    }
}
