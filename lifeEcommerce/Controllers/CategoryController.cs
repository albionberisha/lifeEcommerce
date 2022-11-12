using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using lifeEcommerce.Models.Dtos;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet("GetCategory")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();

            return Ok(categories);
        }

        [HttpGet("CategoriesListView")]
        public async Task<IActionResult> CategoriesListView(string? search, int page = 1, int pageSize = 10)
        {
            var categories = await _categoryService.CategoriesListView(search ,page, pageSize);

            return Ok(categories);
        }

        [HttpPost("PostCategory")]
        public async Task<IActionResult> Post(CategoryCreateDto CategoryToCreate)
        {
            await _categoryService.CreateCategory(CategoryToCreate);

            return Ok("Category created successfully!");
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> Update(CategoryDto CategoryToUpdate)
        {
            await _categoryService.UpdateCategory(CategoryToUpdate);

            return Ok("Category updated successfully!");
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);

            return Ok("Category deleted successfully!");
        }

    }
}
