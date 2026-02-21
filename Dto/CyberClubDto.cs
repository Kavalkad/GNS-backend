using GNS.Data.Entities;

namespace GNS.Dto
{
    public record class CyberClubDto
    {
        public Guid Id { get; set; }
        public string Name { get; }
        public string City { get; }
        public string Address { get; }
        public int EmployeesCount { get; }
        public int GamingPlacesCount { get; }
        public CyberClubDto(CyberClubEntity cc)
        {
            Id = cc.Id;
            Name = cc.Name;
            City = cc.City;
            Address = cc.Address;
            EmployeesCount = cc.Employees.Count;
            GamingPlacesCount = cc.GamingPlaces.Count;
        }
    };

}