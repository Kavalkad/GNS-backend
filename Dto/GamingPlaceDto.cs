
using GNS.Data.Entities;

namespace GNS.Dto
{
    public class GamingPlaceDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public decimal PricePerHour { get; set; }
        public string EquipmentName { get; set; } = string.Empty;


        public GamingPlaceDto(GamingPlaceEntity gp)
        {
            Id = gp.Id;
            Number = gp.Number;
            PricePerHour = gp.PricePerHour;
            EquipmentName = Enum.GetName(gp.Equipment);

        }
       
    }
}