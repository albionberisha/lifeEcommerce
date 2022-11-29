using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using AutoMapper;
using lifeEcommerce.Models.Entities;
using lifeEcommerce.Models.Dtos.ShoppingCard;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Dtos.Order;

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

            foreach (ShoppingCard item in usersShoppingCard)
            {
                var currentProduct = item.Product;

                var calculatedPrice = HelperMethods.GetPriceByQuantity(item.Count, currentProduct.Price, currentProduct.Price50, currentProduct.Price100);

                var model = new ShoppingCardViewDto
                {
                    ShoppingCardItemId = item.Id,
                    ProductId = item.ProductId,
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

        public async Task CreateOrder(AddressDetails addressDetails, List<ShoppingCardViewDto> shoppingCardItems)
        {
            var orders = new List<OrderData>();
            List<int>? shoppingCardItemIdsToRemove = new ();

            var trackingId = Guid.NewGuid().ToString();

            var orderDetailsList = new List<OrderDetails>();

            foreach (ShoppingCardViewDto item in shoppingCardItems)
            {
                var order = new OrderData
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.AddDays(7),
                    OrderTotal = (decimal)item.Total,
                    PhoheNumber = addressDetails.PhoheNumber,
                    StreetAddress = addressDetails.StreetAddress,
                    City = addressDetails.City,
                    Country = addressDetails.Country,
                    PostalCode = addressDetails.PostalCode,
                    Name = addressDetails.Name,
                    TrackingId = trackingId
                };

                var orderDetails = new OrderDetails
                {
                    OrderDataId = order.OrderId,
                    ProductId = item.ProductId,
                    Count = item.ShopingCardProductCount,
                    Price = (decimal)item.Total
                };

                orderDetailsList.Add(orderDetails);

                orders.Add(order);

                shoppingCardItemIdsToRemove.Add(item.ShoppingCardItemId);

            }

            var shoppingCardItemsToRemove = await _unitOfWork.Repository<ShoppingCard>()
                                                                          .GetByCondition(x => shoppingCardItemIdsToRemove.Contains(x.Id))
                                                                          .ToListAsync();

            _unitOfWork.Repository<OrderData>().CreateRange(orders);

            _unitOfWork.Repository<ShoppingCard>().DeleteRange(shoppingCardItemsToRemove);

            _unitOfWork.Repository<OrderDetails>().CreateRange(orderDetailsList);

            _unitOfWork.Complete();
        }
    }
}
