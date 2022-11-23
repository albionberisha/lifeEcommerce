using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using AutoMapper;

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
    }
}
