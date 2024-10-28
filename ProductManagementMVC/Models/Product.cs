namespace ProductManagementMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Picture {  get; set; }
        public float UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
