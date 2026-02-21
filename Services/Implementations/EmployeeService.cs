using GNS.Extensions;
using GNS.Dto;
using GNS.Enums;
using GNS.Services.Interfaces;
using GNS.Data.Repositories.Interfaces;
using GNS.Contracts.Requests;
using GNS.Contracts.Response;

namespace GNS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IHasher _hasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeService(
            IEmployeesRepository employeesRepository,
            IHasher hasher,
            IJwtProvider jwtProvider,
            IHttpContextAccessor contextAccessor
            )
        {
            _employeesRepository = employeesRepository;
            _hasher = hasher;
            _jwtProvider = jwtProvider;
            _contextAccessor = contextAccessor;
        }
        public async Task<LoginEmployeeResponse> Login(LoginEmployeeRequest request)
        {
            var employee = await _employeesRepository.GetByEmail(request.Email);

            bool isFound = _hasher.Verify(request.Password, employee.HashedPassword)
                && _hasher.Verify(request.SecretWord, employee.HashedSecretWord );

            if (!isFound)
            {
                throw new Exception("Employee not found");
            }
            var role = employee.Role;
            var token = _jwtProvider.GenerateToken(employee);

            return new LoginEmployeeResponse {Token = token, Role = nameof(role)};
        }

        public async Task Register(RegisterEmployeeRequest request)
        {
            var hashedPassword = _hasher.Generate(request.Password);
            var hashedSecretWord = _hasher.Generate(request.SecretWord);

            await _employeesRepository.Register(
                request.Email,
                hashedPassword,
                hashedSecretWord,
                request.FirstName,
                request.LastName,
                request.Salary,
                request.RoleName,
                request.CyberClubName
            );
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
            var applicantId = _contextAccessor.GetHttpUserId(); 
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

            var id = _contextAccessor.GetHttpUserId();

            await _employeesRepository.GiveBonus(
                id,
                request.FirstName,
                request.LastName,
                request.Bonus
                );
        }
        public async Task GivePenalty(GivePenaltyRequest request)
        {

            var id = _contextAccessor.GetHttpUserId();

            await _employeesRepository.GivePenalty(
                id,
                request.FirstName,
                request.LastName,
                request.Penalty
                );

        }
    }
}