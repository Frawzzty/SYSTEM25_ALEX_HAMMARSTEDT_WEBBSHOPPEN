using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebShop.Modles;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebShop.Services
{
    internal class DapperServices
    {
        static string connString = "data source=.\\SQLEXPRESS; initial catalog = WebShop2; persist security info = True; Integrated Security = True; TrustServerCertificate=true;";


        /// <summary>
        /// Does not include navigation property... >:(
        /// </summary>
        public static List<Product> GetProductsByString(string searchTerm)
        {
            List<Product> products = new List<Product>();

            string sql = @$"
                SELECT DISTINCT 
                    *
                FROM 
                    Products P
                Join 
                    Categories C on P.CategoryId = C.Id
                WHERE 
                    P.Name Like '%{searchTerm}%' OR  
                    P.SupplierName Like '%{searchTerm}%' OR
                    P.Description Like '%{searchTerm}%' OR
                    C.Name Like '%{searchTerm}%'";

            using (var connection = new SqlConnection(connString))
            {
                products = connection.Query<Product>(sql).ToList();
            }

            return products;
        }

    }
}
