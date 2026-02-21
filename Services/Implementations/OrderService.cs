using GNS.Extensions;
using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;

namespace GNS.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ITimeSlotsService _timeSlotsService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IGamingPlacesRepository _gamingPlacesRepository;
        private readonly IUsersRepository _usersRepository;

        public OrderService(
            ITimeSlotsService timeSlotsService,
            IHttpContextAccessor contextAccessor,
            IOrdersRepository ordersRepository,
            IGamingPlacesRepository gamingPlacesRepository,
            IUsersRepository usersRepository)
        {
            _timeSlotsService = timeSlotsService;
            _contextAccessor = contextAccessor;
            _ordersRepository = ordersRepository;
            _gamingPlacesRepository = gamingPlacesRepository;
            _usersRepository = usersRepository;
        }
        public async Task CreateOrder(CreateOrderRequest request)
        {

            if (!DateTime.TryParse(request.DateTime, out DateTime dt))
            {
                Results.BadRequest();
            }

            var userId = _contextAccessor.GetHttpUserId();
            var gamingPlaceWithCC = await _gamingPlacesRepository.GetByIdWithCC(request.GamingPlaceId);
            var date = DateOnly.FromDateTime(dt);
            var duration = TimeSpan.FromHours(request.Duration);

            var availableTimeSlots = await _timeSlotsService.GetAvailableSlotsAsync(
                gamingPlaceWithCC.CyberClubId,
                gamingPlaceWithCC.Id,
                date,
                duration);

            var requiredTimeSlot = new TimeSlotDto
            (
                TimeOnly.FromDateTime(dt),
                TimeOnly.FromDateTime(dt).Add(duration)
            );
            if (!availableTimeSlots.Contains(requiredTimeSlot))
            {
                Results.Conflict("Required time is not available.");
            }
            await _ordersRepository.CreateOrderAsync(
                userId,
                request.GamingPlaceId,
                DateOnly.FromDateTime(DateTime.Parse(request.DateTime)),
                TimeOnly.FromDateTime(DateTime.Parse(request.DateTime)),
                request.Duration
            );
        }
        public async Task<List<OrderDto>> GetActiveOrders()
        {
            var id = _contextAccessor.GetHttpUserId();
            var activeOrders = await _ordersRepository.GetByUserId(id);

            return activeOrders
                .OrderByDescending(ao => ao.Date)
                .ThenByDescending(ao => ao.StartTime)
                .Select(ao => new OrderDto(ao))
                .ToList();

        }

        public async Task<List<OrderDto>> GetByUserEmail(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email)
                ?? throw new Exception($"User with email {email} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByUserId(userId);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.Date)
                .ThenBy(o => o.StartTime)
                .Select(o => new OrderDto(o))
                .ToList();
        }
        public async Task<List<OrderDto>> GetByUserName(string userName)
        {
            var user = await _usersRepository.GetByUserNameAsync(userName)
                ?? throw new Exception($"User with UserName {userName} not found");

            var userId = user.Id;
            var userOrders = await _ordersRepository.GetByUserId(userId);

            return userOrders
                .OrderBy(o => o.OrderStatus)
                .ThenBy(o => o.Date)
                .ThenBy(o => o.StartTime)
                .Select(o => new OrderDto(o))
                .ToList();
        }

        public async Task<List<OrderDto>> GetTodaysOrders()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var todayOrders = await _ordersRepository.GetByDate(today);
            return todayOrders
                .OrderBy(td => td.StartTime)
                .Select(o => new OrderDto(o))
                .ToList();
        }
        public async Task UpdateOrderStatus(Guid orderId, string status)
        {
            await _ordersRepository.UpdateStatus(orderId, status);
        }

    }
}