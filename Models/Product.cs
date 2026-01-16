using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Modles
{
    internal class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string SupplierName { get; set; }

        
        public decimal UnitPrice { get; set; }
        public decimal UnitSalePrice { get; set; }
        public bool OnSale { get; set; }
        public int StockAmount { get; set; }

        public Product() { }
        public Product(string name, string description, int categoryId, string supplierName, decimal unitPrice, int stockAmount)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
            SupplierName = supplierName;
            UnitPrice = unitPrice;
            OnSale = false;         //Default false, edit in Admin menu
            UnitSalePrice = 0;   //Default null, edit in Admin menu
            StockAmount = stockAmount;
        }
    }
}
