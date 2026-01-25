using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WebShop.Migrations;
using WebShop.Models;

namespace WebShop.Services
{
    internal class DapperServices
    {
        private static string connString = Connections.ConnectionDapper.GetConnectionString();


        /// <summary>
        /// Works, but does not include EF navigation property... >:(
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

        public static void GetCustomerSalesData()
        {
            //Where most customers live?
            //Most popular category per region?
            //Most popular category per user?

            //Total users?
            //Male / Female users in different areas?
            //Male female category interest?
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

                //Get Units sold 30day window
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

        
        public static decimal GetShopRevenueBetweenDates(DateTime startDate, DateTime endDate) //date.now
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
                    O.OrderDate <= '{endDate}'"; //SYSTEM SETTINGS can messup date conversion in sql

            
            decimal total = 0;
            using (var connection = new SqlConnection(connString))
            {
                //Query singel, only return cell
                var shopRevenue = connection.QuerySingle<decimal?>(sql); //Make nullable, will crash if returns null. //Crash?

                if(shopRevenue != null)
                    total = shopRevenue != null ? shopRevenue.Value : 0;
                else 
                    total = 0;
            }

            return total;
        }

    }

}
