namespace WebShop.Models
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
        public decimal UnitSalePrice { get; set; } = 0;    //Default 0
        public bool IsOnSale { get; set; } = false;         //Default false
        public int StockAmount { get; set; }
        public bool IsDeleted { get; set; } = false; //Instead of deleting. 

        public Product() { }
        public Product(string name, string description, int categoryId, string supplierName, decimal unitPrice, int stockAmount)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
            SupplierName = supplierName;
            UnitPrice = unitPrice;
            StockAmount = stockAmount;
        }
    }
}
