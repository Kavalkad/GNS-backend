using GNS.Extensions;
using GNS.Dto;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Data.Entities;

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
        public async Task CreateOrder(CreateOrderRequest request)
        {

            if (!DateTime.TryParse(request.DateTimeStart, out DateTime dtStart))
            {
                Results.BadRequest();
                return;
            }
            if (!DateTime.TryParse(request.DateTimeEnd, out DateTime dtEnd))
            {
                Results.BadRequest();
                return;
            }

            var userId = _contextAccessor.TryGetHttpUserId();
            var gamingPlaceWithCC = await _gamingPlacesRepository.GetByIdWithCC(request.GamingPlaceId);
            var date = DateOnly.FromDateTime(dtStart);
            //var duration = TimeSpan.FromHours(request.Duration);

            /*    var unAvailableTimeSlots = await _timeSlotsService.GetUnAvailableSlotsAsync(
                    cyberClubId: gamingPlaceWithCC.CyberClubId,
                    gamingPlaceId: gamingPlaceWithCC.Id,
                    date: date);
            */
            var requiredTimeSlot = new TimeSlotDto
            (
                TimeOnly.FromDateTime(dtStart),
                TimeOnly.FromDateTime(dtEnd)
            //TimeOnly.FromDateTime(dt).Add(duration)
            );
            /*    if (unAvailableTimeSlots.Contains(requiredTimeSlot))
                 {
                     Results.Conflict("Required time is not available.");
                 }
                 */
            await _ordersRepository.CreateOrderAsync(
                userId,
                request.GamingPlaceId,
                date,
                TimeOnly.FromDateTime(dtStart),
                TimeOnly.FromDateTime(dtEnd)
            );
            await _unitOfWork.SaveChangesAsync();
            TypedResults.Ok(requiredTimeSlot);
            return;
        }
        public async Task<IEnumerable<OrderEntity>> GetByDateAndGamingPlace(DateOnly date, Guid gamingPlaceId)
        {
            var gamingPlaceDateOrders = await _ordersRepository.GetByDate(date);
            return gamingPlaceDateOrders.Where(o => o.GamingPlaceId == gamingPlaceId);
        }
        public async Task<List<OrderDto>> GetActiveOrders()
        {
            var id = _contextAccessor.TryGetHttpUserId();
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