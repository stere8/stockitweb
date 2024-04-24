namespace StockIT.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation property for one-to-many relationship with Product
    public List<Product> Products { get; set; }
}