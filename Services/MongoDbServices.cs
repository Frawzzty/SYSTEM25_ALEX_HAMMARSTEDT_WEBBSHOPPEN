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
        
        private static MongoClient GetClient()
        {
            string connectionString = "mongodb+srv://alex_db_user:9R2VuoQO6GVsusui@cluster0.8z6lhgj.mongodb.net/?appName=Cluster0";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            MongoClient client = new MongoClient(settings);

            return client;
        }

        public static IMongoCollection<UserAction> GetUserActionCollection()
        {
            var client = GetClient();

            var dataBase = client.GetDatabase("UserActionDB"); //Creats new DB if does not exist
            var userActionCollection = dataBase.GetCollection<Models.UserAction>("UserActionCollection"); //Creates new Collection in DB if does not exists

            return userActionCollection; 
        }


        //Add
        public static async void AddUserAction(UserAction userAction)
        {
            if (Settings.GetMongoLoggingEnabled()) //Only logg if enabled
            {
                var userActionCollection = GetUserActionCollection();
                var task = userActionCollection.InsertOneAsync(userAction);

                await task;
            }
        }
        
    }
}
