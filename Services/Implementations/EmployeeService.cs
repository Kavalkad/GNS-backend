using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Contracts.Responses;
using GNS.Data.Entities;
using GNS.Exceptions;
using Microsoft.AspNetCore.Authentication;

namespace GNS.Services.Implementations
{
    public class EmployeeService(
        IEmployeesRepository employeesRepository,
        IHasher hasher,
        ITokenService tokenService,
        ICyberClubService cyberClubService,
        IUnitOfWork unitOfWork,
        IBloomBytesService bloomBytesService,
        IMapper mapper
            ) : IEmployeeService
    {
        private readonly IEmployeesRepository _employeesRepository = employeesRepository;
        private readonly IHasher _hasher = hasher;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICyberClubService _cyberClubService = cyberClubService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IBloomBytesService _bloomBytesService = bloomBytesService;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginEmployeeResponse> LoginAsync(LoginEmployeeRequest request, CancellationToken token = default)
        {
            var employee = await _employeesRepository.FindAsync(e => e.Email == request.Email, token);
                

            bool result = employee is not null
                && _hasher.Verify(request.Password, employee.HashedPassword)
                && _hasher.Verify(request.SecretWord, employee.HashedSecretWord);

            if (!result)
            {
                throw new AuthenticationFailureException("Wrong email, password or secret word");
            }

            var accessToken = _tokenService.GenerateAccessToken(employee.Id, employee.Role);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(employee.Id, token);
            var role = Enum.GetName(employee.Role);

            return new LoginEmployeeResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token.ToString(),
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };
        }

        public async Task RegisterAsync(RegisterEmployeeRequest request, CancellationToken token = default)
        {
            var cyberClub = await _cyberClubService.GetClubByIdAsync(request.CyberClubId, token)
                ?? throw new EntityNotFoundException("CyberClub", request.CyberClubId.ToString());

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

       
        public async Task<EmployeeEntity> GetByIdAsync(Guid employeeId, CancellationToken token = default)
        {
            return await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("employee", employeeId.ToString());
        }
        public async Task<List<EmployeeDto>> GetWithBonusAsync(CancellationToken token = default)
        {
            var employees = await _employeesRepository.GetByExpressionAsync(e => e.Bonus > 0, token)
                ?? throw new EntityNotFoundException("employee", "with bonus");

            return _mapper.MapToEmployeeDto(employees);

        }
        public async Task<List<EmployeeDto>> GetWithPenaltyAsync(CancellationToken token = default)
        {
            var employees = await _employeesRepository.GetByExpressionAsync(e => e.Penalty > 0, token)
                ?? throw new EntityNotFoundException("employee", "with penalty");

            return _mapper.MapToEmployeeDto(employees);

        }
        public async Task<List<EmployeeDto>> GetByCyberClubIdAsync(Guid cyberClubId, CancellationToken token = default)
        {
            var employees = await _employeesRepository.GetByExpressionAsync(e => e.CyberClubId == cyberClubId, token)
                ?? throw new EntityNotFoundException("employee", $"cyberClubId: {cyberClubId}");

            return _mapper.MapToEmployeeDto(employees);
        }

        public async Task<EmployeeDto> GetByNamesAsync(string firstName, string lastName, CancellationToken token = default)
        {
            var employee = await _employeesRepository.FindAsync(
                e => e.FirstName == firstName && e.LastName == lastName, token)
                    ?? throw new EntityNotFoundException("Employee", $"firstName: {firstName}, lastName: {lastName}");

            return _mapper.MapToEmployeeDto(employee);
        }
        public async Task UpdateFirstNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            var employeeId = request.EmployeeId;

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            employee.FirstName = request.Name;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateLastNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            var employeeId = request.EmployeeId;

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            employee.LastName = request.Name;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateRoleNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            var employeeId = request.EmployeeId;
            if (!Enum.TryParse(request.Name, out Role role))
            {
                throw new IncorrectRoleNameException(request.Name);
            }

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            employee.Role = role;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task UpdateCyberClubNameAsync(
            UpdateEmployeeNameRequest request,
            CancellationToken token = default
        )
        {
            var employeeId = request.EmployeeId;

            var cyberClub = await _cyberClubService.FindByCyberClubNameAsync(request.Name, token)
                ?? throw new EntityNotFoundException("CyberClub", request.Name);

           var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

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

        public async Task DeleteAsync(Guid employeeId, CancellationToken token = default)
        {
            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            _employeesRepository.Delete(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }

        public async Task GiveBonusAsync(GiveBonusRequest request, CancellationToken token = default)
        {
            var employeeId = request.EmployeeId;

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            employee.Bonus = request.Bonus;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
        public async Task GivePenaltyAsync(GivePenaltyRequest request, CancellationToken token = default)
        {
            var employeeId = request.EmployeeId;

            var employee = await _employeesRepository.GetByIdAsync(employeeId, token)
                ?? throw new EntityNotFoundException("Employee", employeeId.ToString());

            employee.Penalty = request.Penalty;

            _employeesRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}