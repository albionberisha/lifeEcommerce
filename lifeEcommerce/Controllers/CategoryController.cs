using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity.UI.Services;
using lifeEcommerce.Models.Dtos.Category;
using UAParser;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IStringLocalizer<CategoryController> _localizer;
        private readonly IEmailSender _emailSender;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IStringLocalizer<CategoryController> localizer, IEmailSender emailSender)
        {
            _categoryService = categoryService;
            _logger = logger;
            _localizer = localizer;
            _emailSender = emailSender;
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            //await _emailSender.SendEmailAsync("albion.b@gjirafa.com","Hello From Life", "Content");

            //var category = _localizer["category"];
            //var category1 = _localizer.GetString("category").Value;

            //try
            //{
                int num = 4;
                int num2 = 0;

                int num3 = num / num2;
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex, "Error i LIFE");
            //    _logger.LogInformation(ex, "Error i LIFE");
            //    _logger.LogDebug(ex, "Error i LIFE");
            //}

            return Ok();
            //return Ok($"{category} {category1}");
        }

        [HttpGet("GetUserAgent")]
        public async Task<IActionResult> GetUserAgent()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(userAgent);

            return Ok(c);
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
