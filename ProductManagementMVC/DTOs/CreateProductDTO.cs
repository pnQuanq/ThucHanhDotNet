
namespace ProductManagementMVC.DTOs
{
    public class CreateProductDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public IFormFile PictureFile { get; set; }
        public float UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Id { get; set; }
    }
}
