using GNS.Extensions;
using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Enums;
using GNS.Exceptions;


namespace GNS.Services.Implementations
{
    public class OrderService(
        IHttpContextAccessor contextAccessor,
        IOrdersRepository ordersRepository,
        IUserService userService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IGamingPlaceService gamingPlaceService) : IOrderService
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IOrdersRepository _ordersRepository = ordersRepository;
        private readonly IUserService _userService = userService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IGamingPlaceService _gamingPlaceService = gamingPlaceService;

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken token = default)
        {
            var gamingPlaceId = request.GamingPlaceId;
            var gamingPlace = await _gamingPlaceService.GetByIdWithDetails(gamingPlaceId, token);
            var totalSum = (request.DateTimeEnd.Hour - request.DateTimeStart.Hour) * gamingPlace.PricePerHour;

            var userId = _contextAccessor.TryGetHttpUserId();

            var order = new OrderEntity
            {
                UserId = userId,
                DateTimeStart = request.DateTimeStart,
                DateTimeEnd = request.DateTimeEnd,
                CyberClubName = gamingPlace.CyberClub.Name,
                GamingPlaceNumber = gamingPlace.Number,
                Equipment = gamingPlace.Equipment,
                TotalSum = totalSum,
                OrderStatus = OrderStatus.Booked
            };
            await _ordersRepository.AddAsync(order, token);
            await _unitOfWork.SaveChangesAsync(token);


            return _mapper.MapToOrderDto(order);
        }
        public async Task<List<OrderEntity>> GetByDateAndGamingPlaceAsync(
            DateTime date,
            Guid gamingPlaceId,
            CancellationToken token = default)
        {
            return await _ordersRepository
                .GetByExpressionAsync(o => o.DateTimeStart.Date == date.Date
                    && o.GamingPlaceId == gamingPlaceId, token);
        }
        public async Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken token = default)
        {
            var userId = _contextAccessor.TryGetHttpUserId();
            var activeOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == userId, token)
                ?? throw new EntityNotFoundException("order", $"userId: {userId}");

            return _mapper.MapToOrderDto(activeOrders);

        }

        public async Task<List<OrderDto>> GetByUserEmailAsync(string email, CancellationToken token = default)
        {
            var user = await _userService.FindByExpression(u => u.Email == email, token)
                ?? throw new EntityNotFoundException("user", $" email: {email}");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == userId, token);

            var orderedUserOrders = userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart);

            return _mapper.MapToOrderDto(orderedUserOrders);
        }
        public async Task<List<OrderDto>> GetByUserNameAsync(string userName, CancellationToken token = default)
        {
            var user = await _userService.FindByExpression(u => u.UserName == userName, token)
               ?? throw new EntityNotFoundException("user", $" username: {userName}");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == userId, token);

            var orderedUserOrders = userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart);

            return _mapper.MapToOrderDto(orderedUserOrders);
        }

        public async Task<List<OrderDto>> GetTodaysOrdersAsync(CancellationToken token = default)
        {
            var today = DateTime.Now;
            var todayOrders = await _ordersRepository.GetByExpressionAsync(o => o.DateTimeEnd.Date == today.Date, token);

            var orderedOrders = todayOrders.OrderBy(td => td.DateTimeStart);

            return  _mapper.MapToOrderDto(orderedOrders);
        }
        public async Task UpdateOrderStatusAsync(Guid orderId, string status, CancellationToken token = default)
        {
            if (!Enum.TryParse(status, out OrderStatus orderStatus))
            {
                throw new Exception("Invalid order status name");
            }
            var order = await _ordersRepository.GetByIdAsync(orderId, token)
                ?? throw new EntityNotFoundException("Order", orderId.ToString());

            order.OrderStatus = orderStatus;

            _ordersRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task<OrderEntity> GetByIdAsync(Guid orderId, CancellationToken token = default)
        {
            return await _ordersRepository.GetByIdAsync(orderId, token)
                ?? throw new EntityNotFoundException("order", orderId.ToString());
        }
    }
}