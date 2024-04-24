using System.ComponentModel.DataAnnotations;

public class ProductCreateViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(1, int.MaxValue)] // Ensure positive quantity
    public int Quantity { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [Range(0, double.MaxValue)] // Allow non-negative price
    public decimal Price { get; set; }
    public IFormFile Image { get; set; }

}