using System.ComponentModel.DataAnnotations;
using StockIT.Models;

public class ProductDetailsViewModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public Category category { get; set; }
    public  bool noImage { get; set; }
}