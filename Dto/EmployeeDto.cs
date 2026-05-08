
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
       

        
    }
}