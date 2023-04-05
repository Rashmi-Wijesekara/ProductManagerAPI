using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductManagerAPI.Data;
using ProductManagerAPI.Dtos;
using ProductManagerAPI.Models;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsEFController : ControllerBase
    {
        DataContextEF _entityFramework;
        IMapper _mapper;

        public ProductsEFController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
            _mapper = new Mapper(
                new MapperConfiguration(conf =>
                    {
                        conf.CreateMap<ProductToAddDto, Product>();
                    }
                )
            );
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> products = _entityFramework.Products.ToList<Product>();
            return products;
        }

        [HttpGet("{productId}")]
        public Product GetProduct(int productId)
        {
            Product? product = _entityFramework.Products
                .Where(product => product.ProdId == productId)
                .FirstOrDefault<Product>();

            if(product != null)
            {
                return product;
            }
            throw new Exception("Failed to get the product");
        }

        [HttpPut("{productId}")]
        public IActionResult EditProduct(Product product)
        {
            Product? productDb = _entityFramework.Products
                .Where(prod => prod.ProdId == product.ProdId)
                .FirstOrDefault<Product>();

            if (productDb != null)
            {
                productDb.ProdName = product.ProdName;
                productDb.ProdDescription = product.ProdDescription;
                productDb.Price = product.Price;
                productDb.InStock = product.InStock;

                if(_entityFramework.SaveChanges() > 0)
                {
                    return Ok();
                }
            }
            throw new Exception("Failed to update the product");
        }

        [HttpPost]
        public IActionResult AddProduct(ProductToAddDto product)
        {
            Product productDb = _mapper.Map<Product>(product);
            _entityFramework.Add(productDb);

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            
            throw new Exception("Failed to add the product");
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            Product? productDb = _entityFramework.Products
                .Where(product => product.ProdId == product.ProdId)
                .FirstOrDefault<Product>();

            if (productDb != null)
            {
                _entityFramework.Products.Remove(productDb);

                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok();
                }
            }
            throw new Exception("Failed to delete the product");
        }
    }
}
