using GNS.Exceptions;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class AuthService(
        ICyberClubService cyberClubService,
        IEmployeeService employeeService,
        IGamingPlaceService gamingPlaceService,
        IWorkingHoursService workingHoursService,
        IOrderService orderService
            ) : IAuthService
    {

        private readonly ICyberClubService _cyberClubService = cyberClubService;
        private readonly IEmployeeService _employeeSerice = employeeService;
        private readonly IGamingPlaceService _gamingPlaceService = gamingPlaceService;
        private readonly IWorkingHoursService _workingHoursService = workingHoursService;
        private readonly IOrderService _orderService = orderService;

        public async Task<bool> VerifyOwnerAccessToEmployeeAsync(
            Guid ownerId,
            Guid employeeId,
            CancellationToken token = default
            )
        {
            var ownersCyberClubs = await _cyberClubService.GetOwnerCyberClubsAsync(ownerId, token)
                ?? throw new AccessViolationException();

            var employee = await _employeeSerice.GetByIdAsync(employeeId, token);
            var cyberClubId = employee.CyberClubId;

            return ownersCyberClubs.Any(cc => cc.Id == cyberClubId);

        }
        public async Task<bool> VerifyOwnerAccessToCyberClubAsync(
            Guid ownerId,
            Guid cyberClubId,
            CancellationToken token = default
            )
        {
            var ownersCyberClubs = await _cyberClubService.GetOwnerCyberClubsAsync(ownerId, token)
                ?? throw new AccessViolationException();

            return ownersCyberClubs.Any(cc => cc.Id == cyberClubId);
        }

        public async Task<bool> VerifyManagerAccessToEmployeeAsync(
            Guid managerId,
            Guid employeeId,
            CancellationToken token = default
            )
        {
            var manager = await _employeeSerice.GetByIdAsync(managerId, token)
                ?? throw new EntityNotFoundException("employee", managerId.ToString());

            if (manager.Role != Enums.Role.Manager)
            {
                return false;
            }

            var employee = await _employeeSerice.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("employee", employeeId.ToString());

            if (manager.CyberClubId != employee.CyberClubId)
            {
                return false;
            }
            if (employee.Role >= manager.Role)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> VerifyEmployeeAccessToGamingPlaceAsync(
            Guid employeeId,
            Guid gamingPlaceId,
            CancellationToken token = default
            )
        {
            var employee = await _employeeSerice.GetByIdAsync(employeeId, token);

            var gamingPlace = await _gamingPlaceService.GetByIdAsync(gamingPlaceId, token);
               
            return employee.CyberClubId == gamingPlace.CyberClubId;
        }

        public async Task<bool> VerifyOwnerAccessToGamingPlaceAsync(
            Guid ownerId,
            Guid gamingPlaceId,
            CancellationToken token = default
            )
        {
            var ownersCyberClubs = await _cyberClubService.GetOwnerCyberClubsAsync(ownerId, token)
                ?? throw new EntityNotFoundException("CyberClub", ownerId.ToString());

            var gamingPlace = await _gamingPlaceService.GetByIdAsync(gamingPlaceId, token)
                ?? throw new EntityNotFoundException("gaming place", gamingPlaceId.ToString());

            var gamingPlaceCyberClubId = gamingPlace.CyberClubId;
            return ownersCyberClubs.Any(cc => cc.Id == gamingPlaceCyberClubId);
        }

        public async Task<bool> VerifyOwnerAccessToWorkingHoursAsync(
            Guid ownerId,
            Guid workingHoursId,
            CancellationToken token = default
            )
        {
            var workingHours = await _workingHoursService.GetByIdAsync(workingHoursId, token)
                ?? throw new EntityNotFoundException("working hours", workingHoursId.ToString());
            var cyberClubId = workingHours.CyberClubId;
            var cyberClub = await _cyberClubService.GetClubByIdAsync(cyberClubId, token)
                ?? throw new EntityNotFoundException("cyber club", cyberClubId.ToString());

            return cyberClub.OwnerId == ownerId;
        }

        public async Task<bool> VerifyManagerAccessToCyberClubAsync(Guid managerId, Guid cyberClubId, CancellationToken token = default)
        {
            var manager = await _employeeSerice.GetByIdAsync(managerId, token)
                ?? throw new EntityNotFoundException("employee", managerId.ToString());
                
            return manager.CyberClubId == cyberClubId;
            
        }

        public async Task<bool> VerifyEmployeeAccessToOrderAsync(
            Guid employeeId,
            Guid orderId,
            CancellationToken token = default
            )
        {
            var employee = await _employeeSerice.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            var order = await _orderService.GetByIdAsync(orderId, token);
            var gamingPlace = await _gamingPlaceService.GetByIdAsync(order.GamingPlaceId, token);
            return gamingPlace?.CyberClubId == employee.CyberClubId;

        }
    }
}