using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WebShop.Connections
{
    internal class ConnectionMongoDb
    {
        public static MongoClient GetClient()
        {
            var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

            var connStr = config["MySettings:ConnectionStringMongoDb"];

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connStr));
            MongoClient client = new MongoClient(settings);

            return client;
        }
    }
}
