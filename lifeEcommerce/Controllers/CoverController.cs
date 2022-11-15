using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using lifeEcommerce.Models.Dtos;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoverController : Controller
    {
        private readonly ICoverService _coverService;

        public CoverController(ICoverService coverService)
        {
            _coverService = coverService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cover = await _coverService.GetCover(id);

            if (cover == null)
            {
                return NotFound();
            }

            return Ok(cover);
        }

        [HttpGet]
        public async Task<IActionResult> GetCovers()
        {
            var covers = await _coverService.GetAllCovers();

            return Ok(covers);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UnitCreateDto coverToCreate)
        {
            await _coverService.CreateCover(coverToCreate);

            return Ok("Cover created successfully!");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UnitDto coverToUpdate)
        {
            await _coverService.UpdateCover(coverToUpdate);

            return Ok("Cover updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _coverService.DeleteCover(id);

            return Ok("Category deleted successfully!");
        }

    }
}
