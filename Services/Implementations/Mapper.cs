using GNS.Data.Entities;
using GNS.Dto;
using GNS.Services.Interfaces;

namespace GNS.Services.Implementations
{
    public class Mapper : IMapper
    {
        public CyberClubDto MapToCyberClubDto(CyberClubEntity cyberClub)
        {
            return new CyberClubDto
            {
                Id = cyberClub.Id,
                Name = cyberClub.Name,
                City = cyberClub.City,
                Address = cyberClub.Address
            };
        }
        public List<CyberClubDto> MapToCyberClubDto(IEnumerable<CyberClubEntity> clubs)
        {
            return clubs.Select(MapToCyberClubDto).ToList();
        }
        public EmployeeDto MapToEmployeeDto(EmployeeEntity e)
        {
            return new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Bonus = e.Bonus,
                Penalty = e.Penalty,
                Salary = e.Salary,
                RoleName = Enum.GetName(e.Role) ?? "Undefined"
            };
        }
        public List<EmployeeDto> MapToEmployeeDto(IEnumerable<EmployeeEntity> employees)
        {
            return employees.Select(MapToEmployeeDto).ToList();
        }
        public WorkingHoursDto MapToWorkingHoursDto(WorkingHoursEntity wh)
        {
            return new WorkingHoursDto
            {
                Id = wh.Id,
                StartHour = wh.StartHour,
                EndHour = wh.EndHour,
                DayOfWeek = Enum.GetName(wh.DayOfWeek) ?? "Undefined",
                IsOpen = wh.IsOpen 
            };
        }
        public List<WorkingHoursDto> MapToWorkingHoursDto(IEnumerable<WorkingHoursEntity> workingHours)
        {
            return workingHours.Select(MapToWorkingHoursDto).ToList();
        }

        public List<TimeSlotDto> MapToTimeSlotDtoList(WorkingHoursDto wh)
        {
            var timeSlots = new List<TimeSlotDto>();
            var max = wh.EndHour;

            for (TimeOnly start = wh.StartHour,
                     end = start.AddHours(1);
                        end < max; start.AddHours(1), end.AddHours(1))
            {
                timeSlots.Add(new TimeSlotDto
                {
                    Start = start,
                    End = end
                });
            }
            return timeSlots;
        }
        public List<TimeSlotDto> MapToTimeSlotDtoList(IEnumerable<OrderEntity> orders)
        {
            return orders.Select(o => new TimeSlotDto
            {
                Start = TimeOnly.FromDateTime(o.DateTimeStart),
                End = TimeOnly.FromDateTime(o.DateTimeEnd)
            })
                .OrderBy(ts => ts.Start)
                .ToList();
        }

        public GameDto MapToGameDto(GameEntity g)
        {
            var gameDto = new GameDto
            {
                Id = g.Id,
                Title = g.Title
            };
            if (g.OnPc)
            {
                gameDto.AvailableOn.Add("Pc");
            }
            if (g.OnXbox)
            {
                gameDto.AvailableOn.Add("Xbox");
            }
            if (g.OnPlayStation)
            {
                gameDto.AvailableOn.Add("PlayStation");
            }
            return gameDto;
        }
        public List<GameDto> MapToGameDto(IEnumerable<GameEntity> games)
        {
            return games.Select(MapToGameDto).ToList();
        }
       
        public GamingPlaceDto MapToGamingPlaceDto(GamingPlaceEntity gp)
        {
            return new GamingPlaceDto
            {
                Id = gp.Id,
                Number = gp.Number,
                PricePerHour = gp.PricePerHour,
                EquipmentName = Enum.GetName(gp.Equipment) ?? "Unefined"
            };
        }
        public List<GamingPlaceDto> MapToGamingPlaceDto(IEnumerable<GamingPlaceEntity> gamingPlaces)
        {
            return gamingPlaces.Select(MapToGamingPlaceDto).ToList();
        }
        public OrderDto MapToOrderDto(OrderEntity o)
        {
            return new OrderDto
            {
                Id = o.Id,
                GamingPlaceNumber = o.GamingPlaceNumber,
                CyberClubName = o.CyberClubName,
                Start = o.DateTimeStart.ToString(),
                End = o.DateTimeEnd.ToString(),
                TotalPrice = o.TotalSum,
                EquipmentName = Enum.GetName(o.Equipment) ?? "Undefined",
                OrderStatus = Enum.GetName(o.OrderStatus) ?? "Undefined"
            };
        }
        public List<OrderDto> MapToOrderDto(IEnumerable<OrderEntity> orders)
        {
            return orders.Select(MapToOrderDto).ToList();
        }
    }
}