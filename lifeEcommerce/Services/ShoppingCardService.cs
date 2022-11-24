using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using AutoMapper;
using lifeEcommerce.Models.Entities;
using lifeEcommerce.Models.Dtos.ShoppingCard;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Helpers;

namespace lifeEcommerce.Services
{
    public class ShoppingCardService : IShoppingCardService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddProductToCard(string userId, int productId, int count)
        {
            var shoppingCardItem = new ShoppingCard
            {
                UserId = userId,
                ProductId = productId,
                Count = count
            };

            _unitOfWork.Repository<ShoppingCard>().Create(shoppingCardItem);
            _unitOfWork.Complete();
        }

        public async Task<ShoppingCardDetails> GetShoppingCardContentForUser(string userId)
        {
            var usersShoppingCard = await _unitOfWork.Repository<ShoppingCard>()
                                                                    .GetByCondition(x => x.UserId == userId)
                                                                    .Include(x => x.Product)  
                                                                    .ToListAsync();

            var shoppingCardList = new List<ShoppingCardViewDto>();

            foreach (var item in usersShoppingCard)
            {
                var currentProduct = item.Product;

                var calculatedPrice = HelperMethods.GetPriceByQuantity(item.Count, currentProduct.Price, currentProduct.Price50, currentProduct.Price100);

                var model = new ShoppingCardViewDto
                {
                    ProductImage = currentProduct.ImageUrl,
                    ProductDescription = currentProduct.Description,
                    ProductName = currentProduct.Title,
                    ProductPrice = calculatedPrice,
                    ShopingCardProductCount = item.Count,
                    Total = calculatedPrice * item.Count
                };

                shoppingCardList.Add(model);
            }

            var shoppingCardDetails = new ShoppingCardDetails()
            {
                ShoppingCardItems = shoppingCardList,
                CardTotal = shoppingCardList.Select(x => x.Total).Sum()
            };

            return shoppingCardDetails;
        }

        public async Task Plus(int shoppingCardItemId, int? newQuantity)
        {
            var shoppingCardItem = await _unitOfWork.Repository<ShoppingCard>()
                                                                .GetById(x => x.Id == shoppingCardItemId)
                                                                .FirstOrDefaultAsync();

            if (newQuantity == null)
                shoppingCardItem.Count++;
            else
                shoppingCardItem.Count = (int)newQuantity;

            _unitOfWork.Repository<ShoppingCard>().Update(shoppingCardItem);
            _unitOfWork.Complete();
        }

        public async Task Minus(int shoppingCardItemId, int? newQuantity)
        {
            var shoppingCardItem = await _unitOfWork.Repository<ShoppingCard>()
                                                                .GetById(x => x.Id == shoppingCardItemId)
                                                                .FirstOrDefaultAsync();

            if (newQuantity == null)
                shoppingCardItem.Count--;
            else
                shoppingCardItem.Count = (int)newQuantity;

            _unitOfWork.Repository<ShoppingCard>().Update(shoppingCardItem);
            _unitOfWork.Complete();
        }
    }
}
