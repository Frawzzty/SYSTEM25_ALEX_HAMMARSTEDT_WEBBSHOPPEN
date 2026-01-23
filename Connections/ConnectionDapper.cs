using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
