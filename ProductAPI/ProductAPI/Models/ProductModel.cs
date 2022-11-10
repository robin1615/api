namespace ProductAPI.Models
{
    public class ProductModel
    {
        public int Product_id { get; set; }
        public string Product_name { get; set; }
        public string Product_category { get; set; }
        public string Product_freshness { get; set; }
        public float Price { get; set; }
        public string Comment { get; set; }
    }
}
