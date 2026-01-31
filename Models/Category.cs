namespace WebShop.Models
{
    internal class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Category() { }
        public Category(string name)
        {
            Name = Helpers.FirtUpperCaseRestLower(name);
        }
    }
}
