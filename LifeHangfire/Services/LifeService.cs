using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace LifeHangfire.Services
{
    public class LifeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public LifeService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task SendEmail()
        {
            var createdOrders = await _unitOfWork.Repository<OrderData>()
                                                                    .GetByCondition(x => x.OrderStatus == StaticDetails.Created)
                                                                    .ToListAsync();

            foreach (var order in createdOrders) 
            {
                await _emailSender.SendEmailAsync("albion.b@gjirafa.com", "New Order to Check", $"New order created: {order.OrderId}");
            }
        }
    }
}
