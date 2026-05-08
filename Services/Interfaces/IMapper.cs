using GNS.Data.Entities;
using GNS.Dto;

namespace GNS.Services.Interfaces
{
    public interface IMapper
    {
        CyberClubDto MapToCyberClubDto(CyberClubEntity club);
        List<CyberClubDto> MapToCyberClubDto(IEnumerable<CyberClubEntity> clubs);
        EmployeeDto MapToEmployeeDto(EmployeeEntity e);
        List<EmployeeDto> MapToEmployeeDto(IEnumerable<EmployeeEntity> employees);
        List<WorkingHoursDto> MapToWorkingHoursDto(IEnumerable<WorkingHoursEntity> workingHours);
        WorkingHoursDto MapToWorkingHoursDto(WorkingHoursEntity wh);
        List<TimeSlotDto> MapToTimeSlotsDtoList(WorkingHoursDto wh);
        GameDto MapToGameDto(GameEntity g);
        List<GameDto> MapToGameDto(IEnumerable<GameEntity> games);
       
        GamingPlaceDto MapToGamingPlaceDto(GamingPlaceEntity gp);
        List<GamingPlaceDto> MapToGamingPlaceDto(IEnumerable<GamingPlaceEntity> gamingPlaces);
        OrderDto MapToOrderDto(OrderEntity o);
        List<OrderDto> MapToOrderDto(IEnumerable<OrderEntity> orders);
    }
}