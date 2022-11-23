using lifeEcommerce.Models.Dtos.ShoppingCard;
using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    [Authorize]
    public class CardController : Controller
    {
        private readonly IShoppingCardService _cardService;
        private readonly IConfiguration _configuration;

        public CardController(IShoppingCardService cardService, IConfiguration configuration)
        {
            _cardService = cardService;
            _configuration = configuration;
        }


        [HttpPost("AddToCard")]
        public async Task<IActionResult> AddProductToCard(int count, int productId)
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(userId == null) { return Unauthorized(); }

            _cardService.AddProductToCard(userId, productId, count);

            return Ok("Added to card!");
        }


        [HttpGet("ShoppingCardContent")]
        public async Task<IActionResult> ShoppingCardContent()
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;

            Task<List<ShoppingCardViewDto>>? shoppingCardContentForUser = _cardService.GetShoppingCardContentForUser(userId);

            return Ok(shoppingCardContentForUser);
        }

        [HttpPost("IncreaseQuantityForProduct")]
        public async Task<IActionResult> Plus(int? newQuantity, int shoppingCardItemId)
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null) { return Unauthorized(); }

            _cardService.Plus(shoppingCardItemId, newQuantity);

            return Ok();
        }

        [HttpPost("ProductSummaryForOrder")]
        public async Task<IActionResult> ProductSummary()
        {
            var userData = (ClaimsIdentity)User.Identity;
            var userId = userData.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null) { return Unauthorized(); }

            return Ok();
        }

    }
}
