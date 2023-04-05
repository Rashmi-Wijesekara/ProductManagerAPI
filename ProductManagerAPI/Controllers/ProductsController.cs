using Microsoft.AspNetCore.Mvc;
using ProductManagerAPI.Data;
using ProductManagerAPI.Dtos;
using ProductManagerAPI.Models;

namespace ProductManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        DataContextDapper _dapper;
        public ProductsController(IConfiguration config) 
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("test")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            string sql = @"
                SELECT [ProdId],
                    [ProdName],
                    [ProdDescription],
                    [Price],
                    [InStock] 
                FROM ProductsSchema.Product";

            IEnumerable<Product> products = _dapper.LoadData<Product>(sql);
            return products;
        }

        [HttpGet("{productId}")]
        public Product GetProduct(int productId)
        {
            string sql = @"
                SELECT [ProdId],
                    [ProdName],
                    [ProdDescription],
                    [Price],
                    [InStock] 
                FROM ProductsSchema.Product
                WHERE [ProdId] = "+ productId.ToString();

            Console.WriteLine(sql);

            Product product = _dapper.LoadDataSingle<Product>(sql);
            return product;
        }

        [HttpPut("{productId}")]
        public IActionResult EditProduct(Product product)
        {
            string sql = @"
                UPDATE [ProductsSchema].[Product]
                SET [ProdName] = '" + product.ProdName + 
                "', [ProdDescription] = '"+ product.ProdDescription +
                "', [Price] = " + product.Price +
                ", [InStock] = '" + product.InStock +
                "' WHERE ProdId = " + product.ProdId;

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update the product: "+ product.ProdId);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductToAddDto product)
        {
            string sql = @"
                INSERT INTO ProductsSchema.Product 
                    (ProdName, ProdDescription, Price, InStock)
                VALUES ('" + 
                    product.ProdName +
                    "','" + product.ProdDescription +
                    "','" + product.Price +
                    "','" + product.InStock +
                    "')";

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to create the product");
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            string sql = @"
                DELETE FROM [ProductsSchema].[Product]
                WHERE ProdId = " + productId;

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete the product: " + productId);
        }
    }
}
