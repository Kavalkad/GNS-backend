namespace GNS.Data.Entities
{
    public class CyberClubEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;


        public List<WorkingHoursEntity>? WorkingHours { get; set; }
        public ICollection<EmployeeEntity> Employees { get; set; } = [];
        public Guid OwnerId { get; set; }
        public OwnerEntity Owner { get; set; } = null!;
        public ICollection<GamingPlaceEntity> GamingPlaces { get; set; } = [];

    }
}