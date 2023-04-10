using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductManagerAPI.Data;
using ProductManagerAPI.Dtos;
using ProductManagerAPI.Models;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductEFController : ControllerBase
    {
        //DataContextEF _entityFramework;
        IProductRepository _productRepository;
        IMapper _mapper;

        public ProductEFController(IConfiguration config, IProductRepository productRepository)
        {
            //_entityFramework = new DataContextEF(config);
            _productRepository = productRepository;

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
            IEnumerable<Product> products = _productRepository.GetAllProducts();
            return products;
        }

        [HttpGet("{productId}")]
        public Product GetProduct(int productId)
        {
            return _productRepository.GetProduct(productId);
        }

        [HttpPut("{productId}")]
        public IActionResult EditProduct(Product product)
        {
            Product? productDb = _productRepository.GetProduct(product.ProdId);

            if (productDb != null)
            {
                productDb.ProdName = product.ProdName;
                productDb.ProdDescription = product.ProdDescription;
                productDb.Price = product.Price;
                productDb.InStock = product.InStock;

                if (_productRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to update the product");
            }
            throw new Exception("Failed to get the product");
        }

        [HttpPost]
        public IActionResult AddProduct(ProductToAddDto product)
        {
            Product productDb = _mapper.Map<Product>(product);

            _productRepository.AddEntity<Product>(productDb);

            if (_productRepository.SaveChanges())
            {
                return Ok();
            }
            
            throw new Exception("Failed to add the product");
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            Product? productDb = _productRepository.GetProduct(productId);

            if (productDb != null)
            {
                _productRepository.RemoveEntity<Product>(productDb);

                if (_productRepository.SaveChanges())
                {
                    return Ok();
                }
            }
            throw new Exception("Failed to delete the product");
        }
    }
}
