using GNS.Enums;
namespace GNS.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string CyberClubName { get; set; } = string.Empty;
        public int GamingPlaceNumber { get; set; }
        public Equipment Equipment { get; set; } 
        public decimal TotalSum { get; set; }
        public OrderStatus OrderStatus { get; set; }
        

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;


        public Guid GamingPlaceId { get; set; }
        public GamingPlaceEntity GamingPlace { get; set; } = null!;
    }
}