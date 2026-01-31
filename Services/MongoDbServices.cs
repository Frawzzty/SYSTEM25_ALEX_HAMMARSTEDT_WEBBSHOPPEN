using MongoDB.Driver;


namespace WebShop.Services
{
    internal class MongoDbServices
    {
        
        //Get UserAction Collection
        public static IMongoCollection<Models.UserAction> GetUserActionCollection()
        {
            var client = Connections.ConnectionMongoDb.GetClient();

            var dataBase = client.GetDatabase("UserActionDB"); //Creats new DB if does not exist
            var userActionCollection = dataBase.GetCollection<Models.UserAction>("UserActionCollection"); //Creates new Collection in DB if does not exists

            return userActionCollection; 
        }


        //Add UserAction
        public static async Task AddUserActionAsync(Models.UserAction userAction)
        {
            if (Settings.isMongoLoggingEnabled()) //Only logg if enabled
            {
                await GetUserActionCollection().InsertOneAsync(userAction);
            }
        }
        
    }
}
