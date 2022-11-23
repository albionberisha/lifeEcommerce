using lifeEcommerce.Models.Dtos.ShoppingCard;

namespace lifeEcommerce.Services.IService
{
    public interface IShoppingCardService
    {
        Task AddProductToCard(string userId, int productId, int count);
        Task<List<ShoppingCardViewDto>> GetShoppingCardContentForUser(string userId);
        Task Plus(int shoppingCardItemId, int? newQuantity);
    }
}
