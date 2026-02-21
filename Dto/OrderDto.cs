
using GNS.Data.Entities;

namespace GNS.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CyberClubName { get; set; } = string.Empty;
        public int GamingPlaceNumber { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderDto(OrderEntity o)
        {
            Id = o.Id;
            CyberClubName = o.GamingPlace.CyberClub.Name;
            GamingPlaceNumber = o.GamingPlace.Number;
            EquipmentName = Enum.GetName(o.GamingPlace.Equipment);
            Date = o.Date.ToString();
            StartTime = o.StartTime.ToString();
            EndTime = o.EndTime.ToString();
            TotalPrice = o.TotalSum;
        }
    }
}