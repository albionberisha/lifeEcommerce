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

        public async Task<List<ShoppingCardViewDto>> GetShoppingCardContentForUser(string userId)
        {
            var usersShoppingCard = await _unitOfWork.Repository<ShoppingCard>()
                                                                    .GetByCondition(x => x.UserId == userId)
                                                                    .ToListAsync();

            List<int> productIds = usersShoppingCard.Select(x => x.ProductId).ToList();


            var productData = await _unitOfWork.Repository<Product>()
                                                              .GetByCondition(x => productIds.Contains(x.Id))
                                                              .ToDictionaryAsync(x => x.Id, y => y);

            //var productData = _unitOfWork.Repository<Product>()
            //                                      .GetByCondition(x => productIds.Contains(x.Id));


            var shoppingCardList = new List<ShoppingCardViewDto>();

            foreach (var item in usersShoppingCard)
            {
                var currentProduct = productData.FirstOrDefault(x => x.Id == item.ProductId);

                var calculatedPrice = HelperMethods.GetPriceByQuantity(item.Count, currentProduct.Price, currentProduct.Price50, currentProduct.Price100);

                var model = new ShoppingCardViewDto
                {
                    ProductImage = productData[item.ProductId].ImageUrl,
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

            return shoppingCardList;
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
