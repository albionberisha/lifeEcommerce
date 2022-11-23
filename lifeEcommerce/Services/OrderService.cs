using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using AutoMapper;

namespace lifeEcommerce.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
