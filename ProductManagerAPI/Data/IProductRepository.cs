using ProductManagerAPI.Models;

namespace ProductManagerAPI.Data
{
    public interface IProductRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public IEnumerable<Product> GetAllProducts();
        public Product GetProduct(int productId);
    }
}
