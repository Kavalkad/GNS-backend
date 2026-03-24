using GNS.Extensions;
using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;
using GNS.Enums;

namespace GNS.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IGamingPlacesRepository _gamingPlacesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IHttpContextAccessor contextAccessor,
            IOrdersRepository ordersRepository,
            IGamingPlacesRepository gamingPlacesRepository,
            IUsersRepository usersRepository,
            IUnitOfWork unitOfWork
)
        {
            _contextAccessor = contextAccessor;
            _ordersRepository = ordersRepository;
            _gamingPlacesRepository = gamingPlacesRepository;
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TimeSlotDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken token = default)
        {

            if (!DateTime.TryParse(request.DateTimeStart, out DateTime dtStart))
            {
                throw new Exception("Invalid start time value");
            }

            if (!DateTime.TryParse(request.DateTimeEnd, out DateTime dtEnd))
            {
                throw new Exception("Invalid end time value");
            }

            if (dtStart - dtEnd != TimeSpan.FromHours(1))
            {
                throw new Exception("You can order only 1 hour");
            }

            var userId = _contextAccessor.TryGetHttpUserId();

           
            await _ordersRepository.CreateOrderAsync(
                userId,
                request.GamingPlaceId,
                dtStart,
                dtEnd,
                token
            );
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
            var gamingPlaceDateOrders = await _ordersRepository.GetByDateAsync(date, token);

            return gamingPlaceDateOrders.Where(o => o.GamingPlaceId == gamingPlaceId).ToList();
        }
        public async Task<List<OrderDto>> GetActiveOrdersAsync(CancellationToken token = default)
        {
            var id = _contextAccessor.TryGetHttpUserId();
            var activeOrders = await _ordersRepository.GetByUserIdAsync(id, token);

            return activeOrders
                .OrderByDescending(ao => ao.DateTimeStart)
                .Select(ao => new OrderDto(ao))
                .ToList();

        }

        public async Task<List<OrderDto>> GetByUserEmailAsync(string email, CancellationToken token = default)
        {
            var user = await _usersRepository.GetByEmailAsync(email)
                ?? throw new Exception($"User with email {email} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByUserIdAsync(userId, token);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
        }
        public async Task<List<OrderDto>> GetByUserNameAsync(string userName, CancellationToken token = default)
        {
            var user = await _usersRepository.GetByUserNameAsync(userName, token)
                ?? throw new Exception($"User with UserName {userName} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByUserIdAsync(userId);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
        }

        public async Task<List<OrderDto>> GetTodaysOrdersAsync(CancellationToken token = default)
        {
            var today = DateTime.Now;
            var todayOrders = await _ordersRepository.GetByDateAsync(today, token);
            return todayOrders
                .OrderBy(td => td.DateTimeStart)
                .Select(o => new OrderDto(o))
                .ToList();
        }
        public async Task UpdateOrderStatusAsync(Guid orderId, string status, CancellationToken token = default)
        {
            if (Enum.TryParse(status, out OrderStatus orderStatus))
            {
                throw new Exception("Invalid order status name");
            }
            await _ordersRepository.UpdateStatusAsync(orderId, orderStatus, token);
        }

    }
}