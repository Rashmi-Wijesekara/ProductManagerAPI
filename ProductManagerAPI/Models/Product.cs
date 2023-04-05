namespace ProductManagerAPI.Models
{
    public partial class Product
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public string ProdDescription { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public Product()
        {
            ProdName ??= "";
            ProdDescription ??= "";
        }
    }
}
