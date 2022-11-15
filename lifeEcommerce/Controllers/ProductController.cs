using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using lifeEcommerce.Models.Dtos;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var categories = await _productService.GetAllProducts();

            return Ok(categories);
        }

        [HttpGet("{search}")]
        public async Task<IActionResult> ProductsListView(string? search, int categoryId = 0, int page = 1, int pageSize = 10)
        {
            var products = await _productService.ProductsListView(search, page, pageSize, categoryId);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductCreateDto ProductToCreate)
        {
            await _productService.CreateProduct(ProductToCreate);

            return Ok("Product created successfully!");
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDto ProductToUpdate)
        {
            await _productService.UpdateProduct(ProductToUpdate);

            return Ok("Product updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);

            return Ok("Product deleted successfully!");
        }

    }
}
