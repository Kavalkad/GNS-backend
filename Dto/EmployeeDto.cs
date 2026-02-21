
using GNS.Data.Entities;

namespace GNS.Dto
{
    public record class EmployeeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public decimal Bonus { get; set; }
        public decimal Penalty { get; set; }
        public string CyberClubName { get; set; } = string.Empty;

        public EmployeeDto(EmployeeEntity e)
        {
            Id = e.Id;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Salary = e.Salary;
            Bonus = e.Bonus;
            Penalty = e.Penalty;
            RoleName = Enum.GetName(e.Role);
            CyberClubName = e.CyberClub.Name;
        }
    }
}