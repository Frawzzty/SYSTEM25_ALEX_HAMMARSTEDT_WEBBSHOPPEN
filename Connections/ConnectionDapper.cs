namespace WebShop.Connections
{
    internal class ConnectionDapper
    {
        public static string GetConnectionString()
        {
            var connStr = Settings.GetDbConnectionString();
            return connStr;
        }
    }
}
