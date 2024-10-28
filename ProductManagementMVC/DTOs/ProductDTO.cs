using ProductManagementMVC.Models;

namespace ProductManagementMVC.DTOs
{
    public class ProductDTO
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
