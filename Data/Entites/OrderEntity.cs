using GNS.Enums;
namespace GNS.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public decimal TotalSum { get; set; }
        public OrderStatus OrderStatus { get; set; }
        

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;


        public Guid GamingPlaceId { get; set; }
        public GamingPlaceEntity GamingPlace { get; set; } = null!;
    }
}