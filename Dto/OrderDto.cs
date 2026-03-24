
using GNS.Data.Entities;

namespace GNS.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CyberClubName { get; set; } = string.Empty;
        public int GamingPlaceNumber { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderDto(OrderEntity o)
        {
            Id = o.Id;
            CyberClubName = o.GamingPlace.CyberClub.Name;
            GamingPlaceNumber = o.GamingPlace.Number;
            EquipmentName = Enum.GetName(o.GamingPlace.Equipment);
            Start = o.DateTimeStart.ToString();
            End = o.DateTimeEnd.ToString();
            TotalPrice = o.TotalSum;
        }
    }
}