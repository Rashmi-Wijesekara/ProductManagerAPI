namespace ProductManagerAPI.Dtos
{
    // DTO = Data Transfer Objects
    public class ProductToAddDto
    {
        public string ProdName { get; set; }
        public string ProdDescription { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public ProductToAddDto()
        {
            ProdName ??= "";
            ProdDescription ??= "";
        }
    }
}
