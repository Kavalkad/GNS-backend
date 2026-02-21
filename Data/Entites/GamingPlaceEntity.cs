using GNS.Enums;

namespace GNS.Data.Entities
{
    public class GamingPlaceEntity : BaseEntity
    {

        public int Number { get; set; }
        public decimal PricePerHour { get; set; }
        public Equipment Equipment { get; set; }

        public Guid CyberClubId { get; set; }
        public CyberClubEntity CyberClub { get; set; } = null!;


        public ICollection<GameEntity> Games { get; set; } = new List<GameEntity>();


        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>(); 
    }
}