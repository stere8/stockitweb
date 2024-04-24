﻿namespace StockIT.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? ImagePaths { get; set; } // Comma-separated image paths
}