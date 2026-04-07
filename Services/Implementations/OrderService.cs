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
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IHttpContextAccessor contextAccessor,
            IOrdersRepository ordersRepository,
            IUserService userService,
            IUnitOfWork unitOfWork
)
        {
            _contextAccessor = contextAccessor;
            _ordersRepository = ordersRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        public async Task<TimeSlotDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken token = default)
        {

            if (!Guid.TryParse(request.GamingPlaceId, out Guid gamingPlaceId))
            {
                throw new IncorrectGuidException(request.GamingPlaceId);
            }
            if (!DateTime.TryParse(request.DateTimeStart, out DateTime dtStart))
            {
                throw new Exception("Invalid start time value");
            }

            if (!DateTime.TryParse(request.DateTimeEnd, out DateTime dtEnd))
            {
                throw new Exception("Invalid end time value");
            }

            if (dtEnd - dtStart != TimeSpan.FromHours(1))
            {
                throw new Exception("You can order only 1 hour");
            }

            var userId = _contextAccessor.TryGetHttpUserId();

            var order = new OrderEntity
            {
                UserId = userId,
                DateTimeStart = dtStart,
                DateTimeEnd = dtEnd,
                GamingPlaceId = gamingPlaceId
            };
            await _ordersRepository.AddAsync(order, token);
            await _unitOfWork.SaveChangesAsync(token);

            var requiredTimeSlotDto = new TimeSlotDto
            (
                dtStart,
                dtEnd
            );
            return requiredTimeSlotDto;
        }
        public async Task<List<OrderEntity>> GetByDateAndGamingPlaceAsync(
            DateTime date,
            Guid gamingPlaceId,
            CancellationToken token = default)
        {
            var gamingPlaceDateOrders = await _ordersRepository
                .GetByExpressionAsync(o => o.DateTimeStart.Date == date, token);

            return gamingPlaceDateOrders.Where(o => o.GamingPlaceId == gamingPlaceId).ToList();
        }
        public async Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken token = default)
        {
            var id = _contextAccessor.TryGetHttpUserId();
            var activeOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == id, token);

            return activeOrders
                .OrderByDescending(ao => ao.DateTimeStart)
                .Select(ao => new OrderDto(ao))
                .ToList();

        }

        public async Task<List<OrderDto>> GetByUserEmailAsync(string email, CancellationToken token = default)
        {
            var user = await _userService.FindUserAsync(u =>u.Email == email)
                ?? throw new Exception($"User with email {email} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == userId, token);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
        }
        public async Task<List<OrderDto>> GetByUserNameAsync(string userName, CancellationToken token = default)
        {
            var user = await _userService.FindUserAsync(u => u.UserName == userName, token)
                ?? throw new Exception($"User with UserName {userName} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByExpressionAsync(o => o.UserId == userId, token);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
        }

        public async Task<List<OrderDto>> GetTodaysOrdersAsync(CancellationToken token = default)
        {
            var today = DateTime.UtcNow;
            var todayOrders = await _ordersRepository.GetByExpressionAsync(o => o.DateTimeEnd.Date == today.Date, token);
            return todayOrders
                .OrderBy(td => td.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
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

    }
}