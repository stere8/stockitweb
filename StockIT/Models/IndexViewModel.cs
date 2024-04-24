namespace StockIT.Models;

public class IndexViewModel
{
        public List<Category>? Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
        public List<Product>? Products { get; set; }
        public Category? SelectedCategory { get; set; }

}