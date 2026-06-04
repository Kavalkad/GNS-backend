
namespace GNS.Dto
{
    public record class OrderDto
    {
        public Guid Id { get; set; }
        public string CyberClubName { get; set; } = string.Empty;
        public int GamingPlaceNumber { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        
    }
}