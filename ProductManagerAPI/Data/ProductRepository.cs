using ProductManagerAPI.Models;
using System.Linq;

namespace ProductManagerAPI.Data
{
    public class ProductRepository: IProductRepository
    {
        DataContextEF _entityFramework;

        public ProductRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if(entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> products = _entityFramework.Products.ToList<Product>();
            return products;
        }

        public Product GetProduct(int productId)
        {
            Product? product = _entityFramework.Products
                .Where(product => product.ProdId == productId)
                .FirstOrDefault<Product>();

            if (product != null)
            {
                return product;
            }
            throw new Exception("Failed to get the product");
        }
    }
}
