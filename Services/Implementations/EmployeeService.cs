using GNS.Extensions;
using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using GNS.Exceptions;

namespace GNS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICyberClubService _cyberClubService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBloomBytesService _bloomBytesService;

        public EmployeeService(
            IEmployeesRepository employeesRepository,
            IHasher hasher,
            ITokenService tokenService,
            IHttpContextAccessor contextAccessor,
            ICyberClubService cyberClubService,
            IUnitOfWork unitOfWork,
            IBloomBytesService bloomBytesService
            )
        {
            _employeesRepository = employeesRepository;
            _hasher = hasher;
            _tokenService = tokenService;
            _contextAccessor = contextAccessor;
            _cyberClubService = cyberClubService;
            _unitOfWork = unitOfWork;
            _bloomBytesService = bloomBytesService;
        }
        public async Task<LoginEmployeeResponse> LoginAsync(LoginEmployeeRequest request, CancellationToken token = default)
        {
            var employee = await _employeesRepository.FindAsync(e => e.Email == request.Email, token)
                ?? throw new EntityNotFoundException("Employee", request.Email);

            bool isVerified = _hasher.Verify(request.Password, employee.HashedPassword)
                && _hasher.Verify(request.SecretWord, employee.HashedSecretWord);

            if (!isVerified)
            {
                //// Results.Unauthorized();
                throw new Exception("You entered wrong employee data");
            }

            var accessToken = _tokenService.GenerateAccessToken(employee);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(employee.Id, token);
            var role = Enum.GetName(employee.Role);

            return new LoginEmployeeResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString(),
                Role = role
            };
        }

        public async Task RegisterAsync(RegisterEmployeeRequest request, CancellationToken token = default)
        {


            var cyberClub = await _cyberClubService.FindByCyberClubNameAsync(request.CyberClubName, token)
                ?? throw new EntityNotFoundException("CyberClub", request.CyberClubName);

            // Дядя, а ты точно овнер этого клуба
            var ownerId = _contextAccessor.TryGetHttpUserId();
            if (cyberClub.OwnerId != ownerId)
            {
                Results.Unauthorized();
                return;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync(token);

                var hashedPassword = _hasher.Generate(request.Password);
                var hashedSecretWord = _hasher.Generate(request.SecretWord);

                var employeeRole = Enum.Parse<Role>(request.RoleName);

                var bloomBytesId = await _bloomBytesService.SaveBloomBytesAsync(request.Email, request.UserName, token);

                var newEmployee = new EmployeeEntity
                (
                    email: request.Email,
                    hashedPassword: hashedPassword,
                    userName: request.UserName,
                    role: employeeRole,
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    hashedSecretWord: hashedSecretWord,
                    salary: request.Salary,
                    cyberClubId: cyberClub.Id,
                    bloomBytesId: bloomBytesId
                );

                await _employeesRepository.AddAsync(newEmployee, token);

                await _unitOfWork.SaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(token);
                Results.InternalServerError("Transaction failed. " + ex.Message);
            }


        }
        public async Task<List<EmployeeDto>> GetAllAsync(CancellationToken token = default)
        {
            var employees = await _employeesRepository.GetAllAsync(token)
                ?? throw new Exception("GetAllEmployees exception");
            return employees
                .Select(e => new EmployeeDto(e))
                .OrderByDescending(e => e.RoleName)
                .ToList();
        }
        public async Task<List<EmployeeDto>> GetWithBonusAsync(CancellationToken token = default)
        {
            var employees = await GetAllAsync(token)
                ?? throw new Exception("GetAllEmployees exception");
            return employees.Where(e => e.Bonus != 0).ToList();

        }
        public async Task<List<EmployeeDto>> GetWithPenaltyAsync(CancellationToken token = default)
        {
            var employees = await GetAllAsync(token)
                ?? throw new Exception("GetAllEmployees exception");
            return employees.Where(e => e.Penalty != 0).ToList();

        }
        public async Task<List<EmployeeDto>> GetByCyberClubIdAsync(string cyberClubId, CancellationToken token = default)
        {

            if (!Guid.TryParse(cyberClubId, out Guid resultId))
            {
                throw new IncorrectGuidException(cyberClubId);
            }

            var employees = await _employeesRepository.GetByExpressionAsync(e => e.CyberClubId == resultId, token);

            return employees
                .Select(e => new EmployeeDto(e))
                .ToList();
        }


        /* public async Task<List<EmployeeDto>> GetByCyberClubNameAsync(string cyberClubName, CancellationToken token = default)
         {
             var cyberClub = await _cyberClubService.G
             var employees = await _employeesRepository.GetByExpressionAsync(e => e.CyberClub.Name == cyberClubName, token);

             return employees
                 .Select(e => new EmployeeDto(e))
                 .ToList();
         } 
         */
        public async Task<EmployeeDto> GetByNamesAsync(string firstName, string lastName, CancellationToken token = default)
        {
            var employee = await _employeesRepository.FindAsync(
                e => e.FirstName == firstName && e.LastName == lastName, token)
                    ?? throw new EntityNotFoundException("Employee", "Костыль");

            return new EmployeeDto(employee);
        }
        public async Task UpdateFirstNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);

            employee.FirstName = request.Name;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateLastNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);

            employee.LastName = request.Name;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateRoleNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }
            if (!Enum.TryParse(request.Name, out Role role))
            {
                throw new IncorrectRoleNameException(request.Name);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);

            employee.Role = role;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateCyberClubNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }

            var cyberClub = await _cyberClubService.FindByCyberClubNameAsync(request.Name, token)
                ?? throw new EntityNotFoundException("CyberClub", request.Name);

            var employee = await _employeesRepository.FindAsync(e => e.Id == employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);

            employee.CyberClubId = cyberClub.Id;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task SetZeroBonusesAsync(CancellationToken token = default)
        {
            await _employeesRepository.SetZeroBonusesAsync(token);
        }
        public async Task SetZeroPenaltiesAsync(CancellationToken token = default)
        {
            await _employeesRepository.SetZeroPenaltiesAsync(token);
        }

        public async Task DeleteAsync(DeleteEmployeeRequest request, CancellationToken token = default)
        {
            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }
            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);

            _employeesRepository.Delete(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task GiveBonusAsync(GiveBonusRequest request, CancellationToken token = default)
        {
            var id = _contextAccessor.TryGetHttpUserId();

            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);
            employee.Bonus = request.Bonus;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task GivePenaltyAsync(GivePenaltyRequest request, CancellationToken token = default)
        {
            // var ownerId = _contextAccessor.TryGetHttpUserId();

            if (!Guid.TryParse(request.EmployeeId, out Guid employeeId))
            {
                throw new IncorrectGuidException(request.EmployeeId);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", request.EmployeeId);
            employee.Penalty = request.Penalty;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}