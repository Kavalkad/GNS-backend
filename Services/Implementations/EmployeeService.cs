using GNS.Extensions;
using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using Microsoft.AspNetCore.Mvc.ViewComponents;

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
        public async Task<LoginEmployeeResponse> Login(LoginEmployeeRequest request)
        {
            var employee = await _employeesRepository.GetByEmail(request.Email)
                ?? throw new Exception($"Employee with email {request.Email} not found");
            

            bool isFound = _hasher.Verify(request.Password, employee.HashedPassword)
                && _hasher.Verify(request.SecretWord, employee.HashedSecretWord);

            if (!isFound)
            {
                throw new Exception("You entered wrong emplyee data");
            }

            var accessToken = _tokenService.GenerateAccessToken(employee);
            var refreshToken =  await _tokenService.GenerateRefreshToken(employee.Id);
            var role = Enum.GetName(employee.Role);
            
            return new LoginEmployeeResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.ToString()!,
                Role = role!
            };
        }

        public async Task Register(RegisterEmployeeRequest request)
        {
            // Дядя, а ты точно овнер этого клуба
            var ownerId = _contextAccessor.TryGetHttpUserId();
            var isRequiredOwner = await _cyberClubService.VerifyOwner(ownerId, request.CyberClubName);

            if (!isRequiredOwner)
            {
                Results.InternalServerError($"User is not the owner of this CyberClub({request.CyberClubName})");
                return;
            }
            // Дядя, а ты уверен, что клуб с таким названием существует?

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var cyberClub = await _cyberClubService.GetByCCName(request.CyberClubName)
                    ?? throw new Exception("CyberClub not found");

                var hashedPassword = _hasher.Generate(request.Password);
                var hashedSecretWord = _hasher.Generate(request.SecretWord);
                
                var employeeRole = Enum.Parse<Role>(request.RoleName);

                var bloomBytesEntity = new BloomBytesEntity
                {
                    EmailBytes = _bloomBytesService.GetBytes(request.Email),
                    UserNameBytes = _bloomBytesService.GetBytes(request.UserName)
                };
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
                    bloomBytesId: bloomBytesEntity.Id
                );

                await _employeesRepository.Register(newEmployee);
                await _bloomBytesService.SaveBloomBytesAsync(bloomBytesEntity);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Results.InternalServerError("Transaction failed" + ex.Message);
            }


        }
        public async Task<List<EmployeeDto>> GetAll()
        {
            var employees = await _employeesRepository.GetAllEmployeesAsync()
                ?? throw new Exception("GetAllEmployees exception");
            return employees
                .Select(e => new EmployeeDto(e))
                .OrderByDescending(e => e.RoleName)
                .ToList();
        }
        public async Task<List<EmployeeDto>> GetWithBonus()
        {
            var employees = await GetAll()
                ?? throw new Exception("GetAllEmployees exception");
            return employees.Where(e => e.Bonus != 0).ToList();

        }
        public async Task<List<EmployeeDto>> GetWithPenalty()
        {
            var employees = await GetAll()
                ?? throw new Exception("GetAllEmployees exception");
            return employees.Where(e => e.Penalty != 0).ToList();

        }
        public async Task<List<EmployeeDto>> GetByCCId(Guid cyberClubId)
        {
            var applicantId = _contextAccessor.TryGetHttpUserId();
            var employees = await _employeesRepository.GetByCyberClubId(cyberClubId);

            if (!employees.Any(e => e.Id == applicantId && e.Role > Role.Admin))
            {
                throw new Exception("You can't acces to cyber club employee list");
            }

            return employees
                .Select(e => new EmployeeDto(e))
                .ToList();
        }
        public async Task<List<EmployeeDto>> GetByCCName(string cyberClubName)
        {
            var employees = await _employeesRepository.GetByCyberClubName(cyberClubName)
                ;

            return employees
                .Select(e => new EmployeeDto(e))
                .ToList();
        }
        public async Task<EmployeeDto> GetByNames(string firstName, string lastName)
        {
            var employee = await _employeesRepository.GetByNames(firstName, lastName);
            return new EmployeeDto(employee);
        }
        public async Task UpdateEmployee(UpdateEmployeeRequest request)
        {
            var newSalary = request.NewSalary ?? 0;
            await _employeesRepository.UpdateEmployee(
                request.FirstName,
                request.LastName,
                request.NewFirstName,
                request.NewLastName,
                newSalary,
                request.NewRoleName,
                request.NewCyberClubName
            );
        }
        public async Task SetZeroBonuses()
        {
            await _employeesRepository.SetZeroBonuses();
        }
        public async Task SetZeroPenalties()
        {
            await _employeesRepository.SetZeroPenalties();
        }

        public async Task Delete(DeleteEmployeeRequest request)
        {
            await _employeesRepository.Delete(request.FirstName, request.LastName);
        }

        public async Task GiveBonus(GiveBonusRequest request)
        {

            var id = _contextAccessor.TryGetHttpUserId();

            await _employeesRepository.GiveBonus(
                id,
                request.FirstName,
                request.LastName,
                request.Bonus
                );
        }
        public async Task GivePenalty(GivePenaltyRequest request)
        {

            var id = _contextAccessor.TryGetHttpUserId();

            await _employeesRepository.GivePenalty(
                id,
                request.FirstName,
                request.LastName,
                request.Penalty
                );

        }
    }
}