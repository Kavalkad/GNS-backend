
namespace GNS.Dto
{
    public record class GamingPlaceDto
    {
        public Guid Id { get; set; } 
        public int Number { get; set; } 
        public decimal PricePerHour { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
    }
}