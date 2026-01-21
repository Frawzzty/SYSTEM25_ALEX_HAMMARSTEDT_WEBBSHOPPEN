using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebShop.Modles;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Windows.Markup;
using WebShop.Migrations;

namespace WebShop.Services
{
    internal class DapperServices
    {
        private static string connString = Connections.ConnectionDapper.GetConnectionString();


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


        public static decimal GetProductTotalRevenu(Product product)
        {
            decimal total = 0;
            if (product != null)
            {
                int productId = product.Id;

                //Get Total Revenue
                string sql = @$"
                SELECT 
                    SUM(OD.UnitAmount * OD.SubTotal)
                FROM 
                    Products P
                JOIN
                    OrderDetails OD ON  P.Id = OD.ProductId
                WHERE P.id = {productId}";

                using (var connection = new SqlConnection(connString))
                {
                    total = connection.Query<decimal>(sql).FirstOrDefault();
                }
            }
            return total;
        }

        public static decimal GetProductTotalUnitsSold(Product product)
        {
            decimal total = 0;
            if (product != null)
            {
                int productId = product.Id;

                //Get Total Revenue
                string sql = @$"
                SELECT 
                    SUM(OD.UnitAmount)
                FROM 
                    Products P
                JOIN
                    OrderDetails OD ON  P.Id = OD.ProductId
                WHERE P.id = {productId}";

                using (var connection = new SqlConnection(connString))
                {
                    total = connection.Query<decimal>(sql).FirstOrDefault();
                }
            }
            return total;
        }

        /// <summary>
        /// Gets product revenue in a lookback window of 30 days. (endDate minus 30 days)
        /// </summary>
        public static decimal GetProductTotalUnitsSoldL30D(Product product, DateTime endDate)
        {
            decimal total = 0;
            DateTime startDate = endDate.AddDays(-30);

            if (product != null)
            {
                int productId = product.Id;

                //Get Total Revenue
                string sql = @$"
                SELECT 
                    SUM(OD.UnitAmount)
                FROM 
                    OrderDetails OD
                JOIN 
                    Orders O ON OD.OrderId = O.ID
                WHERE 
                    OD.ProductId = 3
                    AND
                    O.OrderDate >= '{startDate}' 
                    AND 
                    O.OrderDate <= '{endDate}'";

                using (var connection = new SqlConnection(connString))
                {
                    total = connection.Query<decimal>(sql).FirstOrDefault();
                }
            }
            return total;
        }

        /// <summary>
        /// Get shops lifetime revenue
        /// </summary>
        public static decimal GetShopRevenueTotal()
        {

            //Get Total Revenue
            string sql = @$"
                SELECT 
                    SUM(OD.SubTotal)
                FROM 
                    OrderDetails OD
                JOIN 
                    Orders O ON OD.OrderId = O.ID";

            decimal revenue = 0;
            using (var connection = new SqlConnection(connString))
            {
                //Query singel, only return cell
                var shopRevenue = connection.QuerySingle<decimal?>(sql); //Make nullable, will crash if returns null.

                revenue = shopRevenue != null ? shopRevenue.Value : 0;
            }

            return revenue;
        }

        
        public static decimal GetShopRevenueL30D(DateTime startDate, DateTime endDate) //date.now
        {

            //Get Total Revenue
            string sql = @$"
                SELECT 
                    SUM(OD.SubTotal)
                FROM 
                    OrderDetails OD
                JOIN 
                    Orders O ON OD.OrderId = O.ID
                WHERE
                    O.OrderDate >= '{startDate}' 
                    AND 
                    O.OrderDate <= '{endDate}'";

            decimal total = 0;
            using (var connection = new SqlConnection(connString))
            {
                //Query singel, only return cell
                var shopRevenue = connection.QuerySingle<decimal?>(sql); //Make nullable, will crash if returns null.

                total = shopRevenue != null ? shopRevenue.Value : 0;
            }

            return total;
        }

    }

}
