using GNS.Data.Entities;

namespace GNS.Dto
{
    public record class AdminDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal Bonus { get; set; }
        public decimal Penalty { get; set; }
        public AdminDto(EmployeeEntity e)
        {
            Id = e.Id;
            FirstName = e.FirstName;
            LastName = e.LastName;
            Bonus = e.Bonus;
            Penalty = e.Penalty;
        }
    };

}